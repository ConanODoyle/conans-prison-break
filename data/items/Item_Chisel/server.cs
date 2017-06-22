//projectile
AddDamageType("chiselDirect",   '<bitmap:add-ons/Gamemode_PPE/Item_Prison/Item_chisel/CI_chisel> %1',       '%2 <bitmap:add-ons/Gamemode_PPE/Item_Prison/Item_chisel/CI_chisel> %1',1,1);

datablock AudioProfile(glassChip1Sound)
{
   filename    = "./glassChip01.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(glassChip2Sound)
{
   filename    = "./glassChip02.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(glassChip3Sound)
{
   filename    = "./glassChip03.wav";
   description = AudioClose3d;
   preload = true;
};


datablock ProjectileData(chiselProjectile)
{
   directDamage        = 8;
   directDamageType  = $DamageType::chiselDirect;
   radiusDamageType  = $DamageType::chiselRadius;
   explosion           = swordExplosion;
   //particleEmitter     = as;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};


//////////
// item //
//////////
datablock ItemData(chiselItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./Chisel.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Chisel";
	iconName = "Add-ons/Gamemode_PPE/icons/chisel";
	doColorShift = true;
	colorShiftColor = "0.4 0.4 0.4 1.000";

	image = chiselImage;
	canDrop = true;
};

datablock ShapeBaseImageData(chiselImage)
{
   // Basic Item properties
   shapeFile = "./chisel.dts";
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

   goldenImage = ChiselGoldenImage;

   // Projectile && Ammo.
   item = chiselItem;
   ammo = " ";
   projectile = chiselProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.4 0.4 0.4 1.000";

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
	
	stateName[2]                    = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateTimeoutValue[2]            = 0.8;
	stateScript[2]			= "oncharge";
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateAllowImageChange[2]        = false;
	
	stateName[3]			= "Armed";
	stateTransitionOnTriggerUp[3]	= "Fire";
	stateAllowImageChange[3]	= false;

	stateName[4]			= "Fire";
	stateTransitionOnTimeout[4]	= "Ready";
	stateTimeoutValue[4]		= 0.2;
	stateFire[4]			= true;
	stateScript[4]			= "onFire";
	stateWaitForTimeout[4]		= true;
	stateAllowImageChange[4]	= false;

	stateName[5]			= "AbortCharge";
	stateScript[5]			= "onAbortCharge";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;
};

function chiselImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}
function chiselImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
}
function chiselImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}