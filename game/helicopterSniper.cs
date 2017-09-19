datablock StaticShapeData(HelicopterMountShape)
{
	shapeFile = "./shapes/helicopterShape/helicopterMount.dts";
};

datablock PlayerData(HelicopterArmor : PlayerStandardArmor) {
	shapeFile = "./shapes/helicopterShape/helicopterShape2.dts";

	boundingBox = vectorScale("100 100 100", 4);
	uiName = "";
};

datablock TSShapeConstructor(sniperRemotePlatformDTS) {
	baseShape  = "./shapes/helicopterShape/sniperRemotePlatform.dts";
	sequence0  = "./shapes/helicopterShape/sniperRemotePlatform.dsq";
};

datablock PlayerData(SniperPlatformArmor : PlayerStandardArmor) {
	shapeFile = "./shapes/helicopterShape/sniperRemotePlatform.dts";

	boundingBox = vectorScale("100 100 100", 4);
	uiName = "";
	firstPersonOnly = 1;
	// renderFirstPerson = 1;
};

datablock ItemData(SniperControlItem : HammerItem) {
	shapeFile = "./shapes/helicopterShape/SniperControl.dts";

	image = SniperControlImage;

	uiName = "Sniper Controller";

	doColorShift = false;
};

datablock ShapeBaseImageData(SniperControlImage) {
	shapeFile = "./shapes/helicopterShape/SniperControl.dts";
	emap = true;
	
	className = "WeaponImage";

	item = SniperControlItem;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.15;
	stateTransitionOnTimeout[0]		= "Ready";
	stateScript[0]					= "onActivate";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTransitionOnTriggerUp[2]	= "Ready";
};

$CPB::HelicopterCircleTime = 30; //in seconds

//Functions:
//Packaged:
//	serverCmdLight
//Created:
//	spawnHelicopter
//	rotateHelicopter
//	rotateHelicopter2
//	SniperControlImage::onActivate
//	SniperControlImage::onFire


package HelicopterSniper {
	function serverCmdLight(%cl) {
		if (isObject(%pl = %cl.player) && %pl.isUsingSniperPlatform) {
			%cl.setControlObject(%pl);
			%pl.isUsingSniperPlatform = 0;
			$CPB::HelicopterSniper1.client = "";
			return;
		}

		parent::serverCmdLight(%cl);
	}
}; 
activatePackage(HelicopterSniper);


////////////////////


function spawnHelicopter(%pos) {
	if (!isObject($CPB::HelicopterSpinShape)) {
		$CPB::HelicopterSpinShape = new StaticShape() {
			datablock = HelicopterMountShape;
		};
	}

	if (!isObject($CPB::HelicopterBot)) {
		$CPB::HelicopterBot = new AIPlayer() {
			datablock = HelicopterArmor;
		};
	}

	if (!isObject($CPB::HelicopterSniper1)) {
		$CPB::HelicopterSniper1 = new AIPlayer() {
			datablock = SniperPlatformArmor;
		};
	}

	// if (!isObject($CPB::HelicopterSniper2)) {
	// 	$CPB::HelicopterSniper2 = new AIPlayer() {
	// 		datablock = SniperPlatformArmor;
	// 	};
	// }

	if (%pos $= "") {
		%pos = _HelicopterCenter.getPosition();
	}

	$CPB::HelicopterSpinShape.setTransform(%pos);
	// $CPB::HelicopterSpinShape.playThread(0, circle);
	rotateHelicopter();

	$CPB::HelicopterSpinShape.hideNode("ALL");
	$CPB::HelicopterSpinShape.mountObject($CPB::HelicopterBot, 1);

	$CPB::HelicopterBot.mountObject($CPB::HelicopterSniper1, 3);
	// $CPB::HelicopterBot.mountObject($CPB::HelicopterSniper2, 4);

	$CPB::HelicopterSniper1.mountImage(SniperRifleHelicopterImage, 0);
	// $CPB::HelicopterSniper2.mountImage(SniperRifleHelicopterImage, 0);
}

function rotateHelicopter()
{
    cancel($rotateHelicopterSchedule);
    if (!isObject($CPB::HelicopterSpinShape)) {
    	messageAdmins("!!! \c6Cannot find HelicopterSpinShape to spin!");
    	return;
    }
    %i = $CPB::HelicopterSpinShape.rotVal;
    
    $CPB::HelicopterSpinShape.setTransform($CPB::HelicopterSpinShape.position SPC "0 0 1" SPC %i * $pi / 3600);
    
    //time: 360 / (0.05 * (1000/%dt))
    %dt = mCeil($CPB::HelicopterCircleTime / 360 * 0.05 * 1000);
    $CPB::HelicopterSpinShape.rotVal = (%i - 1 + 7200) % 7200;

    $rotateHelicopterSchedule = schedule(%dt, 0, rotateHelicopter);
}

// function rotateHelicopter2(%dt, %i)
// {
//     cancel($b);
//    	%vec = mCos(%i * $pi / 3600) SPC mSin(%i * $pi / 3600) SPC 1;
//    	%pos = vectorAdd($center, vectorScale(%vec, 30));
//     $test2.setTransform(%pos SPC "0 0 1" SPC %i * $pi / 3600);
//     //time: 360 / (0.05 * (1000/%dt))
//     $b = schedule(%dt, 0, b, %dt, (%i - 1 + 7200) % 7200);
// }

function SniperControlImage::onActivate(%this, %obj, %slot) {
	%obj.playThread(1, armReadyBoth);
	if (isObject(%cl = %obj.client) && !%obj.alertSniperControlImage) {
		messageClient(%cl, '', "<font:Arial Bold:24>\c3Use the controller to take control of the helicopter sniper. Press Light to exit the sniper control");
	}
	%obj.alertSniperControlImage = 1;
}

function SniperControlImage::onFire(%this, %obj, %slot) {
	%cl = %obj.client;

	if (!isObject(%cl)) {
		return;
	}

	%obj.isUsingSniperPlatform = 1;
	%cl.setControlObject($CPB::HelicopterSniper1);
	$CPB::HelicopterSniper1.client = %cl;
	messageClient(%cl, '', "\c2[Camera in Free Mode] \c6Press Light to exit");
}

function SniperControlImage::onUnmount(%this, %obj, %slot) {
	%cl = %obj.client;

	if (!isObject(%cl) || !%obj.isUsingSniperPlatform) {
		return parent::onUnmount(%this, %obj, %slot);
	}

	%obj.isUsingSniperPlatform = 0;
	%cl.setControlObject(%obj);
	$CPB::HelicopterSniper1.client = "";

	return parent::onUnmount(%this, %obj, %slot);
}