//Functions:
//Packaged:
//	GameConnection::onDeath
//Created:
//	despawnAll
//	GameConnection::clearVariables


package CPB_Game_Spawn {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc) {
		if (isObject(%pl = %cl.player)) {
			%pl.setShapeName("", 8564862);
			if (isObject(%pl.tempBrick)) {
				%pl.tempBrick.delete();
				%pl.tempBrick = 0;
			}
			%pl.client = 0;
		} else {
			warn("WARNING: No player object in GameConnection::onDeath() for client \'" @ %cl @ "\'");
		}

		if (isObject(%cam = %cl.camera) && isObject(%cl.Player)) {
			if (%cl.getControlObject() == %cam && %cam.getControlObject() > 0.0) {
				%cam.setControlObject(%cl.dummycamera);
			} else {
				%cam.setMode("Corpse", %cl.Player);
				%cl.setControlObject(%cam);
				%cam.setControlObject(0);
			}
		}
		%cl.player = 0;
		%cl.isDead = 1;
	}

	function Observer::onTrigger(%this, %obj, %trig, %state) {
		if (%obj.isDead) {
			return;
		}
		return parent::onTrigger(%this,  %obj, %trig, %state);
	}

	function GameConnection::createPlayer(%cl, %t) {
		%cl.isDead = 0;
		return parent::createPlayer(%cl, %t);
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
	for (%i = 0; %i < $ClientVariableCount; %i++) {
		eval(%cl @ "." @ $ClientVariable[%i] @ " = \"\";");
	}
}