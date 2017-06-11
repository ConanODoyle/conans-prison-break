if (!isObject(PrisonerSpawnPoints)) {
	new SimSet(PrisonerSpawnPoints) {};
}

if (!isObject(InfirmarySpawnPoints)) {
	new SimSet(InfirmarySpawnPoints) {};
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

$CPB::CollectBricksSchedule = 0;

//Object properties:
//fxDTSBrick
//	tower (tower object);

//Functions:
//Created:
//	resetSavedBrickData
//	collectAllPrisonBricks


package CPB_Game_Load {
	function ServerLoadSaveFile_End() {
		parent::ServerLoadSaveFile_End();

		collectAllPrisonBricks($LoadingBricks_BrickGroup, 0);
	}
};
activatePackage(CPB_Game_Load);

function resetSavedBrickData() {
	for (%i = 0; %i < 4; %i++) {
		resetTowerData(%i);
	}

	PrisonerSpawnPoints.clear();
	InfoPopups.clear();
	SecurityCameras.clear();
	LobbySpawnPoints.clear();
}

function collectAllPrisonBricks(%bg, %i) {
	if (%i == 0) { 
		resetSavedBrickData();
	}

	if (%i >= %bg.getCount()) {
		messageAdmins("<font:Palatino Linotype:18>!!! \c5Tower Bricks saved to Towers from " @ %bg);
		messageAdmins("<font:Palatino Linotype:18>!!! \c5--T1 bc: " @ Tower0.getCount());
		messageAdmins("<font:Palatino Linotype:18>!!! \c5--T2 bc: " @ Tower1.getCount());
		messageAdmins("<font:Palatino Linotype:18>!!! \c5--T3 bc: " @ Tower2.getCount());
		messageAdmins("<font:Palatino Linotype:18>!!! \c5--T4 bc: " @ Tower3.getCount());
		messageAdmins("<font:Palatino Linotype:18>!!! \c5# Prisoner Spawns: " @ PrisonerSpawnPoints.getcount());
		messageAdmins("<font:Palatino Linotype:18>!!! \c5# Infirmary Spawns: " @ InfirmarySpawnPoints.getcount());

		Tower0.origBrickCount = Tower0.getCount();
		Tower1.origBrickCount = Tower1.getCount();
		Tower2.origBrickCount = Tower2.getCount();
		Tower3.origBrickCount = Tower3.getCount();
		$CPB::hasCollectedBricks = 1;
		return;
	}

	%b = %bg.getObject(%i);
	%db = %b.getDatablock().getName();
	%name = getSubStr(%b.getName(), 1, strLen(%b.getName()));
	%b.damage = 0;

	if (%b.getDatablock().isOpen) { //close open doors
		%b.door(4);
	}

	if (%name !$= "") {
		%type = getSubStr(%name, 0, 3);

		if (%type $= "pro") {	//allows bricks to have property setting names without special func
			%index = 3;
			while ((%c = getSubStr(%name, %index, 1)) !$= "") {
				if (isInteger(%c)) {
					if (%last $= "r") {
						%b.setRendering(%c);
					} else if (%last $= "c") {
						%b.setColliding(%c);
					} else if (%last $= "y") {
						%b.setRaycasting(%c);
					}
				}

				%last = %c;
				%index++;
			}
		} else if (strPos(strLwr(%db), "camera") >= 0) {
			SecurityCameras.add(%b);
		} else if (strPos(strLwr(%name), "info") >= 0) {
			InfoPopups.add(%b);
		} else if (isFunction(%func = "collect" @ %name @ "Brick")) {
			eval(%func @ "(" @ %b @ ")");
		}
	}

	$CPB::CollectBricksSchedule = schedule(0, 0, collectPrisonBricks, %bg, %i + 1);
}

function collectTowerBrick(%b, %n) {
	%tower = "Tower" @ %n;

	%tower.add(%b);
	%b.tower = %tower;

	%name = strLwr(%b.getName);
	if (strPos(%name, "spawn") >= 0) {
		%tower.spawn = %b;
	} else if (strPos(%name, "support") >= 0) {
		%tower.supportCount++;
		%b.isSupport = 1;
	} else if (strPos(%name, "info") >= 0) {
		InfoPopups.add(%b);
	} else if (isObject(%b.vehicle)) {
		%tower.spotlightBot = %b.vehicle;
		%b.vehicle.setShapeName("Tower " @ %n + 1, "8564862");
		%b.vehicle.setShapeNameDistance(500);
	}
}

function collectTower0Brick(%b) { collectTowerBrick(%b, 0) }
function collectTower1Brick(%b) { collectTowerBrick(%b, 1) }
function collectTower2Brick(%b) { collectTowerBrick(%b, 2) }
function collectTower3Brick(%b) { collectTowerBrick(%b, 3) }

function collectIndicatorBrick(%b) { %b.setColor(4); } //generator indicator to green
function collectBronsonDoorBrick(%b) { %b.setRaycasting(1); } //reset door to raycasting
function collectGeneratorDoorsBrick(%b) { %b.setRaycasting(1); } //generator door open from inside
function collectGeneratorDoorBrick(%b) { //lock generator doors
	%b.setEventEnabled("2", 0);
	%b.setEventEnabled("0 1", 1);
}

function collectGarageDoorBrick(%b) { %b.door(4); } //close garage doors
function collectGarageDoorSwitchBrick(%b) { //reset garage doors buttons
	%b.setEventEnabled("2 5 6", 0);
	%b.setEventEnabled("1 3 4", 1);
	%b.setColor(4);
}

function collectSmokeGrenadeDoorBrick(%b) { %b.door(4); }
function collectWinBrickBrick(%b) { %b.setColliding(1); }

function collectPrisonerSpawnBrick(%b) { PrisonerSpawnPoints.add(%b); }
function collectInfirmarySpawnBrick(%b) { InfirmarySpawnPoints.add(%b); }
function collectLobbySpawnBrick(%b) { LobbySpawnPoints.add(%b); }