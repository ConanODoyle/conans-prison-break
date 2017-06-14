datablock AudioProfile(tearGasGrenadeFireSound)
{
	filename	 = "./grenadeLauncherfire.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(tearGasGrenadeLoadSound)
{
	filename	 = "./loadShell.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock ParticleData(TearGasSmokeParticleA)
{
	textureName			 = "base/data/particles/cloud";
	dragCoefficient		= 1.6;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.2; 
	inheritedVelFactor	= 0.2;
	lifetimeMS			  = 3000;
	lifetimeVarianceMS	= 0;
	useInvAlpha = true;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;

	colors[0]	  = "1 0.35 0.35 0.0";
	colors[1]	  = "1 0.35 0.35 0.25";
	colors[2]	  = "1 0.22 0.19 0.1";
	colors[3]	  = "1 0.22 0.19 0";

	sizes[0]		= 2;
	sizes[1]		= 5.5;
	sizes[2]		= 4;
	sizes[3]		= 1.8;

	times[0]		= 0.0;
	times[1]		= 0.02;
	times[2]		= 0.8;
	times[3]		= 1.0;
};

datablock ParticleData(TearGasDotParticleA)
{
	textureName			 = "base/data/particles/dot";
	dragCoefficient		= 1.6;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0; 
	inheritedVelFactor	= 0.2;
	lifetimeMS			  = 3000;
	lifetimeVarianceMS	= 0;
	useInvAlpha = true;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;

	colors[0]	  = "0.3 0.01 0.01 0.0";
	colors[1]	  = "0.3 0.01 0.01 0.05";
	colors[2]	  = "0.3 0.01 0.01 0.03";
	colors[3]	  = "0.3 0.01 0.01 0.0";

	sizes[0]		= 0.1;
	sizes[1]		= 0.1;
	sizes[2]		= 0.1;
	sizes[3]		= 0.1;

	times[0]		= 0.0;
	times[1]		= 0.02;
	times[2]		= 0.8;
	times[3]		= 1.0;
};

datablock ParticleEmitterData(TearGasSmokeAEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 0;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0.0;
	
	ejectionVelocity = 10;
	velocityVariance = 3.0;

	thetaMin			= 10.0;
	thetaMax			= 100.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	emitter[0] = "TearGasDotParticleA";
	particles = "tearGasSmokeParticleA";	

	useEmitterColors = false;

	uiName = "Tear Gas Smoke";
};

datablock ParticleData(TearGasSmokeParticleB)
{
	textureName			 = "base/data/particles/cloud";
	dragCoefficient		= 1.6;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.25; 
	inheritedVelFactor	= 0;
	lifetimeMS			  = 1000;
	lifetimeVarianceMS	= 0;
	useInvAlpha = true;
	spinRandomMin = 0.0;
	spinRandomMax = 0.0;

	colors[0]	  = "1 0.4 0.4 0.0";
	colors[1]	  = "1 0.4 0.4 0.2";
	colors[2]	  = "1 0.3 0.25 0.05";
	colors[3]	  = "1 0.3 0.25 0";

	sizes[0]		= 0.1;
	sizes[1]		= 1.2;
	sizes[2]		= 0.5;
	sizes[3]		= 0.7;

	times[0]		= 0.0;
	times[1]		= 0.02;
	times[2]		= 0.8;
	times[3]		= 1.0;
};

datablock ParticleEmitterData(TearGasSmokeProjectileEmitter)
{
	ejectionPeriodMS = 5;
	periodVarianceMS = 2;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0.0;
	
	ejectionVelocity = 1;
	velocityVariance = 0;

	thetaMin			= 0.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = tearGasSmokeParticleB;	

	useEmitterColors = false;

	uiName = "Tear Gas Smoke Projectile";
};

//projectile
datablock ProjectileData(TearGasGrenadeProjectile)
{
	projectileShapeName = "./projectile.dts";
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

	muzzleVelocity		= 130;
	velInheritFactor	= 1;
	explodeOnDeath = true;

	armingDelay			= 4400;
	lifetime			= 4400;
	fadeDelay			= 4500;
	bounceElasticity	= 0.01;
	bounceFriction		= 0.2;
	isBallistic			= true;
	gravityMod = 0.5;
	particleEmitter = "TearGasSmokeProjectileEmitter";

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	uiName = "Tear Gas";
};

//////////
// item //
//////////
datablock ItemData(tearGasGrenadeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	// Basic Item Properties
	shapeFile = "./launcher.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Tear Gas Launcher";
	iconName = "Add-ons/Gamemode_PPE/icons/smoke";
	doColorShift = true;
	colorShiftColor = "0.5 0.2 0.2 1.000";

	 // Dynamic properties defined by the scripts
	image = tearGasGrenadeImage;
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
datablock ShapeBaseImageData(tearGasGrenadeImage)
{
	// Basic Item properties
	shapeFile = "./launcher.dts";
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
	item = tearGasGrenadeItem;
	ammo = " ";
	projectile = tearGasGrenadeProjectile;
	projectileType = Projectile;

	goldenImage = tearGasGrenadeGoldenImage;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	maxTearGasShots = 2;

	//casing = " ";
	doColorShift = true;
	colorShiftColor = "0.5 0.2 0.2 1.000";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "AmmoCheck";
	stateSequence[0]		= "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]			= "Ready";
	stateTransitionOnTimeout[1] = "DisplayAmmo";
	stateTimeoutValue[1]		= 0.1;
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]	= true;
	
	stateName[2]						  = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateTimeoutValue[2]				= 0.5;
	stateScript[2]			= "onCharge";
	stateWaitForTimeout[2]		= true;
	stateAllowImageChange[2]		  = true;
	
	stateName[3]			= "Armed";
	stateScript[3]			= "onArmed";
	stateTransitionOnTimeout[3]	= "Ready";
	stateTimeoutValue[3]	= 0.10;
	stateAllowImageChange[3]	= true;

	stateName[4]			= "Fire";
	stateTransitionOnTimeout[4]	= "AmmoCheck";
	stateTimeoutValue[4]		= 0.2;
	stateFire[4]			= true;
	stateSound[4]			= tearGasGrenadeFireSound;
	stateScript[4]			= "onFire";
	stateWaitForTimeout[4]		= true;
	stateAllowImageChange[4]	= true;

	stateName[5]			= "AbortCharge";
	stateScript[5]			= "onAbortCharge";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;

	stateName[6]			= "AmmoCheck";
	stateScript[6]			= "onAmmoCheck";
	stateTransitionOnTimeout[6]	= "PostAmmoCheck";
	stateTimeoutValue[6]		= 0.01;

	stateName[7]			= "PostAmmoCheck";
	stateTransitionOnAmmo[7]	= "Reload";
	stateTransitionOnNoAmmo[7]	= "NoAmmo";

	stateName[8]			= "Reload";
	stateSequence[8]		= "Reload";
	stateTimeoutValue[8]	= 1.0;
	stateSound[8]			= tearGasGrenadeLoadSound;
	stateTransitionOnTimeout[8]	= "Charge";

	stateName[9]			= "NoAmmo";
	stateSequence[9]		= "Ready";
	stateTimeoutValue[9]	= 0.1;
	stateTransitionOnTimeout[9]	= "AmmoCheck";

	stateName[10]			= "DisplayAmmo";
	stateScript[10]			= "onAmmoCheck";
	stateTransitionOnTimeout[10]	= "Ready";
	stateTimeoutValue[10]		= 0.1;
	stateTransitionOnTriggerDown[10]	= "Fire";
};

function tearGasGrenadeImage::onMount(%this, %obj, %slot) {
	if (!%obj.hasSeenTearGasMessage) {
		messageClient(%obj.client, '', "<font:Arial Bold:24>\c3Use tear gas to blind, hurt, and slow prisoners who walk through it! Lasts" @ mFloor($tearGasTime / 1000) @ "seconds.");
	}
	%obj.hasSeenTearGasMessage = 1;
	return parent::onMount(%this, %obj, %slot);
}

function tearGasGrenadeImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, shiftRight);
	serverPlay3D(brickRotateSound, %obj.getHackPosition());
}

function tearGasGrenadeImage::onArmed(%this, %obj, %slot)
{
	%obj.playthread(2, plant);
	serverPlay3D(brickPlantSound, %obj.getHackPosition());
}

function tearGasGrenadeImage::onFire(%this, %obj, %slot)
{
	//statistics
	%obj.totalTearGasShots++;
	setStatistic("TearGasGrenadesThrown", getStatistic("TearGasGrenadesThrown", %obj.client) + 1, %obj.client);
	setStatistic("TearGasGrenadesThrown", getStatistic("TearGasGrenadesThrown") + 1);

	%obj.playthread(2, plant);
	%ret = Parent::onFire(%this, %obj, %slot);

	// %currSlot = %obj.currTool;
	// %obj.tool[%currSlot] = 0;
	// %obj.weaponCount--;
	// messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	// serverCmdUnUseTool(%obj.client);

	return %ret;
}

function tearGasGrenadeImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}

$tearGasShotRechargeTime = 3;

function rechargeTearGasShots(%obj) {
	if (isObject(%obj)) {
		if (%obj.totalTearGasShots > 0 && !isEventPending(%obj.tearGasRechargeSchedule)) {
			%obj.tearGasRechargeSchedule = schedule($tearGasShotRechargeTime * 60 * 1000, %obj, messageClient, %obj.client, '', "<font:Arial Bold:26>\c3You have recharged an extra tear gas shot.");
			schedule($tearGasShotRechargeTime * 60 * 1000, %obj, eval, %obj @ ".totalTearGasShots--;");
			schedule($tearGasShotRechargeTime * 60 * 1000 + 1, %obj, rechargeTearGasShots, %obj);
		}
	}
}

function tearGasGrenadeImage::onAmmoCheck(%this, %obj, %slot)
{	
	if (%obj.totalTearGasShots >= %this.maxTearGasShots) {
		centerPrint(%obj.client, "<br><br><br><br><br><br><font:Arial Bold:24>\c0" @ 0 @ "/" @ %this.maxTearGasShots @ " Tear Gas Canisters Left. Recharging: " @ mFloor(getTimeRemaining(%obj.tearGasRechargeSchedule) / 1000) @ " s", 5);
		%obj.setImageAmmo(%slot, 0);
		rechargeTearGasShots(%obj);
		return;
	} else if (%obj.totalTearGasShots > 0) {
		centerPrint(%obj.client, "<br><br><br><br><br><br><font:Arial Bold:24>\c3" @ %this.maxTearGasShots - %obj.totalTearGasShots @ "/" @ %this.maxTearGasShots @ " Tear Gas Canisters Left. Recharging: " @ mFloor(getTimeRemaining(%obj.tearGasRechargeSchedule) / 1000) @ " s", 5);
		%obj.setImageAmmo(%slot, 1);
		rechargeTearGasShots(%obj);
		return;
	}

	centerPrint(%obj.client, "<br><br><br><br><br><br><font:Arial Bold:24>\c3" @ %this.maxTearGasShots - %obj.totalTearGasShots @ "/" @ %this.maxTearGasShots @ " Tear Gas Canisters Left", 5);
	%obj.setImageAmmo(%slot, 1);
}

function tearGasGrenadeProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal) {
	if (strPos(%col.getClassName(), "Player") >= 0) {
		return;
	}
	serverPlay3D("riotSmokeGrenadeBounce" @ getRandom(1, 3) @ "Sound", %pos);
	return parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
}

$tearGasTime = 30000;
$tearGasRadius = 6;
$tearGasSlowTime = 6000;
$tearGasSlowSpeed = 0.3;


function tearGasGrenadeProjectile::onExplode(%this, %proj, %pos) {
	createTearGasAt(%pos, $tearGasTime, %this.client);
	serverPlay3D("riotSmokeGrenadeExplodeSound", %pos);
}

function createTearGasAt(%pos, %time, %cl) {
	%tearGasEmitter = new ParticleEmitterNode(Smoke)
	{
		dataBlock = GenericEmitterNode;
		emitter = TearGasSmokeAEmitter;
		scale = "1 1 1";
		position = %pos;
		client = %cl;
	};
	MissionCleanup.add(%tearGasEmitter);

	tearGasDamageLoop(%tearGasEmitter, %cl);

	%tearGasEmitter.deleteSchedule = %tearGasEmitter.schedule(%time, delete);
}

function tearGasDamageLoop(%emitter, %killer) {
	if (!isObject(%emitter)) {
		return;
	}

	%pos = %emitter.getPosition();

	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (%cl.isGuard || !isObject(%pl = %cl.player) || %pl.isWearingGasmask) {
			continue;
		}

		%eyePos = getWords(%pl.getEyeTransform(), 0, 2);
		%pos = vectorAdd(%emitter.getPosition(), "0 0 0.2");

		%dist = vectorLen(vectorSub(%eyePos, %pos));
		if (%dist < $tearGasRadius) {
			if (getRegion(%pl) $= "Outside" || getRegion(%pl) $= "Yard") {
				applyTearGas(%pl, %emitter);
			} else if (!isObject(getWord(containerRaycast(%eyePos, %pos, $TypeMasks::FxBrickObjectType), 0))) {
				applyTearGas(%pl, %emitter);
			}
		}
	}

	schedule(200, %emitter, tearGasDamageLoop, %emitter, %killer);
}

function applyTearGas(%pl, %emitter) {
	cancel(%pl.endTearGasSlow);
	%pl.moveRecovery = 0;
	%pl.lastTimeInTearGas = getSimTime();

	%db = %pl.getDatablock();

	%pl.setDamageFlash(1);
	%pl.setWhiteOut(0.8);
	%pl.timeInTearGas += 0.5;
	%pl.playPain();
	setStatistic("timeInTearGas", getStatistic("timeInTearGas", %pl.client) + 0.5, %pl.client);
	setStatistic("timeInTearGas", getStatistic("timeInTearGas") + 0.5);

	if (%pl.timeInTearGas >= 5) {
		stun(getTimeRemaining(mCeil(%emitter.deleteSchedule / 1000)));
	}

	%pl.setMaxForwardSpeed(%db.maxForwardSpeed * $tearGasSlowSpeed);
	%pl.setMaxSideSpeed(%db.maxSideSpeed * $tearGasSlowSpeed);
	%pl.setMaxBackwardSpeed(%db.maxBackwardSpeed * $tearGasSlowSpeed);

	%pl.damage(%emitter, %pl.getPosition(), 0.7, $DamageType::Default);

	resetPlayerMoveSpeed(%pl);
}

function resetPlayerMoveSpeed(%pl) {
	if (isEventPending(%pl.endTearGasSlow)) {
		return;
	}
	%db = %pl.getDatablock();

	%pl.timeInTearGas = 0;

	%pl.setMaxForwardSpeed(%db.maxForwardSpeed * ($tearGasSlowSpeed + %pl.moveRecovery));
	%pl.setMaxSideSpeed(%db.maxSideSpeed * ($tearGasSlowSpeed + %pl.moveRecovery));
	%pl.setMaxBackwardSpeed(%db.maxBackwardSpeed * ($tearGasSlowSpeed + %pl.moveRecovery));

	if ((%pl.moveRecovery + $tearGasSlowSpeed | 0) == 1 || %pl.getMaxForwardSpeed() >= 200) {
		%pl.moveRecovery = 0;
		return;
	}

	if (getSimTime() - %pl.lastTimeInTearGas > $tearGasSlowTime) {
		%pl.moveRecovery += 0.01;
	}

	%pl.setDamageFlash(1 - %pl.moveRecovery);
	%pl.setWhiteOut(0.8 - %pl.moveRecovery);

	%pl.endTearGasSlow = schedule(100, %pl, resetPlayerMoveSpeed, %pl);
}