$CPB::OFF	= -1;	$CPB::PHASENAME["-1"] = "OFF";
$CPB::GAME	= 0;	$CPB::PHASENAME["0"] = "GAME";
$CPB::LOBBY	= 1;	$CPB::PHASENAME["1"] = "LOBBY";
$CPB::INTRO	= 2;	$CPB::PHASENAME["2"] = "INTRO";
$CPB::PWIN	= 3;	$CPB::PHASENAME["3"] = "PWIN";
$CPB::GWIN	= 4;	$CPB::PHASENAME["4"] = "GWIN";
	
$CPB::LastRoundWinners = ""; //"Prisoners" or "Guards"
$CPB::SelectedGuards = "";

//Functions:
//Created:
//	setPhase
//	resetPhase
//	cancelAllRoundSchedules
//	_setPhaseOFF
//	_setPhaseGAME
//	_setPhaseLOBBY
//	_setPhaseINTRO
//	_setPhaseGWIN
//	_setPhasePWIN
//	doIntro


function setPhase(%phase) {
	if ($CPB["::" @ %phase] $= "") {
		messageAdmins("!!! \c6- Cannot set phase to " @ %phase);
	}
	
	if ($CPB::PHASE != $CPB["::" @ %phase]) {
		$CPB::PHASE = $CPB["::" @ %phase];
		eval("_setPhase" @ %phase @ "();");
	}
}

function resetPhase(%phase) {
	if ($CPB["::" @ %phase] $= "") {
		messageAdmins("!!! \c6- Cannot set phase to " @ %phase);
	}
	
	$CPB::PHASE = $CPB["::" @ %phase];
	eval("_setPhase" @ %phase @ "();");
}

function cancelAllRoundSchedules() {
	cancel($CPB::RoundTimerSchedule);
	cancel($CPB::DataCollectionSchedule);
}

function _setPhaseOFF() {
	cancelAllRoundSchedules();
}

function _setPhaseGAME() {
	resetAllClientsFOV();
	cancelAllRoundSchedules();

	startRoundTimer();
	// createKillZones();

	centerprintAll("", 0);
	// startDataCollection($Data::GameNum);

	despawnAll();
	spawnAllGuards();
	spawnAllPrisoners();
}

function _setPhaseLOBBY() {
	cancelAllRoundSchedules();
	despawnAll();

	//show logo, reset guards selected
	displayLogo(_LobbyLogoCam.getPosition(), _LobbyLogoCamTarget.getPosition(), LogoClosedShape, 1);

	for (%i = 0; %i < ClientGroup.getCount(); %i++){
		%cl = ClientGroup.getObject(%i);
		// %cl.clearStatistics();
		%cl.clearVariables();
		commandToClient(%cl, 'showBricks', 0);

		%cl.isPrisoner = 0;
		%cl.isGuard = 0;
		%cl.tower = 0;

		if (isObject(%cl.minigame)) {
			if (getRandom() < 0.2) {
				messageClient(%cl, '', "\c6You have recieved a random hairdo loot drop!");
				%cl.giveRandomHair(1);
			}
			%cl.isPrisoner = 1;
		}
	}
	resetAllTowerData();
	spawnAllLobby();

	//TODO: make this use "map type"
	serverDirectSaveFileLoad("Add-ons/Gamemode_CPB/data/prison.bls", 3, "", 0, 1);

	$CPB::SelectedGuards = "";
	$CPB::GeneratorOpened = 0;
	$CPB::EWSActive = 1;

	displayStatistics();
}

function _setPhaseINTRO() {
	if (!$CPB::hasCollectedBricks) {
		messageAdmins("!!! \c6Cannot start intro - bricks not yet collected!");
		return;
	}

	cancelAllRoundSchedules();
	$Stats::GameNum++;

	clearCenterprintAll();
	clearBottomprintAll();
	clearAllDroppedItems();
	spawnGenRoomKill();
	spawnKillGround();

	if (validateGuardSelection()) {
		doIntro();
		$CPB::hasCollectedBricks = 0;
	}
}

function _setPhaseGWIN() {
	cancelAllRoundSchedules();
	stopDataCollection();
	
	$CPB::CurrRoundTime++;
	bottomPrintInfoAll();
	$CPB::LastRoundWinners = "Guards";

	schedule(5000, 0, setPhase, "LOBBY");
}

function _setPhasePWIN() {
	cancelAllRoundSchedules();
	stopDataCollection();

	$CPB::CurrRoundTime++;
	bottomPrintInfoAll();
	$CPB::LastRoundWinners = "Prisoners";

	schedule(5000, 0, setPhase, "LOBBY");
}

function doIntro() {
	setAllCamerasView(_IntroCam1.getPosition(), _IntroCam1Target.getPosition(), 1, 90);

	schedule(2000, 0, setAllCamerasView, _IntroCam2.getPosition(), _IntroCam2Target.getPosition(), 1, 90);
}

function endRound() {
	checkWinConditions();
}