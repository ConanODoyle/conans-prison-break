datablock ItemData(SteakItem)
{
	category = "Weapon";// Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./steak.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Steak";
	iconName = "Add-ons/Gamemode_PPE/icons/steak";
	doColorShift = true;
	colorshiftColor = "1 1 1 1";

	 // Dynamic properties defined by the scripts
	image = SteakImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

datablock ShapeBaseImageData(SteakHatImage)
{
	shapeFile = "./steak.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 0.1";
	eyeOffset = "0 0 0.18";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = true;
	colorshiftColor = "1 0 0 1";
};

datablock ShapeBaseImageData(SteakImage)
{
	// Basic Item properties
	shapeFile = "./steak.dts";
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
	item = SteakItem;
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
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2]					= "Fire";
	stateTimeoutValue[2]			= 1;
	stateTransitionOnTriggerUp[2]	= "Ready";
	stateWaitForTimeout[2]			= true;
	stateScript[2]					= "onFire";
};
datablock ShapeBaseImageData(SteakDogImage : SteakImage)
{
	rotation = eulerToMatrix("90 90 0");
	offset = "0 0 0.1";
};
exec("./Emote_Heal.cs");

package SteakItemCollision
{
	function Armor::onCollision(%this, %obj, %col, %a, %b, %c, %d, %e, %f)
	{
		%db = %col.getDatablock().getName();
		if (%db $= "SteakItem" && !isObject(%col.spawnBrick) && %obj.getDatablock().getName() $= "ShepherdDogHoleBot" && !%obj.isDisabled())
		{
			if (%obj.isEating)
				return;
			EatSteak(%obj, %col.timeToFinish);
			//talk("Attempting to delete steak");
			%col.delete();
			%obj.hPathBrick = "";
		}
		else
			parent::onCollision(%this, %obj, %col, %a, %b, %c, %d, %e, %f);
	}

	function SteakItem::onPickup(%this, %item, %player)
	{
		return parent::onPickup(%this, %item, %player);
	}

	function serverCmdDropTool(%client, %slot)
	{
		$justDroppedItem = 1;
		parent::serverCmdDropTool(%client, %slot);
		$justDroppedItem = 0;
	}

	function ItemData::onAdd(%this, %obj) {
		%ret = parent::onAdd(%this, %obj);
		if (%this.getName() $= "SteakItem" && !isObject(%obj.spawnBrick)) {
			$SteakGroup.schedule(1, add, %obj);
			if ($justDroppedItem) {
				%obj.timeToFinish = 15000;
			}
		}
		return %ret;
	}
};
activatePackage(SteakItemCollision);

function SteakImage::onFire(%this, %obj, %slot)
{
	serverCmdDropTool(%obj.client, %obj.currTool);
}

if (!isObject($SteakGroup)) {
	$SteakGroup = new ScriptGroup()
	{
		class = "SteakGroup";
	};
}
