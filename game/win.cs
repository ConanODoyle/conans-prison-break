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
		if (!("Tower" @ %i).guard.isDead) {
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
	setPhase("PWIN");
}