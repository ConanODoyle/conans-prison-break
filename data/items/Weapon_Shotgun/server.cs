//audio
datablock AudioProfile(PumpShotgunFireSound)
{
   filename    = "./shotgun_fire.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(PumpShotgunReloadSound)
{
   filename    = "./shotgun_reload.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(PumpShotgunJamSound)
{
   filename    = "./shotgun_jam.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ParticleData(shotgunExplosionParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = 1;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 700;
	lifetimeVarianceMS   = 400;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]					= "0.9 0.9 0.9 0.3";
	colors[1]					= "0.9 0.5 0.6 0.0";
	sizes[0]					= 2.25;
	sizes[1]					= 2.75;

	useInvAlpha = true;
};
datablock ParticleEmitterData(shotgunExplosionEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "shotgunExplosionParticle";

   useEmitterColors = true;
};


datablock ParticleData(shotgunExplosionRingParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 50;
	lifetimeVarianceMS   = 35;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 500.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]					= "1 1 0.0 0.9";
	colors[1]					= "0.9 0.0 0.0 0.0";
	sizes[0]					= 3;
	sizes[1]					= 3;

	useInvAlpha = false;
};
datablock ParticleEmitterData(shotgunExplosionRingEmitter)
{
	lifeTimeMS = 50;

   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "shotgunExplosionRingParticle";

   useEmitterColors = false;
};

datablock ExplosionData(shotgunExplosion)
{
   //explosionShape = "";
	soundProfile = bulletHitSound;

   lifeTimeMS = 150;

   particleEmitter = shotgunExplosionEmitter;
   particleDensity = 5;
   particleRadius = 0.2;

   emitter[0]					= shotgunExplosionRingEmitter;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 2;
   lightStartColor = "0.3 0.6 0.7";
   lightEndColor = "0 0 0";

   impulseRadius = 2;
   impulseForce = 1000;
};

//shell
datablock DebrisData(PumpShotgunShellDebris)
{
	shapeFile = "./shotgunShell.dts";
	lifetime = 2.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

AddDamageType("PumpShotgun",   '<bitmap:add-ons/Weapon_Package_Tier1/CI_L4Shotgun> %1',    '%2 <bitmap:add-ons/Weapon_Package_Tier1/CI_L4Shotgun> %1',0.75,1);
datablock ProjectileData(PumpShotgunProjectile)
{
   projectileShapeName = "add-ons/Weapon_Gun/bullet.dts";
   directDamage        = 9; //14;
   directDamageType    = $DamageType::PumpShotgun;
   radiusDamageType    = $DamageType::PumpShotgun;

   brickExplosionRadius = 0.2;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 15;
   brickExplosionMaxVolume = 20;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 30;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	= 200;
   verticalImpulse     = 100;
   explosion           = gunExplosion;

   muzzleVelocity      = 100;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 0.4;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};

datablock ProjectileData(ShotgunBlastProjectile : PumpShotgunProjectile)
{
   projectileShapeName = "add-ons/Vehicle_Tank/tankbullet.dts";
   directDamage        = 20; //14;
   directDamageType    = $DamageType::PumpShotgun;
   radiusDamageType    = $DamageType::PumpShotgun;

   brickExplosionRadius = 0.4;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 30;
   brickExplosionMaxVolume = 25;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 35;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	= 300;
   verticalImpulse     = 100;
   explosion           = shotgunExplosion;

   muzzleVelocity      = 100;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 70;
   fadeDelay           = 0;
   isBallistic         = true;
   gravityMod = 0.0;
};

//////////
// item //
//////////
datablock ItemData(PumpShotgunItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	// Basic Item Properties
	shapeFile = "./shotgun.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Shotgun";
	iconName = "./pumpshotgun";
	doColorShift = true;
	colorShiftColor = "0.3 0.3 0.31 1.000";

	// Dynamic properties defined by the scripts
	image = PumpShotgunImage;
	canDrop = true;
	
	maxAmmo = 6;
	canReload = 1;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(PumpShotgunImage)
{
   // Basic Item properties
   shapeFile = "./shotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
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
   item = PumpShotgunItem;
   ammo = " ";
   projectile = PumpShotgunProjectile;
   projectileType = Projectile;

   casing = PumpShotgunShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 1000;

   goldenImage = PumpShotgunGoldenImage;

   doColorShift = true;
   colorShiftColor = PumpShotgunItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.15;
	stateSequence[0]				= "Activate";
	stateTransitionOnTimeout[0]		= "Eject";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "FireCheckA";
	stateTransitionOnNoAmmo[1]		= "ReloadCheckA";
	stateScript[1]					= "onReady";
	stateAllowImageChange[1]		= true;
	stateSequence[1]				= "ready";

	stateName[2]					= "Fire";
	stateTransitionOnTimeout[2]		= "Smoke";
	stateTimeoutValue[2]			= 0.1;
	stateFire[2]					= true;
	stateAllowImageChange[2]		= false;
	stateScript[2]					= "onFire";
	stateWaitForTimeout[2]			= true;

	stateName[3]					= "Smoke";
	stateTimeoutValue[3]			= 0.3;
	stateTransitionOnTimeout[3]		= "Eject";

	stateName[4]					= "Eject";
	stateTimeoutValue[4]			= 0.2;
	stateTransitionOnTimeout[4]		= "LoadCheckA";
	stateWaitForTimeout[4]			= true;
	stateEjectShell[4]				= true;
	stateSequence[4]				= "pump";
	stateSound[4]					= PumpShotgunReloadSound;
	stateScript[4]					= "onEject";
	
	stateName[5]					= "LoadCheckA";
	stateScript[5]					= "onLoadCheck";
	stateTransitionOnTriggerUp[5]	= "LoadCheckB";
						
	stateName[6]					= "LoadCheckB";
	stateTransitionOnTimeout[6]		= 0.35;
	stateWaitForTimeout[6]			= true;
	stateTransitionOnAmmo[6]		= "Ready";
	stateTransitionOnNoAmmo[6]		= "Reload";
	
	stateName[7]					= "ReloadCheckA";
	stateTransitionOnTriggerDown[7]	= "Fire";
	stateScript[7]					= "onReloadCheck";
	stateTimeoutValue[7]			= 0.35;
	stateTransitionOnTimeout[7]		= "ReloadCheckB";
						
	stateName[8]					= "ReloadCheckB";
	stateTransitionOnTriggerDown[8]	= "Fire";
	stateTransitionOnAmmo[8]		= "CompleteReload";
	stateTransitionOnNoAmmo[8]		= "Reload";

	stateName[9]					= "Reload";
	stateTransitionOnTimeout[9]		= "Reloaded";
	stateWaitForTimeout[9]			= false;
	stateTimeoutValue[9]			= 0.25;
	stateTransitionOnTriggerDown[9]	= "Fire";
	stateSequence[9]				= "Reload";
	stateScript[9]					= "onReloadStart";
	
	stateName[10]					= "Reloaded";
	stateTransitionOnTimeout[10]	= "ReloadCheckA";
	stateWaitForTimeout[10]			= false;
	stateTimeoutValue[10]			= 0.2;
	stateScript[10]					= "onReloaded";

	stateName[11]					= "CompleteReload";
	stateTimeoutValue[11]			= 0.5;
	stateWaitForTimeout[11]			= true;
	stateTransitionOnTimeout[11]	= "Ready";
	stateSequence[11]				= "fire";
	stateSound[11]					= PumpShotgunReloadSound;
	stateScript[11]					= "onEject";

	stateName[12]					= "FireCheckA";
	stateScript[12]					= "onLoadCheck";
	stateTimeoutValue[12]			= 0.01;
	stateTransitionOnTimeout[12]	= "FireCheckB";
						
	stateName[13]					= "FireCheckB";
	stateTransitionOnAmmo[13]		= "Fire";
	stateTransitionOnNoAmmo[13]		= "Reload";
};

function PumpShotgunImage::onFire(%this,%obj,%slot) {
	if(%obj.shotgunAmmo > 0) {
		%fvec = %obj.getForwardVector();
		%fX = getWord(%fvec,0);
		%fY = getWord(%fvec,1);

		%evec = %obj.getEyeVector();
		%eX = getWord(%evec,0);
		%eY = getWord(%evec,1);
		%eZ = getWord(%evec,2);

		%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
		%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
		serverPlay3D(PumpShotgunfireSound,%obj.getPosition());
		%obj.playThread(2, plant);

		%obj.shotgunAmmo--;
    	%obj.client.bottomprintInfo();

		%obj.spawnExplosion(TTLittleRecoilProjectile, "2.5 2.5 2.5");
            		

		%projectile = %this.projectile;
		%spread = 0.0038;
		%shellcount = 10;

		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);

			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
	
		//shotgun blast projectile: only effective at point blank, sends targets flying off into the distance
		//
		//more or less represents the concussion blast. i can only assume such a thing exists because
		// i've never stood infront of a fucking shotgun before
		///////////////////////////////////////////////////////////

		%projectile = "shotgunBlastProjectile";
	
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
	

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		return %p;
	}
}

function PumpShotgunImage::onEject(%this,%obj,%slot)
{
	%obj.playThread(2, plant);
	%this.onLoadCheck(%obj,%slot);
}

function PumpShotgunImage::onReloadStart(%this,%obj,%slot)
{
	serverPlay3D(PumpShotgunJamSound,%obj.getPosition());
 	%obj.playThread(2, shiftRight);
}

function PumpShotgunImage::onReloaded(%this,%obj,%slot) {
	%this.onLoadCheck(%obj,%slot);
    %obj.shotgunAmmo++;
    %obj.client.bottomprintInfo();
}
