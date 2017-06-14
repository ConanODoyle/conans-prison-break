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
	//statistics
	setStatistic("ChiselAttacks", getStatistic("ChiselAttacks", %obj.client) + 1, %obj.client);
	setStatistic("ChiselAttacks", getStatistic("ChiselAttacks") + 1);

	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
}
function chiselImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, activate);
}

function isBreakableBrick(%brick, %player)
{
	%db = %brick.getDatablock().getName();
	%pole = "brick1x1fHorizPoleData";
	%pole2 = "brick1x1PoleData";
	%plate = "brick1x1fData";
	%window = "Window";
	%window2 = "4x4F_GlassPane";
	%towerSupport = "support";
	if ((%db $= %pole || %db $= %pole2) && (%brick.getColorID() >= 44 && %brick.getColorID() <= 53))
		return 2;
	if (strPos(strLwr(%brick.getName()), "vent") >= 0 && ((%region = getRegion(%brick)) $= "Storage Vent" || %region $= "Laundry"))
		return 2;
	if (%db $= %plate && getRegion(%brick) $= "Outside")
		return 1;
	if ((strPos(%db, %window) >= 0 && strPos(%db, "Door") < 0) || strPos(%db, %window2) >= 0 || strPos(%brick.getName(), %towerSupport) >= 0)
		return 3;
	if (%brick == $Server::PrisonEscape::Generator || %brick == $Server::PrisonEscape::CommDish) {
		return 3;
	}
	
	return 0;
}

function FxDTSBrick::killDelete(%this) {
	if (%this.getDatablock().getName() $= "brick4x4F_GlassPaneData") {
		$Server::PrisonEscape::GeneratorOpened = 1;
	}
	%this.fakeKillBrick((getRandom()-0.5)*20 SPC (getRandom()-0.5)*20 SPC -1, 2);
	%this.schedule(2000, delete);
	serverPlay3D("brickBreakSound", %this.getPosition());

	setStatistic("BricksDestroyed", getStatistic("BricksDestroyed") + 1);

	if (%this.tower > 0) {
		validateTower(%this.tower, %this);
	}
}

package ChiselHit
{
	function chiselProjectile::onCollision(%data, %obj, %col, %fade, %pos, %normal)
	{
		if (%col.getClassName() $= "FxDTSBrick" && %obj.sourceObject.getClassName() $= "Player")
		{
			%type = isBreakableBrick(%col, %obj.sourceObject);
			if (%type > 0)
			{
				//statistics
				setStatistic("ChiselHits", getStatistic("ChiselHits", %obj.client) + 1, %obj.client);
				setStatistic("ChiselHits", getStatistic("ChiselHits") + 1);

				if (%type == 1) {
					setStatistic("PlatesHit", getStatistic("PlatesHit", %obj.client) + 1, %obj.client);
					setStatistic("PlatesHit", getStatistic("PlatesHit") + 1);
					%col.killDelete();
				} else if (%type == 2) {
					setStatistic("PolesHit", getStatistic("PolesHit", %obj.client) + 1, %obj.client);
					setStatistic("PolesHit", getStatistic("PolesHit") + 1);
					%col.killDelete();
				}
				else {
					%col.damage(1, %obj.sourceObject);
				}
				%obj.client.incScore(1);
			}
		}
		return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
	}
};
activatePackage(ChiselHit);

$windowDamage = 10;
$towerDamage = 16;
$towerStages = 4;
$towerColor0 = 60;
$towerColor1 = 59;
$towerColor2 = 57;
$towerColor3 = 55;
$towerColor4 = 56;
$towerColor5 = 55;
$towerColor6 = 54;
$damageFlashColor = 45;

function FxDTSBrick::damage(%brick, %damage, %player)
{
	if(isEventPending(%brick.recolorSchedule))
		cancel(%brick.recolorSchedule);

	if(!%brick.hasOriginalData)
	{
		%brick.origColorID = %brick.getColorID();
		%brick.hasOriginalData = 1;
	}
	if(!%brick.maxDamage)
	{
		%db = %brick.getDatablock().getName();
		if (%brick == $Server::PrisonEscape::CommDish) {
			%brick.maxDamage = 18;
		} else if (strPos(%brick.getName(), "tower") >= 0) {
			%brick.maxDamage = $towerDamage * $towerStages;
			%brick.isTowerSupport = 1;
			%brick.colorStage = 0;
		} else if (%brick == $Server::PrisonEscape::Generator) {
			%brick.maxDamage = 10;
		} else if (%brick.getDatablock().uiname $= "4x4f Glass Pane") {
			%brick.maxDamage = 25;
		} else if (strPos(%brick.getName(), "tower") < 0) {
			%brick.maxDamage = $windowDamage;
		}
	}

	%brick.damage += %damage;
	if (%brick == $Server::PrisonEscape::CommDish) {
		setStatistic("CommDishHit", getStatistic("CommDishHit", %player.client) + 1, %player.client);
		setStatistic("CommDishHit", getStatistic("CommDishHit") + 1);
		%brick.playSound(trayDeflect1Sound);
	} else if (%brick.getDatablock().uiname $= "4x4f Glass Pane") {
		setStatistic("GeneratorWindowsHit", getStatistic("GeneratorWindowsHit", %player.client) + 1, %player.client);
		setStatistic("GeneratorWindowsHit", getStatistic("GeneratorWindowsHit") + 1);
		%brick.playSound("glassChip" @ getRandom(1, 3) @ "Sound");
	} else if (strPos(%brick.getName(), "tower") < 0) {
		setStatistic("WindowsHit", getStatistic("WindowsHit", %player.client) + 1, %player.client);
		setStatistic("WindowsHit", getStatistic("WindowsHit") + 1);
		%brick.playSound("glassChip" @ getRandom(1, 3) @ "Sound");
	} else {
		setStatistic("TowerSupportsHit", getStatistic("TowerSupportsHit", %player.client) + 1, %player.client);
		setStatistic("TowerSupportsHit", getStatistic("TowerSupportsHit") + 1);
	}
	if (%brick == $Server::PrisonEscape::CommDish) {
		if (getRandom() > 0.8) {
			%player.electrocute(2);
		}
	}

	if (%brick.damage >= %brick.maxDamage) {
		if (%brick == $Server::PrisonEscape::CommDish) {
			%brick.spawnExplosion(tankShellProjectile, "0.5 0.5 0.5");
		} else if (strPos(%brick.getName(), "tower") < 0) {
			%brick.playSound(glassExplosionSound);
		}
		%brick.killDelete();
		return;
	}

	if (%brick.isTowerSupport) {
		%brick.colorStage = mFloor(%brick.damage / $towerDamage);
		%brick.origColorID = $towerColor[%brick.colorStage];
	}

	%brick.setColor($damageFlashColor);
	%brick.recolorSchedule = %brick.schedule(50, setColor, %brick.origColorID);
}