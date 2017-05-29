//Functions:
//Created:
//	GameConnection::bottomPrintInfo
//	bottomPrintInfoAll

function GameConnection::bottomPrintInfo(%cl) {
  %time = $CPB::CurrRoundTime;
  %timeString = getTimeString(%time);
  if ($CPB::PHASE == $CPB::GAME || $CPB::PHASE == $CPB::GWIN || $CPB::PHASE == $CPB::PWIN) {
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
        %info = "\c6" @ getNumPlayersOutside() SPC "Prisoners Outside ";
      } else {
        %info = "Satellite Dish Inactive";
      }
    }
    
    %cl.bottomprint("<font:Arial Bold:24>\c6" @ %timeString @ " <br>" @ %info, 500, 0);
  } else {
    %cl.bottomprint("CPB please wait for next round to start", -1, 0);
  }
}

function bottomPrintInfoAll() {
  for (%i = 0; %i < ClientGroup.getCount(); %i++) {
    ClientGroup.getObject(%i).bottomPrintInfo();
  }
}
  
  
  