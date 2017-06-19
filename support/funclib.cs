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

function removeWord(%str, %word) {
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