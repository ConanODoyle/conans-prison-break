datablock ItemData(PrisonTrayGoldenItem)
{
	category = "Weapon";// Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./flattray.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Tray";
	iconName = "";
	doColorShift = true;
	colorshiftColor = "1 1 1 1";
	rotation = eulerToMatrix("0 90 0");

	 // Dynamic properties defined by the scripts
	image = PrisonTrayGoldenImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

datablock ShapeBaseImageData(PrisonTrayGoldenImage)
{
	// Basic Item properties
	shapeFile = "./Tray.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	eyeoffset = "0.563 0.6 -0.4";
	offset = "0.025	0.02 -0.12";
	rotation = eulerToMatrix("0 0 0");

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = PrisonTrayGoldenItem;
	ammo = " ";
	projectileType = Projectile;

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

	// // Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true;
	stateTransitionOnTriggerDown[1] = "Fire";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;

	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.4;
	stateTransitionOnTimeout[2]		= "PostFire";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;

	stateName[3]					= "PostFire";
	stateScript[3]					= "onPostFire";
	stateTransitionOnTriggerUp[3]	= "Ready";
	stateTransitionOnTriggerDown[3] = "ReFire";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;

	stateName[4]					= "ReFire";
	stateScript[4]					= "onReFire";
	stateTimeoutValue[4]			= 0.4;
	stateTransitionOnTimeout[4]		= "PostFire";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;
};

datablock ShapeBaseImageData(PrisonTrayGoldenBackImage : PrisonTrayGoldenImage)
{
	shapeFile = "./strapTray.dts";
	mountPoint = 7;
	offset = "-0.56 -0.1 0.8";
	eyeoffset = "0 0 -10";
	rotation = eulerToMatrix("0 0 180");
};

function PrisonTrayGoldenBackImage::onMount(%this, %obj, %slot)
{
	%obj.hasTrayOnBack = 1;
}

function PrisonTrayGoldenBackImage::onUnMount(%this, %obj, %slot)
{
	%obj.hasTrayOnBack = 0;
}

function PrisonTrayGoldenImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(2, armReadyBoth);
	%obj.isHoldingTray = 1;
	return parent::onMount(%this, %obj, %slot);
}

function PrisonTrayGoldenImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(2, root);
	%obj.isHoldingTray = 0;
	return parent::onUnMount(%this, %obj, %slot);
}

function PrisonTrayGoldenImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(1, activate);
	%start = getWords(%obj.getEyeTransform(), 0, 2);
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(0), 1.8));
	%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType, %obj);
	if (isObject(%hit = getWord(%ray, 0))) {
		if (%hit.getDatablock().getName() !$= "PlayerNoJet") {
			return;
		}

		%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %hit.getHackPosition()));
		%angle = mACos(vectorDot(%hit.getForwardVector(), %targetVector));
		if (%angle < 1.7) {
			centerprint(%obj.client, "You must be facing a back you can attach this to!", 2);
			return;
		}

		if (%hit.hasTrayOnBack) {
			centerprint(%obj.client, "The person is already wearing a back tray!", 2);
			return;
		} else if (%hit.client.bl_id == 6531) {
			centerprint(%obj.client, "Swollow's cape rejects the tray", 2);
			return;
		}
		%obj.progress = 0;

		%obj.isGivingTray = 1;
		%obj.givingTrayTarget = %hit;

		checkTrayAttached(%obj, %hit);
	}
}

function PrisonTrayGoldenImage::onReFire(%this, %obj, %slot) {
	if (isObject(%hit = %obj.givingTrayTarget) && vectorLen(vectorSub(%hit.getPosition(), %obj.getPosition())) < 2.8) {
		%obj.playThread(1, activate);
		%obj.progress++;
		if (checkTrayAttached(%obj, %hit) == 1) {
			%obj.progress = 0;
			%obj.isGivingTray = 0;
			%obj.givingTrayTarget = 0;

			%hit.mountImage(PrisonTrayBackImage, 1);

			%obj.tool[%obj.currtool] = 0;
			%obj.weaponCount--;
			messageClient(%obj.client,'MsgItemPickup','',%obj.currtool,0);
			serverCmdUnUseTool(%obj.client);
			%obj.unMountImage(0);
		}
	} else if (%obj.isGivingTray) {
		%obj.client.centerprint("Tray attaching canceled", 2);
		if (isObject(%obj.givingTrayTarget)) {
			%obj.givingTrayTarget.client.centerprint("Tray attaching canceled", 2);
		}
		%player.progress = 0;
		%obj.isGivingTray = 0;
		%obj.givingTrayTarget = 0;
	} else {
		%obj.playThread(1, activate);
		%start = getWords(%obj.getEyeTransform(), 0, 2);
		%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(0), 1.8));
		%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType, %obj);
		if (isObject(%hit = getWord(%ray, 0))) {
			if (%hit.getDatablock().getName() !$= "PlayerNoJet") {
				return;
			}

			%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %hit.getHackPosition()));
			%angle = mACos(vectorDot(%hit.getForwardVector(), %targetVector));
			if (%angle < 1.7) {
				centerprint(%obj.client, "You must be facing a back you can attach this to!", 2);
				return;
			}

			if (%hit.hasTrayOnBack) {
				centerprint(%obj.client, "The person is already wearing a back tray!", 2);
				return;
			} else if (%hit.client.bl_id == 6531) {
				centerprint(%obj.client, "Swollow's cape rejects the tray", 2);
				return;
			}
			%obj.progress = 0;

			%obj.isGivingTray = 1;
			%obj.givingTrayTarget = %hit;

			checkTrayAttached(%obj, %hit);
		}
	}
}

datablock DebrisData(PrisonTrayGoldenDebris)
{
	shapeFile = "./tray.dts";

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

datablock ExplosionData(PrisonTrayGoldenExplosion)
{
	//explosionShape = "";
	soundProfile = "";

	lifeTimeMS = 150;

	debris = PrisonTrayGoldenDebris;
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

datablock ProjectileData(PrisonTrayGoldenProjectile)
{
	projectileShapeName = "";
	explosion           = PrisonTrayGoldenExplosion;
	explodeondeath 		= true;
	armingDelay         = 0;
	hasLight    = false;
};

datablock DebrisData(PrisonTrayGoldenDebris)
{
	shapeFile = "./tray.dts";

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

datablock ExplosionData(PrisonTrayGoldenExplosion)
{
	//explosionShape = "";
	soundProfile = "";

	lifeTimeMS = 150;

	debris = PrisonTrayGoldenDebris;
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

datablock ProjectileData(PrisonTrayGoldenProjectile)
{
	projectileShapeName = "";
	explosion           = PrisonTrayGoldenExplosion;
	explodeondeath 		= true;
	armingDelay         = 0;
	hasLight    = false;
};
