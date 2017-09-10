//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Load dts shapes and merge animations
datablock TSShapeConstructor(SpotlightDts) {
	baseShape  = "./spotlight.dts";
	sequence0  = "./spl_root.dsq root";

	sequence1  = "./spl_root.dsq run";
	sequence2  = "./spl_root.dsq walk";
	sequence3  = "./spl_root.dsq back";
	sequence4  = "./spl_root.dsq side";

	sequence5  = "./spl_root.dsq crouch";
	sequence6  = "./spl_root.dsq crouchRun";
	sequence7  = "./spl_root.dsq crouchBack";
	sequence8  = "./spl_root.dsq crouchSide";

	sequence9  = "./spl_look.dsq look";
	sequence10 = "./spl_root.dsq headside";
	sequence11 = "./spl_root.dsq headUp";

	sequence12 = "./spl_root.dsq jump";
	sequence13 = "./spl_root.dsq standjump";
	sequence14 = "./spl_root.dsq fall";
	sequence15 = "./spl_root.dsq land";

	sequence16 = "./spl_root.dsq armAttack";
	sequence17 = "./spl_root.dsq armReadyLeft";
	sequence18 = "./spl_root.dsq armReadyRight";
	sequence19 = "./spl_root.dsq armReadyBoth";
	sequence20 = "./spl_root.dsq spearready";  
	sequence21 = "./spl_root.dsq spearThrow";

	sequence22 = "./spl_root.dsq talk";  

	sequence23 = "./spl_death.dsq death1"; 
	
	sequence24 = "./spl_root.dsq shiftUp";
	sequence25 = "./spl_root.dsq shiftDown";
	sequence26 = "./spl_root.dsq shiftAway";
	sequence27 = "./spl_root.dsq shiftTo";
	sequence28 = "./spl_root.dsq shiftLeft";
	sequence29 = "./spl_root.dsq shiftRight";
	sequence30 = "./spl_root.dsq rotCW";
	sequence31 = "./spl_root.dsq rotCCW";

	sequence32 = "./spl_root.dsq undo";
	sequence33 = "./spl_root.dsq plant";

	sequence34 = "./spl_root.dsq sit";

	sequence35 = "./spl_root.dsq wrench";

	sequence36 = "./spl_root.dsq activate";
	sequence37 = "./spl_root.dsq activate2";

	sequence38 = "./spl_root.dsq leftrecoil";
};  

datablock PlayerData(SpotlightArmor) {
	renderFirstPerson = false;
	emap = false;
	
	className = Armor;
	shapeFile = "./spotlight.dts";
	cameraMaxDist = 8;
	cameraTilt = 0.261;//0.174 * 2.5; //~25 degrees
	cameraVerticalOffset = 2.3;
	  
	cameraDefaultFov = 90.0;
	cameraMinFov = 5.0;
	cameraMaxFov = 120.0;
	
	//debrisShapeName = "~/data/shapes/player/debris_player.dts";
	//debris = SpotlightDebris;

	aiAvoidThis = true;

	minLookAngle = -1.5708;
	maxLookAngle = 1.5708;
	maxFreelookAngle = 3.0;

	mass = 120;
	drag = 0.1;
	density = 0.7;
	maxDamage = 250;
	maxEnergy =  10;
	repairRate = 0.33;

	rechargeRate = 0.4;

	runForce = 60 * 90;
	runEnergyDrain = 0;
	minRunEnergy = 0;
	maxForwardSpeed = 0;
	maxBackwardSpeed = 0;
	maxSideSpeed = 0;

	maxForwardCrouchSpeed = 0;
	maxBackwardCrouchSpeed = 0;
	maxSideCrouchSpeed = 0;

	maxForwardProneSpeed = 0;
	maxBackwardProneSpeed = 0;
	maxSideProneSpeed = 0;

	maxForwardWalkSpeed = 0;
	maxBackwardWalkSpeed = 0;
	maxSideWalkSpeed = 0;

	maxUnderwaterForwardSpeed = 0;
	maxUnderwaterBackwardSpeed = 0;
	maxUnderwaterSideSpeed = 0;

	jumpForce = 0 * 90; //8.3 * 90;
	jumpEnergyDrain = 0;
	minJumpEnergy = 0;
	jumpDelay = 0;

	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

	minImpactSpeed = 250;
	speedDamageScale = 3.8;

	boundingBox			= vectorScale("1.8 1.8 2", 4);
	crouchBoundingBox	= vectorScale("1.8 1.8 2", 4);
	
	pickupRadius = 0.75;
	
	// Foot Prints
	//decalData = SpotlightFootprint;
	//decalOffset = 0.25;
	
	jetEmitter = "";
	jetGroundEmitter = "";
	jetGroundDistance = 4;
  
	//footPuffEmitter = LightPuffEmitter;
	footPuffNumParts = 10;
	footPuffRadius = 0.25;

	//dustEmitter = LiftoffDustEmitter;

	splash = PlayerSplash;
	splashVelocity = 4.0;
	splashAngle = 67.0;
	splashFreqMod = 300.0;
	splashVelEpsilon = 0.60;
	bubbleEmitTime = 0.1;
	splashEmitter[0] = PlayerFoamDropletsEmitter;
	splashEmitter[1] = PlayerFoamEmitter;
	splashEmitter[2] = PlayerBubbleEmitter;
	mediumSplashSoundVelocity = 10.0;   
	hardSplashSoundVelocity = 20.0;  
	exitSplashSoundVelocity = 5.0;

	// Controls over slope of runnable/jumpable surfaces
	runSurfaceAngle  = 85;
	jumpSurfaceAngle = 86;

	minJumpSpeed = 20;
	maxJumpSpeed = 30;

	horizMaxSpeed = 0;
	horizResistSpeed = 33;
	horizResistFactor = 0.35;

	upMaxSpeed = 80;
	upResistSpeed = 25;
	upResistFactor = 0.3;
	
	footstepSplashHeight = 0.35;

	//NOTE:  some sounds commented out until wav's are available

	JumpSound			= "";

	// Footstep Sounds
// FootSoftSound        = SpotlightFootFallSound;
// FootHardSound        = SpotlightFootFallSound;
// FootMetalSound     = SpotlightFootFallSound;
// FootSnowSound        = SpotlightFootFallSound;
// FootShallowSound     = SpotlightFootFallSound;
// FootWadingSound  = SpotlightFootFallSound;
// FootUnderwaterSound  = SpotlightFootFallSound;
	//FootBubblesSound     = FootLightBubblesSound;
	//movingBubblesSound = ArmorMoveBubblesSound;
	//waterBreathSound     = WaterBreathMaleSound;

	//impactSoftSound  = ImpactLightSoftSound;
	//impactHardSound  = ImpactLightHardSound;
	//impactMetalSound     = ImpactLightMetalSound;
	//impactSnowSound  = ImpactLightSnowSound;
	
	impactWaterEasy      = Splash1Sound;
	impactWaterMedium  = Splash1Sound;
	impactWaterHard      = Splash1Sound;
	
	groundImpactMinSpeed  = 10.0;
	groundImpactShakeFreq   = "4.0 4.0 4.0";
	groundImpactShakeAmp  = "1.0 1.0 1.0";
	groundImpactShakeDuration = 0.8;
	groundImpactShakeFalloff = 10.0;
	
	//exitingWater   = ExitingWaterLightSound;

	// Inventory Items
	maxItems   = 10;	//total number of bricks you can carry
	maxWeapons = 5;		//this will be controlled by mini-game code
	maxTools = 5;
	
	uiName = "Spotlight";
	rideable = true;
		lookUpLimit = 0.6;
		lookDownLimit = 0.2;

	canRide = true68;
	showEnergyBar = false;
	paintable = true;

	brickImage = SpotlightBrickImage;	//the imageData to use for brick deployment

	numMountPoints = 1;
	mountThread[0] = "armReadyBoth";
	mountNode[0] = 2;
};

function SpotlightArmor::onAdd(%this,%obj) {
	// Vehicle timeout
	%obj.mountVehicle = true;

	// Default dynamic armor stats
	%obj.setRepairRate(0);

}

//called when the driver of a player-vehicle is unmounted
function SpotlightArmor::onDriverLeave(%obj, %player) {
	//do nothing
}

datablock ProjectileData(SpotlightLightData) {
	projectileShapeName = "";
	directDamage        = 0;
	impactImpulse    = 0;
	verticalImpulse      = 0;
	explosion           = "";
	particleEmitter     = "";

	brickExplosionRadius = 0;
	brickExplosionImpact = false;    //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;         
	brickExplosionMaxVolume = 0;       //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

	sound = "";

	muzzleVelocity  = 0;
	velInheritFactor   = 0;

	armingDelay   = 0;
	lifetime    = 30;
	fadeDelay           = 4000;
	bounceElasticity   = 0.5;
	bounceFriction  = 0.20;
	isBallistic   = true;
	gravityMod = 1.0;

	hasLight  = true;
	lightRadius = 1.0;
	lightColor  = "1 1 1";

	explodeOnDeath = 0;

	uiName = "";
};

//exec("./item_lightbeam/Item_Lightbeam.cs");

package SpotlightPackage {
	function Player::burn(%obj,%time) {
		if(%obj.dataBlock $= SpotlightArmor) {
			return;
		} else {
			Parent::burn(%obj,%time);
		}
	}

	function Player::emote(%obj,%emote) {
		if(%obj.dataBlock $= SpotlightArmor) {
			return;
		}
		Parent::emote(%obj,%emote);
	}

	function armor::onTrigger(%this, %obj, %triggerNum, %val) {
		%mount = %obj.getObjectMount();

		//hack so we can shoot if we are the spotlight
		if(%obj.getDataBlock().getID() == SpotlightArmor.getID())
			%mount = %obj;

		if(isObject(%mount) && %triggerNum == 0) {
			if(%mount.getDataBlock() == SpotlightArmor.getId() && %triggerNum == 0 && %val) {
				%client = %obj.client;
				if(isObject(%client)) {
					ServerCmdUnUseTool(%client);
				}
				
				//create new shape if needed
				
				//check if there's a refresh loop running - toggle it on/off
				if (isEventPending(%mount.lightbeamloop)) {
					//talk("clearing");
					clearLightBeam(%mount);
				} else {
					startLightBeamLoop(%mount);
				}

				return;
			}
		}
		
		Parent::onTrigger(%this,%obj,%triggerNum,%val);
	}

	function Armor::onRemove(%this, %obj) {
		if (%obj.getDatablock() == SpotlightArmor.getID()) {
			clearLightBeam(%obj);
		}
		parent::onRemove(%this, %obj);
	}
};
activatePackage(SpotlightPackage);

function AIPlayer::lookAtPlayer_Spotlight( %obj, %opt, %client ) {	
	if (%obj.getDatablock().getName() !$= "SpotlightArmor" || %obj.getClassName() !$= "AIPlayer")
		return;

	if( %opt == 0 ) {
		%obj.isTargetingTarget = 0;
		%aimPlayer = %obj.getAimObject();
		%obj.spotlightTarget = "";
		%obj.spotlightTargetLocation = "";
	
		%obj.clearAim();
		
		if( isObject( %aimPlayer ) ) {
			%obj.setAimLocation( %aimPlayer.getEyePoint() );
		}
		
		return;
	} else if( %opt == 1 ) {
		%player = %client.player;
	} else if( %opt == 2 ) {
		%obj.isTargetingTarget = 0;
		%obj.spotlightTarget = 0;
		%obj.spotlightTargetLocation = "";
		if( %obj.hEventLastLookTime+600 > getSimTime() ) {
			return;
		}
			
		// write down last time this was called
		%obj.hEventLastLookTime = getSimTime();
			
		%closest = 10000;
		%target = 0;
		for (%i = 0; %i < ClientGroup.getCount(); %i++) {
			if(isObject(%player = ClientGroup.getObject(%i).player)) {
				%dist = vectorDist(%player.getHackPosition(), %obj.getHackPosition());
				if (%dist < %closest) {
					%target = %player;
					%closest = %dist;
				}
			}
		}
//		talk(%closest SPC %target.client.name);
		%player = %target;
		
		// %isItUs = %player == %obj;
		
		if( !%player ) {
			
			if( isObject( (%aimPlayer = %obj.getAimObject()) ) ) {
				// %aimPlayer = %obj.getAimObject();
			
				%obj.clearAim();
				%obj.setAimLocation( %aimPlayer.getEyePoint() );
				
				return;
			}
			
			return;
		}	
	} else if( %opt == 3 ) {
		%obj.clearAim();
		%obj.isTargetingTarget = 1;
		%player = %obj.spotlightTarget;
		if (isObject(%player)) {
			if (%player.isShrouded || $CPB::EWSActive) {
				%obj.spotlightTargetLocation = %player.getEyePoint();
				%obj.setAimLocation(%obj.spotlightTargetLocation);
				%obj.spotlightTarget = "";
			} else {
				%obj.setAimObject(%obj.spotlightTarget);
				%obj.setAimLocation(%obj.spotlightTarget.getEyePoint());
			}
		}
		else if (%obj.spotlightTargetLocation !$= "")
			%obj.setAimLocation(%obj.spotlightTargetLocation);
	}
	
	if( isObject( %player ) ) {
		%obj.setAimObject( %player );
	}
}
registerOutputEvent( Bot,"lookAtPlayer_Spotlight","list Clear 0 Activator 1 Closest 2 Target 3", 1 );//,"string 20 100");

function startLightBeamLoop(%obj) {
	if(!isObject(%obj)) {
		return;
	}

	if (isEventPending(%obj.lightbeamloop) || %obj.getState() $= "Dead") {
		clearLightBeam(%obj);
	}

	if (!isObject(%obj.lightbeam)) {
		%obj.lightbeam = new StaticShape(SpotlightBeam){
			datablock = SpotlightBeamShape;
		};
	}

	//draw lightbeam
	%scaleFactor = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%typeMasks = $Typemasks::FxBrickObjectType | $Typemasks::TerrainObjectType | 
	$TypeMasks::StaticObjectType;

	if  (%obj.getClassName() !$= "Player" && isObject(%targetPlayer = %obj.getAimObject()) || (isObject(%targetPlayer = %obj.spotlightTarget) && %obj.isTargetingTarget)) {
		%beamVector = vectorNormalize(vectorSub(%targetPlayer.getEyePoint(), %start));
	} else if (%obj.getClassName() !$= "Player" && %obj.spotlightTargetLocation !$= "" && %obj.isTargetingTarget) {
		%beamVector = vectorNormalize(vectorSub(%obj.spotlightTargetLocation, %start));
	} else {
		%beamVector = vectorNormalize(%obj.getEyeVector());
	}
	%end = vectorAdd(vectorScale(%beamVector, 800), %start);
	%ray = containerRaycast(%start, %end, %typemasks, %obj);
	%hit = firstWord(%ray);

	if (isObject(%hit)) {
		%end = getWords(%ray, 1, 3);
	}

	%obj.lightbeam.drawSpotlightBeam(%start, %end, 0.4, %scaleFactor*1.1);

	//blind players at end of beam
	whiteOutPlayers(%obj, %start, %end, 0);

	//schedule next call
	%obj.lightbeamloop = schedule(50, %obj, startLightBeamLoop, %obj);
}

function whiteOutPlayers(%obj, %start, %end, %i) {
	%scale = getWord(%obj.getScale(), 2);
	%radius = %scale*0.6-0.08;
	if (%i < ClientGroup.getCount()) {
		%client = ClientGroup.getObject(%i);
		if (isObject(%pl = %client.player) && %pl !$= %obj && !%client.isGuard) {
			%pos = %pl.getEyePoint();
			%dist = distanceFromVector(%start, %end, %pos);
			%dist = getWord(%dist, 0);
			
			//findclientbyname(conan).chatMessage(%dist);

			%targetVector = vectorNormalize(vectorSub(%obj.getPosition(), %pos));
			%angle = mACos(vectorDot(%pl.getEyeVector(), %targetVector));
			if (%dist < %radius && %angle < 1.8) {
				%pl.setWhiteOut(1);
				%proj = new Projectile(){
					datablock = PlayerTeleportProjectile;
					initialPosition = %pos;
					initialVelocity = "0 0 0";
				};
				MissionCleanup.add(%proj);
				//%proj.explode();
			}
		}
		schedule(0, 0, whiteOutPlayers, %obj, %start, %end, %i+1);
	}
}

function distanceFromVector(%start, %end, %pos) {
	//calculate parallelogram area of the three points and divide by base to get height
	%startToEnd = vectorSub(%end, %start);
	%startToPos = vectorSub(%pos, %start);
	%normal = vectorNormalize(%startToEnd);

	//projection of startToPos onto normal to determine location on line defined by startToEnd
	%dot = vectorDot(%startToPos, %normal);
	//behind the starting point of the light beam
	if (%dot < 0) {
		%dist = 1000;
	} else if (%dot/vectorLen(%startToEnd) > 1) { //past the ending point of the lightbeam
		%dist = 1000;
	} else {
		//calculate parallelogram area of the three points and divide by base to get height eg dist from beam
		%area = vectorLen(vectorCross(%startToEnd, %startToPos));
		%dist = %area/vectorLen(%startToEnd);		
	}
	return %dist SPC %area;
}

function clearLightBeam(%obj) {
	cancel(%obj.lightbeamloop);
	if (isObject(%obj.lightbeam)) {
		%obj.lightbeam.delete();
	}
	%obj.lightbeamloop = 0;
}

datablock StaticShapeData(SpotlightBeamShape) {
	shapeFile = "./spotlight_beam.dts";
};

function StaticShape::drawSpotlightBeam(%this, %a, %b, %alpha, %size) {
	if (%alpha <= 0) {
		%this.delete();
		return;
	}

	%vector = vectorNormalize(vectorSub(%b, %a));

	%xyz = vectorNormalize(vectorCross("1 0 0", %vector));
	%u = mACos(vectorDot("1 0 0", %vector)) * -1;

	%this.setTransform(vectorScale(vectorAdd(%a, %b), 0.5) SPC %xyz SPC %u);
	%this.setScale(vectorDist(%a, %b) SPC %size SPC %size);
	%this.setNodeColor("ALL", "1 1 1" SPC %alpha);
}