datablock AudioProfile(tierfragGrenadeBounceSound)
{
	filename    = "./frag_bounce.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(tierfragGrenadeTossSound)
{
	filename    = "./grenade_toss.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(tierfragGrenadeExplosionSound)
{
   filename    = "./grenade_explode.wav";
   description = AudioDefault3d;
   preload = false;
};

datablock ParticleData(tierstickGrenadeExplosionParticle)
{
	dragCoefficient		= 3.5;
	windCoefficient		= 0;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1900;
	lifetimeVarianceMS	= 900;
	spinSpeed		= 25.0;
	spinRandomMin		= -25.0;
	spinRandomMax		= 25.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/cloud";

	// Interpolation variables
	colors[0]     = "1 1 1 0.1";
	colors[1]     = "0.5 0.5 0.5 0.9";
	colors[2]     = "0.1 0.1 0.1 0.1";
	colors[3]     = "0.05 0.05 0.05 0.0";

	sizes[0]	= 5.0;
	sizes[1]	= 4.3;
	sizes[2]	= 4.5;
	sizes[3]	= 6.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;
};

datablock ParticleEmitterData(tierstickGrenadeExplosionEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifeTimeMS	   = 81;
	ejectionVelocity = 20;
	velocityVariance = 10.0;
	ejectionOffset   = 1.0;
	thetaMin         = -180;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "tierstickGrenadeExplosionParticle";
};

datablock ParticleData(tierstickGrenadeExplosionParticle2)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= -0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 2000;
	lifetimeVarianceMS	= 1990;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]     = "1 0.5 0.0 1";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "1 1 1 0.0";

	sizes[0]	= 0.3;
	sizes[1]	= 0.3;
	sizes[2]	= 0.3;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(tierstickGrenadeExplosionEmitter2)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	lifetimeMS       = 120;
	ejectionVelocity = 12;
	velocityVariance = 12.0;
	ejectionOffset   = 1.0;
	thetaMin         = 0;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "tierstickGrenadeExplosionParticle2";
};


datablock ParticleData(tierstickGrenadeExplosionRingParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 10;
	textureName          = "base/data/particles/star1";
	spinSpeed        = 0.0;
	spinRandomMin        = 0.0;
	spinRandomMax        = 0.0;
	colors[0]     = "1.0 1.0 1 1";
	colors[1]     = "0.9 0.9 0.9 0.9";
	sizes[0]      = 18;
	sizes[1]      = 7;

	useInvAlpha = false;
};

datablock ParticleEmitterData(tierstickGrenadeExplosionRingEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 20;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "tierstickGrenadeExplosionRingParticle";
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

datablock ExplosionData(stickgrenadesubExplosion)
{
	//explosionShape = "";
	soundProfile = "";

	lifeTimeMS = 50;

	emitter[0] = gravityRocketExplosionRingEmitter;
	emitter[1] = gravityRocketExplosionChunkEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 1;
	lightStartColor = "0.3 0.6 0.7";
	lightEndColor = "0 0 0";
};

datablock ExplosionData(tierstickGrenadeExplosion)
{
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
	lifeTimeMS = 150;

	soundProfile = tierFragGrenadeExplosionSound;
	
	emitter[0] = tierstickGrenadeExplosionRingEmitter;
	emitter[1] = tierstickGrenadeExplosionEmitter;
	emitter[2] = StunBulletStarExplosionEmitter;
	//emitter[1] = "";
	//emitter[2] = "";
	//emitter[0] = "";

	subExplosion[0] = stickgrenadeSubExplosion;

	particleEmitter = tierstickGrenadeExplosionEmitter2;
	particleDensity = 90;
//   particleDensity = 0;
	particleRadius = 1.0;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "6.0 8.0 6.0";
	camShakeAmp = "18.0 18.0 18.0";
	camShakeDuration = 5.5;
	camShakeRadius = 15.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0.45 0.3 0.1";
	lightEndColor = "0 0 0";

	//impulse
	impulseRadius = 10;
	impulseForce = 2100;

	damageRadius = 10;
	radiusDamage = 50;

	uiName = "Grenade Explosion";
};

datablock ParticleData(tierfraggrenadeTrailParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1200;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "0.1 0.1 0.1 0";
	colors[1]	= "0.1 0.1 0.1 0.20";
	colors[2]	= "0.1 0.1 0.1 0";
	sizes[0]	= 0.6;
	sizes[1]	= 0.4;
	sizes[2]	= 0.1;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(tierfraggrenadeTrailEmitter)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 2;

	ejectionVelocity = 0; //0.25;
	velocityVariance = 0; //0.10;

	ejectionOffset = 0;

	thetaMin         = 0.0;
	thetaMax         = 90.0;  

	particles = tierfraggrenadeTrailParticle;
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

AddDamageType("TfraggrenadeDirect",   '<bitmap:add-ons/weapon_package_explosive1/ci_conc_grenade> %1',    '%2 <bitmap:add-ons/weapon_package_explosive1/ci_conc_grenade> %1',1,1);

//projectile
datablock ProjectileData(tierfragGrenadeProjectile)
{
	projectileShapeName = "./frag_grenade.dts";
	directDamage        = 20;
	directDamageType  = $DamageType::TfragGrenadeDirect;
	radiusDamageType  = $DamageType::TfragGrenadeDirect;
	impactImpulse	   = 1200;
	verticalImpulse	   = 1200;
	explosion           = tierstickGrenadeExplosion;
	particleEmitter     = tierfragGrenadeTrailEmitter;

	muzzleVelocity      = 55;
	velInheritFactor    = 0;
	explodeOnPlayerImpact = false;
	explodeOnDeath        = true;  

	brickExplosionRadius = 0;
	brickExplosionImpact = false;          //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

	armingDelay         = 2500; 
	lifetime            = 4000;
	fadeDelay           = 3990;
	bounceElasticity    = 0.3;
	bounceFriction      = 0.1;
	isBallistic         = true;
	gravityMod = 1.0;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	uiName = "Grenade";
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////
// item //
//////////
datablock ItemData(tierfragGrenadeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./frag_grenade.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Grenade";
	iconName = "./icon_conc_grenade";
	l4ditemtype = "grenade";

	 // Dynamic properties defined by the scripts
	image = tierfragGrenadeImage;
	canDrop = true;
};

datablock ShapeBaseImageData(tierfragGrenadeImage)
{
	// Basic Item properties
	shapeFile = "./frag_grenade.dts";
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
	item = tierfragGrenadeItem;
	ammo = " ";
	projectile = tierfragGrenadeProjectile;
	projectileType = Projectile;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	//casing = " ";
	doColorShift = false;
	colorShiftColor = "0.400 0.196 0 1.000";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state


	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.01;
	stateTransitionOnTimeout[0]	= "Ready";
	stateScript[0]                  = "onActivate";

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Tick";
	stateScript[1]                  = "onReady";
	stateTransitionOnTimeout[1]	= "Ready";
	stateWaitForTimeout[1]		= false;
	stateTimeoutValue[1]		= 0.4;
	stateAllowImageChange[1]	= true;

	stateName[2]			= "Tick";
	stateTransitionOnTimeout[2]	= "Charge";
	stateAllowImageChange[2]	= false;
	stateSound[2]				= block_moveBrick_Sound;
	stateScript[2]                  = "onTick";
	stateTimeoutValue[2]		= 0.05;

	stateName[3]			= "SKIPC";
	stateTransitionOnTriggerDown[3]	= "Charge";
	stateAllowImageChange[3]	= false;
	
	stateName[4]                    = "Charge";
	stateTransitionOnTimeout[4]	= "Armed";
	stateTimeoutValue[4]            = 0.1;
	stateWaitForTimeout[4]		= false;
	stateScript[4]                  = "onCharge";
	stateAllowImageChange[4]        = true;
	
	stateName[5]			= "AbortCharge";
	stateTransitionOnTimeout[5]	= "Ready";
	stateTimeoutValue[5]		= 0.1;
	stateWaitForTimeout[5]		= true;
	stateScript[5]			= "onAbortCharge";
	stateAllowImageChange[5]	= false;

	stateName[6]			= "Armed";
	stateScript[6]			= "onArmed";
	stateTransitionOnTriggerUp[6]	= "Fire";
	stateWaitForTimeout[6]		= false;
	stateTimeoutValue[6]		= 0.12;
	stateAllowImageChange[6]	= true;

	stateName[7]			= "Fire";
	stateTransitionOnTimeout[7]	= "Activate";
	stateTimeoutValue[7]		= 0.5;
	stateFire[7]			= true;
	stateSound[7]				= tierfraggrenadetossSound;
	stateSequence[7]		= "fire";
	stateScript[7]			= "onFire";
	stateWaitForTimeout[7]		= true;
	stateAllowImageChange[7]	= false;
};

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function tierfragGrenadeImage::onArmed(%this, %obj, %slot)
{
	%obj.playthread(2, shiftAway);
}

function tierfragGrenadeImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

function tierfragGrenadeImage::onFire(%this, %obj, %slot)
{
	 
	Parent::OnFire(%this, %obj, %slot);
	%obj.client.quantity["fragnades"] -= 1;

	serverPlay3D(tierfraggrenadetossSound,%obj.getPosition());
	%obj.removeItem(%obj.currTool);
	%obj.unMountImage(%slot);
}

function tierFragGrenadeImage::onTick(%this, %obj, %slot)
{
	%obj.lasttierfragslot = %obj.currTool;
	%obj.playThread(2, shiftLeft);
	serverPlay3D(BrickMoveSound, %obj.getHackPosition());
}

function tierfraggrenadeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	serverPlay3D(tierfraggrenadeBounceSound,%obj.getTransform());

	if (%obj.explodeticks >= 0) {%obj.explodeticks += 1;}
	if (%obj.explodeticks == 3) {%obj.explode();}
}

function spawnGrenadeStunExplosion(%pos, %obj) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (isObject(%pl = %cl.player) && !%cl.isGuard && !%cl.isWearingBucket) {
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
			if (%dist <= $STUNDISTANCE * 10 && %pl.getDatablock().getID() != BuffArmor.getID()) {
				stun(%pl, mCeil((($STUNDISTANCE * 10 - %dist) / ($STUNDISTANCE * 10)) * $STUNMAX * 5));
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

package GrenadeStunExplosion {
	function ProjectileData::onExplode(%db, %obj, %pos) {

		%ret = parent::onExplode(%db, %obj, %pos);

		if (%db.getID() == tierfragGrenadeProjectile.getID()) {
			spawnGrenadeStunExplosion(%pos, %obj);
		}

		return %ret;
	}
};
activatePackage(GrenadeStunExplosion);