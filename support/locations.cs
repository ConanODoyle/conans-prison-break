//Functions:
//Packaged:
//  roundTimer
//Created:
//  getLocation
//	GameConnection::getLocation
//	Player::getLocation
//	AIPlayer::getLocation
//	SimObject::getLocation
//  GameConnection::isOutside
//  collectPlayerLocations
//  getNumPrisonersOutside


package CPB_Support_Locations {
	function roundTimer() {
		parent::roundTimer();
		collectPlayerLocations(%i);
	}
};
activatePackage(CPB_Support_Locations);


////////////////////


function getLocation(%obj) {
	if (!isObject(%obj)) {
		return "Dead";
	}

	%pos = %obj.getPosition();
	%map = $currMap;
	for (%i = 0; %i < $locationNum["::" @ $currMap]; %i++) {
		%pos0 = $location[%i @ "::" @ %map @ "::pos0"];
		%pos1 = $location[%i @ "::" @ %map @ "::pos1"];
		if (isPosInBounds(%pos, %pos0, %pos1)) {
			return $location[%i @ "::" @ %map @ "::name"];
		}
	}
	return "Outside";
}

function GameConnection::isOutside(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return 0;
	}

	%loc = getLocation(%pl);
	if (%loc $= "Outside" || %loc $= "Yard" || getSubStr(%loc, 0, 5) $= "Tower") {
		return 1;
	}
	return 0;
}

function collectPlayerLocations(%ct) {
	for (%i = 0; %i < 5; %i++) {
		%i = %i + %ct;
		if (%i >= ClientGroup.getCount()) {
			$RegionCollectingInProgress = 0;
			return;
		} else if ($RegionCollectingInProgress && %i == 0) {
			return;
		} else if (%i == 0) {
			$RegionCollectingInProgress = 1;
			deleteVariables("$Live*");
		}

		%cl = ClientGroup.getObject(%i);
		if (isObject(%pl = %cl.player)) {
			%loc = getLocation(%pl);
			if (%cl.isPrisoner) {
				$Live_PAlive++;
				$Live_PC[%loc]++;
			} else {
				$Live_GAlive++;
				$Live_GC[%loc]++;
			}
		}
		%i = %i - %ct;
	}
	schedule(1, 0, collectPlayerLocations, %ct + 5);
}

function getNumPrisonersOutside() {
	return $Live_PCOutside + $Live_PCYard;
}