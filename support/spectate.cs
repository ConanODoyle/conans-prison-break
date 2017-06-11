$SPECTATE::CONTROLCPPRIORITY = 10;

//Object properties:
//Client
//	isSpectating
//	spectatingClientIDX

//Functions:
//Packaged:
//	Observer::onTrigger
//	SimObject::onCameraEnterOrbit
//	SimObject::onCameraLeaveOrbit
//Created:
//	spectateNextPlayer
//	GameConnection::spectateObject
//	Player::spectateObject
//	GameConnection::stopSpectatingObject
//	Player::stopSpectatingObject

package CPB_Support_Spectate {
	function Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();

		if (%cl.isSpectating) {
			if (%trig == $LEFTCLICK && !%state) {
				spectateNextPlayer(%cl, 1);
			} else if (%trig == $RIGHTCLICK && !%state) {
				spectateNextPlayer(%cl, -1);
			}
			return;
		}

		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function SimObject::onCameraEnterOrbit(%obj) {
		//removes the bottomprint counter
		return;
	}

	function SimObject::onCameraLeaveOrbit(%obj) {
		//removes the bottomprint counter
		return;
	}
};
activatePackage(CPB_Support_Spectate);

function spectateNextPlayer(%cl, %num) {
	if (isObject(%cl.player)) { //should never be called
		%cl.setControlObject(%cl.player);
		return;
	}
	%cl.isSpectating = 1;

	%clientCount = ClientGroup.getCount();

	if (%num > 0) {
		%dir = 1;
	} else {
		%dir = -1;
	}

	%cl.spectatingClientIDX = (%cl.spectatingClientIDX + %num) % %clientCount;
	for (%i = 0; %i < %clientCount; %i++) {
		%targ = ClientGroup.getObject(%cl.spectatingClientIDX);
		if (isObject(%targPlayer = %targ.player)) {
			break;
		}
		%cl.spectatingClientIDX = (%cl.spectatingClientIDX + %dir + %clientCount) % %clientCount;
	}
	
	%cl.priorityCenterprint("<font:Consolas:18><just:left>\c6Left Click<just:right>\c6Right Click<font:Arial Bold:22> <br><just:left>\c3Next Player<just:right>\c3Prev Player ", -1, $SPECTATE::CONTROLCPPRIORITY);

	if (!isObject(%targPlayer))
		return;

	%cl.setControlObject(%cl.camera);
	%cl.camera.setControlObject(%cl.camera);
	%cl.camera.setMode(Corpse, %targPlayer);
}

function GameConnection::spectateObject(%cl, %obj, %canControl) {
	%cl.camera.setMode("Corpse", %obj);
	%cl.setControlObject(%cl.camera);
	if (%canControl $= "0" || %canControl $= "false") {
		%cl.camera.setControlObject(%cl.dummyCamera);
	} else {
		%cl.camera.setControlObject(%cl.camera);
	}
}

function Player::spectateObject(%pl, %obj, %canControl) {
	if (isObject(%cl = %pl.client)) {
		%cl.spectateObject(%obj, %canControl);
	}
}

function GameConnection::stopSpectatingObject(%cl) {
	%cl.camera.setMode("Observer");
	%cl.setControlObject(%pl);
	%cl.customizingMode = "";
	%cl.player.setControlObject(%cl.player);
}

function Player::stopSpectatingObject(%pl) {
	if (isObject(%cl = %pl.client)) {
		%cl.stopSpectatingObject();
	}
}