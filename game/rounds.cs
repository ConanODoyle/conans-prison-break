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
//	endRound
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
	// spawnDog();

	schedule(500, 0, spawnHelicopter, _HelicopterCenter.getPosition());
}

function _setPhaseLOBBY() {
	resetSavedBrickData();
	cancelAllRoundSchedules();
	despawnAll();

	//show logo, reset guards selected
	// displayLogo(_LobbyLogoCam.getPosition(), _LobbyLogoCamTarget.getPosition(), LogoClosedShape, 1);
	displayNewLogo(_newLogoPos.getPosition());

	resetTowerSelectionBricks();

	_escapeDoor1.door(4);
	_escapeDoor2.door(4);

	if (!isObject($DefaultMinigame)) {
		createDefaultMinigame();
	}

	for (%i = 0; %i < ClientGroup.getCount(); %i++){
		%cl = ClientGroup.getObject(%i);
		// %cl.clearStatistics();
		%cl.clearVariables();
		commandToClient(%cl, 'showBricks', 0);

		%cl.isPrisoner = 0;
		%cl.isGuard = 0;
		%cl.tower = 0;
		%cl.isSelectedToBeGuard = 0;
		%cl.guardClass = "";
		%cl.guardEquipment = "";

		%cl.lastColor = "";
		%cl.playedAlarmSound = "";

		if (isObject(%cl.minigame)) {
			if (getRandom() < 0.5) {
				messageClient(%cl, '', "\c6You have recieved a random hairdo loot drop!");
				%cl.giveRandomHair();
			}
			%cl.isPrisoner = 1;
		}

		if (isObject(%cl.pickedTowerBrick)) {
			%cl.pickedTowerBrick.clearTower(%cl);
		}
	}
	resetAllTowerData();
	spawnAllLobby();
	bottomPrintInfoAll();

	//TODO: make this use "map type"
	if ($currMap $= "") {
		messageAdmins("!!! \c6No map is currently set!");
		return;
	}
		
	serverDirectSaveFileLoad("saves/autosaver/" @ $currMap @ ".bls", 3, "", 0, 1);

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
		resetTowerSelectionBricks();
	} else {
		messageAdmins("!!! - Cannot start game, guard selection not valid!");
	}
}

function _setPhaseGWIN() {
	cancelAllRoundSchedules();
	stopDataCollection();
	
	$CPB::CurrRoundTime++;
	bottomPrintInfoAll();
	$CPB::LastRoundWinners = "Guards";

	schedule(5000, 0, setPhase, "LOBBY");
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if ((%cl = ClientGroup.getObject(%i)).isGuard) {
			%cl.giveRandomHair();
			%cl.giveRandomHair();
			messageAll('', "\c3" @ %cl.name @ "\c6 won two hairdos for winning the round as a guard!");
		}
	}
}

function _setPhasePWIN() {
	cancelAllRoundSchedules();
	stopDataCollection();

	$CPB::CurrRoundTime++;
	bottomPrintInfoAll();
	$CPB::LastRoundWinners = "Prisoners";

	schedule(5000, 0, setPhase, "LOBBY");
}

function endRound() {
	checkWinConditions();
}

function doIntro() {
	//show logo open
	displayLogo(_IntroCam1.getPosition(), _IntroCam1Target.getPosition(), LogoClosedShape, 1);
	whiteOutAll(0.6);

	setAllCamerasView(_IntroCam1.getPosition(), _IntroCam1Target.getPosition(), 1, 50);

	schedule(500, 0, _doIntro2);
}

function _doIntro2() {
	displayLogo(_IntroCam1.getPosition(), _IntroCam1Target.getPosition(), LogoOpenShape, 1);
	$LogoShape.setScale("2.2 2.2 2.2");
	$LogoShape.schedule(100, setScale, "2 2 2");

	serverPlay3d(Beep_Siren_Sound, $LogoDish.getPosition());
	serverPlay3d(BrickBreakSound, $LogoDish.getPosition());
	schedule(50, 0, serverPlay3d, BrickBreakSound, $LogoDish.getPosition());
	schedule(100, 0, serverPlay3d, BrickBreakSound, $LogoDish.getPosition());

	schedule(5000, 0, _doIntro3);
}

function _doIntro3() {
	$LogoShape.delete();
	$LogoDish.delete();

	setAllCamerasView(_IntroCam2.getPosition(), _IntroCam2Target.getPosition(), 1, 60);

	schedule(5000, 0, resetAllClientsFOV);
	schedule(5000, 0, setPhase, "GAME");
}

// 		$Server::PrisonEscape::roundPhase = 1;
// 		for (%i = 0; %i < ClientGroup.getCount(); %i++)
// 		{
// 			%t = ClientGroup.getObject(%i);
// 			if (%t.isCustomizing) {
// 				stopPlayerHairCustomization(%t.player);
// 			}
// 		}
// 		spawnKillGround();
// 		spawnGenRoomKill();
// 		despawnAll();

// 		setAllCamerasView($Server::PrisonEscape::LoadingCamBrick.getPosition(), $Server::PrisonEscape::LoadingCamBrickTarget.getPosition(), 50);
// 		$nextRoundPhaseSchedule = schedule(100, 0, serverCmdSetPhase, %cl, 11);
// 	}
// 	else if (%phase == 11) //start the round caminations and spawn everyone but dont give them control of their bodies yet
// 	{
// 		displayLogo($Server::PrisonEscape::LoadingCamBrick.getPosition(), $Server::PrisonEscape::LoadingCamBrickTarget.getPosition(), LogoOpenShape, 0);
// 		$LogoShape.setScale("2.2 2.2 2.2");
// 		$LogoShape.schedule(100, setScale, "2 2 2");
// 		serverPlay3d(Beep_Siren_Sound, $LogoDish.getPosition());
// 		serverPlay3d(BrickBreakSound, $LogoDish.getPosition());
// 		schedule(50, 0, serverPlay3d, BrickBreakSound, $LogoDish.getPosition());
// 		schedule(100, 0, serverPlay3d, BrickBreakSound, $LogoDish.getPosition());

// 		cancel($Server::PrisonEscape::statisticLoop);
// 		clearStatistics();
// 		createLocationsLookupTable();
// 		clearCenterprintAll();
// 		clearBottomprintAll();

// 		schedule(2000, 0, displayIntroCenterprint);
// 		$nextRoundPhaseSchedule = schedule(5000, 0, serverCmdSetPhase, $fakeClient, 15);

// $Server::PrisonEscape::roundPhase = 2;
// 		for (%i = 0; %i < ClientGroup.getCount(); %i++) {
// 			%cl = ClientGroup.getObject(%i);
// 			%cl.camera.setWhiteOut(0.5);
// 		}
// 		setRiotMusic(3);
// 		if (!isObject($Server::PrisonEscape::commDish) || !isObject($Server::PrisonEscape::generator) || $Server::PrisonEscape::PrisonerSpawnPoints.getCount() <= 0) {
// 			PPE_messageAdmins("!!! \c5Cannot start round: Bricks missing!");
// 			return;	
// 		} else if (!$Server::PrisonEscape::haveAssignedBricks) {
// 			PPE_messageAdmins("!!! \c5Cannot start round: bricks have not been assigned!");
// 			return;
// 		}
// 		//reset statistics here because alivetime matters

// 		//assign guards
// 		for (%i = 0; %i < 4; %i++)
// 		{
// 			%guardClient = getWord($Server::PrisonEscape::Guards, %i);
// 			assignGuard(%guardClient);
// 		}
// 		//spawn guards
// 		spawnGuards();
// 		spawnEmittersLoop(0);

// 		//spawn prisoners through timer start code.

// 		//play music
// 		//camera on prison
// 		displayIntroCenterprint();
// 		setAllCamerasView($Server::PrisonEscape::PrisonPreview.getPosition(), $Server::PrisonEscape::PrisonPreviewTarget.getPosition(), 50);

// 		//camera for each guard - give guards control of their body here

// 		//autocall phase 2
// 		//call through the caminations, when they're done
// 		$nextRoundPhaseSchedule = schedule(10000, $fakeClient, serverCmdSetPhase, $fakeClient, 2);

// 		startSpotlights();