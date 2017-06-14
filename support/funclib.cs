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
		return;
	}

	for (%i = 0; %i < getWordCount(%str); %i++) {
		if (getWord(%str, %i) $= %word) {
			return 1;
		}
	}
	return 0;
}