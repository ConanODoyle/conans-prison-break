//////////
// item //
//////////
datablock ItemData(tearGasGrenadeGoldenItem)
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
	uiName = "Golden Tear Gas Launcher";
	iconName = "Add-ons/Gamemode_PPE/icons/smoke";
	doColorShift = true;
	colorShiftColor = "0.5 0.2 0.2 1.000";

	 // Dynamic properties defined by the scripts
	image = tearGasGrenadeGoldenImage;
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
datablock ShapeBaseImageData(tearGasGrenadeGoldenImage)
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
	item = tearGasGrenadeGoldenItem;
	ammo = " ";
	projectile = tearGasGrenadeProjectile;
	projectileType = Projectile;

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
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= "mountPoint";
	stateEmitterTime[0]				= 1000;

	stateName[1]			= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateTransitionOnTimeout[1] = "DisplayAmmo";
	stateTimeoutValue[1]		= 0.01;
	stateAllowImageChange[1]	= true;
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "mountPoint";
	stateEmitterTime[1]				= 1000;
	
	stateName[2]						  = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateTimeoutValue[2]				= 0.5;
	stateScript[2]			= "onCharge";
	stateWaitForTimeout[2]		= true;
	stateAllowImageChange[2]		  = true;
	stateEmitter[2]					= GoldenEmitter;
	stateEmitterNode[2]				= "mountPoint";
	stateEmitterTime[2]				= 1000;
	
	stateName[3]			= "Armed";
	stateScript[3]			= "onArmed";
	stateTransitionOnTimeout[3]	= "Ready";
	stateTimeoutValue[3]	= 0.10;
	stateAllowImageChange[3]	= true;
	stateEmitter[3]					= GoldenEmitter;
	stateEmitterNode[3]				= "mountPoint";
	stateEmitterTime[3]				= 1000;

	stateName[4]			= "Fire";
	stateTransitionOnTimeout[4]	= "AmmoCheck";
	stateTimeoutValue[4]		= 0.2;
	stateFire[4]			= true;
	stateSound[4]			= tearGasGrenadeFireSound;
	stateScript[4]			= "onFire";
	stateWaitForTimeout[4]		= true;
	stateAllowImageChange[4]	= true;
	stateEmitter[4]					= GoldenEmitter;
	stateEmitterNode[4]				= "mountPoint";
	stateEmitterTime[4]				= 1000;

	stateName[5]			= "AbortCharge";
	stateScript[5]			= "onAbortCharge";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;
	stateEmitter[5]					= GoldenEmitter;
	stateEmitterNode[5]				= "mountPoint";
	stateEmitterTime[5]				= 1000;

	stateName[6]			= "AmmoCheck";
	stateScript[6]			= "onAmmoCheck";
	stateTransitionOnTimeout[6]	= "PostAmmoCheck";
	stateTimeoutValue[6]		= 0.01;
	stateEmitter[6]					= GoldenEmitter;
	stateEmitterNode[6]				= "mountPoint";
	stateEmitterTime[6]				= 1000;

	stateName[7]			= "PostAmmoCheck";
	stateTransitionOnAmmo[7]	= "Reload";
	stateTransitionOnNoAmmo[7]	= "NoAmmo";
	stateEmitter[7]					= GoldenEmitter;
	stateEmitterNode[7]				= "mountPoint";
	stateEmitterTime[7]				= 1000;

	stateName[8]			= "Reload";
	stateSequence[8]		= "Reload";
	stateTimeoutValue[8]	= 1.0;
	stateSound[8]			= tearGasGrenadeLoadSound;
	stateTransitionOnTimeout[8]	= "Charge";
	stateEmitter[8]					= GoldenEmitter;
	stateEmitterNode[8]				= "mountPoint";
	stateEmitterTime[8]				= 1000;

	stateName[9]			= "NoAmmo";
	stateSequence[9]		= "Ready";
	stateTimeoutValue[9]	= 0.1;
	stateTransitionOnTimeout[9]	= "AmmoCheck";
	stateEmitter[9]					= GoldenEmitter;
	stateEmitterNode[9]				= "mountPoint";
	stateEmitterTime[9]				= 1000;

	stateName[10]			= "DisplayAmmo";
	stateScript[10]			= "onAmmoCheck";
	stateTransitionOnTimeout[10]	= "Ready";
	stateTimeoutValue[10]		= 0.01;
	stateTransitionOnTriggerDown[10]	= "Fire";
	stateTimeoutValue[10]		= 0.01;
};

function tearGasGrenadeGoldenImage::onMount(%this, %obj, %slot) {
	if (!%obj.hasSeenTearGasMessage) {
		messageClient(%obj.client, '', "<font:Arial Bold:24>\c3Use tear gas to blind, hurt, and slow prisoners who walk through it! Lasts " @ mFloor($tearGasTime / 1000) @ "seconds.");
	}
	%obj.hasSeenTearGasMessage = 1;
	return parent::onMount(%this, %obj, %slot);
}

function tearGasGrenadeGoldenImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, shiftRight);
	serverPlay3D(brickRotateSound, %obj.getHackPosition());
}

function tearGasGrenadeGoldenImage::onArmed(%this, %obj, %slot)
{
	%obj.playthread(2, plant);
	serverPlay3D(brickPlantSound, %obj.getHackPosition());
}

function tearGasGrenadeGoldenImage::onFire(%this, %obj, %slot)
{
	//statistics
	%obj.totalTearGasShots++;
	setStatistic("TearGasGrenadeGoldensThrown", getStatistic("TearGasGrenadeGoldensThrown", %obj.client) + 1, %obj.client);
	setStatistic("TearGasGrenadeGoldensThrown", getStatistic("TearGasGrenadeGoldensThrown") + 1);

	%obj.playthread(2, plant);
	%ret = Parent::onFire(%this, %obj, %slot);

	// %currSlot = %obj.currTool;
	// %obj.tool[%currSlot] = 0;
	// %obj.weaponCount--;
	// messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	// serverCmdUnUseTool(%obj.client);

	return %ret;
}

function tearGasGrenadeGoldenImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}

function tearGasGrenadeGoldenImage::onAmmoCheck(%this, %obj, %slot)
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