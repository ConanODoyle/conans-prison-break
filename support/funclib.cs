function isInteger(%val) {
	return (stripChars(%val, "0123456789-") $= "" ? true : false);
}

function getIntegerList(%min, %max) {
	if (%max < %min) {
		warn("Cannot generate integer list between " @ %min @ " and " @ %max);
		return;
	}

	for (%i = %min; %i <= %max; %i++) {
		%ret = %ret SPC %i;
	}
	return trim(%ret);
}

function containsWord(%str, %word) {
	if (getWordCount(%word) > 1) {
		warn("Cannot look for multi-word phrase in string! (" @ %word @ ")");
		return 0;
	}

	for (%i = 0; %i < getWordCount(%str); %i++) {
		if (getWord(%str, %i) $= %word) {
			return 1;
		}
	}
	return 0;
}

function removeWordString(%str, %word) {
	if (getWordCount(%word) > 1) {
		warn("Cannot remove multi-word phrase in string! (" @ %word @ ")");
		return %str;
	}

	%ret = "";
	for (%i = 0; %i < getWordCount(%str); %i++) {
		if ((%w = getWord(%str, %i)) !$= %word) {
			%ret = trim(%ret SPC %w);
		}
	}
	return %ret;
}

function getRandomVector() {
	%angle = getRandom(0, 314159 * 2) / 10000;
	%z = (getRandom() - 0.5) * 2;
	%s = mSqrt(1 - (%z * %z));
	%x = %s * mCos(%angle);
	%y = %s * mSin(%angle);
	return %x SPC %y SPC %z;
}

function WeaponImage::onLoadCheck(%this,%obj,%slot) {
	if(%obj.toolAmmo[%obj.currTool] <= 0 && %this.item.maxAmmo > 0 && %obj.getState() !$= "Dead") {
		%obj.setImageAmmo(%slot,0);
	} else {
		%obj.setImageAmmo(%slot,1);
	}
}

package CPB_Support_FuncLib {
	function servercmdLight(%client) {
		if(isObject(%client.player) && isObject(%client.player.getMountedImage(0))) {
			%p = %client.player;
			%im = %p.getMountedImage(0);
			if(%im.item.maxAmmo > 0 && %im.item.canReload == 1 && %p.toolAmmo[%p.currTool] < %im.item.maxAmmo){
				if(%p.getImageState(0) $= "Ready") {
					%p.setImageAmmo(0,0);
				}
				return;
			}
		}
		
		Parent::servercmdLight(%client);
	}
};
activatePackage(CPB_Support_FuncLib);