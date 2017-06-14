datablock ItemData(PrisonBucketGoldenItem)
{
	category = "Weapon";// Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./bucketitem.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Bucket";
	iconName = "";
	doColorShift = true;
	colorshiftColor = "1 1 0 1";

	 // Dynamic properties defined by the scripts
	image = PrisonBucketGoldenImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

datablock ShapeBaseImageData(PrisonBucketGoldenHatImage)
{
	shapeFile = "./buckethat.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 0.1";
	eyeOffset = "0 0 0.18";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = true;
	colorshiftColor = "1 1 0 1";

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true;
	stateTimeoutValue[1]			= 1;
	stateTransitionOnTimeout[1]		= "postReady";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;

	stateName[2]					= "postReady";
	stateAllowImageChange[2]		= true;
	stateTimeoutValue[2]			= 1;
	stateTransitionOnTimeout[2]		= "Ready";
	stateEmitter[2]					= GoldenEmitter;
	stateEmitterNode[2]				= "emitterPoint";
	stateEmitterTime[2]				= 1000;
};

datablock ShapeBaseImageData(PrisonBucketGoldenImage)
{
	// Basic Item properties
	shapeFile = "./bucketitem.dts";
	emap = true;
	offset = "0.04 0 0";

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = PrisonBucketGoldenItem;
	ammo = " ";

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = false;

	doColorShift = true;
	colorshiftColor = "1 1 0 1";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true;
	stateTransitionOnTriggerUp[1] 	= "PreEquip";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;

	stateName[3]					= "PreEquip";
	stateTransitionOnTriggerDown[3] = "Equip";
	stateEmitter[3]					= GoldenEmitter;
	stateEmitterNode[3]				= "emitterPoint";
	stateEmitterTime[3]				= 1000;

	stateName[2]					= "Equip";
	stateTimeoutValue[2]			= 1;
	stateTransitionOnTriggerUp[2]	= "Ready";
	stateWaitForTimeout[2]			= true;
	stateScript[2]					= "onEquip";
	stateEmitter[2]					= GoldenEmitter;
	stateEmitterNode[2]				= "emitterPoint";
	stateEmitterTime[3]				= 1000;
};

datablock ShapeBaseImageData(PrisonBucketGoldenEquippedImage : PrisonBucketGoldenImage) {
	shapeFile = "base/data/shapes/empty.dts";
	armReady = false;
};

function PrisonBucketGoldenImage::onMount(%this, %obj, %slot)
{
	if (%slot == 2) {
		return parent::onMount();
	}
	if (isObject(%image = %obj.getMountedImage(2)) && %image.getID() $= PrisonBucketGoldenHatImage.getID()) {
		%obj.mountImage(PrisonBucketGoldenEquippedImage, 0);
		return;
	}
	%obj.playThread(2, armReadyBoth);
	return parent::onMount(%this, %obj, %slot);
	//unequip hat
	// %player = %obj;
	// %client = %obj.client;
	// if(%player.getMountedImage(2) $= nametoID(PrisonBucketGoldenHatImage))
	// {
	// 	%player.unmountImage(2);
	// 	%client.applyBodyParts();
	// 	%client.applyBodyColors();
	// 	%player.unhideNode("headskin");
	// 	%player.isWearingBucket = 0;
	// }
}

function PrisonBucketGoldenImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(2, root);
	return parent::onUnMount(%this, %obj, %slot);
}

function PrisonBucketGoldenImage::onEquip(%this, %obj, %slot)
{
	%client = %obj.client;
	%obj.unmountImage(2);

	if(%obj.getMountedImage(2) $= nametoID(PrisonBucketGoldenHatImage))
	{
		%client.applyBodyParts();
		%client.applyBodyColors();
		%obj.unhideNode("headskin");
		%obj.isWearingBucket = 0;
	}
	else
	{
		%obj.unmountImage(3);
		serverPlay3D(weaponSwitchSound, %obj.getHackPosition());
		%obj.mountImage(PrisonBucketGoldenHatImage, 2);
		%obj.mountImage(PrisonBucketGoldenEquippedImage, 0);

		for(%i = 0;$hat[%i] !$= "";%i++)
		{
			%obj.hideNode($hat[%i]);
			%obj.hideNode($accent[%i]);
		}
		%obj.isWearingBucket = 1;
	}
}

function PrisonBucketGoldenEquippedImage::onEquip(%this, %obj, %slot) {
	%client = %obj.client;
	if(%obj.getMountedImage(2) $= nametoID(PrisonBucketGoldenHatImage))
	{
		%obj.unmountImage(2);
		%client.applyBodyParts();
		%client.applyBodyColors();
		%obj.unhideNode("headskin");
		%obj.isWearingBucket = 0;
	}
	%obj.mountImage(PrisonBucketGoldenImage, 0);
}

datablock DebrisData(PrisonBucketGoldenDebris)
{
	shapeFile = "./buckethat.dts";

	lifetime = 5.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;
	spinSpeed			= 300.0;
	minSpinSpeed = -600.0;
	maxSpinSpeed = 600.0;

	gravModifier = 6;
};

datablock ExplosionData(PrisonBucketGoldenExplosion)
{
	//explosionShape = "";
	soundProfile = "";

	lifeTimeMS = 150;

	debris = PrisonbucketGoldenDebris;
	debrisNum = 1;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 45;
	debrisThetaMax = 115;
	debrisVelocity = 9;
	debrisVelocityVariance = 8;

	faceViewer	  = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 1;
	camShakeRadius = 2.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 2;
	lightStartColor = "0.3 0.6 0.7";
	lightEndColor = "0 0 0";

	impulseRadius = 0;
	impulseForce = 0;
};

datablock ProjectileData(PrisonBucketGoldenProjectile)
{
	projectileShapeName = "";
	explosion           = PrisonBucketGoldenExplosion;
	explodeondeath 		= true;
	armingDelay         = 0;
	hasLight    = false;
};