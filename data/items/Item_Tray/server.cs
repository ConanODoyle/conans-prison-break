datablock AudioProfile(TapeSound)
{
	filename = "./Bandage.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(trayDeflect1Sound)
{
   filename    = "./tray1.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(trayDeflect2Sound)
{
   filename    = "./tray2.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(trayDeflect3Sound)
{
   filename    = "./tray3.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(trayEquipSound)
{
   filename    = "./tray_pullup1.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ItemData(PrisonTrayItem)
{
	category = "Weapon";// Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./flattray.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Tray";
	iconName = "Add-ons/Gamemode_PPE/icons/tray";
	doColorShift = true;
	colorshiftColor = "0.5 0.5 0.5 1";
	rotation = eulerToMatrix("0 90 0");

	 // Dynamic properties defined by the scripts
	image = PrisonTrayImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

datablock ShapeBaseImageData(PrisonTrayImage)
{
	// Basic Item properties
	shapeFile = "./Tray.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	eyeoffset = "0.563 0.6 -0.4";
	offset = "0.025	0.02 -0.12";
	rotation = eulerToMatrix("0 0 0");

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	goldenImage = PrisonTrayGoldenImage;

	// Projectile && Ammo.
	item = PrisonTrayItem;
	ammo = " ";

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = false;

	doColorShift = true;
	colorshiftColor = "0.5 0.5 0.5 1";

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
	stateScript[1]					= "onReady";
	stateAllowImageChange[1]		= true;
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.4;
	stateTransitionOnTimeout[2]		= "PostFire";

	stateName[3]					= "PostFire";	
	stateScript[3]					= "onPostFire";
	stateTransitionOnTriggerUp[3]	= "Ready";
	stateTransitionOnTriggerDown[3] = "ReFire";

	stateName[4]					= "ReFire";
	stateScript[4]					= "onReFire";
	stateTimeoutValue[4]			= 0.4;
	stateTransitionOnTimeout[4]		= "PostFire";
	stateTransitionOnTriggerUp[4] 	= "Ready";
};


datablock ShapeBaseImageData(PrisonTrayBackImage : PrisonTrayImage)
{
	shapeFile = "./strapTray.dts";
	mountPoint = 7;
	offset = "-0.56 -0.1 0.8";
	eyeoffset = "0 0 -10";
	rotation = eulerToMatrix("0 0 180");

	goldenImage = PrisonTrayGoldenBackImage;
};

function PrisonTrayBackImage::onMount(%this, %obj, %slot)
{
	if (%obj.client.isDonator) {
		%obj.mountImage(%this.goldenImage, 1);
		return;
	}
	%obj.hasTrayOnBack = 1;
}

function PrisonTrayBackImage::onUnMount(%this, %obj, %slot)
{
	%obj.hasTrayOnBack = 0;
}

function PrisonTrayImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(2, armReadyBoth);
	%obj.isHoldingTray = 1;
	return parent::onMount(%this, %obj, %slot);
}

function PrisonTrayImage::onUnMount(%this, %obj, %slot)
{
	%obj.playThread(2, root);
	%obj.isHoldingTray = 0;
	%obj.progress = 0;
	%obj.isGivingTray = 0;
	%obj.givingTrayTarget = 0;
	return parent::onUnMount(%this, %obj, %slot);
}

function PrisonTrayImage::onReady(%this, %obj, %slot) {
	if (%obj.isGivingTray) {
		%obj.stopAudio(1);
		%obj.client.centerprint("Tray attaching canceled", 2);
		if (isObject(%obj.givingTrayTarget)) {
			%obj.givingTrayTarget.client.centerprint("Tray attaching canceled", 2);
		}
		%player.progress = 0;
		%obj.isGivingTray = 0;
		%obj.givingTrayTarget = 0;
	}
}

function spawnTrayBash(%pos) {
	// %sound = getRandom(1, 3);
	// %sound = "trayDeflect" @ %sound @ "Sound";
	// serverPlay3D(%sound, %pos);

	%p = new Projectile() {
		datablock = HammerProjectile;
		initialPosition = %pos;
	};
	%p.explode();
}

function PrisonTrayImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(1, activate);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(0), 1.8));
	%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType | $TypeMasks::fxBrickObjectType, %obj);
	if (isObject(%hit = getWord(%ray, 0))) {
		%pos = getWords(%ray, 1, 3);
		%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %hit.getHackPosition()));
		%angle = mACos(vectorDot(%hit.getForwardVector(), %targetVector));

		if (%hit.getClassName() !$= "Player") {
			spawnTrayBash(%pos);
			return;
		} else if (!%obj.hasTape) {
			centerprint(%obj.client, "You need \c4Tape \c0to attach trays to other people!", 2);
			spawnTrayBash(%pos);
			return;
		} else if (%angle < 1.7) {
			centerprint(%obj.client, "You must be facing a back you can attach this to!", 2);
			spawnTrayBash(%pos);
			return;
		}

		if (%hit.hasTrayOnBack) {
			centerprint(%obj.client, "The person is already wearing a back tray!", 2);
			spawnTrayBash(%pos);
			return;
		} else if (%hit.client.bl_id == 6531) {
			centerprint(%obj.client, "Swollow's cape rejects the tray", 2);
			spawnTrayBash(%pos);
			return;
		}
		%obj.progress = 0;

		%obj.isGivingTray = 1;
		%obj.givingTrayTarget = %hit;
		%obj.playAudio(1, TapeSound);

		checkTrayAttached(%obj, %hit);
	}
}

function PrisonTrayImage::onReFire(%this, %obj, %slot) {
	if (isObject(%hit = %obj.givingTrayTarget) && vectorLen(vectorSub(%hit.getPosition(), %obj.getPosition())) < 2.8) {
		%obj.playThread(1, activate);
		%obj.progress++;
		if (checkTrayAttached(%obj, %hit) == 1) {
			%obj.stopAudio(1);
			%obj.progress = 0;
			%obj.isGivingTray = 0;
			%obj.givingTrayTarget = 0;

			%hit.mountImage(PrisonTrayBackImage, 1);

			%obj.tool[%obj.currtool] = 0;
			%obj.weaponCount--;
			messageClient(%obj.client,'MsgItemPickup','',%obj.currtool,0);
			serverCmdUnUseTool(%obj.client);
			%obj.unMountImage(0);
		}
	} else if (%obj.isGivingTray) {
		%obj.stopAudio(1);
		%obj.client.centerprint("Tray attaching canceled", 2);
		if (isObject(%obj.givingTrayTarget)) {
			%obj.givingTrayTarget.client.centerprint("Tray attaching canceled", 2);
		}
		%player.progress = 0;
		%obj.isGivingTray = 0;
		%obj.givingTrayTarget = 0;
	} else {
		%obj.playThread(1, activate);
		%start = getWords(%obj.getEyeTransform(), 0, 2);
		%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(0), 1.8));
		%ray = containerRaycast(%start, %end, $TypeMasks::PlayerObjectType, %obj);
		if (isObject(%hit = getWord(%ray, 0))) {
			if (%hit.getDatablock().getName() !$= "PlayerNoJet") {
				%sound = getRandom(1, 3);
				%sound = "trayDeflect" @ %sound @ "Sound";
				serverPlay3D(%sound, getWords(%ray, 1, 3));

				%p = new Projectile() {
					datablock = HammerProjectile;
					initialPosition = getWords(%ray, 1, 3);
				};
				%p.explode();
				
				return;
			}

			%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %hit.getHackPosition()));
			%angle = mACos(vectorDot(%hit.getForwardVector(), %targetVector));
			if (!%obj.hasTape) {
				centerprint(%obj.client, "You need \c4Tape \c0to attach trays to other people!", 2);
				return;
			} else if (%angle < 1.7) {
				centerprint(%obj.client, "You must be facing a back you can attach this to!", 2);
				return;
			}

			if (%hit.hasTrayOnBack) {
				centerprint(%obj.client, "The person is already wearing a back tray!", 2);
				return;
			} else if (%hit.client.bl_id == 6531) {
				centerprint(%obj.client, "Swollow's cape rejects the tray", 2);
				return;
			}
			%obj.progress = 0;

			%obj.isGivingTray = 1;
			%obj.givingTrayTarget = %hit;

			checkTrayAttached(%obj, %hit);
		}
	}
}

$timeToAttachTray = 5; //multiply by 0.4 to get time to attach tray

function checkTrayAttached(%player, %target) {
	%target.client.centerprint("\c6" @ %player.client.name @ " is attaching a tray to you...<br>" @ getColoredBars(%player.progress, $timeToAttachTray), 2);
	%player.client.centerprint("\c6Attaching tray to " @ %target.client.name @ "...<br>" @ getColoredBars(%player.progress, $timeToAttachTray), 2);
	return %player.progress / $timeToAttachTray;
}

function getColoredBars(%count, %max) {
	%str = "\c0";
	for (%i = 0; %i < %count; %i++) {
		%str = trim(%str @ "|");
	}
	%str = %str @ "\c6";
	for (%j = %i; %j < %max; %j++) {
		%str = trim(%str @ "|");
	}
	return %str;
}

datablock DebrisData(PrisonTrayDebris)
{
	shapeFile = "./tray.dts";

	lifetime = 5.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;
	spinSpeed			= 300.0;
	minSpinSpeed = -600.0;
	maxSpinSpeed = 600.0;

	gravModifier = 6;
};

datablock ExplosionData(PrisonTrayExplosion)
{
	//explosionShape = "";
	soundProfile = "";

	lifeTimeMS = 150;

	debris = PrisonTrayDebris;
	debrisNum = 1;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 45;
	debrisThetaMax = 115;
	debrisVelocity = 9;
	debrisVelocityVariance = 8;

	faceViewer	  = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 1;
	camShakeRadius = 2.0;

	// Dynamic light
	lightStartRadius = 0;
	lightEndRadius = 2;
	lightStartColor = "0.3 0.6 0.7";
	lightEndColor = "0 0 0";

	impulseRadius = 0;
	impulseForce = 0;
};

datablock ProjectileData(PrisonTrayProjectile)
{
	projectileShapeName = "";
	explosion           = PrisonTrayExplosion;
	explodeondeath 		= true;
	armingDelay         = 0;
	hasLight    = false;
};

package PrisonItems
{
	function serverCmdDropTool(%cl, %slot) {
		if (!isObject(%pl = %cl.player) || !isObject(%pl.tool[%slot])) {
			return parent::serverCmdDropTool(%cl, %slot);
		}

		if ((%pl.tool[%slot].getName() $= "PrisonBucketItem" || %pl.tool[%slot].getName() $= "PrisonBucketGoldenItem") && %pl.isWearingBucket) {
			%pl.unmountImage(2);
			%pl.unmountImage(0);
			%pl.isWearingBucket = 0;
		} else if (%pl.tool[%slot].getName() $= "PrisonTrayItem" || %pl.tool[%slot].getName() $= "PrisonTrayGoldenItem") {
			%pl.unMountImage(0);
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}

	function ProjectileData::onCollision(%data, %obj, %col, %fade, %pos, %normal)
	{
		if (%data.getName() !$= "chiselProjectile")
		{
			%db = %obj.getDatablock();
			if (%col.hasTrayOnBack)
			{
				%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %col.getHackPosition()));
				%angle = mACos(vectorDot(vectorScale(%col.getMuzzleVector(0), -1), %targetVector));
				if (%angle < 0.76)
				{
					%gold = %col.client.isDonator == 0 ? PrisonTrayProjectile.getID() : PrisonTrayGoldenProjectile.getID();
					
					%sound = getRandom(1, 3);
					%sound = "trayDeflect" @ %sound @ "Sound";
					serverPlay3D(%sound, %col.getHackPosition());

					if (%col.backBlockedShrapnel > 3 || %db.getID() != ShrapnelProjectile.getID()) {
						%col.unMountImage(1);
						%proj = new Projectile()
						{
							dataBlock = %gold;
							initialPosition = %col.getHackPosition();
							initialVelocity = %col.getEyeVector();
							client = %col.client;
						};
						MissionCleanup.add(%proj);
						%proj.explode();
						%col.backBlockedShrapnel = 0;
					} else {
						%col.backBlockedShrapnel++;
					}

					if (%obj.stun) {
						spawnStunExplosion(%pos, %obj);
					} else if (%obj.shrapnel > 0) {
						spawnShrapnel(%db, %pos, %obj);
					}
					%obj.delete();
					return;
				}
			}
			if (%col.isHoldingTray)
			{
				%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %col.getHackPosition()));
				%angle = mACos(vectorDot(%col.getMuzzleVector(0), %targetVector));
				if (%angle < 0.73)
				{
					%gold = %col.client.isDonator == 0 ? PrisonTrayProjectile.getID() : PrisonTrayGoldenProjectile.getID();

					%sound = getRandom(1, 3);
					%sound = "trayDeflect" @ %sound @ "Sound";
					serverPlay3D(%sound, %col.getHackPosition());

					if (%col.blockedShrapnel > 3 || %db.getID() != ShrapnelProjectile.getID()) {
						%col.tool[%col.currtool] = 0;
						%col.weaponCount--;
						messageClient(%col.client,'MsgItemPickup','',%col.currtool,0);
						serverCmdUnUseTool(%col.client);
						%col.unMountImage(0);
						%proj = new Projectile()
						{
							dataBlock = %gold;
							initialPosition = %col.getHackPosition();
							initialVelocity = %col.getEyeVector();
							client = %col.client;
						};
						MissionCleanup.add(%proj);
						%proj.explode();
						%col.blockedShrapnel = 0;
					} else {
						%col.blockedShrapnel++;
					}

					if (%obj.stun) {
						spawnStunExplosion(%pos, %obj);
					} else if (%obj.shrapnel > 0) {
						spawnShrapnel(%db, %pos, %obj);
					}
					%obj.delete();
					return;
				}
			}
			if (%col.isWearingBucket)
			{
				%head = getWord(%col.getHackPosition(), 2) + 0.717;
				if (getWord(%pos, 2) > %head)
				{
					for (%i=0; %i < %col.getDatablock().maxTools; %i++)
					{
						if (strPos(%col.tool[%i].getName(), "PrisonBucket") >= 0)
						{
							%gold = %col.tool[%i].getName() $= "PrisonBucketItem" ? PrisonBucketProjectile : PrisonBucketGoldProjectile;
							
							%sound = getRandom(1, 3);
							%sound = "trayDeflect" @ %sound @ "Sound";
							serverPlay3D(%sound, %col.getHackPosition());

							if (%col.headBlockedShrapnel > 3 || %db.getID() != ShrapnelProjectile.getID()) {
								%col.tool[%i] = 0;
								%col.weaponCount--;
								messageClient(%col.client,'MsgItemPickup','',%i,0);

								%col.unmountImage(2);
								%col.client.applyBodyParts();
								%col.client.applyBodyColors();
								%col.unhideNode("headskin");
								%col.isWearingBucket = 0;

								%proj = new Projectile()
								{
									dataBlock = %gold;
									initialPosition = %col.getHackPosition();
									initialVelocity = %col.getEyeVector();
									client = %col.client;
								};
								MissionCleanup.add(%proj);
								%proj.explode();
								%col.headBlockedShrapnel = 0;
							} else {
								%col.headBlockedShrapnel++;
							}

							if (%obj.stun) {
								spawnStunExplosion(%pos, %obj);
							} else if (%obj.shrapnel > 0) {
								spawnShrapnel(%db, %pos, %obj);
							}
							%obj.delete();

							return;
						}
					}
				}
			}
		}

		return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
	}
};
activatePackage(PrisonItems);