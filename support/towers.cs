if (!isObject($CPB::TowerGroup)) {
	$CPB::TowerGroup = new ScriptObject(TowerGroup) {};
	for (%i = 0; %i < 4; %i++) {
		if (!isObject("Tower" @ %i)) {
			TowerGroup.add(new SimSet("Tower" @ %i) {});
		}
	}
}

$TOWER::DEATHALERTCPPRIORITY = 10;

//Object properties:
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
	%cl.tower = %towerNum;
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
		//guard spawn as prisoner
	}
	%tower.guard = %cl;
	%cl.isGuard = 1;
	%cl.isPrisoner = 0;
	//respawn client at tower
	spawnGuard(%towerNum - 1, 0);
}

function removeGuard(%cl) {
	if (!isObject(%cl)) {

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
	if (isObject(%cl.player))
		%cl.player.setDamageLevel(70);

	//destroy the bricks but sequentially as to not lag everyone to death
	%tower.destroy();

	priorityCenterprintAll("<font:Impact:40>\c4Tower \c3" @ %id @ "\c4 has fallen!", 10, $TOWER::DEATHALERTCPPRIORITY);
	schedule(80, 0, priorityCenterprintAll, "<font:Impact:35>\c4Tower \c3" @ %id @ "\c4 has fallen!", 10, $TOWER::DEATHALERTCPPRIORITY);

	%type = $DamageType::MurderBitmap[$DamageType::Tower];
	messageAll('', "<bitmap:" @ %type @ "> " @ %id @ " [" @ getTimeString($Server::PrisonEscape::currTime-1) @ "]");
}

function validateTower(%id, %brick) {
	%tower = $Server::PrisonEscape::Towers.tower[%id];
	%tower.remove(%brick);
	if (%tower.getCount() <= %tower.origCount - 4) {
		killTower(%id);
	}
	validateGameWin();
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