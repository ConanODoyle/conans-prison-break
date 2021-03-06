$CPB::RoundTimerSchedule = $CPB::RoundTimerSchedule;
$CPB::RoundTime = 16 * 60;
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
		return;
	}
	$CPB::RoundTimerSchedule = schedule(1000, 0, roundTimer);
}
