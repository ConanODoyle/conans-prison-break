//audio
datablock AudioProfile(LightMachinegunFire1Sound) {
	filename    = "./LMG_fire.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(LightMachinegunClickSound) {
	filename    = "./LMG_clickofdeath.wav";
	description = AudioClose3d;
	preload = true;
};

datablock ExplosionData(TTLittleRecoilExplosion) {
	explosionShape = "";

	lifeTimeMS = 150;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "1 1 1";
	camShakeAmp = "0.1 0.3 0.2";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;
};

datablock ProjectileData(TTLittleRecoilProjectile) {
	lifetime						= 10;
	fadeDelay						= 10;
	explodeondeath						= true;
	explosion						= TTLittleRecoilExplosion;

};

AddDamageType("LMG", '<bitmap:add-ons/Weapon_Package_Tier2/ci_lmg1> %1', '%2 <bitmap:add-ons/Weapon_Package_Tier2/ci_lmg1> %1', 0.75, 1);
datablock ProjectileData(LightMachinegunProjectile) {
	projectileShapeName = "Add-ons/Weapon_Gun/bullet.dts";
	directDamage        = 20;
	directDamageType    = $DamageType::LMG;
	radiusDamageType    = $DamageType::LMG;

	brickExplosionRadius = 0;
	brickExplosionImpact = true;          //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

	impactImpulse	     = 0;
	verticalImpulse     = 20;
	explosion           = gunExplosion;

	muzzleVelocity      = 200;
	velInheritFactor    = 1;

	armingDelay         = 0;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.0;
	bounceFriction      = 0.0;
	isBallistic         = false;
	gravityMod = 0.1;
	explodeOnDeath = true;
	explodeOnPlayerImpact = false;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";
};

datablock DebrisData(LMGCasing) {
	shapeFile = "./casing.dts";
	lifetime = 8.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.2;
	friction = 0.6;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 2;
};

//////////
// item //
//////////
datablock ItemData(LightMachinegunItem) {
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./lmgv2.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Light MG";
	iconName = "./lightmg";
	doColorShift = true;
	colorShiftColor = "0.4 0.4 0.42 1.000";

	 // Dynamic properties defined by the scripts
	image = LightMachinegunImage;
	canDrop = true;
	
	maxAmmo = 70;
	canReload = 0;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(LightMachinegunImage) {
	// Basic Item properties
	shapeFile = "./lmgv2.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0; //"0.7 1.2 -0.5";
	rotation = eulerToMatrix( "0 0 0" );

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon fires from the muzzle point,
	// we need to turn this on.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = LightMachinegunItem;
	ammo = " ";
	projectile = LightMachinegunProjectile;
	projectileType = Projectile;

	casing = LMGCasing;
	shellExitDir        = "1.0 0.1 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 50.0;	
	shellVelocity       = 3.0;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	doColorShift = true;
	colorShiftColor = LightMachinegunItem.colorShiftColor;

	goldenImage = LightMachinegunGoldenImage;

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.05;
	stateTransitionOnTimeout[0]       = "LoadCheckA";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnNoAmmo[1]       = "LoadCheckA";
	stateTransitionOnTriggerDown[1]  = "Click";
	stateTransitionOnTimeout[1]      = "LoadCheckA";
	stateTimeoutValue[1]             = 0.3;
	stateWaitForTimeout[1]           = 0;
	stateScript[1]                   = "onReady";
	stateSequence[1]                = "Ready";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "FireLoadCheckA";
	stateTimeoutValue[2]            = 0.04;
	stateSound[2]				= LightMachinegunfire1Sound;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateEjectShell[2]       	  = true;
	stateEmitter[2]					= gunFlashEmitter;
	stateEmitterTime[2]				= 0.03;
	stateEmitterNode[2]				= "muzzleNode";
	stateWaitForTimeout[2]			= true;

	stateName[3]			= "Delay";
	stateTransitionOnTimeout[3]     = "FireLoadCheckA";
	stateTimeoutValue[3]            = 0.01;
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnAmmo[5]		= "Ready";
	stateTransitionOnNoAmmo[5]		= "Reload";

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 0.1;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Wait";
	stateWaitForTimeout[6]			= true;
	
	stateName[7]				= "Wait";
	stateTimeoutValue[7]			= 0.1;
	stateScript[7]				= "onReloadWait";
	stateTransitionOnTimeout[7]		= "Reloaded";
	
	stateName[8]				= "FireLoadCheckA";
	stateScript[8]				= "onLoadCheck";
	stateTimeoutValue[8]			= 0.01;
	stateTransitionOnTimeout[8]		= "FireLoadCheckB";
	
	stateName[9]				= "FireLoadCheckB";
	stateTransitionOnAmmo[9]		= "Smoke";
	stateTransitionOnNoAmmo[9]		= "ReloadSmoke";
	
	stateName[10] 				= "Smoke";
	stateEmitter[10]			= gunSmokeEmitter;
	stateEmitterTime[10]			= 0.3;
	stateEmitterNode[10]			= "muzzleNode";
	stateTimeoutValue[10]			= 0.2;
	stateTransitionOnTimeout[10]		= "Halt";
	stateTransitionOnTriggerDown[10]	= "Fire";
	
	stateName[11] 				= "ReloadSmoke";
	stateEmitter[11]			= gunSmokeEmitter;
	stateEmitterTime[11]			= 0.3;
	stateEmitterNode[11]			= "muzzleNode";
	stateTimeoutValue[11]			= 0.2;
	stateTransitionOnTimeout[11]		= "Reload";
	
	stateName[12]				= "Reloaded";
	stateTimeoutValue[12]			= 0.04;
	stateScript[12]				= "onReloaded";
	stateTransitionOnTimeout[12]		= "Ready";

	stateName[13]			= "Halt";
	stateTransitionOnTimeout[13]     = "Ready";
	stateTimeoutValue[13]            = 0.1;
	stateEmitter[13]					= gunSmokeEmitter;
	stateEmitterTime[13]				= 0.48;
	stateEmitterNode[13]				= "muzzleNode";
	stateScript[13]                  = "onHalt";

	stateName[14]                     = "Click";
	stateTransitionOnTimeout[14]      = "Fire";
	stateTimeoutValue[14]             = 0.2;
	stateWaitForTimeout[14]           = 1;
	stateScript[14]                   = "onClick";
	stateAllowImageChange[14]         = true;
	stateSound[14]				= LightMachinegunClickSound;

	// stateName[15]                    = "Fire2";
	// stateTransitionOnTimeout[15]     = "Delay";
	// stateTimeoutValue[15]            = 0.03;
	// stateSound[15]				= LightMachinegunfire1Sound;
	// stateFire[15]                    = true;
	// stateAllowImageChange[15]        = false;
	// stateSequence[15]                = "Fire";
	// stateScript[15]                  = "onFire2";
	// stateEjectShell[15]       	  = true;
	// stateEmitter[15]					= gunFlashEmitter;
	// stateEmitterTime[15]				= 0.05;
	// stateEmitterNode[15]				= "muzzleNode";
	// stateWaitForTimeout[15]			= true;

};

function LightMachinegunImage::onFire(%this,%obj,%slot) { 
	%fX = getWord(%fvec,0);
	%fY = getWord(%fvec,1);
	
	%evec = %obj.getEyeVector();
	%eX = getWord(%evec,0);
	%eY = getWord(%evec,1);
	%eZ = getWord(%evec,2);
	
	%eXY = mSqrt(%eX*%eX+%eY*%eY);
	
	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;

	%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,"-1")));
	
	%obj.lastShotTime = getSimTime();
	%shellcount = 1;
	
	if(vectorLen(%obj.getVelocity()) < 0.1 && (getSimTime() - %obj.lastShotTime) > 1000) {
		%spread = 0.00026 + 0.002 * (%obj.LMGHeat / $LMGMaxHeat);
	} else {
		%spread = 0.00026 + 0.002 * (%obj.LMGHeat / $LMGMaxHeat);
	}

	%projectile = LightMachinegunProjectile;
	
	if (%obj.isFiring) {
		%obj.playThread(2, plant);
	}
	%shellcount = 1;

	%obj.LMGHeat++;
	%obj.isFiring = 1;

	%obj.client.bottomPrintInfo();

	%obj.spawnExplosion(TTLittleRecoilProjectile,"1.2 1.2 1.2");

	for(%shell=0; %shell<%shellcount; %shell++) {
		%vector = %obj.getMuzzleVector(%slot);
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)() {
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function LightMachinegunImage::onReloadStart(%this,%obj,%slot) {           		
	%obj.client.bottomPrintInfo();
	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule)) {
		releaseHeat(%obj);
	}
	%obj.isFiring = 0;
}

function LightMachinegunImage::onReloadWait(%this,%obj,%slot) {
	%obj.client.bottomPrintInfo();
}

function LightMachinegunImage::onReloaded(%this,%obj,%slot) {
	%obj.isFiring = 0;
}

function LightMachinegunImage::onHalt(%this,%obj,%slot) {
        %obj.client.bottomPrintInfo();
	%obj.isFiring = 0;
}

function LightMachinegunImage::onMount(%this,%obj,%slot) {
   	Parent::onMount(%this,%obj,%slot);
	%obj.client.bottomPrintInfo();
}

function LightMachinegunImage::onUnMount(%this,%obj,%slot) {
	%obj.isFiring = 0;
	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule)) {
		releaseHeat(%obj);
	}
   	Parent::onUnMount(%this,%obj,%slot);
}

function LightMachinegunImage::onLoadCheck(%this,%obj,%slot) {
	if(%obj.LMGHeat >= $LMGMaxHeat) {
		%obj.setImageAmmo(%slot,0);
	} else {
		%obj.setImageAmmo(%slot,1);
	}

	if (%obj.LMGHeat $= "") {
		%obj.LMGHeat = 0;
	}

	%obj.client.bottomPrintInfo();

	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule)) {
		releaseHeat(%obj);
	}
}


$LMGMaxHeat = 70;
$LMGHtRchgTime = 900;
$LMGHtRchgScaling = 20;
$LMGHtRchgScalingMax = 600;
$LMGHtRchgScalingMaxHtPrct = 30;

function releaseHeat(%obj) {
	if (isEventPending(%obj.heatSchedule) || %obj.isFiring) {
		%obj.scalingRecharge = 0;
		return;
	}

	if (%obj.LMGHeat > 0) {
		%obj.LMGHeat--;
		%obj.heatSchedule = schedule($LMGHtRchgTime - %obj.scalingRecharge, %obj, releaseHeat, %obj);
		%obj.scalingRecharge += $LMGHtRchgScaling + ($LMGHtRchgScalingMaxHtPrct * ($LMGMaxHeat - %obj.LMGHeat) / $LMGMaxHeat);
		%obj.scalingRecharge = ($LMGHtRchgScalingMax < %obj.scalingRecharge ? $LMGHtRchgScalingMax : %obj.scalingRecharge);
	} else {
		%obj.scalingRecharge = 0;
	}
	%obj.client.bottomPrintInfo();
}

function LightMachinegunProjectile::Damage(%this, %obj, %col, %fade, %pos, %normal) {
	if (%this.directDamage <= 0.0) {
		return;
	}
	%damageType = $DamageType::Direct;
	if (%this.DirectDamageType) {
		%damageType = %this.DirectDamageType;
	}
	%scale = getWord(%obj.getScale(), 2);
	%directDamage = mClampF(%this.directDamage, -100.0, 100) * %scale;
	if (%col.getDatablock().getName() $= "BuffArmor") {
		%col.Damage(%obj, %pos, %directDamage - 2, %damageType);
	} else {
		%col.Damage(%obj, %pos, %directDamage, %damageType);
	}
}

