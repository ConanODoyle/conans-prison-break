$CPB::OFF	= -1;
$CPB::GAME	= 0;
$CPB::LOBBY	= 1;
$CPB::INTRO	= 2;
$CPB::PWIN	= 3;
$CPB::GWIN	= 4;


//Functions:
//Created:
//	setPhase
//	setPhaseGAME

function setPhase(%phase) {
  if ($CPB["::" @ %phase] $= "") {
    messageAdmins("!!! \c6- Cannot set phase to " @ %phase);
  }
  
  $CPB::PHASE = $CPB["::" @ %phase];
  eval("setPhase" @ %phase);
}