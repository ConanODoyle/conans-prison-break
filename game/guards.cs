//Functions:
//Created:
//	serverCmdSetGuard
//	serverCmdUnsetGuard
//	lockInGuards
//	spawnGuard
//	spawnAllGuards


function serverCmdUnsetGuard(%cl, %name) {
	if ($CPB::PHASE != $CPB::LOBBY || !%cl.isAdmin) {
		return;
	}

	fxDTSBrick::clearTower(0, %cl);
	%cl.isSelectedToBeGuard = 0;
	%cl.isGuard = 0;
	%cl.isPrisoner = 1;
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
	%cl.isPrisoner = 0;
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
			return;
		}
		%isTaken[%cl.tower.getName()] = 1;
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
	messageAll('', "\c3" @ %tower.guard.name @ "\c4 has been spawned at \c5Tower " @ %tower);
	return 1;
}

function spawnAllGuards() {
	for (%i = 0; %i < $CPB::GUARDCOUNT; %i++) {
		spawnGuard(%i);
	}
}