//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Load dts shapes and merge animations
datablock TSShapeConstructor(ShepherdDogDts)
{
	baseShape  = "./dog.dts";
	sequence0  = "./dog_root.dsq root";

	sequence1  = "./dog_run.dsq run";
	sequence2  = "./dog_walk.dsq walk";
	sequence3  = "./dog_back.dsq back";
	sequence4  = "./dog_side.dsq side";

	sequence5  = "./dog_crouch.dsq crouch";
	sequence6  = "./dog_crouchwalk.dsq crouchRun";
	sequence7  = "./dog_crouchback.dsq crouchBack";
	sequence8  = "./dog_crouchside.dsq crouchSide";

	sequence9  = "./dog_look.dsq look";
	sequence10 = "./dog_headside.dsq headside";
	sequence11 = "./dog_root.dsq headUp";

	sequence12 = "./dog_jump.dsq jump";
	sequence13 = "./dog_standjump.dsq standjump";
	sequence14 = "./dog_root.dsq fall";
	sequence15 = "./dog_root.dsq land";

	sequence16 = "./dog_activateLoop.dsq armAttack";
	sequence17 = "./dog_root.dsq armReadyLeft";
	sequence18 = "./dog_root.dsq armReadyRight";
	sequence19 = "./dog_root.dsq armReadyBoth";
	sequence20 = "./dog_root.dsq spearready";  
	sequence21 = "./dog_root.dsq spearThrow";

	sequence22 = "./dog_side.dsq talk";  

	sequence23 = "./dog_death.dsq death1"; 
	
	sequence24 = "./dog_root.dsq shiftUp";
	sequence25 = "./dog_root.dsq shiftDown";
	sequence26 = "./dog_root.dsq shiftAway";
	sequence27 = "./dog_root.dsq shiftTo";
	sequence28 = "./dog_root.dsq shiftLeft";
	sequence29 = "./dog_root.dsq shiftRight";
	sequence30 = "./dog_root.dsq rotCW";
	sequence31 = "./dog_root.dsq rotCCW";

	sequence32 = "./dog_root.dsq undo";
	sequence33 = "./dog_root.dsq plant";

	sequence34 = "./dog_sit.dsq sit";

	sequence35 = "./dog_root.dsq wrench";

   sequence36 = "./dog_activate.dsq activate";
   sequence37 = "./dog_activate.dsq activate2";

   sequence38 = "./dog_root.dsq leftrecoil";
};    

datablock PlayerData(ShepherdDogArmor)
{
   renderFirstPerson = false;
   emap = false;
   
   className = Armor;
   shapeFile = "./dog.dts";
   cameraMaxDist = 8;
   cameraTilt = 0.261;//0.174 * 2.5; //~25 degrees
   cameraVerticalOffset = 1.3;
     
   cameraDefaultFov = 90.0;
   cameraMinFov = 5.0;
   cameraMaxFov = 120.0;
   
   //debrisShapeName = "~/data/shapes/player/debris_player.dts";
   //debris = ShepherdDogDebris;

   aiAvoidThis = true;

   minLookAngle = -1.5708;
   maxLookAngle = 1.5708;
   maxFreelookAngle = 3.0;

   mass = 120;
   drag = 0.1;
   density = 0.7;
   maxDamage = 100;
   maxEnergy =  10;
   repairRate = 0.33;

   rechargeRate = 0.4;

   runForce = 80 * 90;
   runEnergyDrain = 0;
   minRunEnergy = 0;
   maxStepHeight= "1";
   maxForwardSpeed = 8;
   maxBackwardSpeed = 6;
   maxSideSpeed = 4;

   maxForwardCrouchSpeed = 3;
   maxBackwardCrouchSpeed = 2;
   maxSideCrouchSpeed = 2;

   maxForwardProneSpeed = 0;
   maxBackwardProneSpeed = 0;
   maxSideProneSpeed = 0;

   maxForwardWalkSpeed = 7;
   maxBackwardWalkSpeed = 4;
   maxSideWalkSpeed = 4;

   maxUnderwaterForwardSpeed = 8;
   maxUnderwaterBackwardSpeed = 4;
   maxUnderwaterSideSpeed = 2;

   jumpForce = 15 * 90; //8.3 * 90;
   jumpEnergyDrain = 0;
   minJumpEnergy = 0;
   jumpDelay = 0;

   minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;

   minImpactSpeed = 30;
   speedDamageScale = 3.8;

   boundingBox			= vectorScale("1.6 1.6 2.2", 4);
   crouchBoundingBox	= vectorScale("1.6 1.6 1.43", 4);
   
   pickupRadius = 0.75;
   
   // Foot Prints
   //decalData   = ShepherdDogFootprint;
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
//   FootSoftSound        = ShepherdDogFootFallSound;
//   FootHardSound        = ShepherdDogFootFallSound;
//   FootMetalSound       = ShepherdDogFootFallSound;
//   FootSnowSound        = ShepherdDogFootFallSound;
//   FootShallowSound     = ShepherdDogFootFallSound;
//   FootWadingSound      = ShepherdDogFootFallSound;
//   FootUnderwaterSound  = ShepherdDogFootFallSound;
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
	
	uiName = "Shepherd Dog";
	rideable = false;
		lookUpLimit = 0.585398;
		lookDownLimit = 0.385398;

	canRide = true;
	showEnergyBar = false;
	paintable = true;

	brickImage = brickImage;	//the imageData to use for brick deployment

   numMountPoints = 0;
};



function ShepherdDogArmor::onAdd(%this,%obj)
{
   // Vehicle timeout
   %obj.mountVehicle = true;

   // Default dynamic armor stats
   %obj.setRepairRate(0);

}



//called when the driver of a player-vehicle is unmounted
function ShepherdDogArmor::onDriverLeave(%obj, %player)
{
	//do nothing
}