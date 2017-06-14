//projectile
datablock ParticleData(soapParticleA)
{
	textureName			 = "base/data/particles/bubble";
	dragCoefficient		= 0.1;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.1; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 400;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "0.8 0.8 1 0";
	colors[1]	  = "0.8 0.8 1 1";
	colors[2]	  = "0.8 0.8 1 0";

	sizes[0]		= 0;
	sizes[1]		= 0.5;
	sizes[2]		= 0;

	times[0]		= 0.0;
	times[1]		= 0.1;
	times[2]		= 1.0;
};

datablock ParticleData(soapParticleB)
{
	textureName			 = "base/data/particles/bubble";
	dragCoefficient		= 0.1;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.1; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 400;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "0.7 0.9 1 0";
	colors[1]	  = "0.7 0.9 1 1";
	colors[2]	  = "0.7 0.9 1 0";

	sizes[0]		= 0;
	sizes[1]		= 0.3;
	sizes[2]		= 0;

	times[0]		= 0.0;
	times[1]		= 0.1;
	times[2]		= 1.0;
};

datablock ParticleEmitterData(PrisonSoapEmitter) {
	ejectionPeriodMS = 10;
	periodVarianceMS = 5;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0;
	
	ejectionVelocity = 8;
	velocityVariance = 5;

	thetaMin			= 0.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = "soapParticleA soapParticleB";	

	useEmitterColors = false;

	uiName = "Soap Emitter";
};

//////////
// item //
//////////
datablock ItemData(PrisonSoapItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	// Basic Item Properties
	shapeFile = "./soap.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Soap";
	iconName = "Add-ons/Gamemode_PPE/icons/soap";
	doColorShift = true;
	colorShiftColor = "0.95 0.6 0.57 1.000";

	// Dynamic properties defined by the scripts
	image = PrisonSoapImage;
	canDrop = true;
};

datablock ItemData(PrisonSoapPickupItem : PrisonSoapItem)
{
	isSlidingItem = 1;
	uiname = "";
};

datablock ShapeBaseImageData(PrisonSoapPickupImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateEmitter[0]					= PrisonSoapEmitter;
	stateEmitterNode[0]				= "emitterPoint";
	stateEmitterTime[0]				= 1000;


	stateName[1]			= "Ready";
	stateTimeoutValue[1]		= 0.5;
	stateTransitionOnTimeout[1]	= "Activate";
	stateEmitter[1]					= PrisonSoapEmitter;
	stateEmitterNode[1]				= "emitterPoint";
	stateEmitterTime[1]				= 1000;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(PrisonSoapImage)
{
	// Basic Item properties
	shapeFile = "./soap.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0.07 0.05";
	rotation = eulerToMatrix("0 90 0");
	//eyeOffset = "0.1 0.2 -0.55";

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	goldenImage = PrisonSoapGoldenImage;	

	canMountToBronson = 1;

	// Projectile && Ammo.
	item = PrisonSoapItem;
	ammo = " ";
	projectile = "";
	projectileType = Projectile;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;

	//casing = " ";
	doColorShift = true;
	colorShiftColor = PrisonSoapItem.colorShiftColor;

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
	
	stateName[2]						  = "Charge";
	stateTransitionOnTimeout[2]	= "Armed";
	stateTimeoutValue[2]				= 0.8;
	stateScript[2]			= "onCharge";
	stateWaitForTimeout[2]		= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateAllowImageChange[2]		  = false;
	
	stateName[3]			= "Armed";
	stateTransitionOnTriggerUp[3]	= "Fire";
	stateAllowImageChange[3]	= true;

	stateName[4]			= "Fire";
	stateTransitionOnTimeout[4]	= "Ready";
	stateTimeoutValue[4]		= 0.2;
	stateFire[4]			= true;
	stateScript[4]			= "onFire";
	stateWaitForTimeout[4]		= true;
	stateAllowImageChange[4]	= true;

	stateName[5]			= "AbortCharge";
	stateScript[5]			= "onAbortCharge";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5] = 0.1;
};

function PrisonSoapImage::onCharge(%this, %obj, %slot) {
	%obj.playThread(2, spearReady);
}

function PrisonSoapImage::onFire(%this, %obj, %slot) {
	//statistics
	setStatistic("SoapThrown", getStatistic("SoapThrown", %obj.client) + 1, %obj.client);
	setStatistic("SoapThrown", getStatistic("SoapThrown") + 1);

	%obj.playThread(2, spearThrow);
	%i = new Item(Soap) {
		datablock = PrisonSoapPickupItem;
		position = %obj.getMuzzlePoint(%slot);
		spawnTime = getSimTime();
		minigame = getMinigameFromObject(%obj);
	};
	%i.setCollisionTimeout(%obj);
	%i.mountImage(PrisonSoapPickupImage, 0);
	%i.setVelocity(VectorScale(%obj.getMuzzleVector(%slot), 50.0 * getword(%obj.getScale(), 2)));
	%i.schedulePop();
	//%ret = Parent::onFire(%this, %obj, %slot);

	%currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);

	return;
}

function PrisonSoapImage::onAbortCharge(%this, %obj, %slot) {
	%obj.playThread(2, activate);
}

//slide player
datablock PlayerData(EmptyHoleBot : PlayerNoJet) {
	shapeFile = "base/data/shapes/empty.dts";

	boundingBox			= vectorScale("1.24 1.24 1", 4);
	crouchBoundingBox	= vectorScale("1.24 1.24 1", 4);

	uiname = "EmptyHoleBot";

	maxStepHeight = 0;
	slowdownMax = 80;
};

function Player::doSoapSlide(%pl, %tick, %initialVel, %golden) {
	%mount = %pl.getMountedObject(0);
	if (!isObject(%pl)) {
		return;
	}
	if(%tick == 0) {
		if (%golden) {
			%pl.mountImage(PrisonBucketGoldenEquippedImage, 0);
			%pl.mountImage(PrisonBucketGoldenEquippedImage, 1);
			%pl.mountImage(PrisonBucketGoldenEquippedImage, 2);
			%pl.mountImage(PrisonBucketGoldenEquippedImage, 3);
			removeUniform(%mount);
		} else {
			%pl.mountImage(PrisonSoapPickupImage, 0);
			%pl.mountImage(PrisonSoapPickupImage, 1);
		}
		%pl.isSliding = 1;
		%pl.setMaxForwardSpeed(%initialVel);
		%pl.setVelocity(vectorScale(%pl.getForwardVector(), %pl.getDatablock().slideSpeed));
		%pl.lastSpeed = vectorLen(%pl.getVelocity());
	}
	cancel(%pl.soapSlideSched);	
	%mount.canDismount = 0;

	if (%pl.lastSpeed - vectorLen(%pl.getVelocity()) > 2 || (%pl.lastSpeed < 1 && %pl.lastSpeed !$= 0)) {
		%pl.isSliding = 0;
		%pl.setMoveY(0);
		%pl.setMaxForwardSpeed(%pl.getDatablock().maxForwardSpeed);

		%mount.canDismount = 1;
		%mount.dismount();
		%mount.schedule(1,setTransform, %pl.getHackPosition() SPC rotFromTransform(%mount.getTransform()));
		%mount.playThread(0, root);
		%pl.delete();
		return;
	}
	%pl.lastSpeed = vectorLen(%pl.getVelocity());
	%max = %pl.getDatablock().slowdownMax;
	%p = 1-(%tick/%max)*(%tick/%max);
	%pl.setMoveY(%p);
	%mount.playThread(3, activate2);
	if(%tick >= %max) {
		%pl.isSliding = 0;

		%mount.canDismount = 1;
		%mount.dismount();
		%mount.setTransform(%pl.getTransform());
		%mount.playThread(3, root);
		%mount.setTransform(%pl.getPosition() SPC rotFromTransform(%obj.getTransform()));
		%pl.delete();
		return;
	}
	%pl.soapSlideSched = %pl.schedule(64, doSoapSlide, %tick++);
}

package SoapItem {
	function Armor::onCollision(%this, %obj, %col, %vel, %speed) {
		%name = %col.getDatablock().getName();
		if (strPos(%name, "Soap") && %col.getDatablock().isSlidingItem && %this.getName() !$= "LaundryCartArmor" && %this.getName() !$= "EmptyHoleBot" && %this.getName() !$= "SpotlightArmor") {
			if (getSimTime() - %col.spawnTime < 100 || isObject(%obj.getObjectMount()) || isObject(%obj.getMountedObject(0)) || %obj.isSliding) {
				return;
			}
			if (strPos(%name, "Golden") >= 0) {
				%golden = 1;
			}
			%col.delete();
			if (!%obj.isSliding) {
				if (isObject(%cl = %obj.client) && %cl.isGuard) {
					setStatistic("GuardsSoaped", getStatistic("GuardsSoaped") + 1);
					%obj.setWhiteOut(0.8);
					stun(%obj, 10);
					removeUniform(%obj);
				} else {
					setStatistic("SoapUsed", getStatistic("SoapUsed", %obj.client) + 1, %obj.client);
					setStatistic("SoapUsed", getStatistic("SoapUsed") + 1);
					//talk("Soap!");
					%soap = new AIPlayer(Soap) {
						datablock = EmptyHoleBot;
					};
					%soap.setTransform(%obj.getTransform());
					%vel = %obj.getVelocity();
					%soap.mountObject(%obj, 1);
					%soap.doSoapSlide(0, vectorLen(%vel) + 3, %golden);
					%obj.playThread(0, sit);
					removeUniform(%obj);
				}
			}
			return;
		}
		return parent::onCollision(%this, %obj, %col, %vel, %speed);
	}
};
activatePackage(SoapItem);

function removeUniform(%player) {
	%client = %player.client;
	if (!isObject(%client)){
		return;
	}
	%color = %client.headColor;
	//%player.setNodeColor(chest, %color);
	%player.setNodeColor(lshoe, %color);
	%player.setNodeColor(rshoe, %color);
	%player.setNodeColor(pants, %color);
	if (%cl.isDonator) {
		%player.setNodeColor(pants, "1 1 1 1");
	}
}