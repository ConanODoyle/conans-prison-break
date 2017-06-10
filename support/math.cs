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