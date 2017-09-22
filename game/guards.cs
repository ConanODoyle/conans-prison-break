$CPB::GUARDCOUNT = 4;

//Functions:
//Created:
//	serverCmdAddGuard
//	serverCmdRemoveGuard
//	validateGuardSelection
//	spawnGuard
//	spawnAllGuards


function serverCmdAddGuard(%cl, %name) {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	}

	%targ = findClientByName(%name);
	if (!isObject(%targ)) {
		messageClient(%cl, '', "Cannot find client by the name of " @ %name @ "!");
		return;
	} else if (%targ.isSelectedToBeGuard) {
		messageClient(%cl, '', %targ.name @ " is already selected to be a guard!");
		return;
	} else if (!isObject(%targ.minigame)) {
		messageClient(%cl, '', %targ.name @ " is not in the minigame!");
		return;
	} else if (getWordCount($CPB::SelectedGuards) >= $CPB::GUARDCOUNT) {
		messageClient(%cl, '', "There are already " @ $CPB::GUARDCOUNT @ " guards!");
		return;
	}

	%targ.isSelectedToBeGuard = 1;
	%targ.isGuard = 1;
	%targ.isPrisoner = 0;
	$CPB::SelectedGuards = trim($CPB::SelectedGuards SPC %targ.bl_id);

	if (isObject(%targ.player)) {
		%targ.player.delete();
	}
	%targ.createPlayer(getGuardLobbySpawnPoint());

	messageAll('', "\c3" @ %cl.name @ "\c6 added \c3" @ %targ.name @ "\c6 to the guard selection");
}

function serverCmdRemoveGuard(%cl, %name) {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	}

	%targ = findClientByName(%name);
	if (!isObject(%targ)) {
		messageClient(%cl, '', "Cannot find client by the name of " @ %name @ "!");
		return;
	} else if (!%targ.isSelectedToBeGuard) {
		messageClient(%cl, '', %targ.name @ " is not selected to be a guard!");
		return;
	} else if (getWordCount($CPB::SelectedGuards) <= 0) {
		messageClient(%cl, '', "There are no guards!");
		return;
	} else if (!containsWord($CPB::SelectedGuards, %targ.bl_id)) {
		messageClient(%cl, '', %targ.name @ " is not in the selected guards list!");
		return;
	}

	%targ.isSelectedToBeGuard = 0;
	%targ.isGuard = 0;
	%targ.isPrisoner = 1;
	%targ.guardClass = "";
	if (isObject(%targ.pickedTowerBrick)) {
		%targ.pickedTowerBrick.clearTower(%targ);
	}
	$CPB::SelectedGuards = removeWordString($CPB::SelectedGuards, %targ.bl_id);

	if (isObject(%targ.player)) {
		%targ.player.delete();
	}
	spawnDeadLobby();

	if (%cl != FakeClient.getID()) {
		messageAll('', "\c3" @ %cl.name @ "\c6 removed \c3" @ %targ.name @ "\c6 from the guard selection");
	} else {
		messageAll('', "\c3" @ %targ.name @ "\c6 was removed from the guard selection");
	}
}

function validateGuardSelection() {
	if ($CPB::PHASE != $CPB::LOBBY) {
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
			return;
		}
		if (%cl.guardClass == 0) {
			messageAdmins("!!! \c6Cannot lock in guards - a guard has no class!");
			return;
		}
		if (!isObject(%cl.tower)) {
			messageAdmins("!!! \c6Cannot lock in guards - a guard has no tower!");
			return;	
		}
		%isTaken[%cl.tower.getName()] = 1;
		if (isObject(%cl.pickedTowerBrick)) {
			%cl.pickedTowerBrick.softClearTower(%cl);
		} else {
			messageAdmins("!!! \c6Client has no towerbrick to soft clear!");
		}
	}
	return 1;
}

function spawnGuard(%tower) {
	%tower = ("Tower" @ %tower);
	if (!isObject(%tower)) {
		warn("Cannot spawn guard - no guard at tower " @ %tower @ "!");
		return 0;
	}

	if (isObject(%pl = %tower.guard.player)) {
		%pl.delete();
	}
	%tower.guard.createPlayer(%tower.spawn.getTransform());
	messageAll('', "\c3" @ %tower.guard.name @ "\c4 has been spawned at \c5Tower " @ %tower.towerNum + 1);

	giveGuardItems(%tower.guard.player);

	return 1;
}

function spawnAllGuards() {
	for (%i = 0; %i < $CPB::GUARDCOUNT; %i++) {
		spawnGuard(%i);
	}
}