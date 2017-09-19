$STUNBLINDRADIUS = 10;
$STUNBLINDBONUS = 0.4;
$STUNDISTANCE = 2;
$STUNMAX = 3;


datablock AudioProfile(SniperRifleSpotlightBoltSound)
{
	filename	 = "./sniperBolt_sniperSpotlight.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(SniperRifleSpotlightFireSound)
{
	filename	 = "./fire2.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(SniperRifleSpotlightUnholsterSound)
{
	filename	 = "./sniperunholster_sniperSpotlight.wav";
	description = AudioClose3d;
	preload = true;
};

datablock ParticleData(SniperRifleSpotlightSmokeParticle)
{
	dragCoefficient		= 3;
	gravityCoefficient	= -0.5;
	inheritedVelFactor	= 0.2;
	constantAcceleration = 0.0;
	lifetimeMS			  = 1525;
	lifetimeVarianceMS	= 55;
	textureName			 = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]	  = "0.5 0.5 0.5 0.9";
	colors[1]	  = "0.5 0.5 0.5 0.0";
	sizes[0]		= 0.15;
	sizes[1]		= 0.25;

	useInvAlpha = false;
};
datablock ParticleEmitterData(SniperRifleSpotlightSmokeEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 1.0;
	velocityVariance = 1.0;
	ejectionOffset	= 0.0;
	thetaMin			= 0;
	thetaMax			= 40;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "SniperRifleSpotlightSmokeParticle";
};


//bullet trail effects
datablock ParticleData(SniperRifleSpotlightBulletTrailParticle)
{
	dragCoefficient		= 3;
	gravityCoefficient	= -0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration = 0.0;
	lifetimeMS			  = 625;
	lifetimeVarianceMS	= 85;
	textureName			 = "base/data/particles/dot";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]	  = "0.3 0.3 0.9 0.4";
	colors[1]	  = "0.5 0.5 0.5 0.0";
	sizes[0]		= 0.09;
	sizes[1]		= 0.18;

	useInvAlpha = false;
};

datablock ParticleEmitterData(SniperRifleSpotlightBulletTrailEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0.0;
	velocityVariance = 0.0;
	ejectionOffset	= 0.0;
	thetaMin			= 0;
	thetaMax			= 90;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "SniperRifleSpotlightBulletTrailParticle";
};

datablock ParticleData(SniperRifleSpotlightRingStarParticle)
{
	dragCoefficient		= 8;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.2;
	constantAcceleration = 0.0;
	lifetimeMS			  = 170;
	lifetimeVarianceMS	= 5;
	textureName			 = "base/data/particles/star1";
	spinSpeed		= 0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 1.0;
	colors[0]	  = "1 1 0.0 0.9";
	colors[1]	  = "0.9 0.5 0.0 0.0";
	sizes[0]		= 0.1;
	sizes[1]		= 0.2;

	useInvAlpha = false;
};
datablock ParticleEmitterData(SniperRifleSpotlightExplosionRingEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0.0;
	ejectionOffset	= 0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel  = 1000;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "SniperRifleSpotlightRingStarParticle";

	useEmitterColors = true;
	uiName = "Sniper Rifle Spotlight Flash";
};

datablock ParticleData(SniperRifleSpotlightExplosionParticle)
{
	dragCoefficient		= 8;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.2;
	constantAcceleration = 0.0;
	lifetimeMS			  = 750;
	lifetimeVarianceMS	= 400;
	textureName			 = "base/data/particles/cloud";
	spinSpeed		= 0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 1.0;
	colors[0]	  = "0.7 0.8 1 0.9";
	colors[1]	  = "1 1 1 0.0";
	sizes[0]		= 0.2;
	sizes[1]		= 0.7;

	useInvAlpha = true;
};
datablock ParticleEmitterData(SniperRifleSpotlightExplosionEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 1;
	velocityVariance = 0.0;
	ejectionOffset	= 0;
	thetaMin			= 0;
	thetaMax			= 90;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "SniperRifleSpotlightExplosionParticle";

	useEmitterColors = true;
	uiName = "Sniper Rifle Spotlight Smoke";
};

datablock ExplosionData(SniperRifleSpotlightExplosion)
{
	//explosionShape = "";
	soundProfile = bulletHitSound;

	lifeTimeMS = 150;

	particleEmitter = "SniperRifleSpotlightExplosionEmitter";
	particleDensity = 5;
	particleRadius = 0.2;

	emitter[0] = "SniperRifleSpotlightExplosionRingEmitter";

	faceViewer	  = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";
};

datablock DebrisData(SniperRifleSpotlightDebris)
{
	shapeFile = "./bullet.dts";
	lifetime = 8.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.2;
	friction = 0.6;
	numBounces = 5;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 2;
};


////////////////////


AddDamageType("SniperRifleSpotlight",	'<bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1',	 '%2 <bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1', 0.5, 1);
datablock ProjectileData(SniperRifleSpotlightProjectile)
{
	projectileShapeName = "./bullet.dts";
	directDamage		  = 100;
	directDamageType	 = $DamageType::SniperrifleSpotlight;
	radiusDamageType	 = $DamageType::SniperrifleSpotlight;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;			 //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;			 //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

	impactImpulse		  = 0;
	verticalImpulse	  = 0;
	explosion			  = SniperRifleSpotlightExplosion;
	particleEmitter	  = SniperRifleSpotlightBulletTrailEmitter;

	muzzleVelocity		= 200;
	velInheritFactor	 = 1;

	armingDelay			= 00;
	lifetime				= 4000;
	fadeDelay			  = 3500;
	bounceElasticity	 = 0.5;
	bounceFriction		= 0.20;
	isBallistic			= false;
	gravityMod = 0.0;

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	aimSpotlight = 1;
};

AddDamageType("SniperRifleSpotlight",	'<bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1',	 '%2 <bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1', 0.5, 1);
datablock ProjectileData(SniperRifleSpotlightProjectile)
{
	projectileShapeName = "./bullet.dts";
	directDamage		  = 100;
	directDamageType	 = $DamageType::SniperrifleSpotlight;
	radiusDamageType	 = $DamageType::SniperrifleSpotlight;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;			 //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;			 //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

	impactImpulse		  = 0;
	verticalImpulse	  = 0;
	explosion			  = SniperRifleSpotlightExplosion;
	particleEmitter	  = SniperRifleSpotlightBulletTrailEmitter;

	muzzleVelocity		= 200;
	velInheritFactor	 = 1;

	armingDelay			= 0;
	lifetime				= 4000;
	fadeDelay			  = 3500;
	bounceElasticity	 = 0.7;
	bounceFriction		= 0.2;
	isBallistic			= true;
	gravityMod = 0.0;

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	aimSpotlight = 1;
};

AddDamageType("ShrapnelSniper",	'<bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1',	 '%2 <bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1', 0.5, 1);
AddDamageType("Shrapnel",	'<bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1',	 '%2 <bitmap:Add-Ons/Gamemode_PPE/ci/CI_Sniper> %1', 0.5, 1);
datablock ProjectileData(SniperShrapnelSpotlightProjectile : SniperRifleSpotlightProjectile) {
	projectileShapeName = "./sniperShrapnelBullet.dts";
	directDamageType = $DamageType::ShrapnelSniper;
	radiusDamageType = $DamageType::ShrapnelSniper;

	shrapnelCount = 8;
	shrapnelScale = "1 1 1";
};


////////////////////


datablock ProjectileData(ShrapnelProjectile) {
	projectileShapeName = "./sniperShrapnel.dts";
	directDamage = 30;
	directDamageType = $DamageType::Shrapnel;
	radiusDamageType = $DamageType::Shrapnel;

	explosion = gunExplosion;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;			 //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;			 //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

	impactImpulse		  = 0;
	verticalImpulse	  = 0;
	explosion = gunExplosion;

	lifetime = 200;
	fadeDelay = 450;
	muzzleVelocity = 50;
	armingDelay = 500;

	hasLight = false;
	brickExplosionForce = 0;
	bounceElasticity	 = 0.7;
	bounceFriction		= 0.2;
	isBallistic			= true;

	particleEmitter = SniperRifleSpotlightBulletTrailEmitter;
	uiName = "Shrapnel Projectile";

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";
};

//////////
// item //
//////////
datablock ItemData(SniperRifleSpotlightItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./sniper rifle v2.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Sniper Rifle Spotlight";
	iconName = "Add-ons/Gamemode_PPE/icons/sniper";
	doColorShift = false;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	 // Dynamic properties defined by the scripts
	image = SniperRifleSpotlightImage;
	canDrop = true;
};


////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(SniperRifleSpotlightImage)
{
	// Basic Item properties
	shapeFile = "./sniper rifle v2.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.38 0.55 -0.35";
	rotation = eulerToMatrix( "0 0 0" );

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = SniperRifleSpotlightItem;
	ammo = " ";
	projectile = SniperRifleSpotlightProjectile;
	projectileType = Projectile;

	goldenImage = SniperRifleSpotlightGoldenImage;

	casing = SniperRifleSpotlightDebris;
	shellExitDir		  = "1.0 0 0.8";
	shellExitOffset	  = "0 0 0";
	shellExitVariance	= 15.0;	
	shellVelocity		 = 3.0;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;
	minShotTime = 2.00;	//minimum time allowed between shots (needed to prevent equip/dequip exploit)

	doColorShift = false;
	colorShiftColor = SniperRifleSpotlightItem.colorShiftColor;//"0.400 0.196 0 1.000";

	//casing = " ";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.15;
	stateTransitionOnTimeout[0]		= "Smoke";
	stateSequence[0]				= "Ready";
	stateSound[0]					= SniperRifleSpotlightUnholsterSound;

	stateName[1]					= "Ready";
	stateTimeoutValue[1]			= 0.14;
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "Fire";
	stateTransitionOnTimeout[2]		= "Smoke";
	stateTimeoutValue[2]			= 0.1;
	stateFire[2]					= true;
	stateAllowImageChange[2]		= false;
	stateSequence[2]				= "Fire";
	stateScript[2]					= "onFire";
	stateWaitForTimeout[2]			= true;
	stateEmitter[2]					= GunFlashEmitter;
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzlePoint";
	stateSound[2]					= gunShot1Sound;

	stateName[3] 					= "Smoke";
	stateEmitter[3]					= SniperRifleSpotlightSmokeEmitter;
	stateEmitterTime[3]				= 0.5;
	stateEmitterNode[3]				= "muzzlePoint";
	stateTimeoutValue[3]			= 0.7;
	stateTransitionOnTimeout[3]		= "Reload";
	stateWaitForTimeout[3]			= true;

	stateName[4]					= "Reload";
	stateSequence[4]				= "Reload";
	stateTimeoutValue[4]			= 1.23;
	stateTransitionOnTimeout[4]		= "PostReload";
	stateSound[4]					= "SniperRifleSpotlightBoltSound";
	stateEjectShell[4]				= true;

	stateName[5]					= "PostReload";
	stateScript[5]					= "onReload";
	stateTransitionOnTimeout[5]		= "Ready";
	stateTimeoutValue[5]			= 0.05;
	stateWaitForTimeout[5]			= true;
};

datablock ShapeBaseImageData(SniperRifleHelicopterImage : SniperRifleSpotlightImage) {
	golden = "";
	eyeOffset = "0 0 0";
};

function SniperRifleSpotlightImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, plant);
	%cl = %obj.client;
	if (!isObject(%cl)) {
		return parent::onFire(%this, %obj, %slot);
	}

		
	if (%cl.tower.guardOption $= $CPB::Classes::Stun) {
		%projectile = SniperRifleSpotlightProjectile;
		%stun = 1;
	} else if (%cl.tower.guardOption $= $CPB::Classes::Shrapnel) {
		%projectile = SniperShrapnelSpotlightProjectile;
		%shrapnel = 1;
	} else {
		%projectile = SniperRifleSpotlightProjectile;
	}

	%initPos = %obj.getMuzzlePoint(%slot);
	%inheritFactor = %projectile.velInheritFactor;
	%objectVelocity = %obj.getVelocity();
	%eyeVector = %obj.getEyeVector();
	%rawMuzzleVector = %obj.getMuzzleVector(%slot);
	%dot = VectorDot(%eyeVector, %rawMuzzleVector);
	%muzzlevector = %obj.getMuzzleVector(%slot);
	if (%dot < 0.6)
	{
		if (VectorLen(%objectVelocity) < 14.0)
		{
			%inheritFactor = 0;
		}
	}
	%gunVel = VectorScale(%projectile.muzzleVelocity, getWord(%obj.getScale(), 2));
	%muzzleVelocity = VectorAdd(VectorScale(%muzzlevector, %gunVel), VectorScale(%objectVelocity, %inheritFactor));

	%p = new Projectile(){
		dataBlock = %projectile;
		initialVelocity = %muzzleVelocity;
		initialPosition = %initPos;
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
		stun = %stun;
		shrapnel = 1;
	};
	MissionCleanup.add(%p);

	%obj.setImageAmmo(%slot, 0);
	return %p;
}

function SniperRifleSpotlightImage::onReload(%this, %obj, %slot)
{
	%obj.setImageAmmo(%slot, 1);
}

function SniperRifleSpotlightImage::onMount(%this, %obj, %slot)
{
	return parent::onMount(%this, %obj, %slot);
}

function SniperRifleSpotlightImage::onUnMount(%this, %obj, $slot)
{
	return parent::onUnMount(%this, %obj, %slot);
}

package SniperRifleSpotlight
{
	function ProjectileData::onCollision(%db, %obj, %col, %fade, %pos, %normal)
	{
		// talk("hitloc: " @ %pos);
		// talk("origin: " @ $s.createBoxAt(%obj.initialPosition, "1 1 1 1", 1));
		if (%db.aimSpotlight || %obj.aimSpotlight) {
			aimSpotlight(%obj);
		}

		if (%obj.shrapnel > 0) {
			spawnShrapnel(%obj.getDatablock(), %pos, %obj);
		}

		if (%obj.stun) {
			spawnStunExplosion(%pos, %obj);
		} else if (%db.getID() == ShrapnelProjectile.getID()) {
			%p = new Projectile() {
				datablock = arrowProjectile;
				initialPosition = %pos;
				client = %obj.client;
			};
			%p.explode();
			serverPlay3D(bulletHitSound, %pos);
		}

		return parent::onCollision(%db, %obj, %col, %fade, %pos, %normal);
	}
};
activatePackage(SniperRifleSpotlight);

function aimSpotlight(%obj) {
	%guardPos = %obj.originPoint;
	%type = $TypeMasks::PlayerObjectType;
	%radius = 12;

	//look for nearby spotlight from firing position
	initContainerRadiusSearch(%guardPos, %radius, %type);
	%target = 0;
	while (isObject(%next = containerSearchNext())) {
		if (%next.getDatablock().getName() $= "SpotlightArmor") {
			%target = %next;
			break;
		}
	}

	//end it here if no spotlight found
	if (!isObject(%target)) {
		echo("Bullet could not find spotlight...");
		return;
	}

	//track player if hit player directly

	if (%col.getClassName() $= "Player" && !%col.isGuard && $CPB::EWSActive) {
		%target.spotLightTarget = %col;
		%target.spotLightTargetLocation = "";
	} else {
		if (!$CPB::EWSActive) {
			%target.spotLightTarget = 0;
			%target.spotLightTargetLocation = %pos;	
			return;
		}
		//do container search for nearby players (4x stud radius) to track
		initContainerRadiusSearch(%pos, 2, $TypeMasks::PlayerObjectType);
		%pl = 0;
		while (isObject(%next = containerSearchNext())) {
			if (%next.getClassName() $= "Player") {
				%pl = %next;
				break;
			}
		}

		if (!isObject(%pl) && !%pl.isGuard) {
			%target.spotLightTarget = 0;
			%target.spotLightTargetLocation = %pos;	
		} else {
			%target.spotLightTarget = %pl;
			%target.spotLightTargetLocation = "";	
		}
	}
}

function spawnShrapnel(%db, %pos, %obj) {
	for (%i = 0; %i < %db.shrapnelCount; %i++) {
		%p = new Projectile() {
			datablock = ShrapnelProjectile;
			initialPosition = %pos;
			initialVelocity = vectorScale(getRandomVector(), ShrapnelProjectile.muzzleVelocity);
			originPoint = %pos;
			client = %obj.client;
			sourceObject = %obj.sourceObject;
			scale = %db.shrapnelScale;
			sourceSlot = %obj.sourceSlot;
		};
		MissionCleanup.add(%p);
	}
}


datablock ParticleData(StunBulletStarParticle)
{
	dragCoefficient		= 8;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.2;
	constantAcceleration = 0.0;
	lifetimeMS			  = 50;
	lifetimeVarianceMS	= 5;
	textureName			 = "base/data/particles/star1";
	spinSpeed		= 0;
	spinRandomMin		= -120.0;
	spinRandomMax		= 120.0;
	colors[0]	  = "1 1 1 1";
	colors[1]	  = "1 1 1 0.2";
	sizes[0]		= 7;
	sizes[1]		= 0.2;

	useInvAlpha = false;
};
datablock ParticleEmitterData(StunBulletStarExplosionEmitter)
{
	lifeTimeMS = 10;

	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset	= 0;
	thetaMin			= -180;
	thetaMax			= 180;
	phiReferenceVel  = 1000;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "StunBulletStarParticle";

	useEmitterColors = true;
	uiName = "Stun Bullet Flash";
};

datablock ParticleData(StunBulletDotParticle)
{
	dragCoefficient		= 8;
	gravityCoefficient	= -0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration = 0.0;
	lifetimeMS			  = 625;
	lifetimeVarianceMS	= 85;
	textureName			 = "base/data/particles/dot";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]	  = "0.6 0.6 0.6 0.8";
	colors[1]	  = "0.5 0.5 0.5 0.0";
	sizes[0]		= 0.1;
	sizes[1]		= 0.12;

	useInvAlpha = false;
};
datablock ParticleEmitterData(StunBulletDotExplosionEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 5.0;
	ejectionOffset	= 0;
	thetaMin			= 0;
	thetaMax			= 180;
	phiReferenceVel  = 0;
	phiVariance		= 360;
	overrideAdvance = false;
	particles = "StunBulletDotParticle";

	useEmitterColors = true;
	uiName = "Stun Bullet Dot";
};

datablock ExplosionData(StunBulletExplosion)
{
	//explosionShape = "";
	soundProfile = bulletHitSound;

	lifeTimeMS = 150;

	particleEmitter = "StunBulletStarExplosionEmitter";
	particleDensity = 5;
	particleRadius = 0.2;

	emitter[0] = "StunBulletDotExplosionEmitter";

	faceViewer	  = true;
	explosionScale = "0.7 0.7 0.7";

	shakeCamera = true;
	camShakeFreq = "6.0 8.0 6.0";
	camShakeAmp = "18.0 18.0 18.0";
	camShakeDuration = 5.5;
	camShakeRadius = 15.0;

	// Dynamic light
	lightStartRadius = 1;
	lightEndRadius = 0;
	lightStartColor = "1 1 1";
	lightEndColor = "1 1 1";
};

datablock ProjectileData(StunBulletProjectile) {
	explosion = StunBulletExplosion;
	explosionScale = "1 1 1";
};

function spawnStunExplosion(%pos, %obj) {

	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (isObject(%pl = %cl.player) && !%cl.isGuard && !%cl.player.isWearingBucket) {
			%eyePos = getWords(%pl.getEyeTransform(), 0, 3);
			%eyeVec = %pl.getEyeVector();
			%angle = mACos(vectorDot(%eyeVec, vectorNormalize(vectorSub(%pos, %eyePos))));
			%dist = VectorLen(vectorSub(%eyePos, %pos));
			// talk(%cl.name SPC %angle);
			// talk("    " SPC %eyevec SPC "eye" SPC vectorSub(%pos, %eyePos) SPC "target");
			if (%dist <= $STUNBLINDRADIUS && %angle < 1.9) {
				%pl.setWhiteOut((($STUNBLINDRADIUS - %dist) / $STUNBLINDRADIUS) + $STUNBLINDBONUS);
			}
			%dist = VectorLen(vectorSub(%pl.getHackPosition(), %pos));
			if (%dist <= $STUNDISTANCE && %pl.getDatablock().getID() != BuffArmor.getID()) {
				stun(%pl, mCeil((($STUNDISTANCE - %dist) / $STUNDISTANCE) * $STUNMAX));
			}
		}
	}

	%p = new Projectile() {
		datablock = StunBulletProjectile;
		initialPosition = %pos;
		client = %obj.client;
	};
	%p.explode();
}

// Member Fields:
//	dataBlock = "gunProjectile"
//	initialPosition = "20.6241 77.2236 0.066668"
//	initialVelocity = "57.7638 -67.9753 -11.946"
//	position = "20.6241 77.2236 0.066668"
//	rotation = "1 0 0 0"
//	scale = "1 1 1"
//	sourceObject = "27310"
//	sourceSlot = "0"
// Tagged Fields:
//	client = "27304"
//	originPoint = "9.53349 90.2749 2.3603"
// Methods:
//	addScheduledEvent() -
//	Bounce() -
//	cancelEvents() -
//	clearEvents() -
//	clearNTObjectName() -
//	clearScopeToClient() - clearScopeToClient(%client)Undo the effects of a scopeToClient() call.
//	delete() - obj.delete()
//	dump() - obj.dump()
//	dumpEvents() -
//	explode() - ()
//	getClassName() - obj.getClassName()
//	getDataBlock() - ()Return the datablock this GameBase is using.
//	getField() -
//	getForwardVector() - Returns a vector indicating the direction this object is facing.
//	getGhostID() -
//	getGroup() - obj.getGroup()
//	getId() - obj.getId()
//	getLastImpactNormal() - ()
//	getLastImpactPosition() - ()
//	getLastImpactVelocity() - ()
//	getName() - obj.getName()
//	getObjectBox() - Returns the bounding box relative to the object's origin.
//	getPosition() - Get position of object.
//	getScale() - Get scaling as a Point3F.
//	getTaggedField() - obj.getTaggedFieldCount(int idx)
//	getTransform() - Get transform of object.
//	getType() - obj.getType()
//	getUpVector() - Returns a vector indicating the relative upward direction of this object.
//	getVelocity() - ()
//	getWorldBox() - Returns six fields, two Point3Fs, containing the min and max points of the worldbox.
//	getWorldBoxCenter() - Returns the center of the world bounding box.
//	inspectPostApply() - ()Simulates clicking 'apply' after making changes in the editor.
//	onAdd() -
//	onCameraEnterOrbit() -
//	onCameraLeaveOrbit() -
//	playSportBallSound() -
//	processInputEvent() -
//	redirect() -
//	save() - obj.save(fileName, <selectedOnly>)
//	schedule() - object.schedule(time, command, <arg1...argN>);
//	scheduleNoQuota() - object.schedule(time, command, <arg1...argN>);
//	scopeToClient() - (NetConnection %client)Cause the NetObject to be forced as scoped on the specified NetConnection.
//	serializeEvent() -
//	serializeEventToString() -
//	setDatablock() - (DataBlock db)Assign this GameBase to use the specified datablock.
//	SetEventEnabled() -
//	setField() -
//	setName() - obj.setName(newName)
//	setNTObjectName() -
//	setScale() - (Point3F scale)
//	setScopeAlways() - Always scope this object on all connections.
//	setTransform() - (Transform T)
//	ToggleEventEnabled() -