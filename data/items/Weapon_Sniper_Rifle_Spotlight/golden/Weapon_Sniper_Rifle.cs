

//bullet trail effects


datablock ProjectileData(SniperRifleSpotlightGoldenProjectile)
{
	projectileShapeName = "./bullet.dts";
	directDamage		  = 100;
	directDamageType	 = $DamageType::SniperrifleSpotlight;
	radiusDamageType	 = $DamageType::SniperrifleSpotlight;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;			 //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;			 //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

	impactImpulse		  = 0;
	verticalImpulse	  = 0;
	explosion			  = SniperRifleSpotlightExplosion;
	particleEmitter	  = projectileGoldenEmitter;

	muzzleVelocity		= 200;
	velInheritFactor	 = 1;

	armingDelay			= 00;
	lifetime				= 4000;
	fadeDelay			  = 3500;
	bounceElasticity	 = 0.5;
	bounceFriction		= 0.20;
	isBallistic			= false;
	gravityMod = 0.0;

	hasLight	 = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";
};

//////////
// item //
//////////
datablock ItemData(SniperRifleSpotlightGoldenItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./sniper rifle v2.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Golden Sniper Rifle Spotlight";
	iconName = "Add-ons/Gamemode_PPE/icons/sniper";
	doColorShift = false;
	colorShiftColor = "0.95 0.85 0.05 1.000";

	 // Dynamic properties defined by the scripts
	image = SniperRifleSpotlightGoldenImage;
	canDrop = true;
};


////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(SniperRifleSpotlightGoldenImage)
{
	// Basic Item properties
	shapeFile = "./sniper rifle v2.dts";
	emap = true;

	// Specify mount point & offset for 3rd person, and eye offset
	// for first person rendering.
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.35 0.55 -0.35";
	rotation = eulerToMatrix( "0 0 0" );

	// When firing from a point offset from the eye, muzzle correction
	// will adjust the muzzle vector to point to the eye LOS point.
	// Since this weapon doesn't actually fire from the muzzle point,
	// we need to turn this off.  
	correctMuzzleVector = true;

	// Add the WeaponImage namespace as a parent, WeaponImage namespace
	// provides some hooks into the inventory system.
	className = "WeaponImage";

	// Projectile && Ammo.
	item = SniperRifleSpotlightGoldenItem;
	ammo = " ";
	projectile = SniperRifleSpotlightProjectile;
	projectileType = Projectile;

	casing = SniperRifleSpotlightDebris;
	shellExitDir		  = "1.0 0 0.8";
	shellExitOffset	  = "0 0 0";
	shellExitVariance	= 15.0;	
	shellVelocity		 = 3.0;

	//melee particles shoot from eye node for consistancy
	melee = false;
	//raise your arm up or not
	armReady = true;
	minShotTime = 2.00;	//minimum time allowed between shots (needed to prevent equip/dequip exploit)

	doColorShift = false;
	colorShiftColor = SniperRifleSpotlightGoldenItem.colorShiftColor;//"0.400 0.196 0 1.000";

	//casing = " ";

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]						  = "Activate";
	stateTimeoutValue[0]				= 0.15;
	stateTransitionOnTimeout[0]		= "Smoke";
	stateSequence[0]				= "Ready";
	stateSound[0]					= SniperRifleSpotlightUnholsterSound;
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= mountPoint;
	stateEmitterTime[0]				= 1000;

	stateName[6]					="PreReady";
	stateTimeoutValue[6]				= 0.05;
	stateTransitionOnNoAmmo[6]		= "Reload";
	stateTransitionOnAmmo[6]		 	= "Ready";
	stateWaitForTimeout[6]			= true;
	stateEmitter[6]					= GoldenEmitter;
	stateEmitterNode[6]				= mountPoint;
	stateEmitterTime[6]				= 1000;

	stateName[1]							= "Ready";
	stateTimeoutValue[1]				= 0.14;
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]			= true;
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= mountPoint;
	stateEmitterTime[1]				= 1000;

	stateName[2]						  = "Fire";
	stateTransitionOnTimeout[2]	  = "Smoke";
	stateTimeoutValue[2]				= 0.1;
	stateFire[2]						  = true;
	stateAllowImageChange[2]		  = false;
	stateSequence[2]					 = "Fire";
	stateScript[2]						= "onFire";
	stateWaitForTimeout[2]			= true;
	stateEmitter[2]					= GunFlashEmitter;
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzlePoint";
	stateSound[2]					= gunShot1Sound;

	stateName[3] 					= "Smoke";
	stateEmitter[3]					= SniperRifleSpotlightSmokeEmitter;
	stateEmitterTime[3]				= 0.5;
	stateEmitterNode[3]				= "muzzlePoint";
	stateTimeoutValue[3]				= 0.7;
	stateTransitionOnTimeout[3]	  = "Reload";
	stateWaitForTimeout[3]			= true;

	stateName[4]			= "Reload";
	stateSequence[4]		= "Reload";
	stateTimeoutValue[4]	= 1.23;
	stateTransitionOnTimeout[4]	  = "PostReload";
	stateSound[4]					= "SniperRifleSpotlightBoltSound";
	stateEjectShell[4]				= true;
	stateEmitter[4]					= GoldenEmitter;
	stateEmitterNode[4]				= mountPoint;
	stateEmitterTime[4]				= 1000;

	stateName[5]			= "PostReload";
	stateScript[5]			= "onReload";
	stateTransitionOnTimeout[5] = "Ready";
	stateTimeoutValue[5]				= 0.05;
	stateWaitForTimeout[5]			= true;
	stateEmitter[5]					= GoldenEmitter;
	stateEmitterNode[5]				= mountPoint;
	stateEmitterTime[5]				= 1000;
	//stateSequence[4]	= "Ready";

};

function SniperRifleSpotlightGoldenImage::onFire(%this,%obj,%slot)
{
	//statistics
	setStatistic("SniperShotsFired", getStatistic("SniperShotsFired", %obj.client) + 1, %obj.client);
	setStatistic("SniperShotsFired", getStatistic("SniperShotsFired") + 1);

	%obj.playThread(2, plant);
	Parent::onFire(%this,%obj,%slot);
	%obj.setImageAmmo(%slot, 0);
}

function SniperRifleSpotlightGoldenImage::onReload(%this, %obj, %slot)
{
	%obj.setImageAmmo(%slot, 1);
}

function SniperRifleSpotlightGoldenImage::onMount(%this, %obj, %slot)
{
	return parent::onMount(%this, %obj, %slot);
}

function SniperRifleSpotlightGoldenImage::onUnMount(%this, %obj, $slot)
{
	return parent::onUnMount(%this, %obj, %slot);
}

package SniperRifleSpotlight {
	function SniperRifleSpotlightGoldenImage::setImageAmmo(%slot, %hasAmmo) {
		talk(%slot SPC %hasAmmo);
		parent::setImageAmmo(%slot, %hasAmmo);
	}

	function SniperRifleSpotlightGoldenProjectile::onCollision(%data, %obj, %col, %fade, %pos, %normal) {
		%guard = %obj.originPoint;
		%type = $TypeMasks::PlayerObjectType;
		%radius = 10;

		//look for nearby spotlight from firing position
		initContainerRadiusSearch(%guard, %radius, %type);
		%target = 0;
		while (isObject(%next = containerSearchNext())) {
			if (%next.getDatablock().getName() $= "SpotlightArmor") {
				%target = %next;
				break;
			}
		}

		//end it here if no spotlight found
		if (!isObject(%target)) {
			return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
		}

		//track player if hit player directly
		if (%col.getClassName() $= "Player" && !%col.isGuard && isObject($Server::PrisonEscape::CommDish)) {
			%target.spotLightTarget = %col;
			%target.spotLightTargetLocation = "";

			//statistics: increment %client.shotsHit
			setStatistic("SniperShotsHit", getStatistic("SniperShotsHit", %obj.client) + 1, %obj.client);
			setStatistic("SniperShotsHit", getStatistic("SniperShotsHit") + 1);
		} else {
			if (!isObject($Server::PrisonEscape::CommDish)) {
				%target.spotLightTarget = 0;
				%target.spotLightTargetLocation = %pos;	
				return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
			}
			//do container search for nearby players (4x stud radius) to track
			initContainerRadiusSearch(%pos, 2, $TypeMasks::PlayerObjectType);
			%player = 0;
			while (isObject(%next = containerSearchNext())) {
				if (%next.getClassName() $= "Player") {
					%player = %next;
					break;
				}
			}

			if (!isobject(%player) && !%player.isGuard) {
				%target.spotLightTarget = 0;
				%target.spotLightTargetLocation = %pos;	
			} else {
				%target.spotLightTarget = %player;
				%target.spotLightTargetLocation = "";	
			}	
		}

		parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
	}
};
activatePackage(SniperRifleSpotlight);