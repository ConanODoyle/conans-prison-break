$CPB::EWSActive = 1;
$CPB::EWSAlertThreshold = 6;

//Object properties:
//GameConnection
//	lastColor
//	playedAlarmSound

//Functions:
//Created:
//	GameConnection::bottomPrintInfo
//	bottomPrintInfoAll
//	serverCmdToggleBottomprintBar


function GameConnection::bottomPrintInfo(%cl) {
	%time = $CPB::CurrRoundTime;
	%timeString = "<font:Arial Bold:34>\c6" @ getTimeString(%time);

	if ($CPB::PHASE == $CPB::GAME) {
		if (%cl.isPrisoner) {
			if (%cl.isDead) {
				%timeToRespawn = getNextRespawnTime();
				%info = "<font:Arial Bold:28>\c0Next Respawn Wave In " @ getTimeString(%timeToRespawn) @ " ";
			} else if (%cl.isAlive) {
				%loc = getLocation(%cl.player);
				%locCount = $Live_PC[%loc];
				if (%locCount == 0) {
					%locCount = 1;
				}
				%totalP = $Live_PAlive;
				
				%info = "<font:Arial Bold:28>\c6[\c1" @ %loc @ "\c6: " @ (%locCount + 0) @ "/" @ %totalP @ "] <br>";
				if (isObject(%cl.player) && %cl.player.getDatablock().getID() == BuffArmor.getID()) {
					if (%cl.player.getDamagePercent() > 0.5) {
						%damageColor = "\c0";
					} else {
						%damageColor = "\c6";
					}
					%info = %info @ %damageColor @ "HP: " @ %cl.player.getDatablock().maxDamage - %cl.player.getDamageLevel() @ " ";
				}
			}
		} else if (%cl.isGuard) {
			if ($CPB::EWSActive) {
				cancel(%cl.EWSAlertBottomprintSched);

				%color = "<font:Consolas:24>\c6";
				%prisonersOutside = getNumPrisonersOutside();

				if (%prisonersOutside > $CPB::EWSAlertThreshold) {

					if (!%cl.playedAlarmSound) {
						%cl.play3D(AlarmSound, %cl.player.getPosition());
						%cl.play3D(brickPlantSound, %cl.player.getPosition());
						%cl.play3D(brickRotateSound, %cl.player.getPosition());
						%cl.play3D(brickMoveSound, %cl.player.getPosition());
					}
					%cl.playedAlarmSound = 1;

					if (%cl.lastColor $= "") {
						%cl.lastColor = "\c6";
					}

					if (%cl.lastColor $= "\c6") {
						%color = "<font:Consolas:24>\c0";
						%cl.lastColor = "\c0";
					} else {
						%color = "<font:Consolas:24>\c6";
						%cl.lastColor = "\c6";
					}
					%cl.EWSAlertBottomprintSched = %cl.schedule(500, bottomPrintInfo);
				} else {
					if (%cl.playedAlarmSound) {
						%cl.play3D(Beep_Popup_Sound, %cl.player.getPosition());
						%cl.play3D(brickPlantSound, %cl.player.getPosition());
						%cl.play3D(brickRotateSound, %cl.player.getPosition());
						%cl.play3D(brickMoveSound, %cl.player.getPosition());
					}
					%cl.playedAlarmSound = 0;
				}

				%info = %color @ "[" @ %prisonersOutside SPC "Prisoners Outside] ";
			} else {
				if (%cl.playedAlarmSound) {
					%cl.play3D(Beep_Popup_Sound, %cl.player.getPosition());
					%cl.play3D(brickPlantSound, %cl.player.getPosition());
					%cl.play3D(brickRotateSound, %cl.player.getPosition());
					%cl.play3D(brickMoveSound, %cl.player.getPosition());
				}
				%cl.playedAlarmSound = 0;
				%info = "<font:Arial Bold:34>Satellite Dish Inactive";
			}

			if (%cl.tower.guardEquipment == $CPB::Equipment::Shotgun) {
				%pl = %cl.player;
				%timeString = "<just:left><font:impact:24>\c312 Gauge: \c6" @ %pl.shotgunAmmo + 0 @ "/" @ PumpShotgunItem.maxAmmo @ "<just:center>" @ %timeString @ "              ";
			}

			if (%cl.tower.guardOption == $CPB::Classes::LMG) {
				%pl = %cl.player;
				if (isObject(%pl)) {
					if (%pl.LMGHeat <= $LMGMaxHeat / 3) {
						%pl.heatColor = "\c6";
					} else if (%pl.LMGHeat <= $LMGMaxHeat - 10) {
						%pl.heatColor = "\c3";
					} else {
						%pl.heatColor = "\c0";
					}
				} else {
					%pl.heatColor = "\c0";
				}
				%timeString = %timeString @  "<just:right><font:impact:24>\c3Heat: " @ %pl.heatColor @ (%pl.LMGHeat + 0) @ "/" @ $LMGMaxHeat;
			}
		}
		
		%timeString = "<just:center>" @ %timeString;
		%cl.bottomprint(%timeString @ " <br><just:center>" @ %info @ "<br>", 500, %cl.hideBottomprintBar);
	} else if ($CPB::PHASE == $CPB::GWIN || $CPB::PHASE == $CPB::PWIN) { 
		if ($CPB::PHASE == $CPB::PWIN) {
			%color = "<color:" @ $PRISONER::CHATCOLOR @ ">";
			%info = %color @ "<font:Arial Bold:26>Prisoners Win!";
		} else if ($CPB::PHASE == $CPB::GWIN) {
			%color = "<color:" @ $GUARD::CHATCOLOR @ ">";
			%info = %color @ "<font:Arial Bold:26>Guards Win!";
		}
		%cl.bottomprint("<just:center><font:Arial Bold:34>\c6" @ %timeString @ " <br><just:center>" @ %info, 500, 0);
	} else {
		%header = "<just:center><font:Courier New Bold:48><shadowcolor:000000><shadow:0:3><color:E65714>Conan's Prison Break <br><font:Arial Bold:30><shadow:0:0>\c7-------- <br>";
		%footer = "<shadow:0:3><color:ffffff>Please wait for the next round to start<font:Impact:1> <br>";
		%cl.bottomprint(%header @ %footer, -1, 1);
	}
}

function bottomPrintInfoAll() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		ClientGroup.getObject(%i).bottomPrintInfo();
	}
}

function serverCmdToggleBottomprintBar(%cl) {
	%cl.hideBottomprintBar = !%cl.hideBottomprintBar;
	if (%cl.hideBottomprintBar) {
		messageClient(%cl, '', "\c6You have turned the bottomprint bar \c0OFF");
	} else {
		messageClient(%cl, '', "\c6You have turned the bottomprint bar \c2ON");
	}
}