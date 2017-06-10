datablock PlayerData(EmptyHoleBot : PlayerNoJet) {
	shapeFile = "base/data/shapes/empty.dts";

	boundingBox			= vectorScale("1.24 1.24 1", 4);
	crouchBoundingBox	= vectorScale("1.24 1.24 1", 4);

	uiname = "EmptyHoleBot";

	maxStepHeight = 0;
	slowdownMax = 80;
	jumpForce = 0;
};