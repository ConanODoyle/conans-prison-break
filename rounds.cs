$CPB::OFF	= -1;
$CPB::GAME	= 0;
$CPB::LOBBY	= 1;
$CPB::INTRO	= 2;
$CPB::PWIN	= 3;
$CPB::GWIN	= 4;

//Functions:
//Created:
//	setPhase
//	_setPhaseGAME
//	_setPhaseGWIN
//	_setPhasePWIN

function setPhase(%phase) {
  if ($CPB["::" @ %phase] $= "") {
    messageAdmins("!!! \c6- Cannot set phase to " @ %phase);
  }
  
  $CPB::PHASE = $CPB["::" @ %phase];
  eval("_setPhase" @ %phase @ "();");
}

function _setPhaseGAME() {
  cancel($CPB::RoundTimerSchedule);
  startRoundTimer();
  createKillZones();
  priorityCenterprintAll("", 0, 11);
  startDataCollection($Data::GameNum);
}

function _setPhaseLOBBY() {
  cancel($CPB::RoundTimerSchedule);
}

function _setPhaseGWIN() {
  cancel($CPB::RoundTimerSchedule);
  stopDataCollection();
  bottomPrintInfoAll();
}

function _setPhasePWIN() {
  cancel($CPB::RoundTimerSchedule);
  stopDataCollection();
  bottomPrintInfoAll();
}