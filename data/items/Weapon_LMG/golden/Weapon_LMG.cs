//////////
// item //
//////////
datablock ItemData(LightMachinegunGoldenItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./lmg.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Light MG";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.96 0.89 0.08 1.000";

	 // Dynamic properties defined by the scripts
	image = LightMachinegunGoldenImage;
	canDrop = true;
	
	maxAmmo = 70;
	canReload = 0;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(LightMachinegunGoldenImage)
{
    // Basic Item properties
    shapeFile = "./lmg.dts";
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
    item = LightMachinegunGoldenItem;
    ammo = " ";
    projectile = LightMachinegunProjectile;
    projectileType = Projectile;

    casing = GunShellDebris;
    shellExitDir        = "1.0 0.1 1.0";
    shellExitOffset     = "0 0 0";
    shellExitVariance   = 10.0;	
    shellVelocity       = 5.0;

    //melee particles shoot from eye node for consistancy
    melee = false;
    //raise your arm up or not
    armReady = true;

    doColorShift = true;
    colorShiftColor = LightMachinegunGoldenItem.colorShiftColor;

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
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= "mountPoint";
	stateEmitterTime[0]				= 1000;

	stateName[1]                     = "Ready";
	stateTransitionOnNoAmmo[1]       = "LoadCheckA";
	stateTransitionOnTriggerDown[1]  = "Click";
	stateTransitionOnTimeout[1]      = "LoadCheckA";
	stateTimeoutValue[1]             = 0.3;
	stateWaitForTimeout[1]           = 0;
	stateScript[1]                   = "onReady";
	stateSequence[1]                = "Ready";
	stateAllowImageChange[1]         = true;
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "mountPoint";
	stateEmitterTime[1]				= 1000;

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
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzleNode";
	stateWaitForTimeout[2]			= true;

	stateName[3]			= "Delay";
	stateTransitionOnTimeout[3]     = "FireLoadCheckA";
	stateTimeoutValue[3]            = 0.01;
	stateEmitter[3]					= GoldenEmitter;
	stateEmitterNode[3]				= "mountPoint";
	stateEmitterTime[3]				= 1000;
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	stateEmitter[4]					= GoldenEmitter;
	stateEmitterNode[4]				= "mountPoint";
	stateEmitterTime[4]				= 1000;
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnAmmo[5]		= "Ready";
	stateTransitionOnNoAmmo[5]		= "Reload";
	stateEmitter[5]					= GoldenEmitter;
	stateEmitterNode[5]				= "mountPoint";
	stateEmitterTime[5]				= 1000;

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 0.1;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Wait";
	stateWaitForTimeout[6]			= true;
	stateEmitter[6]					= GoldenEmitter;
	stateEmitterNode[6]				= "mountPoint";
	stateEmitterTime[6]				= 1000;
	
	stateName[7]				= "Wait";
	stateTimeoutValue[7]			= 0.1;
	stateScript[7]				= "onReloadWait";
	stateTransitionOnTimeout[7]		= "Reloaded";
	stateEmitter[7]					= GoldenEmitter;
	stateEmitterNode[7]				= "mountPoint";
	stateEmitterTime[7]				= 1000;
	
	stateName[8]				= "FireLoadCheckA";
	stateScript[8]				= "onLoadCheck";
	stateTimeoutValue[8]			= 0.01;
	stateTransitionOnTimeout[8]		= "FireLoadCheckB";
	stateEmitter[8]					= GoldenEmitter;
	stateEmitterNode[8]				= "mountPoint";
	stateEmitterTime[8]				= 1000;
	
	stateName[9]				= "FireLoadCheckB";
	stateTransitionOnAmmo[9]		= "Smoke";
	stateTransitionOnNoAmmo[9]		= "ReloadSmoke";
	stateEmitter[9]					= GoldenEmitter;
	stateEmitterNode[9]				= "mountPoint";
	stateEmitterTime[9]				= 1000;
	
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
	stateEmitter[12]					= GoldenEmitter;
	stateEmitterNode[12]				= "mountPoint";
	stateEmitterTime[12]				= 1000;

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
	stateEmitter[14]					= GoldenEmitter;
	stateEmitterNode[14]				= "mountPoint";
	stateEmitterTime[14]				= 1000;

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

function getPrisonEscapeTime() {
	return "<font:Arial Bold:34><just:center>\c6" @ getTimeString($Server::PrisonEscape::currTime-1);
}

function LightMachinegunGoldenImage::onFire(%this,%obj,%slot)
{ 
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
	
	if(vectorLen(%obj.getVelocity()) < 0.1 && (getSimTime() - %obj.lastShotTime) > 1000)
	{
		%spread = 0.00056 * (%obj.LMGHeat / $LMGMaxHeat);
	}
	else
	{
		%spread = 0.00085 * (%obj.LMGHeat / $LMGMaxHeat);
	}

	%projectile = LightMachinegunProjectile;
	
	if (%obj.isFiring)
		%obj.playThread(2, plant);
	%shellcount = 1;

	%obj.LMGHeat++;
	%obj.isFiring = 1;

	setStatistic("LMGShotsFired", getStatistic("LMGShotsFired", %obj.client) + 1, %obj.client);
	setStatistic("LMGShotsFired", getStatistic("LMGShotsFired") + 1);

	commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4); 

	%obj.spawnExplosion(TTLittleRecoilProjectile,"1.2 1.2 1.2");

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
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
	return %p;
}

function LightMachinegunGoldenImage::onReloadStart(%this,%obj,%slot)
{           		
	commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4); 
	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule))
	{
		releaseHeat(%obj);
	}
	%obj.isFiring = 0;
}

function LightMachinegunGoldenImage::onReloadWait(%this,%obj,%slot)
{
	commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4); 
}

function LightMachinegunGoldenImage::onReloaded(%this,%obj,%slot)
{
	%obj.isFiring = 0;
}

function LightMachinegunGoldenImage::onHalt(%this,%obj,%slot)
{
	if($Pref::Server::TTAmmo == 0 || $Pref::Server::TTAmmo == 1)
	{
        commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4);
	}
	%obj.isFiring = 0;
}

function LightMachinegunGoldenImage::onMount(%this,%obj,%slot)
{
   	Parent::onMount(%this,%obj,%slot);
	if($Pref::Server::TTAmmo == 0 || $Pref::Server::TTAmmo == 1)
	{
		commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4);
	}
}

function LightMachinegunGoldenImage::onUnMount(%this,%obj,%slot)
{
	%obj.isFiring = 0;
	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule))
	{
		releaseHeat(%obj);
	}
   	Parent::onUnMount(%this,%obj,%slot);
}

function LightMachinegunGoldenImage::onLoadCheck(%this,%obj,%slot)
{
	if(%obj.LMGHeat >= $LMGMaxHeat) 
		%obj.setImageAmmo(%slot,0);
	else
		%obj.setImageAmmo(%slot,1);

	if (%obj.LMGHeat $= "") {
		%obj.LMGHeat = 0;
	}

	if (%obj.LMGHeat <= $LMGMaxHeat / 3) {
		%obj.heatColor = "\c6";
	} else if (%obj.LMGHeat <= $LMGMaxHeat / 4 * 3) {
		%obj.heatColor = "\c3";
	} else {
		%obj.heatColor = "\c0";
	}
	commandToClient(%obj.client,'bottomPrint',getPrisonEscapeTime() @ "<just:right><font:impact:24><color:fff000>Heat <font:impact:28>" @ %obj.heatColor @ %obj.LMGHeat @ "/" @ $LMGMaxHeat, 4, 2, 3, 4);

	if(%obj.LMGHeat >= 1 && !isEventPending(%obj.heatSchedule))
	{
		releaseHeat(%obj);
	}
}

if ($LMGMaxHeat $= "") {
	$LMGMaxHeat = 60;
}

function releaseHeat(%obj) {
	if (isEventPending(%obj.heatSchedule) || %obj.isFiring) {
		return;
	}

	if (%obj.LMGHeat > 0) {
		%obj.LMGHeat--;
		%obj.heatSchedule = schedule(1000, %obj, releaseHeat, %obj);
	}
}

function LightMachinegunProjectile::Damage(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%this.directDamage <= 0.0)
	{
		return;
	}
	%damageType = $DamageType::Direct;
	if (%this.DirectDamageType)
	{
		%damageType = %this.DirectDamageType;
	}
	%scale = getWord(%obj.getScale(), 2);
	%directDamage = mClampF(%this.directDamage, -100.0, 100) * %scale;
	if (%col.getDatablock().getName() $= "BuffArmor")
	{
		%col.Damage(%obj, %pos, 2, %damageType);
	}
	else
	{
		%col.Damage(%obj, %pos, %directDamage, %damageType);
	}
}

