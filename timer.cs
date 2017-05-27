$CPB::RoundTimerSchedule = 0;
$CPB::RoundTime = 20 * 60;
$CPB::CurrRoundTime = -1;

//Functions:
//Created:
//	startRoundTimer
//	roundTimer

function startRoundTimer() {
  $CPB::CurrRoundTime = $CPB::RoundTime;
  roundTimer();
}

function roundTimer() { //package if need to do things per second
  cancel($CPB::RoundTimerSchedule);
  for (%i = 0; %i < ClientGroup.getCount(); %i++) {
    %cl = ClientGroup.getObject(%i);
    %cl.bottomPrintTimer();
  }
  $CPB::CurrRoundTime--;
  if ($CPB::CurrRoundTime < 0) {
    endRound();
  }
  $CPB::RoundTimerSchedule = schedule(1000, 0, roundTimer);
}