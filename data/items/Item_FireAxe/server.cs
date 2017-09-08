//projectile
	AddDamageType("fireAxeDirect",'<bitmap:add-ons/Gamemode_PPE/Item_Prison/Item_chisel/CI_chisel> %1', '%2 <bitmap:add-ons/Gamemode_PPE/Item_Prison/Item_chisel/CI_chisel> %1',1,1);

datablock ProjectileData(fireAxeProjectile)
{
	directDamage  = 20;
	directDamageType  = $DamageType::fireAxeDirect;
	radiusDamageType  = $DamageType::fireAxeDirect;
	explosion  = swordExplosion;

	muzzleVelocity= 50;
	velInheritFactor = 1;

	armingDelay= 0;
	lifetime= 100;
	fadeDelay  = 70;
	bounceElasticity = 0;
	bounceFriction= 0;
	isBallistic= false;
	gravityMod = 0.0;

	hasLight = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";
};


//////////
// item //
//////////
datablock ItemData(fireAxeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./fireAxe.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Fire Axe";
	iconName = "Add-ons/Gamemode_PPE/icons/chisel";
	doColorShift = true;
	colorShiftColor = "0.4 0.4 0.4 1.000";

	image = fireAxeImage;
	canDrop = true;
};

datablock ShapeBaseImageData(fireAxeImage)
{
	// Basic Item properties
	shapeFile = "./fireAxe.dts";
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

	goldenImage = fireAxeGoldenImage;

	// Projectile && Ammo.
	item = fireAxeItem;
	ammo = " ";
	projectile = fireAxeProjectile;
	projectileType = Projectile;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = false;

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
	stateName[0]						= "Activate";
	stateTimeoutValue[0]				= 0.5;
	stateTransitionOnTimeout[0]			= "Ready";
	stateSequence[0]					= "activate";
	stateSound[0]						= weaponSwitchSound;

	stateName[1]						= "Ready";
	stateSequence[1]					= "activate";
	stateTransitionOnTriggerDown[1]		= "Charge";
	stateAllowImageChange[1]			= true;
	
	stateName[2]						= "Charge";
	stateTransitionOnTimeout[2]			= "Armed";
	stateTimeoutValue[2]				= 1.2;
	stateScript[2]						= "onCharge";
	stateWaitForTimeout[2]				= false;
	stateSequence[2]					= "charge";
	stateTransitionOnTriggerUp[2]		= "AbortCharge";
	stateAllowImageChange[2]			= false;
	
	stateName[3]						= "Armed";
	stateScript[3]						= "onArmed";
	stateTransitionOnTriggerUp[3]		= "Fire";
	stateAllowImageChange[3]			= false;

	stateName[4]						= "Fire";
	stateTransitionOnTimeout[4] 		= "Cooldown";
	stateTimeoutValue[4]				= 0.2;
	stateFire[4]						= true;
	stateScript[4]						= "onFire";
	stateSequence[4]					= "attack";
	stateWaitForTimeout[4]				= true;
	stateAllowImageChange[4]			= false;

	stateName[5]						= "AbortCharge";
	stateScript[5]						= "onAbortCharge";
	stateSequence[5]					= "cooldown";
	stateTransitionOnTimeout[5]			= "Ready";
	stateTimeoutValue[5]				= 0.1;

	stateName[6]						= "Cooldown";
	stateTimeoutValue[6]				= 0.8;
	stateTransitionOnTimeout[6]			= "PostCooldown";
	stateSequence[6]					= "cooldown";

	stateName[7]						= "PostCooldown";
	stateTransitionOnTriggerUp[7]		= "Ready";
};

// datablock ShapeBaseImageData(fireAxeLeftImage : fireAxeImage) {
// 	mountPoint = 1;
// };

function fireAxeImage::onMount(%this, %obj, %slot) {
	// if (%slot == 0) {
	// 	if (%obj.getMountedImage(1).getID() == fireAxeLeftImage.getID()) {
	// 		%obj.unMountImage(1);
	// 	}
	// }
	%obj.playthread(1, root);
	return parent::onMount(%this, %obj, %slot);
}

function fireAxeImage::onUnMount(%this, %obj, %slot) {
	// if (%slot == 0) {
	// 	%obj.mountImage(fireAxeImage, 1);
	// }
	return parent::onUnMount(%this, %obj, %slot);
}

// package CPB_Items_FireAxe {
// 	function serverCmdDropTool(%cl, %i) {
// 		if (!isObject(%cl.player)) {
// 			parent::serverCmdDropTool(%cl, %i);
// 		}
		
// 		if (%pl.tool[%i] == FireAxeItem) {
// 			parent::serverCmdDropTool(%cl, %i);
// 			%pl.unMountImage(1);
// 			return;
// 		}
		
// 		parent::serverCmdDropTool(%cl, %i);
// 	}
// };
// activatePackage(CPB_Items_FireAxe);

function fireAxeImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(1, armReadyRight);
}

function fireAxeImage::onArmed(%this, %obj, %slot)
{
	%obj.playthread(2, plant);
	%obj.client.play2D(brickPlantSound);
}

function fireAxeImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, shiftDown);
	%obj.schedule(100, playthread, 1, root);
	Parent::onFire(%this, %obj, %slot);
}

function fireAxeImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, rotateLeft);
	%obj.playthread(1, root);
}
 
