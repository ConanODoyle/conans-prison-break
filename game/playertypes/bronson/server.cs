//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Load dts shapes and merge animations
datablock TSShapeConstructor(BuffDts)
{
	baseShape  = "./buff_bot.dts";
	sequence0  = "./buff_root.dsq root";

	sequence1  = "./buff_runFast.dsq run";
	sequence2  = "./buff_runFast.dsq walk";
	sequence3  = "./buff_backFast.dsq back";
	sequence4  = "./buff_side.dsq side";

	sequence5  = "./buff_crouch.dsq crouch";
	sequence6  = "./buff_crouchrun.dsq crouchRun";
	sequence7  = "./buff_crouchback.dsq crouchBack";
	sequence8  = "./buff_crouchside.dsq crouchSide";

	sequence9  = "./buff_look.dsq look";
	sequence10 = "./buff_headlook.dsq headside";
	sequence11 = "./buff_root.dsq headUp";

	sequence12 = "./buff_jump.dsq jump";
	sequence13 = "./buff_jump.dsq standjump";
	sequence14 = "./buff_fall.dsq fall";
	sequence15 = "./buff_root.dsq land";

	sequence16 = "./buff_armAttack.dsq armAttack";
	sequence17 = "./buff_armReadyLeft.dsq armReadyLeft";
	sequence18 = "./buff_armReadyRight.dsq armReadyRight";
	sequence19 = "./buff_armReadyBoth.dsq armReadyBoth";
	sequence20 = "./buff_spearready.dsq spearready";     
	sequence21 = "./buff_spearThrow.dsq spearThrow";

	sequence22 = "./buff_talk.dsq talk";  

	sequence23 = "./buff_death1.dsq death1"; 
	
	sequence24 = "./buff_shiftUp.dsq shiftUp";
	sequence25 = "./buff_shiftDown.dsq shiftDown";
	sequence26 = "./buff_shiftAway.dsq shiftAway";
	sequence27 = "./buff_shiftTo.dsq shiftTo";
	sequence28 = "./buff_shiftLeft.dsq shiftLeft";
	sequence29 = "./buff_shiftRight.dsq shiftRight";
	sequence30 = "./buff_rotCW.dsq rotCW";
	sequence31 = "./buff_rotCCW.dsq rotCCW";

	sequence32 = "./buff_undo.dsq undo";
	sequence33 = "./buff_plant.dsq plant";

	sequence34 = "./buff_sit.dsq sit";

	sequence35 = "./buff_wrench.dsq wrench";

   sequence36 = "./buff_activate.dsq activate";
   sequence37 = "./buff_activate2.dsq activate2";

   sequence38 = "./buff_leftRecoil.dsq leftrecoil";
};    

datablock PlayerData(BuffArmor)
{
   renderFirstPerson = false;
   emap = false;
   
   className = Armor;
   shapeFile = "./buff_bot.dts";
   cameraMaxDist = 8;
   cameraTilt = 0.261;//0.174 * 2.5; //~25 degrees
   cameraVerticalOffset = 1.3;
     
   cameraDefaultFov = 90.0;
   cameraMinFov = 5.0;
   cameraMaxFov = 120.0;
   
   //debrisShapeName = "~/data/shapes/player/debris_player.dts";
   //debris = BuffDebris;

   aiAvoidThis = true;

   minLookAngle = -1.5708;
   maxLookAngle = 1.5708;
   maxFreelookAngle = 3.0;

   mass = 140;
   drag = 0.1;
   density = 0.7;
   maxDamage = 600;
   maxEnergy =  10;
   repairRate = 0.33;

   rechargeRate = 0.4;

   runForce = 60 * 140;
   runEnergyDrain = 0;
   minRunEnergy = 0;
   maxStepHeight= "1";
   maxForwardSpeed = 6.5;
   maxBackwardSpeed = 3.5;
   maxSideSpeed = 5.5;

   maxForwardCrouchSpeed = 3;
   maxBackwardCrouchSpeed = 2;
   maxSideCrouchSpeed = 2;

   maxForwardProneSpeed = 0;
   maxBackwardProneSpeed = 0;
   maxSideProneSpeed = 0;

   maxForwardWalkSpeed = 5;
   maxBackwardWalkSpeed = 2;
   maxSideWalkSpeed = 3;

   maxUnderwaterForwardSpeed = 5;
   maxUnderwaterBackwardSpeed = 5;
   maxUnderwaterSideSpeed = 3;

   jumpForce = 12 * 140; //8.3 * 90;
   jumpEnergyDrain = 0;
   minJumpEnergy = 0;
   jumpDelay = 0;

   minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

   minImpactSpeed = 250;
   speedDamageScale = 3.8;

   boundingBox			= vectorScale("1.35 1.35 2.7", 4);
   crouchBoundingBox	= vectorScale("1.35 1.35 1.1", 4);
   
   pickupRadius = 0.75;
   
   // Foot Prints
   //decalData   = BuffFootprint;
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

   JumpSound			= "JumpSound";

   // Footstep Sounds
//   FootSoftSound        = BuffFootFallSound;
//   FootHardSound        = BuffFootFallSound;
//   FootMetalSound       = BuffFootFallSound;
//   FootSnowSound        = BuffFootFallSound;
//   FootShallowSound     = BuffFootFallSound;
//   FootWadingSound      = BuffFootFallSound;
//   FootUnderwaterSound  = BuffFootFallSound;
   //FootBubblesSound     = FootLightBubblesSound;
   //movingBubblesSound   = ArmorMoveBubblesSound;
   //waterBreathSound     = WaterBreathMaleSound;

   //impactSoftSound      = ImpactLightSoftSound;
   //impactHardSound      = ImpactLightHardSound;
   //impactMetalSound     = ImpactLightMetalSound;
   //impactSnowSound      = ImpactLightSnowSound;
   
   impactWaterEasy      = Splash1Sound;
   impactWaterMedium    = Splash1Sound;
   impactWaterHard      = Splash1Sound;
   
   groundImpactMinSpeed    = 10.0;
   groundImpactShakeFreq   = "4.0 4.0 4.0";
   groundImpactShakeAmp    = "1.0 1.0 1.0";
   groundImpactShakeDuration = 0.8;
   groundImpactShakeFalloff = 10.0;
   
   //exitingWater         = ExitingWaterLightSound;

   // Inventory Items
	maxItems   = 10;	//total number of bricks you can carry
	maxWeapons = 5;		//this will be controlled by mini-game code
	maxTools = 5;
	
	uiName = "Buff";
	rideable = false;
		lookUpLimit = 0.585398;
		lookDownLimit = 0.385398;

	canRide = true;
	showEnergyBar = false;
	paintable = true;

	brickImage = brickImage;	//the imageData to use for brick deployment

   numMountPoints = 0;
};



function BuffArmor::onAdd(%this,%obj)
{
   // Vehicle timeout
   %obj.mountVehicle = true;

   // Default dynamic armor stats
   %obj.setRepairRate(0);
}



//called when the driver of a player-vehicle is unmounted
function BuffArmor::onDriverLeave(%obj, %player)
{
	//do nothing
}

datablock ExplosionData(BuffBashExplosion) {
   lifetimeMS = 400;

   emitter[0] = pushBroomSparkEmitter;
   particleEmitter = pushBroomExplosionEmitter;
   particleDensity = 80;
   particleRadius = 1.0;

   cameraShakeFalloff = false;
   camShakeFreq = "2.0 3.0 1.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 2.5;
   camShakeRadius = 3;
   camShakeFalloff = 1;

   soundProfile = spearExplosionSound;

   uiName = "Buff Bash";

   //explosionShape = "";
   lifeTimeMS = 400;
   
   faceViewer     = true;
   explosionScale = "1 1 1";

   // Dynamic light
   lightStartRadius = 0;
   lightEndRadius = 0;
   lightStartColor = "0.0 0.0 0.0";
   lightEndColor = "0 0 0";
};

datablock ProjectileData(BuffBashProjectile)
{
   //projectileShapeName = "~/data/shapes/arrow.dts";
   directDamage        = 50;
   impactImpulse       = 0;
   verticalImpulse     = 0;
   explosion           = BuffBashExplosion;
   //particleEmitter     = as;

   muzzleVelocity      = 40;
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
   uiName = "Buff Bash Projectile";
};

datablock ItemData(BuffBashItem) {
   category = "Weapon";  // Mission editor category
   className = "Weapon"; // For inventory system

   shapeFile = "base/data/shapes/empty.dts";
   mass = 1;
   density = 0.2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

   uiName = "Buff Bash";
   iconName = "";
   doColorShift = false;
   colorShiftColor = "0.4 0.4 0.4 1.000";

   image = BuffBashImage;
   canDrop = true;
};

datablock ShapeBaseImageData(BuffBashImage) {
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   item = BuffBashItem;
   armReady = false;

   offset = "-1 -0.5 0";

   mountPoint = 0;

   canMountToBronson = 1;

   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   ammo = " ";
   projectile = BuffBashProjectile;
   projectileType = Projectile;

   melee = false;
   //raise your arm up or not
   armReady = false;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.4 0.4 0.4 1.000";

   stateName[0]        = "Activate";
   stateTimeoutValue[0]    = 1.3;
   stateTransitionOnTimeout[0]   = "Ready";

   stateName[1]         = "Ready";
   stateTransitionOnTriggerDown[1]  = "Fire";
   stateAllowImageChange[1]   = true;

   stateName[2]         = "Fire";
   stateTransitionOnTimeout[2]   = "Cooldown";
   stateTimeoutValue[2]    = 0.2;
   stateFire[2]         = true;
   stateScript[2]       = "onFire";
   stateWaitForTimeout[2]     = true;
   stateAllowImageChange[2]   = true;

   stateName[3]         = "Cooldown";
   stateTransitionOnTriggerUp[3] = "Ready";
};

function BuffBashImage::onFire(%this, %obj, %slot) {
   setStatistic("BuffAttacks", getStatistic("BuffAttacks", %obj.client) + 1, %obj.client);
   setStatistic("BuffAttacks", getStatistic("BuffAttacks") + 1);

   %obj.playThread(1, activate2);
   return parent::onFire(%this, %obj, %slot);
}

package BuffHit {
   function BuffBashProjectile::onCollision(%data, %obj, %col, %fade, %pos, %normal) {
      if (%col.getClassName() $= "FxDTSBrick" && %obj.sourceObject.getClassName() $= "Player") {
         %type = isBreakableBrick(%col, %obj.sourceObject);
         if (%type > 0) {
            //statistics
            setStatistic("BuffHits", getStatistic("BuffHits", %obj.client) + 1, %obj.client);
            setStatistic("BuffHits", getStatistic("BuffHits") + 1);
            if (%type == 1) {
               %col.killDelete();
            } else if (%type == 2) {
               %col.killDelete();
            } else {
               %col.damage(1, %obj);
            }
            %obj.client.incScore(1);
         }
      }
      return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
   }
};
activatePackage(BuffHit);

// package Buff {
//    function GameConnection::applyBodyColors(%this)
//    {
//       if (isObject(%this.player) && %this.player.getDatablock().getName() $= "BuffArmor") 
//       {
//          %color = %this.headColor;
//          %tint = max(getWord(%this.headColor, 0) - 0.14, 0) SPC max(getWord(%this.headColor, 1) - 0.16, 0) SPC getWords(%this.headColor, 2, 3);

//          %this.player.setNodeColor("ALL", %color);
//          %this.player.setNodeColor("nipples", %tint);
//          %this.player.setNodeColor("face", "0 0 0 1");
//          %this.player.setNodeColor("pants", %this.hipColor);
//          %this.player.setNodeColor("lShoe", %this.llegColor);
//          %this.player.setNodeColor("rShoe", %this.rlegColor);
//       }
//       else
//       {
//          return parent::applyBodyColors(%this);
//       }
//    }
// };
// activatePackage(Buff);