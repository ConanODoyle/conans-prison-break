$CAMERAS::INFOCPPRIORITY = 1;

//Object properties:
//Player
//	isInCamera
//	canLeaveCamera
//	isPreviewingCameras

//Functions:
//Packaged:
//	Observer::onTrigger
//	serverCmdLight
//Created:
//	fxDTSBrick::setPlayerCamera
//	fxDTSBrick::doorToggleLoop
//	fxDTSBrick::endDoorToggleLoop
//	fxDTSBrick::previewCameras
//	getFormattedCameraCenterprint


package CPB_Game_Cameras {
	function Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();
		if (%cl.player.isPreviewingCameras) {
			if (%state == 1) {
				%count = SecurityCameras.getCount();
				SecurityCameras.getObject(%cl.currCamera).endDoorToggleLoop();

				if (%trig == $RIGHTCLICK) {
					%cl.currCamera = (%cl.currCamera + 1) % %count;
					%b = SecurityCameras.getObject(%cl.currCamera);
				} else if (%trig == $LEFTCLICK) {
					%cl.currCamera = (%cl.currCamera - 1 + %count) % %count;
					%b = SecurityCameras.getObject(%cl.currCamera);
				}

				%b.doorToggleLoop(5000);
				%b.setPlayerCamera(0, %cl);
				%b.numViewers++;
				%cl.centerprint(getFormattedCameraCenterprint(%b, %cl.currCamera));
			}
			return;
		}
		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function serverCmdLight(%cl) {
		if (%cl.player.isInCamera && %cl.player.canLeaveCamera) {
			if (isObject(%cl.player)) {
				%cl.setControlObject(%cl.player);
			}
			%cl.player.isInCamera = 0;
			if (%cl.player.isPreviewingCameras) {
				SecurityCameras.getObject(%cl.currCamera).endDoorToggleLoop();
			}
			%cl.player.isPreviewingCameras = 0;
			%cl.player.canLeaveCamera = 1;
			%cl.priorityCenterprint("", $CAMERAS::INFOCPPRIORITY);
			return;
		} else {
			return parent::serverCmdLight(%cl);
		}
	}
};
activatePackage(CPB_Game_Cameras);


////////////////////


function fxDTSBrick::setPlayerCamera(%this, %option, %cl) {
	%cam = %cl.camera;
	if(!isObject(%cam)) {
		return; //should never happen
	}

	%pos = %this.getPosition();
	switch (%option) {
		case 1:  %rot = "1 0 0 0";
		case 2:  %rot = "0 0 1 1.5708";  //n
		case 3:  %rot = "0 0 1 3.14159"; //e
		case 4:  %rot = "0 0 -1 1.5708"; //s
		case 5:  %rot = "-1 0 0 1.5708"; //w
		case 6:  %rot = "1 0 0 1.5708";  //u
		default: %rot = "0 0 0 0";       //d
	}
	
	if (%option) { //fixed camera
		%cam.setTransform(%pos SPC %rot);
		%cam.setFlyMode();
		%cam.setMode(Observer);
		%cam.setControlObject(%cl.dummyCamera);
	} else { //free camera
		%cam.setMode(Observer);
		%angleID = %this.getAngleID();
		switch (%angleID) {
			case 0: %rot = "0 0 1 1.5708"; %pos = vectorAdd(%pos, "0.5 0 0");
			case 1: %rot = "0 0 1 3.1416"; %pos = vectorAdd(%pos, "0 -0.5 0");
			case 2: %rot = "0 0 1 -1.5708"; %pos = vectorAdd(%pos, "-0.5 0 0");
			case 3: %rot = "0 0 1 0"; %pos = vectorAdd(%pos, "0 0.5 0");
		}
		%cam.setDollyMode(%pos, %pos);
		%cam.setTransform(%pos SPC %rot);
		%cam.setControlObject(%cam);
	}

	//client controls camera
	%cl.setControlObject(%cam);
	%cam.setwhiteout(0.5);

	%pl = %cl.player;
	if(isObject(%pl)) {
		%pl.isInCamera = 1;
		%pl.canLeaveCamera = 1;
	}
}

function fxDTSBrick::doorToggleLoop(%this, %time) {
	if (isEventPending(%this.doorToggleLoop)) {
		cancel(%this.doorToggleLoop);
	}
	if (%time == 0) {
		return;
	}

	if (%this.defaultState $= "") {
		if (%this.getDatablock().isOpen) {
			if (%this.getDatablock().getName() $= %this.getDatablock().openCCW) {
				%this.defaultState = 3;
			} else {
				%this.defaultState = 2;
			}
		} else {
			%this.defaultState = 4;
		}
	}

	if (%this.getDatablock().isOpen) {
		%this.door(4);
	} else {
		%name = strlwr(%this.getName());
		if (strPos(%name, "left") >= 0 || (!%this.nextTurnRight && strPos(%name, "right") < 0 && strPos(%name, "left") < 0)) {
			%this.door(2);
			%this.nextTurnRight = 1;
		} else {
			%this.door(3);
			%this.nextTurnRight = 0;
		}
	}
	%this.doorToggleLoop = %this.schedule(%time / 4, doorToggleLoop, %time);
}

function fxDTSBrick::endDoorToggleLoop(%this) {
	if (isEventPending(%this.doorToggleLoop)) {
		if (%this.numViewers >= 1) {
			%this.numViewers--;
		}
		if (%this.numViewers <= 0 && %this.defaultState !$= "") {
			cancel(%this.doorToggleLoop);
			%this.door(%this.defaultState);
		}
	}
}

registerOutputEvent("fxDTSBrick", "previewCameras", "", 1);

function fxDTSBrick::previewCameras(%this, %cl) {
	if (getSimTime() - %cl.lastUsedCameraTime < 2000 || %cl.player.isInCamera) {
		return;
	} else if (!$CPB::EWSActive) {
		%cl.centerprint("The camera's satellite dish have been destroyed by the prisoners!", 2);
		return;
	}
	%cl.lastUsedCameraTime = getSimTime();
	%cl.player.isInCamera = 1;
	%cl.player.isPreviewingCameras = 1;
	%cl.player.canLeaveCamera = 1; 

	if (SecurityCameras.getCount() <= 0) {
		messageAdmins("!!! \c6Cannot use camera previews - no cameras in SimSet!");
		return;
	}
	if (%cl.currCamera $= "") {
		%cl.currCamera = 0;
	}

	%count = SecurityCameras.getCount();
	%cl.currCamera = (%cl.currCamera - 1 + %count) % %count;
	SecurityCameras.getObject(%cl.currCamera).setPlayerCamera(0, %cl);
	SecurityCameras.getObject(%cl.currCamera).numViewers++;
	SecurityCameras.getObject(%cl.currCamera).doorToggleLoop(5000);

	messageClient(%cl,'',"\c2Camera in Free Mode \c6- Use Light key to exit the cameras");

	%cl.priorityCenterprint(getFormattedCameraCenterprint(SecurityCameras.getObject(%cl.currCamera), %cl.currCamera), -1, $CAMERAS::INFOCPPRIORITY);
}

function getFormattedCameraCenterprint(%b, %index) {
	%slots = "";
	for (%i = 0; %i < SecurityCameras.getCount(); %i++) {
		if (%i != %index) {
			%slots = trim(%slots SPC "[ ]");
		} else {
			%slots = %slots SPC "[\c3X\c5]";
		}
	}
	%slots = "\c5" @ %slots;

	%name = SecurityCameras.getObject(%index).getName();
	%name = getSubStr(%name, strPos(%name, "_", 1) + 1, strLen(%name));
	%name = strReplace(%name, "_", " ");
	%final = "<br><br><br><br><br>\c6" @ %name @ " <br><font:Palatino Linotype:18>\c3Left Click <font:Consolas:18>" @ %slots @ "<font:Palatino Linotype:18>\c3 Right Click ";
	return %final;
}