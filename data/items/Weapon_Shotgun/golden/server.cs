datablock ItemData(PumpShotgunGoldenItem)
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
	uiName = "Pump Shotgun";
	iconName = "./pumpshotgun";
	doColorShift = true;
	colorShiftColor = "0.95 0.9 0.1 1.000";

	// Dynamic properties defined by the scripts
	image = PumpShotgunGoldenImage;
	canDrop = true;
	
	maxAmmo = 6;
	canReload = 1;
};

////////////////
//weapon image//
////////////////

datablock ShapeBaseImageData(PumpShotgunGoldenImage)
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
   item = PumpShotgunGoldenItem;
   ammo = " ";
   projectile = PumpShotgunGoldenProjectile;
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

   doColorShift = true;
   colorShiftColor = PumpShotgunGoldenItem.colorShiftColor;

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
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= "mountPoint";
	stateEmitterTime[0]				= 1000;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "FireCheckA";
	stateTransitionOnNoAmmo[1]		= "ReloadCheckA";
	stateScript[1]					= "onReady";
	stateAllowImageChange[1]		= true;
	stateSequence[1]				= "ready";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "mountPoint";
	stateEmitterTime[1]				= 1000;

	stateName[2]					= "Fire";
	stateTransitionOnTimeout[2]		= "Smoke";
	stateTimeoutValue[2]			= 0.1;
	stateFire[2]					= true;
	stateAllowImageChange[2]		= false;
	stateScript[2]					= "onFire";
	stateWaitForTimeout[2]			= true;

	stateName[3]					= "Smoke";
	stateTimeoutValue[3]			= 0.4;
	stateTransitionOnTimeout[3]		= "Eject";

	stateName[4]					= "Eject";
	stateTimeoutValue[4]			= 0.5;
	stateTransitionOnTimeout[4]		= "LoadCheckA";
	stateWaitForTimeout[4]			= true;
	stateEjectShell[4]				= true;
	stateSequence[4]				= "pump";
	stateSound[4]					= PumpShotgunReloadSound;
	stateScript[4]					= "onEject";
	stateEmitter[4]					= GoldenEmitter;
	stateEmitterNode[4]				= "mountPoint";
	stateEmitterTime[4]				= 1000;
	
	stateName[5]					= "LoadCheckA";
	stateScript[5]					= "onLoadCheck";
	stateTransitionOnTriggerUp[5]	= "LoadCheckB";
						
	stateName[6]					= "LoadCheckB";
	stateTransitionOnAmmo[6]		= "Ready";
	stateTransitionOnNoAmmo[6]		= "Reload";
	
	stateName[7]					= "ReloadCheckA";
	stateScript[7]					= "onReloadCheck";
	stateTimeoutValue[7]			= 0.01;
	stateTransitionOnTimeout[7]		= "ReloadCheckB";
						
	stateName[8]					= "ReloadCheckB";
	stateTransitionOnAmmo[8]		= "CompleteReload";
	stateTransitionOnNoAmmo[8]		= "Reload";
						
	stateName[9]					= "ForceReload";
	stateTransitionOnTimeout[9]		= "ForceReloaded";
	stateTimeoutValue[9]			= 0.25;
	stateSequence[9]				= "Reload";
	stateSound[9]					= Block_MoveBrick_Sound;
	stateScript[9]					= "onReloadStart";
	
	stateName[10]					= "ForceReloaded";
	stateTransitionOnTimeout[10]	= "ReloadCheckA";
	stateTimeoutValue[10]			= 0.2;
	stateScript[10]					= "onReloaded";
	
	stateName[11]					= "Reload";
	stateTransitionOnTimeout[11]	= "Reloaded";
	stateTransitionOnTriggerDown[11]= "Fire";
	stateWaitForTimeout[11]			= false;
	stateTimeoutValue[11]			= 0.25;
	stateSequence[11]				= "Reload";
	stateScript[11]					= "onReloadStart";
	
	stateName[12]					= "Reloaded";
	stateTransitionOnTimeout[12]	= "ReloadCheckA";
	stateWaitForTimeout[12]			= false;
	stateTimeoutValue[12]			= 0.2;
	stateScript[12]					= "onReloaded";

	stateName[13]					= "CompleteReload";
	stateTimeoutValue[13]			= 0.5;
	stateWaitForTimeout[13]			= true;
	stateTransitionOnTimeout[13]	= "Ready";
	stateSequence[13]				= "fire";
	stateSound[13]					= PumpShotgunReloadSound;
	stateScript[13]					= "onEject";

	stateName[14]					= "FireCheckA";
	stateScript[14]					= "onLoadCheck";
	stateTimeoutValue[14]			= 0.01;
	stateTransitionOnTimeout[14]	= "FireCheckB";
						
	stateName[15]					= "FireCheckB";
	stateTransitionOnAmmo[15]		= "Fire";
	stateTransitionOnNoAmmo[15]		= "Reload";
};

function PumpShotgunGoldenImage::onFire(%this,%obj,%slot) {
	if(%obj.toolAmmo[%obj.currTool]					> 0) {
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
		%obj.playThread(2, activate);

		%obj.toolAmmo[%obj.currTool]--;

		%obj.spawnExplosion(TTLittleRecoilProjectile, "1.5 1.5 1.5");
            		

		%projectile = %this.projectile;
		%spread = 0.0035;
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
	} else {
		serverPlay3D(PumpShotgunJamSound,%obj.getPosition());
	}
}

function PumpShotgunGoldenImage::onEject(%this,%obj,%slot)
{
	%obj.playThread(2, plant);
	%this.onLoadCheck(%obj,%slot);
}

function PumpShotgunGoldenImage::onReloadStart(%this,%obj,%slot)
{       		
 	%obj.playThread(2, shiftto);
    serverPlay3D(block_MoveBrick_Sound, %obj.getPosition());
}

function PumpShotgunGoldenImage::onReloaded(%this,%obj,%slot) {
	%this.onLoadCheck(%obj,%slot);
    %obj.toolAmmo[%obj.currTool]++;
}
