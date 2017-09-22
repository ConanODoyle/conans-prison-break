datablock StaticShapeData(LogoClosedShape)
{
	shapeFile = "./shapes/logo/LogoClosed.dts";
};

datablock StaticShapeData(LogoOpenShape)
{
	shapeFile = "./shapes/logo/LogoOpen.dts";
};

datablock StaticShapeData(NewLogoShape)
{
	shapeFile = "./shapes/logo/newLogo.dts";
};

datablock StaticShapeData(LogoDishShape)
{
	shapeFile = "./shapes/logo/LogoDish.dts";
};

//Functions:
//Created:
//	displayLogo
//  applyLogoColors
//  doLogoFadeIn
//  doLogoFadeOut
//  clearLogo

function displayNewLogo(%pos) {
	if (isObject($LogoShape)) {
		$LogoShape.delete();
	}

	$LogoShape = new StaticShape(Logo) {
		datablock = NewLogoShape;
		scale = "2.0 2.0 2.0";
	};

	$LogoShape.setTransform(%pos);
	$LogoShape.playThread(0, rotate);
	applyLogoColors($LogoShape, 1);
}

function displayLogo(%camPos, %targetPos, %logo, %bg) {
	if (!isObject(%logo)) {
		return;
	}

	if (isObject($LogoShape)) {
		$LogoShape.delete();
	}
	
	if (isObject($LogoDish) && %bg) {
		$LogoDish.delete();
	}

	%pos = %targetPos;
	%delta = vectorSub(%camPos, %pos);
	%deltaX = getWord(%delta, 0);
	%deltaY = getWord(%delta, 1);
	%deltaZ = getWord(%delta, 2);
	%deltaXYHyp = vectorLen(%deltaX SPC %deltaY SPC 0);

	%rotZ = mAtan(%deltaX, %deltaY) * -1; 
	%rotX = mAtan(%deltaZ, %deltaXYHyp) - $tilt;

	%aa = eulerRadToMatrix(%rotX SPC 0 SPC %rotZ); //this function should be called eulerToAngleAxis...
	%camTransform = %pos SPC %aa;
	%dishTransform = vectorSub(%pos, vectorScale(vectorNormalize(%delta), 1.5)) SPC %aa;

	$LogoShape = new StaticShape(Logo) {
		datablock = %logo;
		scale = "2.0 2.0 2.0";
	};
	if (%bg) {
		if (isObject($LogoDish)) {
			$LogoDish.delete();
		}
		$LogoDish = new StaticShape(Logo) {
			datablock = LogoDishShape;
			scale = "0.7 0.7 0.7";
		};
		MissionCleanup.add($LogoDish);
		$LogoDish.startFade(0, 0, 1);
		$LogoDish.setTransform(%dishTransform);
		$LogoDish.setNodeColor("main", "1 0.92 0 1");
		$LogoDish.setNodeColor("Accent", "0.95 0.86 0 1");
		$LogoDish.playThread(0, rotate);
	}
	MissionCleanup.add($LogoShape);

	$LogoShape.startFade(0, 0, 1);
	$LogoShape.setTransform(%camTransform);
	applyLogoColors($LogoShape, 1);
}

function applyLogoColors(%item, %alpha, %isNew) {
	if (%alpha <= 0 || !isObject(%item)) {
		%item.delete();
		return;
	}

	%item.startFade(0, 0, 1);

	if (%item.getDatablock().getName() !$= "NewLogoShape" && !%isNew) {
		%item.setNodeColor("outline", "0 0 0 " @ %alpha);
		%item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
		%item.setNodeColor("skin", "0.9 0.712 0.456 " @ %alpha);
		%item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
		%item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);
	} else {
		%skinColor = "0.9 0.712 0.456 " @ %alpha;
		%item.setNodeColor("cHeadSkin", %skinColor);
		%item.setNodeColor("cChest", $GuardColor);
		%item.setNodeColor("cLArm", $GuardColor);
		%item.setNodeColor("cRArm", $GuardColor);
		%item.setNodeColor("cPack", "0.2 0.2 0.2 1");
		%item.setNodeColor("clHand", %skinColor);
		%item.setNodeColor("cRHand", %skinColor);
		%item.setNodeColor("cPants", "0.1 0.1 0.1 1");
		%item.setNodeColor("cCopHat", $GuardColor);
		%item.setNodeColor("pHeadSkin", %skinColor);
		%item.setNodeColor("pLHand", %skinColor);
		%item.setNodeColor("pRHand", %skinColor);
		%item.setNodeColor("pPants", $PrisonerColor);
		%item.setNodeColor("pChest", $PrisonerColor);
		%item.setNodeColor("pChestStripes", "0 0 0 " @ %alpha);
		%item.setNodeColor("pLArmSlim", $PrisonerColor);
		%item.setNodeColor("pRArmSlim", $PrisonerColor);
		%item.setNodeColor("Cylinder", "0.3 0.3 0.3 1");
		%item.setNodeColor("sniperBolt", "0.3 0.3 0.3 1");
		%item.setNodeColor("sniperStock", "0.12 0.12 0.12 1");
		%item.setNodeColor("sniperBarrel", "0.4 0.4 0.4 1");
		%item.setNodeColor("sniperScope", "0 0 0 1");
	}
}

function doLogoFadeOut(%item, %alpha) {
	if (%alpha <= 0 || !isObject(%item)) {
		%item.delete();
		return;
	}

	%item.setNodeColor("outline", "0 0 0 " @ %alpha);
	%item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
	%item.setNodeColor("skin", "0.9 0.712 0.456 " @ %alpha);
	%item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
	%item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);

	schedule(10, %item, doLogoFadeOut, %item, %alpha-0.01);
}

function doLogoFadeIn(%item, %alpha) {
	if (%alpha >= 1 || !isObject(%item)) {
		return;
	}
	%item.unhideNode("ALL");

	%item.setNodeColor("outline", "0 0 0 " @ %alpha);
	%item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
	%item.setNodeColor("skin", "0.96 0.762 0.486 " @ %alpha);
	%item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
	%item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);

	schedule(10, %item, doLogoFadeIn, %item, %alpha + 0.01);
}

function clearLogo() {
	if (isObject($LogoShape)) {
		$LogoShape.delete();
	}
	
	if (isObject($LogoDish)) {
		$LogoDish.delete();
	}
}