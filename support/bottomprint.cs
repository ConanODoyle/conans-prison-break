$CPB::EWSActive = 1;

//Functions:
//Created:
//	GameConnection::bottomPrintInfo
//	bottomPrintInfoAll


// "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %pl.heatColor @ %pl.LMGHeat @ "/" @ $LMGMaxHeat

function GameConnection::bottomPrintInfo(%cl) {
	%time = $CPB::CurrRoundTime;
	%timeString = getTimeString(%time);
	if (%cl.isGuard && %cl.tower.guardOption == $CPB::Classes::LMG) {
		%pl = %cl.player;
		if (isObject(%pl)) {
			if (%pl.LMGHeat <= $LMGMaxHeat / 3) {
				%pl.heatColor = "\c6";
			} else if (%pl.LMGHeat <= $LMGMaxHeat / 4 * 3) {
				%pl.heatColor = "\c3";
			} else {
				%pl.heatColor = "\c0";
			}
		} else {
			%pl.heatColor = "\c0";
		}
		%timeString = %timeString @  "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %pl.heatColor @ (%pl.LMGHeat + 0) @ "/" @ $LMGMaxHeat;
	}

	if ($CPB::PHASE == $CPB::GAME) {
		if (%cl.isPrisoner) {
			if (%cl.isDead) {
				%timeToRespawn = getNextRespawnTime();
				%info = "\c0Next Respawn Wave In " @ getTimeString(%timeToRespawn) @ " ";
			} else if (%cl.isAlive) {
				%loc = %cl.getLocation();
				%locCount = $Live_PC[%loc];
				%totalP = $Live_PAlive;
				
				%info = "\c6[\c1" @ %loc @ "\c6: " @ %loc @ "/" @ %totalP @ "] ";
			}
		} else if (%cl.isGuard) {
			if ($CPB::EWSActive) {
				%color = "\c6";
				%info = %color @ getNumPlayersOutside() SPC "Prisoners Outside ";
			} else {
				%info = "Satellite Dish Inactive";
			}
		}
		
		%cl.bottomprint("<font:Arial Bold:34>\c6" @ %timeString @ " <br><font:Arial Bold:34>" @ %info, 500, 0);
	} else if ($CPB::PHASE == $CPB::GWIN || $CPB::PHASE == $CPB::PWIN) { 
		if ($CPB::PHASE == $CPB::PWIN) {
			%color = "<color:" @ $PRISONER::CHATCOLOR @ ">";
			%info = %color @ "Prisoners Win!";
		} else if ($CPB::PHASE == $CPB::GWIN) {
			%color = "<color:" @ $GUARD::CHATCOLOR @ ">";
			%info = %color @ "Guards Win!";
		}
		%cl.bottomprint("<font:Arial Bold:34>\c6" @ %timeString @ " <br><font:Arial Bold:34>" @ %info, 500, 0);
	} else {
		%cl.bottomprint("CPB please wait for next round to start", -1, 0);
	}
}

function bottomPrintInfoAll() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		ClientGroup.getObject(%i).bottomPrintInfo();
	}
}
	
	
	