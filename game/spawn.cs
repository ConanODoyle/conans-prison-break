//Functions:
//Packaged:
//	GameConnection::onDeath
//Created:
//	despawnAll
//	GameConnection::clearVariables
//	spawnDeadLobby
//	spawnAllLobby

package CPB_Game_Spawn {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc) {
		if (!%cl.isPrisoner && !%cl.isGuard) {
			return parent::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc);
		}

		if (isObject(%pl = %cl.player)) {
			%pl.setShapeName("", 8564862);
			if (isObject(%pl.tempBrick)) {
				%pl.tempBrick.delete();
				%pl.tempBrick = 0;
			}
			%pl.client = 0;
			%pl.playThread(0, "death1");
			%pl.playThread(1, "death1");
			%pl.playThread(2, "death1");
			%pl.playThread(3, "death1");
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

		if (%cl.isGuard) {
			checkPrisonerWinCondition();
		}
	}

	function Observer::onTrigger(%this, %obj, %trig, %state) {
		if ((%cl = %obj.getControllingClient()).isDead && !%cl.isSpectating) {
			return;
		}
		return parent::onTrigger(%this, %obj, %trig, %state);
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

function spawnDeadLobby() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (!isObject(%pl = (%cl = ClientGroup.getObject(%i)).player)) {
			%cl.createPlayer(LobbySpawnPoints.getObject(getRandom(0, LobbySpawnPoints.getCount() - 1)).getTransform());
		}
	}
}

function spawnAllLobby() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (isObject(%pl = (%cl = ClientGroup.getObject(%i)).player)) {
			%pl.delete();
		}
		%cl.createPlayer(LobbySpawnPoints.getObject(getRandom(0, LobbySpawnPoints.getCount() - 1)).getTransform());
	}
}

function spawnDeadPrisonersInfirmary() {
	%iSCount = InfirmarySpawnPoints.getCount();
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (!isObject(%cl.player) && %cl.isPrisoner) {
			%cl.createPlayer(InfirmarySpawnPoints.getObject(getRandom(0, %iSCount)).getTransform());
			%count++;
		}
	}
	return %count;
}