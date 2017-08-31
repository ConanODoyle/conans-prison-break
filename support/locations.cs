//Functions:
//Packaged:
//  roundTimer
//Created:
//  getLocation
//  GameConnection::isOutside
//  collectPlayerLocations
//  getNumPlayersOutside


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
	for (%i = 0; %i < $locationNum; %i++) {
		%pos0 = $location[%i @ "::" @ $currMap @ "::pos0"];
		%pos1 = $location[%i @ "::" @ $currMap @ "::pos1"];
		if (isPosInBounds(%pos, %pos0, %pos1)) {
			return $location[%i @ "::" @ $currMap @ "::name"];
		}
	}
	return "Outside";
}

function GameConnection::isOutside(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return 0;
	}

	%loc = getLocation(%pl);
	if (%loc $= "Outside" || %loc $= "Yard") {
		return 1;
	}
	return 0;
}

function collectPlayerLocations(%i) {
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
	schedule(1, 0, collectPlayerLocations, %i++);
}

function getNumPlayersOutside() {
	return $Live_PCOutside + $Live_PCYard;
}