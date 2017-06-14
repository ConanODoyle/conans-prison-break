//projectile
datablock ProjectileData(RiotSmokeGrenadeGoldenProjectile)
{
	projectileShapeName = "./smoke grenade projectile.dts";
	directDamage		= 0;
	directDamageType  	= $DamageType::rocketDirect;
	radiusDamageType  	= $DamageType::rocketRadius;
	impactImpulse		= 0;
	verticalImpulse		= 0;
	explosion			= "";
	//particleEmitter	  = as;

	brickExplosionRadius = 10;
	brickExplosionImpact = false; //destroy a brick if we hit it directly?
	brickExplosionForce  = 25;             
	brickExplosionMaxVolume = 100;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 60; 

	muzzleVelocity		= 30;
	velInheritFactor	= 1;
	explodeOnDeath = true;
	particleEmitter     = GoldenEmitter;

	armingDelay			= 5000;
	lifetime			= 5000;
	fadeDelay			= 4500;
	bounceElasticity	= 0.3;
	bounceFriction		= 0.1;
	isBallistic			= true;
	gravityMod = 1.0;

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	uiName = "Smoke Grenade";
};

datablock ParticleData(smokeParticleA)
{
	textureName			 = "base/data/particles/cloud";
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.02; 
	inheritedVelFactor	= 0.2;
	lifetimeMS			  = 5000;
	lifetimeVarianceMS	= 0;
	useInvAlpha = false;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;

	colors[0]	  = "1 1 1 0.0";
	colors[1]	  = "1 1 1 1";
	colors[2]	  = "1 1 1 1";
	colors[3]	  = "1 1 1 0";

	sizes[0]		= 5;
	sizes[1]		= 12.5;
	sizes[2]		= 14;
	sizes[3]		= 13.8;

	times[0]		= 0.0;
	times[1]		= 0.1;
	times[2]		= 0.8;
	times[3]		= 1.0;
};

datablock ParticleEmitterData(smokeAEmitter)
{
	ejectionPeriodMS = 50;
	periodVarianceMS = 0;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0.0;
	
	ejectionVelocity = 12;
	velocityVariance = 3.0;

	thetaMin			= 30.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = smokeParticleA;	

	useEmitterColors = true;

	uiName = "Smoke A";
};


//////////
// item //
//////////
datablock ItemData(riotSmokeGrenadeGoldenItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	// Basic Item Properties
	shapeFile = "./smoke grenade.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Smoke Grenade";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "1 0.8 0 1.000";

	 // Dynamic properties defined by the scripts
	image = riotSmokeGrenadeGoldenImage;
	canDrop = true;
};

//function chisel::onUse(%this,%user)
//{
//	//mount the image in the right hand slot
//	%user.mountimage(%this.image, $RightHandSlot);
//}

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(riotSmokeGrenadeGoldenImage)
{
	// Basic Item properties
	shapeFile = "./smoke grenade.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
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
	item = riotSmokeGrenadeGoldenItem;
	ammo = " ";
	projectile = RiotSmokeGrenadeGoldenProjectile;
	projectileType = Projectile;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	//casing = " ";
	doColorShift = true;
	colorShiftColor = "1 0.8 0 1";

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
	stateAllowImageChange[2]		  = true;
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

function riotSmokeGrenadeGoldenImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}
function riotSmokeGrenadeGoldenImage::onFire(%this, %obj, %slot)
{
	//statistics
	setStatistic("SmokeGrenadesThrown", getStatistic("SmokeGrenadesThrown", %obj.client) + 1, %obj.client);
	setStatistic("SmokeGrenadesThrown", getStatistic("SmokeGrenadesThrown") + 1);

	%obj.playthread(2, spearThrow);
	%ret = Parent::onFire(%this, %obj, %slot);

	%currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);

	return %ret;
}
function riotSmokeGrenadeGoldenImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}

function RiotSmokeGrenadeGoldenProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal) {
	if (strPos(%col.getClassName(), "Player") >= 0) {
		return;
	}
	serverPlay3D("riotSmokeGrenadeBounce" @ getRandom(1, 3) @ "Sound", %pos);
	return parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
}

if ($smokeTime $= "") {
	$smokeTime = 10000;
}

function RiotSmokeGrenadeGoldenProjectile::onExplode(%this, %proj, %pos) {
	createSmokeScreenAt(%pos, $smokeTime);
	serverPlay3D("riotSmokeGrenadeExplodeSound", %pos);
}