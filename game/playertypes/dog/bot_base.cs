datablock fxDTSBrickData (BrickShepherdDogBot_HoleSpawnData)
{
	brickFile = "Add-ons/Bot_Hole/4xSpawn.blb";
	category = "Special";
	subCategory = "Holes";
	uiName = "Shepherd Dog Hole";
	iconName = "Add-Ons/Bot_ShepherdDog/icon_ShepherdDog";

	bricktype = 2;
	cancover = 0;
	orientationfix = 1;
	indestructable = 1;

	isBotHole = 1;
	holeBot = "ShepherdDogHoleBot";
};

datablock PlayerData(ShepherdDogHoleBot : ShepherdDogArmor)
{
	uiName = "";
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	maxItems   = 0;
	maxWeapons = 0;
	maxTools = 0;
	
	rideable = false;
	canRide = false;

	maxdamage = 125;//Bot Health
	jumpSound = "";//Removed due to bots jumping a lot
	
	//Hole Attributes
	isHoleBot = 1;

	//Spawning option
	hSpawnTooClose = 0;//Doesn't spawn when player is too close and can see it
	  hSpawnTCRange = 8;//above range, set in brick units
	hSpawnClose = 0;//Only spawn when close to a player, can be used with above function as long as hSCRange is higher than hSpawnTCRange
	  hSpawnCRange = 32;//above range, set in brick units

	hType = Neutral; //Enemy,Friendly, Neutral
	  hNeutralAttackChance = 0;
	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "ShepherdDog";//cannot contain spaces
	hTickRate = 3000;
	
	//Wander Options
	hWander = 1;//Enables random walking
	  hSmoothWander = 1;//This is in addition to regular wander, makes them walk a bit longer, and a bit smoother
	  hReturnToSpawn = 0;//Returns to spawn when too far
	  hSpawnDist = 64;//Defines the distance bot can travel away from spawnbrick
	  hGridWander = 0;//Locks the bot to a grid, overwrites other settings
	
	//Searching options
	hSearch = 1;//Search for Players
	  hSearchRadius = 64;//in brick units
	  hSight = 1;//Require bot to see player before pursuing
	  hStrafe = 0;//Randomly strafe while following player
	hSearchFOV = 1;//if enabled disables normal hSearch
	  hFOVRadius = 8;//max 10
	  hHearing = 1;//If it hears a player it'll look in the direction of the sound

	  hAlertOtherBots = 1;//Alerts other bots when he sees a player, or gets attacked

	//Attack Options
	hMelee = 1;//Melee
	  hAttackDamage = 20;//Melee Damage
	hShoot = 0;
	  hWep = "";
	  hShootTimes = 4;//Number of times the bot will shoot between each tick
	  hMaxShootRange = 256;//The range in which the bot will shoot the player
	  hAvoidCloseRange = 1;//
		hTooCloseRange = 7;//in brick units

	//Misc options
	hAvoidObstacles = 1;
	hSuperStacker = 0;//When enabled makes the bots stack a bit better, in other words, jumping on each others heads to get to a player
	hSpazJump = 0;//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 1;//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things

	hIdle = 1;// Enables use of idle actions, actions which are done when the bot is not doing anything else
	  hIdleAnimation = 1;//Plays random animations/emotes, sit, click, love/hate/etc
	  hIdleLookAtOthers = 1;//Randomly looks at other players/bots when not doing anything else
	    hIdleSpam = 0;//Makes them spam click and spam hammer/spraycan
	  hSpasticLook = 1;//Makes them look around their environment a bit more.
	hEmote = 1;

	hMeleeCI = "Dog";
};

function ShepherdDogHoleBot::onAdd(%this, %obj)
{
	%ret = parent::onAdd(%this,%obj);
	
	%color = getRandom(0,3);
	if(%color == 0)
		%obj.chestColor = "1 1 1 1";
	if(%color == 1)
		%obj.chestColor = "0.6 0.6 0.7 1";
	if(%color == 2)
		%obj.chestColor = "0.5 0.27 0.05 1";
	if(%color == 3)
		%obj.chestColor = "0.98 0.86 0.67 1";

	%obj.canBark = 1;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
	
	%obj.mountImage(dogYellowKeyImage, 2);
	// allow people to take control of the bot when they mount it
	//%obj.controlOnMount = 1;
	return %ret;
}


datablock AudioProfile(ShepherdDogDeath1Sound)
{
   filename    = "./death1.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(ShepherdDogDeath2Sound)
{
   filename    = "./death2.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(ShepherdDogBark1Sound)
{
   filename    = "./bark1.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(ShepherdDogBark2Sound)
{
   filename    = "./bark2.wav";
   description = AudioDefault3d;
   preload = true;
};

datablock AudioProfile(ShepherdDogBark3Sound)
{
   filename    = "./bark3.wav";
   description = AudioDefault3d;
   preload = true;
};


function ShepherdDogHoleBot::onBotLoop(%this,%obj)
{
	if (%obj.getState() $= "Dead")
		return;

	if (%obj.canBark && (getRandom(1, 10) == 1 || (%obj.hIsFollowing && %obj.hIsFollowing != %obj.owner)))
	{
		serverPlay3D("ShepherdDogBark" @ getRandom(1, 3) @ "Sound", %obj.getPosition());
		%obj.canBark = 0;
		schedule(getRandom(1000, 4000), 0, eval, %obj.getID() @ ".canBark = 1;");
	}

	if (isObject(%obj.hFollowing))
	{
		%obj.setAimObject(%obj.hFollowing);
	}

	//follow its owner if its not targeting anyone else
	%ret = parent::onBotLoop(%this, %obj);
	if (isObject(%obj.owner) && !%obj.hFollowing)
	{
		%obj.hFollowPlayer(%obj.owner, 1, 1);
		%obj.emote(loveImage);
		%obj.lastEmote = getSimTime();
	} 
	//look for a steak if its not targeting anyone
	else if (!%obj.isFollowingWhistle)
	{
		%distance = 10000;
		%pos = %obj.getEyePoint();
		%typemasks = $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | 
			$TypeMasks::StaticObjectType | $TypeMasks::PlayerObjectType;

		//iterate through backwards in case a steak gets deleted while searching for one
		for (%i = $SteakGroup.getCount()-1; %i >= 0; %i--)
		{
			echo("Looking for steaks: steakCount " @ $SteakGroup.getCount());
			%steak = $SteakGroup.getObject(%i);
			echo("Steak " @ %steak);
			if (isObject(%steak.spawnbrick)) {
				echo("    Steak is a spawned item, removing...");
				$SteakGroup.remove(%steak);
				continue;
			} else if ((%region = getRegion(%steak)) !$= "Outside" && %region !$= "Yard") {
				echo("    Steak is inside, removing...");
				$SteakGroup.remove(%steak);
				continue;
			}

			%steakPos = vectorAdd(%steak.getPosition(), "0 0 0.2");
			%distanceToSteak = vectorDist(%pos, %steakPos);
			//if distance > 2 check for line of sight
			if (%distanceToSteak < %distance && ((%str = getRegion(%steak)) $= "Outside" || %str $= "Yard"))
			{
				if (%distanceToSteak > 2)
				{
					echo("    Steak is far away - checking for LOS");
					%ray = containerRaycast(%pos, %steakPos, %typemasks, %obj);
					if (!isObject(getWord(%ray, 0)))
					{
						echo("    Steak is visible - pathing");
						%distance = %distanceToSteak;
						%closestSteak = %steak;
					} else {
						//talk(%ray);
					}
				}
				else
				{
					echo("    Steak is very close - pathing regardless of LOS");
					%distance = %distanceToSteak;
					%closestSteak = %steak;
				}
			}
		}

		//target the steak
		if (isObject(%closestSteak))
		{
			%obj.hFollowingSteak = 1;
			echo("Pathing to steak " @ %closestSteak);
			%obj.setMoveDestination(%closestSteak.getPosition());
			%obj.setAimObject(%closestSteak);
		}
	}
	//look for nearby prisoners to target if it just followed a whistle
	else if (%obj.isFollowingWhistle || !%obj.hFollowing) 
	{
		//do container search then set as target
	}
	return %ret;
}

function ShepherdDogHoleBot::onBotCollision( %this, %obj, %col, %normal, %speed )
{
	if (%obj.getState() $= "Dead")
		return;

	if (%obj.canBark)
	{
		serverPlay3D("ShepherdDogBark" @ getRandom(1, 3) @ "Sound", %obj.getPosition());
		%obj.canBark = 0;
		schedule(getRandom(500, 1000), 0, eval, %obj.getID() @ ".canBark = 1;");
	}
}

function ShepherdDogHoleBot::onBotFollow(%this,%obj,%targ)
{
	//Called when the target follows a player each tick, or is running away
	%pos = %obj.getPosition();
	%targpos = %targ.getPosition();
	%xypos = getWord(%pos, 0) SPC getWord(%pos, 1) SPC 0;
	%xytargpos = getWord(%targpos, 0) SPC getWord(%targpos, 1) SPC 0;

	%xydist = vectorDist(%xypos, %xytargpos);
	%zdist = getWord(%targpos, 2) - getWord(%pos, 2);

	//bark at players too high for them to reach, or emote love if their target is their owner
	if (%zdist > 2.4 && %xydist < 2 && !%obj.hIsRunning) 
	{
		if (%targ.getID() == %obj.owner && %obj.lastEmote < getSimTime()-5000)
		{
			%obj.emote(loveImage);
			%obj.lastEmote = getSimTime();
		}
		else if (%obj.canBark)
		{
			serverPlay3D("ShepherdDogBark" @ getRandom(1, 3) @ "Sound", %obj.getPosition());
			%obj.canBark = 0;
			schedule(getRandom(500, 1000), 0, eval, %obj.getID() @ ".canBark = 1;");
		}
	}
}

function ShepherdDogHoleBot::onBotDamage(%this,%obj,%source,%pos,%damage,%type)
{
	//Called when the bot is being damaged
	if (%obj.isEating)
	{
		StopEatSteak(%obj);
		%obj.setAimObject(%source);
		%obj.hFollowingSteak = 0;
	}
	return parent::onBotDamage(%this, %obj, %source, %pos, %damage, %type);
}

package BotHole_Dogs
{
	function ShepherdDogHoleBot::onDisabled(%this, %obj, %state) //makes bots have death sound and animation and runs the required bot hole command
	{
		if (!%obj.getState() $= "Dead")
			return;

		holeBotDisabled(%obj);
		serverPlay3D("ShepherdDogDeath" @ getRandom(1, 2) @ "Sound", %obj.getPosition());
		%obj.unMountImage(2);
		%obj.playThread(2, "death1");

		%i = new Item()
		{
			datablock = yellowKeyItem;
			canPickup = true;
			rotate = false;
			position = %obj.getHackPosition();
			minigame = getMinigameFromObject(%obj);
		};
		MissionCleanup.add(%i);
		%i.setVelocity("0 0 15");
		%i.schedule(60000, fadeOut);
		%i.schedule(61000, delete);

		centerprintAll("<font:Impact:30>The guard dog has died and dropped its key!", 20);
		setStatistic("GuardDogDied", $Server::PrisonEscape::currTime);

		messageAll('', "<bitmap:" @ $DamageType::MurderBitmap[$DamageType::Dog] @ "> [" @ getTimeString($Server::PrisonEscape::currTime) @ "]");
		
		//return parent::onDisabled(%this, %obj, %state); //disabled to prevent dog body from despawning
	}

	function ShepherdDogArmor::onDisabled(%this, %obj, %state) //for players; slightly different behavior
	{
		if (!%obj.getState() $= "Dead") 
		{
			return;
		}

		holeBotDisabled(%obj);
		serverPlay3D("ShepherdDogDeath" @ getRandom(1, 2) @ "Sound", %obj.getPosition());
		%obj.playThread(2, "death1");

		return parent::onDisabled(%this, %obj, %state);
	}

	function AIPlayer::hMeleeAttack(%obj, %col)
	{
		if (%obj.getDatablock().getName() !$= "ShepherdDogHoleBot") 
		{
			return parent::hMeleeAttack(%obj, %col);
		}

		if (!isObject(%col)) 
		{
			return parent::hMeleeAttack(%obj, %col);
		}

		if (%col.getType() & $TypeMasks::PlayerObjectType)
		{
			if (%col.client.isBeingStunned)
				return;
			if (!(%ret = parent::hMeleeAttack(%obj, %col)))
				return %ret;

			%col.setVelocity(vectorScale(%obj.getForwardVector(), 10));
			stun(%col, 2);
			return %ret;
		}
	}

	function AIPlayer::activateStuff(%this) {
		if (%this.getDatablock().getName() $= ShepherdDogHoleBot) {
			return;
		}
		return parent::activateStuff(%this);
	}

	function Observer::onTrigger(%this, %obj, %triggerNum, %val)
	{
		%player = %obj.getControllingClient().player;
		if (isObject(%player) && %player.isTumbling)
			return;
		parent::onTrigger(%this, %obj, %triggerNum, %val);
	}

	function Player::playDeathCry(%this)
	{
		//this is for players
		if (%this.getDatablock().getName() $= "ShepherdDogArmor") 
		{
			return;
		}
		parent::playDeathCry(%this);
	}

	function ShepherdDogArmor::onTrigger(%this, %player, %slot, %val)
	{
		if (isObject(%player.client) && %player.getDatablock().getName() $= "ShepherdDogArmor" && %slot == 0 && %val)
		{
			serverPlay3D("ShepherdDogBark" @ getRandom(1, 3) @ "Sound", %player.getPosition());
		}
		parent::onTrigger(%this, %player, %slot, %val);
	}

	function ShepherdDogHoleBot::hSpazzClick(%obj, %amount, %panic)
	{
		//dont want the dog to spazzclick
		return;
	}

	function WheeledVehicleData::onCollision(%this, %obj, %col, %vel, %zVel) {
		if (%this.getName() $= "deathVehicle" && %col.getDatablock().getName() $= "ShepherdDogHoleBot") 
		{
			return;
		}
		else 
		{
			return parent::onCollision(%this, %obj, %col, %vel, %zVel);
		}
	}

	function checkHoleBotTeams(%obj, %target, %neutralAttack, %melee) {
		if (%obj.getDatablock().getName() $= "ShepherdDogHoleBot" && %obj.hFollowingSteak) {
			return 0;
		}
		return parent::checkHoleBotTeams(%obj, %target, %neutralAttack, %melee);
	}
};
activatePackage(BotHole_Dogs);

datablock ShapeBaseImageData(healingImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.1";
	eyeOffset = "0 0 -0.18";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = true;
	colorshiftColor = "1 1 1 1";

	stateName[0]				= "Activate";
	stateTimeoutValue[0]		= 0.01;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1]				= "Ready";
	stateTimeoutValue[1]		= 1;
	stateEmitter[1]				= "healCrossEmitter";
	stateEmitterTime[1]			= 30000;
	stateEmitterNode[1]			= "mountPoint";
};

function EatSteak(%obj, %eatTime)
{
	//talk("Dog starting to eat for " @ %eatTime);
	//make it hold the steak
	%obj.mountImage(SteakDogImage, 1);
	%obj.playThread(0, sit);

	//stop bot hole activity
	%obj.stopHoleLoop();

	%obj.isEating = 1;

	%x = hGetRandomFloat(0,10,1);

	%y = hGetRandomFloat(0,10,1);
	
	%z = hGetRandomFloat(0,3,1);

	%vec = %x SPC %y SPC %z;
	%obj.setAimVector( %vec );

	%obj.emote(loveImage);
	%obj.mountImage(healingImage, 0);

	%obj.eatSteakSchedule = schedule(%eatTime, 0, StopEatSteak, %obj);
}

function StopEatSteak(%obj)
{
	%obj.unMountImage(0);
	//remove the image from it
	%obj.unMountImage(1);
	%obj.playThread(0, root);
	%obj.startHoleLoop();
	%obj.isEating = 0;

	//if dog was interrupted while eating, drop the steak
	if (isEventPending(%obj.eatSteakSchedule))
	{
		%pos = vectorSub(vectorAdd(%obj.getPosition(), "0 0 3"), %obj.getEyeVector());
		%velocity = vectorAdd(vectorScale(%obj.getEyeVector(), -3), "0 0 10");

		%i = new Item()
		{
			minigame = getMinigameFromObject(%obj);
			datablock = SteakItem;
			canPickup = true;
			rotate = false;
			timeToFinish = getTimeRemaining(%obj.eatSteakSchedule);

			position = %pos;
		};
		MissionCleanup.add(%i);
		%i.schedule(30000 - 500, fadeOut);
		%i.schedule(30000, delete);
		%i.setVelocity(%velocity);
		
		$SteakGroup.add(%i);

		cancel(%obj.eatSteakSchedule);
		%obj.unMountImage(0);
		%obj.unMountImage(1);
	}
	else
	{
		//statistics
		$Server::PrisonEscape::SteaksEaten++;

		%obj.setDamageLevel(%obj.getDamageLevel() - 30);
		%obj.hFollowingSteak = 0;
	}
}

datablock ShapeBaseImageData(dogYellowKeyImage : yellowKeyImage) {
	offset = "-0.4 0.03 0.3";
	rotation = eulerToMatrix("90 0 0");
	mountPoint = 0;
};