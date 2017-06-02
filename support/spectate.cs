$SPECTATE::CONTROLCPPRIORITY = 10;

//Object properties:
//Client
//	isSpectating
//	spectatingClientIDX

//Functions:
//Packaged:
//	Observer::onTrigger
//Created:
//	spectateNextPlayer
//	GameConnection::spectateObject
//	Player::spectateObject

package CPB_Support_Spectate {
	Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();

		if (%cl.isSpectating && !%state) {
			if (%trig == $LEFTCLICK) {
				spectateNextPlayer(%cl, 1);
			} else if (%trig == $RIGHTCLICK) {
				spectateNextPlayer(%cl, -1);
			}
			return;
		}

		return parent::onTrigger(%this, %obj, %trig, %state);
	}
};
activatePackage(CPB_Support_Spectate);

function spectateNextPlayer(%cl, %num) {
	if (isObject(%cl.player)) { //should never be called
		%cl.setControlObject(%cl.player);
		return;
	}

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
	
	%cl.priorityCenterprint("<font:Consolas:18><just:left>\c6Left Click<just:right>\c6Right Click<font:Arial Bold:22> <br><just:left>\c3Next Player<just:right>\c3Prev Player ", 1000, $SPECTATE::CONTROLCPPRIORITY);

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
		%cl.camera.setControlObject(0);
	}
}

function Player::spectateObject(%pl, %obj, %canControl) {
	if (isObject(%cl = %pl.client)) {
		%cl.spectateObject(%obj, %canControl);
	}
}