//Functions:
//Created:
//	checkWinConditions
//	checkPrisonerWinCondition
//	checkGuardWinCondition
//	fxDTSBrick::prisonersWin


function checkWinConditions() {
	if (checkPrisonerWinCondition()) {
		setPhase("PWIN");
		return;
	} else if (checkGuardWinCondition()) {
		setPhase("GWIN");
		return;
	} else {
		talk("Neither team won...");
		setPhase("LOBBY");
	}
}

function checkPrisonerWinCondition() {
	for (%i = 0; %i < $CPB::GUARDCOUNT; %i++) {
		if (!("Tower" @ %i).isDestroyed) {
			return 0;
		}
	}
	return 1;
}

function checkGuardWinCondition() {
	if ($CPB::CurrRoundTime < 0) {
		for (%i = 0; %i < $CPB::GUARDCOUNT; %i++) {
			if (("Tower" @ %i).guard.isDead) {
				%count++;
			}
		}
		
		if (%count < 0) {
			return 0;
		} else {
			return 1;
		}
	}
}


////////////////////


registerOutputEvent("fxDTSBrick", "prisonersWin", "", 1);

function fxDTSBrick::prisonersWin(%b, %cl) {
	if (%cl.isGuard || !%cl.isPrisoner || $CPB::Phase != $CPB::GAME || !isObject(%cl.minigame)) {
		return;
	}

	$CPB::firstPrisonerOut = %cl;
	%b.setColliding(0);

	%winString = "<font:Palatino Linotype:28>\c6The prisoners have escaped!";

	%id = %b.getAngleID();
	switch (%id) {
		case 1: %vec = "0 5 2";
		case 0: %vec = "-5 0 2";
		case 3: %vec = "0 -5 2";
		case 2: %vec = "5 0 2";
	}
	%start = %b.getPosition();
	%end = vectorAdd(%vec, %start);
	setAllCamerasView(%end, %start);
	setAllCameraControlNone();
	setAllCameraControlPlayers();

	setPhase("PWIN");
}