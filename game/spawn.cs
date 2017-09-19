$CPB::RespawnWaveTime = 10 * 1.5;
$SPAWN::BRONSONDEATHCPPRIORITY = 9;

//Object properties:
//GameConnection
//	isDead

//Functions:
//Packaged:
//	GameConnection::onDeath
//	roundTimer
//	Observer::onTrigger
//	GameConnection::createPlayer
//	serverCmdCreateMinigame
//Created:
//	despawnAll
//	GameConnection::clearVariables
//	spawnDeadLobby
//	spawnAllLobby
//	getPrisonerLobbySpawnPoint
//	getGuardLobbySpawnPoint
//	spawnAllPrisoners
//	respawnPrisonersInfirmary
//	getPrisonerCellSpawnPoint
//	resetPrisonerCellSpawnPointCounts
//	getInfirmarySpawnPoint
//	resetInfirmarySpawnPointCounts
//	getNextRespawnTime


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
		%cl.isAlive = 0;

		if (%cl.isGuard) {
			checkPrisonerWinCondition();
		}
	}

	function roundTimer() {
		if (($CPB::CurrRoundTime) % $CPB::RespawnWaveTime == 0 && $CPB::CurrRoundTime < $CPB::RoundTime) {
			respawnPrisonersInfirmary();
		}
		parent::roundTimer();
	}

	function Observer::onTrigger(%this, %obj, %trig, %state) {
		if ((%cl = %obj.getControllingClient()).isDead && !%cl.isSpectating && isObject(%cl.minigame) && $CPB::PHASE != $CPB::LOBBY) {
			return;
		}
		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function GameConnection::createPlayer(%cl, %t) {
		%cl.isDead = 0;
		%cl.isAlive = 1;
		clearCenterprint(%cl);

		//if players self-respawn while dead during lobby phase, spawns them in appropriate place
		if ($CPB::PHASE == $CPB::LOBBY) {
			if (%cl.isPrisoner) {
				%t = getPrisonerLobbySpawnPoint();
			} else if (%cl.isGuard) {
				%t = getGuardLobbySpawnPoint();
			}
		}

		%ret = parent::createPlayer(%cl, %t);

		if (%cl.isPrisoner) {
			if ($CPB::PHASE == $CPB::LOBBY) {
				%cl.player.setShapeNameDistance(400);
			} else {
				%cl.player.setShapeNameDistance(12);
			}
			%cl.player.setShapeNameColor($PRISONER::CHATCOLOR);
		} else if (%cl.isGuard) {
			%cl.player.setShapeNameDistance(400);
			%cl.player.setShapeNameColor($GUARD::CHATCOLOR);
		}

		return %ret;
	}

	function serverCmdCreateMinigame(%cl, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k, %l, %m, %n) {
		if (!%cl.isSuperAdmin) {
			messageAdmins("!!! \c6- Attempt to create minigame by " @ %cl.name);
			return;
		}
		return parent::serverCmdCreateMinigame(%cl, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j, %k, %l, %m, %n);
	}

	function serverCmdLeaveMinigame(%cl) {
		%cl.isPrisoner = 0;
		%cl.isGuard = 0;
		parent::serverCmdLeaveMinigame(%cl);
	}

	function serverCmdJoinMinigame(%cl, %mg) {
		%cl.isPrisoner = 1;
		%cl.isGuard = 0;
		parent::serverCmdLeaveMinigame(%cl, %mg);
	}
};
activatePackage(CPB_Game_Spawn);

function despawnAll() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (isObject(%pl = (%cl = ClientGroup.getObject(%i)).player) && isObject(%cl.minigame)) {
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
		if ($ClientVariable[%i] !$= "") {
			eval(%cl @ "." @ $ClientVariable[%i] @ " = \"\";");
		}
	}
}

function spawnDeadLobby() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (!isObject(%pl = (%cl = ClientGroup.getObject(%i)).player) && isObject(%cl.minigame)) {
			%cl.createPlayer(getPrisonerLobbySpawnPoint());
		}
	}
}

function spawnAllLobby() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (!isObject(%cl.minigame)) {
			continue;
		}
		
		if (isObject(%pl = %cl.player) && isObject(%cl.minigame)) {
			%pl.delete();
		}
		%cl.createPlayer(getPrisonerLobbySpawnPoint());
	}
}

function getPrisonerLobbySpawnPoint() {
	return LobbySpawnPoints.getObject(getRandom(0, LobbySpawnPoints.getCount() - 1)).getTransform();
}

function getGuardLobbySpawnPoint() {
	%i = 0;
	while (isObject(%b = "_GuardVisualItems" @ %i) && isObject(%item = %b.item)) {
		colorVisualItem(%item);
		%i++;
	}
	return _GuardClassesRoom.getTransform();
}

function spawnAllPrisoners() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		commandToClient(%cl, 'showBricks', 0);
		%b = getPrisonerCellSpawnPoint();
		if (!isObject(%cl.player) && %cl.isPrisoner) {
			%cl.createPlayer(%b.getTransform());
			%count++;
		}
	}
	return %count;
}

function respawnPrisonersInfirmary() {
	%iSCount = InfirmarySpawnPoints.getCount();
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (!isObject(%cl.player) && %cl.isPrisoner) {
			%cl.createPlayer(getInfirmarySpawnPoint().getTransform());
			%count++;
		}
	}
	return %count;
}

function getPrisonerCellSpawnPoint() {
	%count = PrisonerSpawnPoints.getCount();
	%start = getRandom(0, %count - 1);
	for (%i = 0; %i < %count; %i++)	{
		%index = (%i + %start) % %count;
		%brick = PrisonerSpawnPoints.getObject(%index);
		if (%brick.spawnCount < 2) {
			break;
		}
		%brick = "";
	}
	if (isObject(%brick)) {
		%brick.spawnCount++;
		return %brick;
	} else {
		echo("Can't find a spawnpoint with less than 2 spawns! Resetting...");
		resetPrisonerCellSpawnPointCounts();
		return PrisonerSpawnPoints.getObject(%start);
	}
}

function resetPrisonerCellSpawnPointCounts() {
	for (%i = 0; %i < PrisonerSpawnPoints.getCount(); %i++) {
		PrisonerSpawnPoints.getObject(%i).spawnCount = 0;
	}
}

function getInfirmarySpawnPoint() {
	%count = InfirmarySpawnPoints.getCount();
	%start = getRandom(0, %count - 1);
	for (%i = 0; %i < %count; %i++)	{
		%index = (%i + %start) % %count;
		%brick = InfirmarySpawnPoints.getObject(%index);
		if (%brick.spawnCount < 1) {
			break;
		}
		%brick = "";
	}
	if (isObject(%brick)) {
		%brick.spawnCount++;
		return %brick;
	} else {
		echo("Can't find an infirmary spawnpoint with less than 1 spawn! Resetting...");
		resetInfirmarySpawnPointCounts();
		return InfirmarySpawnPoints.getObject(%start);
	}
}

function resetInfirmarySpawnPointCounts() {
	for (%i = 0; %i < InfirmarySpawnPoints.getCount(); %i++) {
		InfirmarySpawnPoints.getObject(%i).spawnCount = 0;
	}
}

function getNextRespawnTime() {
	if ($CPB::PHASE != $CPB::GAME) {
		return -1;
	}

	return $CPB::CurrRoundTime % $CPB::RespawnWaveTime;
}