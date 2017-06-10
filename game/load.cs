if (!isObject(PrisonerSpawnPoints)) {
	new SimSet(PrisonerSpawnPoints) {};
}

if (!isObject(InfoPopups)) {
	new SimSet(InfoPopups) {};
}

if (!isObject(SecurityCameras)) {
	new SimSet(SecurityCameras) {};
}

if (!isObject(LobbySpawnPoints)) {
	new SimSet(LobbySpawnPoints) {};
}

function resetSavedBrickData() {
	for (%i = 0; %i < 4; %i++) {
		resetTowerData(%i);
	}

	PrisonerSpawnPoints.clear();
	InfoPopups.clear();
	SecurityCameras.clear();
	LobbySpawnPoints.clear();
}