datablock StaticShapeData(HelicopterMountShape)
{
	shapeFile = "./shapes/helicopterShape/helicopterMount.dts";
};

datablock PlayerData(HelicopterArmor : PlayerStandardArmor) {
	shapeFile = "./shapes/helicopterShape/helicopterShape2.dts";

	boundingBox = vectorScale("100 100 100", 4);
	uiName = "";
};

datablock TSShapeConstructor(SniperRemotePlatformDTS) {
	baseShape  = "./shapes/platform/sniperRemotePlatform.dts";
	sequence0  = "./shapes/platform/s_plat.dsq";
};

datablock PlayerData(SniperPlatformArmor : PlayerStandardArmor) {
	shapeFile = "./shapes/platform/sniperRemotePlatform.dts";

	boundingBox = vectorScale("100 100 10", 4);
	uiName = "";
	firstPersonOnly = 1;
	// renderFirstPerson = 1;
};

datablock ItemData(SniperControlItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./shapes/helicopterShape/controller/SniperControl.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	image = SniperControlImage;
	iconName = "Add-ons/Gamemode_PPE/icons/sniper";

	uiName = "Sniper Controller";

	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";
	canDrop = true;
};

datablock ShapeBaseImageData(SniperControlImage) {
	shapeFile = "./shapes/helicopterShape/controller/SniperControl.dts";
	emap = true;
	
	className = "WeaponImage";

	item = SniperControlItem;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.15;
	stateTransitionOnTimeout[0]		= "Ready";
	stateScript[0]					= "onActivate";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "PreFire";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "PreFire";
	stateTransitionOnTriggerUp[2]	= "Fire";

	stateName[3]					= "Fire";
	stateScript[3]					= "onFire";
	stateTransitionOnTimeout[3]		= "Ready";
	stateTimeoutValue[3]			= 0.1;
};

$CPB::HelicopterCircleTime = 120; //in seconds

//Functions:
//Packaged:
//	serverCmdLight
//Created:
//	spawnHelicopter
//	rotateHelicopter
//	rotateHelicopter2
//	SniperControlImage::onActivate
//	SniperControlImage::onFire
//	SniperControlImage::onUnmount
//	enterSniperPlatform
//	exitSniperPlatform


package HelicopterSniper {
	function serverCmdLight(%cl) {
		if (isObject(%pl = %cl.player) && %pl.isUsingSniperPlatform) {
			exitSniperPlatform(%pl);
			return;
		}

		parent::serverCmdLight(%cl);
	}

	// function Armor::onRemove(%this, %obj) {
	// 	ret
	// }
}; 
activatePackage(HelicopterSniper);


////////////////////


function spawnHelicopter(%pos) {
	if (isObject($CPB::HelicopterSpinShape)) {
		$CPB::HelicopterSpinShape.delete();
	}

	if (isObject($CPB::HelicopterBot)) {
		$CPB::HelicopterBot.delete();
	}

	if (isObject($CPB::HelicopterSniper)) {
		$CPB::HelicopterSniper.delete();
	}


	$CPB::HelicopterSpinShape = new StaticShape() {
		datablock = HelicopterMountShape;
	};
	$CPB::HelicopterBot = new AIPlayer() {
		datablock = HelicopterArmor;
	};
	$CPB::HelicopterSniper = new AIPlayer() {
		datablock = SniperPlatformArmor;
	};

	// if (!isObject($CPB::HelicopterSpotlight)) {
	// 	$CPB::HelicopterSpotlight = new AIPlayer() {
	// 		datablock = SpotlightArmor;
	// 	};
	// }

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

	$CPB::HelicopterBot.unMountObject($CPB::HelicopterSniper);
	$CPB::HelicopterBot.setNodeColor("ALL", "0 0.1 0.6 1");
	// $CPB::HelicopterBot.mountObject($CPB::HelicopterSniper2, 4);

	$CPB::HelicopterSniper.setTransform("0 0 1000");
	$CPB::HelicopterSniper.mountImage(SniperRifleHelicopterImage, 0);
	$CPB::HelicopterBot.mountObject($CPB::HelicopterSniper, 3);
	// $CPB::HelicopterBot.mountObject($CPB::HelicopterSpotlight, 4);
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
		messageClient(%cl, '', "<font:Arial Bold:24>\c3Click to take control of the helicopter sniper. Press Light or unequip the item to exit.");
	}
	%obj.alertSniperControlImage = 1;
}

function SniperControlImage::onFire(%this, %obj, %slot) {
	%cl = %obj.client;

	if (!isObject(%cl)) {
		return;
	} else if (isObject($CPB::HelicopterSniper.client)) {
		messageClient(%cl, '', "The helicopter sniper is currently in use by " @ $CPB::HelicopterSniper.client.name @ "!");
		return;
	} else if (!isObject($CPB::HelicopterSniper)) {
		messageClient(%cl, '', "There is no helicopter sniper!");
		messageAdmins("!!! \c6There is no helicopter sniper!");
		return;
	}

	enterSniperPlatform(%obj);
}

function SniperControlImage::onUnmount(%this, %obj, %slot) {
	%cl = %obj.client;

	if (!isObject(%cl) || !%obj.isUsingSniperPlatform) {
		return parent::onUnmount(%this, %obj, %slot);
	}

	exitSniperPlatform(%obj);

	return parent::onUnmount(%this, %obj, %slot);
}

function enterSniperPlatform(%pl) {
	%cl = %pl.client;
	if (!isObject(%cl)) {
		return;
	} else if ($CPB::HelicopterSniper.client !$= "") {
		return;
	}

	$CPB::HelicopterSniper.client = %cl;
	%pl.isUsingSniperPlatform = 1;
	%cl.setControlObject($CPB::HelicopterSniper);
	messageClient(%cl, '', "\c2[Sniper Platform Enabled] \c6Press Light or unequip the item to exit");
	messageGuards("\c2" @ %cl.name @ " \c6has taken control of the helicopter sniper");
}

function exitSniperPlatform(%pl) {
	%cl = %pl.client;
	if (!isObject(%cl)) {
		return;
	} else if (%cl != $CPB::HelicopterSniper.client) {
		return;
	}

	$CPB::HelicopterSniper.client = "";
	%pl.isUsingSniperPlatform = 0;
	%cl.setControlObject(%pl);
	messageGuards("\c2" @ %cl.name @ " \c6has exited out of the helicopter sniper");
}