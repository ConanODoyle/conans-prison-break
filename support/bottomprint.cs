$CPB::EWSActive = 1;
$CPB::EWSAlertThreshold = 6;

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
				%loc = %cl.getLocation();
				%locCount = $Live_PC[%loc];
				%totalP = $Live_PAlive;
				
				%info = "<font:Arial Bold:28>\c6[\c1" @ %loc @ "\c6: " @ %loc @ "/" @ %totalP @ "] ";
				if (%cl.player.getDatablock().getID() == BuffArmor.getID()) {
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
					if (!%lastColor) {
						%lastColor = "\c6";
					}

					if (%lastColor $= "\c6") {
						%color = "<font:Consolas:24>\c0";
						%lastColor = "\c0";
					} else {
						%color = "<font:Consolas:24>\c6";
						%lastColor = "\c6";
					}
					%cl.EWSAlertBottomprintSched = %cl.schedule(500, bottomPrintInfo);
				}

				%info = %color @ "[" @ %prisonersOutside SPC "Prisoners Outside] ";
			} else {
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
		%cl.bottomprint(%timeString @ " <br><just:center>" @ %info, 500, %cl.hideBottomprintBar);
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
		%cl.bottomprint("<just:center><font:Arial Bold:34>Conan's Prison Break <br><just:center>Please wait for next round to start", -1, 0);
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