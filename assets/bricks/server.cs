%error = ForceRequiredAddOn( "Support_Doors" );
if( %error == $Error::AddOn_NotFound )
{
	error("Brick Doors: Support_Doors is missing somehow, what did you do?");
}

datablock fxDTSBrickData(brickGarageDoorUpData)
{
	brickFile = "./garagedoor_up.blb";
	uiName = "Garage Door";
	//iconName = "Add-Ons/Brick_Security_Camera/Security Camera Right";

	isDoor = 1;
	isOpen = 1;

	closedCW = "brickGarageDoorData";
	openCW = "brickGarageDoorUpData";

	closedCCW = "brickGarageDoorData";
	openCCW = "brickGarageDoorHalfData";
};

datablock fxDTSBrickData(brickGarageDoorHalfData : brickGarageDoorUpData)
{
	brickFile = "./garagedoor_half.blb";
	
	isOpen = 1;
	//iconName = "Add-Ons/Brick_Security_Camera/Security Camera";
};

datablock fxDTSBrickData(brickGarageDoorData : brickGarageDoorUpData)
{
	brickFile = "./garagedoor.blb";
	
	isOpen = 0;

	category = "Special";
	subCategory = "Doors";
};

datablock fxDTSBrickData(brickSecurityCameraRightData)
{
	brickFile = "./rcam.blb";
	uiName = "Security Camera";
	//iconName = "Add-Ons/Brick_Security_Camera/Security Camera Right";

	isDoor = 1;
	isOpen = 1;

	closedCW = "brickSecurityCameraData";
	openCW = "brickSecurityCameraLeftData";

	closedCCW = "brickSecurityCameraData";
	openCCW = "brickSecurityCameraRightData";
};

datablock fxDTSBrickData(brickSecurityCameraLeftData : brickSecurityCameraRightData)
{
	brickFile = "./lcam.blb";
	
	isOpen = 1;
	//iconName = "Add-Ons/Brick_Security_Camera/Security Camera";
};


datablock fxDTSBrickData(brickSecurityCameraData : brickSecurityCameraRightData)
{
	brickFile = "./fcam.blb";
	
	isOpen = 0;

	category = "Special";
	subCategory = "Doors";
};
////////////////////

datablock fxDTSBrickData(brickTrayData)
{
	brickFile = "./tray.blb";
	category = "Special";
	subCategory = "Misc";
	uiName = "Trays";
};

datablock fxDTSBrickData(brickPhoneData)
{
	brickFile = "./phone.blb";
	category = "Special";
	subCategory = "Misc";
	uiName = "Phone";
	iconName = "";
};

datablock fxDTSBrickData(brickGeneratorData)
{
	brickFile = "./Generator.blb";
	category = "Special";
	subCategory = "Misc";
	uiName = "Generator";
	iconName = "";
	//orientationFix = 1;
	//collisionShapeName = "./generator.dts";
};

datablock fxDTSBrickData(brickSatDishData)
{
	brickFile = "./satdish.blb";
	category = "Special";
	subCategory = "Misc";
	uiName = "Satellite Dish";
	iconName = "";
	collisionShapeName = "./dishCOL.dts";
};