if (!isObject($CPB::TowerGroup)) {
	$CPB::TowerGroup = new ScriptObject(TowerGroup) {};
	for (%i = 0; %i < 4; %i++) {
		if (!isObject("Tower" @ %i)) {
			TowerGroup.add(new SimSet("Tower" @ %i) {
				supportCount = 4;
				});
		}
	}
}

$TOWER::DEATHALERTCPPRIORITY = 10;

//Object properties:
//Client
//	tower
//	isGuard
//	isPrisoner
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
//	validateTower
//	killTower
//	SimSet::destroy

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
	%tower = $Server::PrisonEscape::Towers.tower[%id];
	%tower.remove(%brick);
	if (%tower.getCount() <= %tower.origCount - %tower.supportCount) {
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