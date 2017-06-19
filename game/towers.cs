if (!isObject($CPB::TowerGroup)) {
	$CPB::TowerGroup = new SimGroup(TowerGroup) {};
	for (%i = 0; %i < 4; %i++) {
		if (!isObject("Tower" @ %i)) {
			TowerGroup.add(new SimSet("Tower" @ %i) {
				supportCount = 4;
				});
		}
	}
}

$TOWER::DEATHALERTCPPRIORITY = 10;
$CPB::GUARDCOUNT = 4;

$ClientVariable[$ClientVariableCount++] = "tower";
$ClientVariable[$ClientVariableCount++] = "isGuard";
$ClientVariable[$ClientVariableCount++] = "isPrisoner";
$ClientVariable[$ClientVariableCount++] = "isSelectedToBeGuard";

//Object properties:
//Client
//	tower
//	isGuard
//	isPrisoner
//	isSelectedToBeGuard
//Tower#
//	isDestroyed
//	guardClient
//	spawn
//	spotlightBot
//	origBrickCount
//	guardOption
//	supportCount

//Functions:
//Packaged:
//	GameConnection::onDrop
//Created:
//	assignGuard
//	serverCmdReplaceGuard
//	removeGuard
//	spawnGuard
//	killTower
//	validateTower
//	enableTowerSpotlights
//	disableTowerSpotlights
//	SimSet::destroy
//	fxDTSBrick::setTower
//	fxDTSBrick::clearTower
//	serverCmdSetGuard
//	serverCmdUnsetGuard
//	lockInGuards


package CPB_Game_Towers {
	function GameConnection::onDrop(%this, %val) {
		if (%this.isGuard) {
			messageAdmins("<font:Palatino Linotype:36>!!! \c6Tower \c3" @ %this.tower @ "\c6's guard has just left the game!");
		} else if (%this.bl_id !$= "" && %cl.isSelectedToBeGuard) {
			serverCmdUnsetGuard($superAdmin, %this.name);
		}

		return parent::onDrop(%this, %val);
	}
};
activatePackage(CPB_Game_Towers);

function assignGuard(%cl, %towerNum) {
	//randomly pick a tower to assign the client to
	if (%towerNum $= "") {
		%towerNum = getRandom(0, 3);
	} else {
		%towerNum = %towerNum % 4;
	}

	//if its filled, look for an empty spot
	if (isObject(("Tower" @ %towerNum).guard)) {
		for (%i = 0; %i < 4; %i++) {
			%towerNum = (%towerNum % 4) + 1;
			if (!isObject(("Tower" @ %towerNum).guard))
				break;
		}
	}

	echo(%cl.name @ " assigned to tower " @ %towerNum);

	if (%i == 4) { //remove guard status of overridden player
		("Tower" @ %towerNum).guard.isGuard = 0;
		("Tower" @ %towerNum).guard.isPrisoner = 1;
	}

	%cl.isGuard = 1;
	%cl.isPrisoner = 0;
	%cl.tower = "Tower" @ %towerNum;
	("Tower" @ %towerNum).guard = %cl;
}

function serverCmdReplaceGuard(%cl, %towerNum, %name) {
	if (!%cl.isAdmin) {
		return;
	}	
	%cl = fcn(%name);

	if (!isObject(%cl)) {
		return;
	}
	%towerNum = ((%towerNum - 1) % 4) + 1;
	%tower = ("Tower" @ (%towerNum - 1));
	if (!isObject(%tower)) {
		messageAdmins("!!! \c5Failed to replace guard at tower " @ %towerNum - 1 @ " - tower does not exist!");
		return;
	} else if (%tower.isDestroyed) {
		messageAdmins("!!! \c5Failed to replace guard at tower " @ %towerNum - 1 @ " - tower is destroyed!");
		return;
	}

	if (isObject(%tower.guard)) {
		%tower.guard.isGuard = 0;
		%tower.guard.isPrisoner = 1;
		%tower.guard.tower = "";
	}

	%tower.guard = %cl;
	%cl.isGuard = 1;
	%cl.isPrisoner = 0;
	%cl.tower = "Tower" @ %towerNum;
	//respawn client at tower
	spawnGuard(%towerNum - 1, 0);
}

function removeGuard(%cl) {
	if (!isObject(%cl)) {
		return;
	}

	for (%i = 0; %i < 4; %i++) {
		%towerNum = %i + 1;
		if (("Tower" @ %towerNum).guard == %cl) {
			("Tower" @ %towerNum).guard = "";
			%cl.isGuard = 0;
			return;
		}
	}
	echo("Cannot find " @ %cl.name @ " in the list of guards!");
}

function resetTowerData(%id) {
	%tower = ("Tower" @ %id);

	%tower.isDestroyed = "";
	%tower.guardClient = "";
	%tower.spawn = "";
	%tower.spotlightBot = "";
	%tower.origBrickCount = "";
	%tower.guardOption = "";
	%tower.supportCount = "";
}

function killTower(%id) {
	%id = %id % 4;
	%tower = ("Tower" @ %id);
	%tower.isDestroyed = 1;
	%cl = %tower.guard;

	//setStatistic("Tower" @ %id + 1 @ "Destroyed", $Server::PrisonEscape::currTime);

	//remove the guard's items
	if (isObject(%cl.player)) {
		%cl.player.setDamageLevel(70);
	}

	//destroy the bricks but sequentially as to not lag everyone to death
	%tower.destroy();

	priorityCenterprintAll("<font:Impact:40>\c4Tower \c3" @ %id @ "\c4 has fallen!", 10, $TOWER::DEATHALERTCPPRIORITY);
	schedule(80, 0, priorityCenterprintAll, "<font:Impact:35>\c4Tower \c3" @ %id @ "\c4 has fallen!", 10, $TOWER::DEATHALERTCPPRIORITY);

	//chatlog death
	%type = $DamageType::MurderBitmap[$DamageType::Tower];
	messageAll('', "<bitmap:" @ %type @ "> " @ %id @ " [" @ getTimeString($CPB::currRoundTime - 1) @ "]");
	echo("Tower " @ %id @ " fell (Time: " @ getTimeString($CPB::currRoundTime) @ ")");
}

function validateTower(%id, %brick) {
	if (isObject(%id) || %id < 10) {
		validateTower(getSubStr(%id.getName(), strLen(%id.getName()) - 2, 1));
		return;
	}

	%tower = ("Tower" @ %towerNum);
	%tower.remove(%brick);
	if (%tower.getCount() <= %tower.origBrickCount - %tower.supportCount) {
		killTower(%id);
	}
	validateGameWin();
}

function enableTowerSpotlights() {
	for (%i = 0; %i < 4; %i++) {
		startLightBeamLoop(("Tower" @ %towerNum).spotlightBot); //
	}
}

function disableTowerSpotlights() {
	for (%i = 0; %i < 4; %i++) {
		clearLightBeam(("Tower" @ %towerNum).spotlightBot); //
	}
}

function SimSet::destroy(%this) {
	if (%this.getCount() <= 0) {
		return;
	}
	%brick = %this.getObject(%this.getCount()-1);
	%brick.fakeKillBrick((getRandom()-0.5)*20 SPC (getRandom()-0.5)*20 SPC 15, 2);
	%brick.schedule(2000, delete);

	%this.remove(%brick);

	if (isObject(%brick.item)) {
		%brick.item.delete();
	}
	if (isObject(%brick.vehicle)){
		%brick.vehicle.kill();
	}
	if (isObject(%brick.emitter)){
		%brick.emitter.delete();
	}
	if (isObject(%brick.light)){
		%brick.light.delete();
	}

	serverPlay3D("brickBreakSound", %brick.getPosition());

	%this.schedule(1, destroy);
}

registerOutputEvent(fxDTSBrick, "setTower", "list 1 1 2 2 3 3 4 4", 1);
registerOutputEvent(fxDTSBrick, "clearTower", "", 1);

function fxDTSBrick::setTower(%b, %tower, %cl) {
	if (!%cl.isSelectedToBeGuard) {
		messageClient(%cl, '', "You cannot pick a tower if you are not a selected guard!");
		return;
	} else if (%cl.guardClass $= "") {
		messageClient(%cl, '', "You cannot pick a tower without picking a class first!");
		return;
    }
	
	%cl.tower = ("Tower" @ %tower);
	if (isObject(%cl.tower.guard)) {
		messageClient(%cl.tower.guard, '', "\c6You have been replaced by \c3" @ %cl.name @ "\c6 at tower \c5" @ %tower);
		messageClient(%cl, '', "\c3" @ %cl.tower.guard.name @ "\c6 has been removed from tower \c5" @ %tower);
		%cl.tower.guard.tower = "";
	}
	%cl.tower.guard = %cl;
	%cl.tower.guardOption = %cl.guardClass;

	messageClient(%cl, '', "\c6You have been assigned to Tower \c5" @ %tower);
	%cl.pickedTowerBrick = %b;
}

function fxDTSBrick::clearTower(%b, %cl) {
	%cl.pickedTowerBrick.setEventEnabled("ALL", 1);
	%cl.tower.guardOption = "";
	%cl.tower.guard = "";
	%cl.tower = "";
}

function serverCmdUnsetGuard(%cl, %name) {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	}

	fxDTSBrick::clearTower(0, %cl);
	%cl.isSelectedToBeGuard = 0;
	%cl.isGuard = 0;
	if (isObject(%cl.player)) {
		%cl.player.delete();
	}
	spawnDeadLobby();
	$CPB::SelectedGuards = removeWord($CPB::SelectedGuards, %cl.bl_id);
	updateInfoBoard();
}

function serverCmdSetGuard(%cl, %name) {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	}

	%cl.isSelectedToBeGuard = 1;
	%cl.isGuard = 1;
	if (isObject(%cl.player)) {
		%cl.player.delete();
	}
	%cl.createPlayer(_GuardClassesRoom);
	$CPB::SelectedGuards = trim($CPB::SelectedGuards SPC %cl.bl_id);
	updateInfoBoard();
}

function lockInGuards() {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	} else if (getWordCount($CPB::SelectedGuards) != $CPB::GUARDCOUNT) {
		messageAdmins("!!! \c6Cannot lock in guards - not enough guards selected!");
		return;
	}

	for (%i = 0; %i < getWordCount($CPB::SelectedGuards); %i++) {
		%cl = findClientByBL_ID(getWord($CPB::SelectedGuards, %i));
		if (!isObject(%cl)) {
			messageAdmins("!!! \c6Cannot lock in guards - a guard does not exist!");
			return;
		}
		if (%isTaken[%cl.tower.getName()]) {
			messageAdmins("!!! \c6Cannot lock in guards - two guards share one tower!");
			return
		}
		%isTaken[%cl.tower.getName()] = 1;
	}
	return 1;
}