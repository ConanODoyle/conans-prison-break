//Functions:
//Packaged:
//	GameConnection::onDeath
//Created:
//	despawnAll
//	GameConnection::clearVariables


package CPB_Game_Spawn {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc) {
		if (isObject(%pl = %client.player)) {
			%pl.setShapeName("", 8564862);
			if (isObject(%pl.tempBrick)) {
				%pl.tempBrick.delete();
				%pl.tempBrick = 0;
			}
			%pl.client = 0;
		} else {
			warn("WARNING: No player object in GameConnection::onDeath() for client \'" @ %client @ "\'");
		}

		if (isObject(%cam = %cl.camera) && isObject(%cl.Player)) {
			if (%cl.getControlObject() == %cam && %cam.getControlObject() > 0.0) {
				%client.camera.setControlObject(%client.dummycamera);
			}
			else
			{
				%client.camera.setMode("Corpse", %client.Player);
				%client.setControlObject(%client.camera);
				%client.camera.setControlObject(0);
			}
		}
		%client.Player = 0;
	}
};
activatePackage(CPB_Game_Spawn);

function despawnAll() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (isObject(%pl = (%cl = ClientGroup.getObject(%i)).player)) {
			%pl.delete();
			%cl.setControlObject(%cl.camera);
			%cl.camera.setControlObject(%cl.dummycamera);
		}
	}
}

function GameConnection::clearVariables(%cl) {
	%cl.setScore(0);
	//other vars
}