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
  bottomPrintInfoAll();
  $CPB::CurrRoundTime--;
  if ($CPB::CurrRoundTime < 0) {
    endRound();
  }
  $CPB::RoundTimerSchedule = schedule(1000, 0, roundTimer);
}