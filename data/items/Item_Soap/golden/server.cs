//projectile
datablock ProjectileData(PrisonSoapGoldenProjectile)
{
	projectileShapeName = "./soap.dts";
	directDamage		= 0;
	directDamageType  	= $DamageType::rocketDirect;
	radiusDamageType  	= $DamageType::rocketRadius;
	impactImpulse		= 0;
	verticalImpulse		= 0;
	explosion			= "";
	//particleEmitter	  = as;

	brickExplosionRadius = 0;
	brickExplosionImpact = false; //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0; 

	muzzleVelocity		= 30;
	velInheritFactor	= 1;
	explodeOnDeath = true;
	particleEmitter = "GoldenEmitter";

	armingDelay			= 10000;
	lifetime			= 10000;
	fadeDelay			= 4500;
	bounceElasticity	= 0.1;
	bounceFriction		= 0;
	isBallistic			= true;
	gravityMod = 1.0;

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	uiName = "Soap";
};

//////////
// item //
//////////
datablock ItemData(PrisonSoapGoldenItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	// Basic Item Properties
	shapeFile = "./soap.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Soap";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "1 0.89 0.02 1.000";

	 // Dynamic properties defined by the scripts
	image = PrisonSoapGoldenImage;
	canDrop = true;
};

datablock ItemData(PrisonSoapGoldenPickupItem  : PrisonSoapGoldenItem)
{
	isSlidingItem = 1;
	uiname = "";
};

datablock ShapeBaseImageData(PrisonSoapGoldenPickupImage)
{
	shapeFile = "base/data/shapes/empty.dts";

	mountPoint = 7;

	canMountToBronson = 1;

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= "emitterPoint";
	stateEmitterTime[0]				= 1000;


	stateName[1]			= "Ready";
	stateTimeoutValue[1]		= 0.5;
	stateTransitionOnTimeout[1]	= "Activate";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;
};

//function chisel::onUse(%this,%user)
//{
//	//mount the image in the right hand slot
//	%user.mountimage(%this.image, $RightHandSlot);
//}

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(PrisonSoapGoldenImage)
{
	// Basic Item properties
	shapeFile = "./soap.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0.07 0.05";
	rotation = eulerToMatrix("0 90 0");
	//eyeOffset = "0.1 0.2 -0.55";

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = PrisonSoapGoldenItem;
	ammo = " ";
	projectile = PrisonSoapGoldenProjectile;
	projectileType = Projectile;

	canMountToBronson = 1;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	//casing = " ";
	doColorShift = true;
	colorShiftColor = "1 0.89 0.02 1.000";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]	= true;
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;
	
	stateName[2]						  = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateTimeoutValue[2]				= 0.8;
	stateScript[2]			= "oncharge";
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateAllowImageChange[2]		  = false;
	stateEmitter[2]					= GoldenEmitter;
	stateEmitterNode[2]				= "emitterPoint";
	stateEmitterTime[2]				= 1000;
	
	stateName[3]			= "Armed";
	stateTransitionOnTriggerUp[3]	= "Fire";
	stateAllowImageChange[3]	= true;
	stateEmitter[3]					= GoldenEmitter;
	stateEmitterNode[3]				= "emitterPoint";
	stateEmitterTime[3]				= 1000;

	stateName[4]			= "Fire";
	stateTransitionOnTimeout[4]	= "Ready";
	stateTimeoutValue[4]		= 0.2;
	stateFire[4]			= true;
	stateScript[4]			= "onFire";
	stateWaitForTimeout[4]		= true;
	stateAllowImageChange[4]	= true;
	stateEmitter[4]					= GoldenEmitter;
	stateEmitterNode[4]				= "emitterPoint";
	stateEmitterTime[4]				= 1000;

	stateName[5]			= "AbortCharge";
	stateScript[5]			= "onAbortCharge";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;
	stateEmitter[5]					= GoldenEmitter;
	stateEmitterNode[5]				= "emitterPoint";
	stateEmitterTime[5]				= 1000;
};

function PrisonSoapGoldenImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}
function PrisonSoapGoldenImage::onFire(%this, %obj, %slot)
{
	//statistics
	setStatistic("SoapThrown", getStatistic("SoapThrown", %obj.client) + 1, %obj.client);
	setStatistic("SoapThrown", getStatistic("SoapThrown") + 1);

	%obj.playThread(2, spearThrow);
	%i = new Item(Soap) {
		datablock = PrisonSoapGoldenPickupItem;
		position = %obj.getMuzzlePoint(%slot);
		spawnTime = getSimTime();
		minigame = getMinigameFromObject(%obj);
	};
	%i.setCollisionTimeout(%obj);
	%i.mountImage(PrisonSoapGoldenPickupImage, 0);
	%i.setVelocity(VectorScale(%obj.getMuzzleVector(%slot), 50.0 * getword(%obj.getScale(), 2)));
	%i.schedulePop();
	//%ret = Parent::onFire(%this, %obj, %slot);

	%currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);

	return;
}
function PrisonSoapGoldenImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}