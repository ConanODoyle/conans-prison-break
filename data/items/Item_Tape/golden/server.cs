datablock ItemData(GoldenTapeItem)
{
	category = "Weapon";// Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./TapeFlat.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "GoldenTape";
	iconName = "";
	doColorShift = true;
	colorshiftColor = "1 1 1 1";

	 // Dynamic properties defined by the scripts
	image = GoldenTapeImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

datablock ShapeBaseImageData(GoldenTapeImage)
{
	// Basic Item properties
	shapeFile = "./Tape.dts";
	emap = true;
	rotation = eulerToMatrix("0 90 0");
	offset = "0 0.2 0";

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = GoldenTapeItem;
	ammo = " ";

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	doColorShift = true;
	colorshiftColor = "1 0 0 1";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateAllowImageChange[1]		= true;
	stateTimeoutValue[1]			= 1;
	stateScript[1]					= "onReady";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "mountPoint";
	stateEmitterTime[1]				= 1000;
	stateTransitionOnTimeout[1]		= "Loop";

	stateName[2]					= "Loop";
	stateTransitionOnTimeout[2]		= "Loop1";
	stateTimeoutValue[2]			= 10;
	stateEmitter[2]					= GoldenEmitter;
	stateEmitterNode[2]				= "mountPoint";
	stateEmitterTime[2]				= 1000;

	stateName[3]					= "Loop1";
	stateTransitionOnTimeout[3]		= "Loop";
	stateTimeoutValue[3]			= 10;
	stateEmitter[3]					= GoldenEmitter;
	stateEmitterNode[3]				= "mountPoint";
	stateEmitterTime[3]				= 1000;
};

function GoldenTapeImage::onReady(%this, %obj, %slot) {
	if (isObject(%obj.client)) {
		%obj.client.centerprint("\c6<font:Palatino Linotype:32>\c3Allows you to GoldenTape trays onto other player's backs.<br>\c6 Click their back while holding a tray to use!", 5);
	}
}