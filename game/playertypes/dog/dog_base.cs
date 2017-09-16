$MOVEDELTATHRESHOLD = 3;
$MOVEDELTATHRESHOLDTIME = 5000;

$CPB::DOGATTACKDAMAGE = 20;
AddDamageType("Dog",	'<bitmap:Add-Ons/Gamemode_PPE/ci/Dog> %1',	 '%2 <bitmap:Add-Ons/Gamemode_PPE/ci/Dog> %1', 0.5, 1);
//useful functions:
//obj.setAimObject/Vector/Location
//obj.hDetectWall(%keepMoving) (set to 1, since I'm not using hState)
//obj.hJump(%time to stay jumping, default to 1000)

package CPB_DogAI {
	function Armor::onCollision(%db, %this, %col, %vec, %speed) {
		if (%db.getID() == ShepherdDogHoleBot.getID() || %db.getID() == ShepherdDogArmor.getID()) {
			if (minigameCanDamage(%this, %col) && getSimTime() - %this.lastattacked > 1500) {
				%this.dogAttack( %col );
			}
		}
		return parent::onCollision(%db, %this, %col, %vec, %speed);
	}

	function Armor::Damage(%data, %obj, %sourceObject, %position, %damage, %damageType) {
		if (%db.getID() == ShepherdDogHoleBot.getID() || %db.getID() == ShepherdDogArmor.getID()) {
			schedule(2000, %obj, eval, %obj @ ".lastDamaged = " @ getSimTime() @ ";");
			if (!%obj.isFollowingWhistle && !(%this.getDamagePercent() > 0.8 && getSimTime() - %this.lastDamaged < 8000)) {
				%obj.chase(%sourceObject);
			}
		}
		return parent::Damage(%data, %obj, %sourceObject, %position, %damage, %damageType);
	}
};
activatePackage(CPB_DogAI);

function AIPlayer::setMode(%this, %mode, %targ) {
	// general approach: 3 distinct modes, same movement pattern but different passive loops

	// idle mode with priority:
	// 1) look for steak
	// 2) look for player3333
	// 3) wander

	// chase mode with priority:
	// 1) chase attackers
	// 2) chase target
	// 3) look for steak

	// run away mode with priority:
	// 1) look for steak
	// 2) run to closest tower
	// 3) attack attackers

	//actions that cause mode change:
	//-	attacked - immediately attack attacker
	//- whistle blown - immediately disengage mode to target tower and go there
	//- target goes inside - immediately disengage and head to nearest tower
	//- steak thrown - ignore whistle for 10 sec, go for steak. attack interrupts

	//decide what to run based on target + target state + mode

	if (%mode $= "Wander") {
		cancel(%this.chaseLoop);
		cancel(%this.retreatLoop);

		if (%this.getDamagePercent() > 0.8 && getSimTime() - %this.lastDamaged < 8000) {
			%this.setMode("Retreat"); 
			%closest = getClosestTower(%this.position);
			if (isObject(%closest)) { //have it path to closest alive tower
				%this.retreatPos = getWords(%closest.spawn.getPosition(), 0, 1) SPC getWord(%this.position, 2);
			}
		} else {
			%this.retreatPos = "";
		}
	} else if (%mode = "Chase") {
		cancel(%this.wanderLoop);
		cancel(%this.retreatLoop);

		%this.retreatPos = "";

		%this.chase(%targ);

	} else if (%mode = "Retreat") {
		cancel(%this.chaseLoop);
		cancel(%this.wanderLoop);

		%this.retreat(%this.retreatPos);
	}
}

function AIPlayer::chaseClosestPrisoner(%this, %obj) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (%cl.isOutside() && (%cl = ClientGroup.getObject(%i)).isPrisoner) {
			if (%curr = vectorDist(%cl.player.position, %this.position) > 16) {
				%ray = ContainerRayCast(%this.getHackPosition(), %cl.player.getHackPosition());
				if (!isObject(%ray)) {
					continue;
				}
			}

			if (%curr < %closest || %closest $= "") {
				%closest = %curr;
				%closestPlayer = %cl.player;
			}
		}
	}

	if (isObject(%closestPlayer)) {
		%this.chase(%closestPlayer);
		cancel(%this.chaseLoop);
	} else {	
		%this.hLastMoveRandom = 1;
		%this.hClearMovement();
		%xRand = getrandom(-10,10);
		%yRand = getrandom(-10,10);
		
		%fPos = %this.hReturnForwardBackPos(-15);

		%this.setMoveDestination( vectoradd(%fPos,%xRand SPC %yRand SPC 0) );

		if(%this.hEmote)
		{
			%this.emote(wtfImage);
		}
		return;
	}
}

function AIPlayer::retreat(%this, %targPos) {
	cancel(%this.retreatLoop);

	if (%targPos !$= "") {
		if (vectorDist(%this.position, %targPos) < 2) { //arrived at location, go to wander mode
			%this.setMode("Wander");
		} else {
			%this.setAimLocation(%targPos);
			%this.setMoveY(1);
			%this.dogAvoidObstacle();
		}
	}
}

function AIPlayer::chase(%this, %obj) {
	cancel(%this.chaseLoop);
	if (!isObject(%obj) || %obj.isDisabled()) {
		return;
	}

	%this.chaseTarget = %obj;

	//useful variables
	%targPos = %obj.position;
	%pos = %this.position;
	%zDist = getWord(%targPos, 2) - getWord(%pos, 2);
	%xyDist = vectorSub(getWords(%targPos, 0, 1));

	//check if I haven't moved much, if so lose the target and go back to wander mode
	if (getSimTime() - %this.lastDeltaThresholdTime > $MOVEDELTATHRESHOLDTIME) {
		%this.hDetectWall(1);
		%this.setMode("Wander");
		return;
	}

	%this.setAimObject(%obj);
	%this.setMoveY(1);

	if (vectorLen(vectorSub(%pos, %targPos)) < 2) { //strafe if close to player
		%this.setMoveX();
		if(%obj.hLastDir) {
			%obj.setMoveX(%obj.hLastDir);
		} else {
			%chance = getRandom(0,2);
			if(!getRandom(0,1)) {
				%obj.hLastDir = %chance;
			}

			switch (%chance) {
				case 1: %obj.setMoveX(1);		
				case 2: %obj.setMoveX(-1);
				default: %obj.setMoveX(0);
			}
			schedule(getRandom(1, 3) * 1000, %obj, eval, %obj @ ".hLastDir = 0;");
		}
	} else { //cancel strafe if out of range
		%this.setMoveX(0);
		%obj.hLastDir = 0;
	}
	%this.dogAvoidObstacle(); //handles getting stuck, supposedly, so dont bother with stuck check

	// %this.lastPositionDelta = vectorLen(vectorSub(%this.position, %this.lastPosition));
	// %this.lastPosition = %this.position;

	// //Record the last time I moved more than a few studs
	// if (%this.lastPositionDelta > $MOVEDELTATHRESHOLD) {
	// 	%this.lastDeltaThresholdTime = getSimTime();
	// }
	%this.chaseLoop = %this.schedule(1500, chase, %obj);
}


//Obstacle avoidance function, self explanatory, well if we're talking about [i]what[/i] it does
//How it does it: it shoots a ray forward, left, right, above, as well as one determined by jump height
//then at the end uses this knowledge to react to the world
function AIPlayer::dogAvoidObstacle( %obj, %noStrafe, %onlyJump, %onStuck )
{		
	%data = %obj.getDataBlock();
	
	%leftBlocked = 0;
	%rightBlocked = 0;
	%forwardBlocked = 0;
	%jumpBlocked = 0;
	%canJump = 1;
	%avoided = 0;
	%canJet = 0;
	
	if(%obj.hLastDirAttempts >= 3)
		%obj.hLastDir = 0;
	
	//if we have a path brick and we're close to it, just try and go to it to prevent circling around target
	if( isObject(%obj.chaseTarget) )
	{
		%targ = %obj.chaseTarget;
		%targPos = %targ.getPosition();
	}
	%scale = getWord(%obj.getScale(),0);

	%pos = vectorAdd(%obj.getPosition(),"0 0" SPC 0.8*%scale);
	%vec = %obj.getForwardVector();
	
	%xVec = getWord(%vec, 0);
	%yVec = getWord(%vec, 1);

	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
	

	%dist = 5*%scale;
	%sDist = 3*%scale;
	//Forward Check
	%fPos = vectorAdd(%pos,vectorScale(%vec,%dist));//5

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		%blockingBrick = 0;
	
		if( %target.getType() & $TypeMasks::FxBrickObjectType )
		{
			// if we encounter a brick remember it
			%blockingBrick = %target;
			
			if( %blockingBrick.numEvents )
				%obj.activateStuff();
				
		}
		else if( %target.getClassName() $= "AIPlayer" )// && getWord(%targPos,2) > getWord(%obj.getPosition(),2)+2)
		{
			%obj.clearMoveX();
			%obj.hJump();
		}
		else if( %target.getClassName() $= "Player" )
		{
			%forwardBlockPlayer = 1;

			if( %noStrafe && !getRandom(0,3) ) 
				%forwardBlockPlayer = 0;
		}
		
		

		%forwardBlocked = 1;
	}

	if( %onStuck )
		%forwardBlocked = 1;
		
	if( !%forwardBlocked ) //no obstacle to avoid/handle, return;
	{
		return;
	}

	//Left Check
	%sVec = -%yVec SPC %xVec SPC 0;
	%fPos = vectorAdd(%pos,vectorScale(%svec,%sDist));

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		// echo("left Blocked");
		%leftBlocked = 1;
	}

	//Right Check
	%sVec = %yVec SPC -%xVec SPC 0;
	%fPos = vectorAdd(%pos,vectorScale(%svec,%sDist));

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		// echo("Right Blocked");
		%rightBlocked = 1;
	}

	//Up Jump Check
	%target = ContainerRayCast(%pos, vectorAdd(%pos,"0 0" SPC 5*%scale), %mask,%obj);
	if(%target)
	{
		//echo("Jump Blocked");
		//If there's an bot above me tell him to jump up so we can get that bastard
		if(isObject(%target) && %target.getClassName() $= "AIPlayer")
		{
			%obj.clearMoveX();
			%obj.hJump();
			%leftBlocked = 1;
			%rightBlocked = 1;
			
			%target.clearMoveX();
			%target.hJump();
			%target.hStackMode = 1;
		}
		
		%jumpBlocked = 1;
	}

	//Can I jump it?
	
	// temporary hack for the horse, since I don't exactly know how  the jump formula works in torque
	if( %data.jumpForce == 1530 )
		%unitPerForce = 0.004;
	else
		%unitPerForce = 0.0025;
		
		
	%jumpHeight = %unitPerForce * %data.jumpForce;
	
	%pos = vectorAdd(%pos,"0 0" SPC %jumpHeight * %scale);
	%fPos = vectorAdd(%pos,vectorScale(%vec,%dist));

	%target = ContainerRayCast(%pos, %fPos, %mask, %obj);
	if(%target)
	{
		// %target.setColor(0);
		// echo("Too High to jump");
		%canJump = 0;
		//if it's a player or bot try to jump if he's blocking our way
		if(%target.getClassName() $= "player") 
			%canJump = 1;

		if(%target.getClassName() $= "AIPlayer") 
			%canJump = 1;
		
		if( %data.canJet && !getRandom( 0, 1 ) )
		{
			%canJump = 1;
			%canJet = 1;
		}

		
		//Random jumping always good as well
		if(!getRandom(0,2)) 
			%canJump = 1;
	}
		
	if(%forwardBlocked && !%avoided && !%jumpBlocked && %canJump && !%forwardBlockPlayer)
	{
		//if in water we need to hold jump a bit longer so we can get out, this causes some problems with sea based bots, bot overall it works good
		if(%obj.getWaterCoverage() && %data.drag >= 0.05)
			%obj.hJump();
		else
			%obj.hJump(150);
		
		if( getRandom(0,1) && !%obj.getWaterCoverage())
		{
			%obj.hCrouch(1500);
		}
		//return;
		%hasJumped = 1;
		
		%obj.hAvoidJumpTries++;
	}
	else
		%obj.hAvoidJumpTries = 0;
	
	//if we are set to only jump, return
	if(%onlyJump)
		return;
		
	if(%forwardBlocked && ( !%hasJumped || %obj.hAvoidJumpTries > 2 ) && !%noStrafe)
	{
		%obj.hAvoidJumpTries = 0;

		if(!%rightBlocked && !%leftBlocked)
		{
			//Check if we're already going one way, good for long walls
			if(%obj.hLastDir)
			{
				%obj.setMoveX(%obj.hLastDir);
				%avoided = 1;
				%obj.hLastDirAttempts++;
			}
			else
			{
				%chance = getRandom(1,2);
				if(!getRandom(0,1))
					%obj.hLastDir = %chance;

				switch (%chance)
				{
					case 1: %obj.setMoveX(1);		
					case 2: %obj.setMoveX(-1);
				}
				%avoided = 1;
			}
		}
		else if(!%leftBlocked)
		{
			%avoided = 1;
			%obj.setMoveX(-1);
		}
		else if(!%rightBlocked)
		{
			%avoided = 1;
			%obj.setMoveX(1);
		}
	}
	//Check if the bot is stuck
	if( !%onStuck &&  vectorDist(%obj.hLastPosition ,%obj.getPosition()) < 2*%scale && !%obj.hLastMoveRandom && getSimTime() - 3000 > %obj.lastattacked )
	{
		//echo("I'm stuck!");
		if(isObject(%obj.chaseTarget) && vectorDist(%pos, %obj.chaseTarget.getPosition()) <= 4)
		{
			%obj.hLastMoveRandom = 0;
			return;
		}
		%obj.hLastMoveRandom = 1;
		%obj.hClearMovement();
		%xRand = getrandom(-10,10);
		%yRand = getrandom(-10,10);
		
		%fPos = %obj.hReturnForwardBackPos(-15);

		%obj.setMoveDestination( vectoradd(%fPos,%xRand SPC %yRand SPC 0) );

		if(%obj.hEmote)
		{
			%obj.emote(wtfImage);
		}
		return;
	}
	%obj.hLastMoveRandom = 0;
	%obj.hLastPosition = %obj.getPosition();
}

function AIPlayer::dogAttack( %obj, %col )
{
	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		%client = %col.client;

		%name = %col.client.name;
		%obj.playthread(2,activate2);

		if (!isObject(%obj.hFakeProjectile)) {
			%obj.hFakeProjectile = new scriptObject(){};
			%obj.hFakeProjectile.sourceObject = %obj;
			%obj.hFakeProjectile.client = %obj;
		}

		%col.damage(%obj.hFakeProjectile, %col.getposition(), $CPB::DOGATTACKDAMAGE, $DamageType::Dog);
		%obj.lastattacked = getSimTime();
	}
}