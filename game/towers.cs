if (!isObject($CPB::TowerGroup)) {
	$CPB::TowerGroup = new SimGroup(TowerGroup) {};
	for (%i = 0; %i < 4; %i++) {
		if (!isObject("Tower" @ %i)) {
			TowerGroup.add(new SimSet("Tower" @ %i) {
					towerNum = %i;
				});
		}
	}
}

$TOWER::DEATHALERTCPPRIORITY = 10;

$ClientVariable[$ClientVariableCount++] = "tower";
$ClientVariable[$ClientVariableCount++] = "isGuard";
$ClientVariable[$ClientVariableCount++] = "isPrisoner";
$ClientVariable[$ClientVariableCount++] = "isSelectedToBeGuard";

datablock ItemData(TowerItemA : HammerItem) {
	shapeFile = "./shapes/towerItem/towerItem.dts";

	uiName = "TowerItem";
};

//Object properties:
//Client
//	tower
//	isGuard
//	isPrisoner
//	isSelectedToBeGuard
//Tower#
//	isDestroyed
//	guard
//	spawn
//	spotlightBot
//	origBrickCount
//	guardOption
//	supportCount

//Functions:
//Packaged:
//	GameConnection::onDrop
//	MinigameSO::removeMember
//Created:
//	assignGuard
//	serverCmdReplaceGuard
//	removeGuard
//	resetTowerData
//	resetAllTowerData
//	spawnGuard
//	killTower
//	validateTower
//	enableTowerSpotlights
//	disableTowerSpotlights
//	SimSet::destroy
//	fxDTSBrick::setTower
//	fxDTSBrick::clearTower
//	fxDTSBrick::softClearTower
//	fxDTSBrick::onSetTower
//	fxDTSBrick::onClearTower
//	getClosestTower


package CPB_Game_Towers {
	function GameConnection::onDrop(%this, %val) {
		if (%this.isGuard && isObject(%this.tower)) {
			messageAdmins("<font:Palatino Linotype:36>!!! \c6Tower \c3" @ %this.tower.towerNum @ "\c6's guard has just left the game!");
		} else if (%this.bl_id !$= "" && %this.isSelectedToBeGuard) {
			serverCmdRemoveGuard(FakeClient, %this.name);
		}

		return parent::onDrop(%this, %val);
	}

	function MinigameSO::removeMember(%mg, %cl) {
		if (%cl.isGuard && isObject(%cl.tower)) {
			messageAdmins("<font:Palatino Linotype:36>!!! \c6Tower \c3" @ %cl.tower.towerNum @ "\c6's guard has just left the game!");
		} else if (%cl.bl_id !$= "" && %cl.isSelectedToBeGuard) {
			serverCmdRemoveGuard(FakeClient, %cl.name);
		}

		return parent::removeMember(%mg, %cl);
	}
};
activatePackage(CPB_Game_Towers);


////////////////////


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
	%cl.tower.guardOption = %cl.guardClass;
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
			%cl.isGuard = 0;
			if (isObject(%cl.minigame)) {
				%cl.isPrisoner = 1;
			}
			resetTowerData(("Tower" @ %towerNum));
			return;
		}
	}
	echo("Cannot find " @ %cl.name @ " in the list of guards!");
}


////////////////////


function resetTowerData(%id) {
	%tower = ("Tower" @ %id);

	%tower.isDestroyed = "";
	%tower.guard = "";
	%tower.spawn = "";
	%tower.spotlightBot = "";
	%tower.origBrickCount = "";
	%tower.guardOption = "";
	%tower.supportCount = "";
}

function resetAllTowerData() {
	for (%i = 0; %i < 4; %i++) {
		resetTowerData(%i);
	}
}

function killTower(%tower) {
	%id = %tower.towerNum;
	%tower.isDestroyed = 1;
	%cl = %tower.guard;

	//remove the guard's items
	if (isObject(%cl.player)) {
		%cl.player.clearTools();
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

function validateTower(%tower, %brick) {
	%tower.remove(%brick);
	if (%tower.getCount() <= %tower.origBrickCount - %tower.supportCount) {
		killTower(%tower);
	}
	if (checkPrisonerWinCondition()) {
		setPhase("PWIN");
	}
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

registerOutputEvent("fxDTSBrick", "setTower", "list 1 0 2 1 3 2 4 3", 1);
registerOutputEvent("fxDTSBrick", "clearTower", "", 1);
registerInputEvent("fxDTSBrick", "onClearTower", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");
registerInputEvent("fxDTSBrick", "onSetTower", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function fxDTSBrick::setTower(%b, %tower, %cl) {
	if (!%cl.isSelectedToBeGuard) {
		messageClient(%cl, '', "You cannot pick a tower if you are not a selected guard!");
		return;
	} else if (%cl.guardClass $= "") {
		messageClient(%cl, '', "You cannot pick a tower without picking a class first!");
		return;
    } else if (isObject(%cl.pickedTowerBrick)) {
    	%cl.pickedTowerBrick.clearTower(%cl);
    }
	
	%cl.tower = ("Tower" @ %tower);
	if (isObject(%cl.tower.guard)) {
		messageClient(%cl.tower.guard, '', "\c6You have been replaced by \c3" @ %cl.name @ "\c6 at \c5Tower " @ %tower + 1);
		messageClient(%cl, '', "\c3" @ %cl.tower.guard.name @ "\c6 has been removed from \c5Tower " @ %tower + 1);
		%cl.tower.guard.tower = "";
	}
	%cl.tower.guard = %cl;
	%cl.tower.guardOption = %cl.guardClass;

	messageClient(%cl, '', "\c6You have been assigned to \c5Tower " @ %tower + 1);
	%cl.pickedTowerBrick = %b;
	%b.onSetTower(%cl.player, %cl);
	%b.item.setShapeName(%cl.name @ " - " @ $CPB::Classes[%cl.guardClass]);
}

function fxDTSBrick::clearTower(%b, %cl) {
	%cl.pickedTowerBrick.item.setShapeName("");
	%cl.pickedTowerBrick.onClearTower(%cl.player, %cl);
	%cl.pickedTowerBrick = "";
	%cl.tower.guardOption = "";
	%cl.tower.guard = "";
	%cl.tower = "";
}

function fxDTSBrick::softClearTower(%b, %cl) {
	%cl.pickedTowerBrick.item.setShapeName("");
	%cl.pickedTowerBrick.onClearTower(%cl.player, %cl);
	%cl.pickedTowerBrick = "";
}

function fxDTSBrick::onSetTower(%b, %pl, %cl, %pos, %vec) {
	$InputTarget_["Self"] = %b;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%cl);

	%b.processInputEvent("onSetTower", %cl);
}

function fxDTSBrick::onClearTower(%b, %pl, %cl, %pos, %vec) {
	$InputTarget_["Self"] = %b;
	$InputTarget_["Player"] = %pl;
	$InputTarget_["Client"] = %cl;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%cl);

	%b.processInputEvent("onClearTower", %cl);
}

function getClosestTower(%pos) {
	for (%i = 0; %i < 4; %i++) {
		%tower = ("Tower" @ %i);
		if (%tower.isDestroyed) {
			continue;
		} else {
			if (%dist = vectorDist(%pos, %tower.spawn.getPosition()) < %closestDist || %closestDist $= "") {
				%closestDist = %dist;
				%closestTower = %tower;
			}
		}
	}
	return %closestTower;
}
