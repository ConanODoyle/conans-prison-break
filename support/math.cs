function isInteger(%val) {
	return (stripChars(%val, "0123456789-") $= "" ? true : false);
}