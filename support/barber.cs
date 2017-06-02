$HairCount = 31;
$Hair[0]	= "None";
$Hair[1]	= "Comb-OverHair";
$Hair[2]	= "Corn RowsHair";
$Hair[3]	= "SuavHair";
$Hair[4]	= "MohawkHair";
$Hair[5]	= "NeatHair";
$Hair[6]	= "JudgeHair";
$Hair[7]	= "Wig";
$Hair[8]	= "BobCutHair";
$Hair[9]	= "GoateeHair";
$Hair[10]	= "Old WomanHair";
$Hair[11]	= "EmoHair";
$Hair[12]	= "BroadHair";
$Hair[13]	= "PunkHair";
$Hair[14]	= "AfroHair";
$Hair[15]	= "PhoenixHair";
$Hair[16]	= "DaphneHair";
$Hair[17]	= "Flat TopHair";
$Hair[18]	= "MomHair";
$Hair[19]	= "ThinningHair";
$Hair[20]	= "BaldingHair";
$Hair[21]	= "PompadourHair";
$Hair[22]	= "FamiliarHair";
$Hair[23]	= "MulletHair";
$Hair[24]	= "SlickHair";
$Hair[25]	= "Turban";
$Hair[26]	= "PonytailHair";
$Hair[27]	= "BowlHair";
$Hair[28]	= "BunnHair";
$Hair[29]	= "ShaggyHair";
$Hair[30]	= "SaiyanHair";

//Object properties:
//Client
//	customizingMode
//	customizationMount
//	isCustomizing
//	lastCustomizeTime
//Player
//	isCustomizing
//	canDismount
//	chairBot

//Functions:
//Packaged:
//	GameConnection::onDeath
//	GameConnection::onDrop
//	Observer::onTrigger
//	Player::activateStuff
//	Armor::onTrigger
//	Armor::onDisabled
//	serverCmdHat
//	serverCmdHair
//	serverCmdPlantBrick
//	serverCmdLight
//Created:
//	serverCmdBarber
//	startBarber
//	stopBarber
//	toggleBarberSelection
//	getBarberCenterprint
//	getHairName
//	Player::equipHair

package CPB_Support_Barber {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceClient, %damageType, %part) {
		if (isObject(%cl.player.chairBot)) {
			%cl.player.chairBot.delete();
		}

		return parent::onDeath(%cl, %sourceObj, %sourceClient, %damageType, %part);
	}

	function GameConnection::onDrop(%cl) {
		if (%cl.isCustomizing) {
			stopPlayerHairCustomization(%cl.player);
		}
		return parent::onDrop(%cl);
	}

	function Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();
		if (%cl.isCustomizing && getSimTime() - %cl.lastCustomizeTime > 1000) {
			eval("toggle" @ %cl.customizingMode @ "Selection(" @ %cl @ ");");
			return;
		}
		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function Player::activateStuff(%obj) {
		if (%obj.isCustomizing) {
			return;
		}

		return parent::activateStuff(%obj);
	}

	function Armor::onTrigger(%this, %obj, %trig, %state) {
		if (%obj.isCustomizing) {
			return;
		}

		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function Armor::onDisabled(%this, %obj, %enabled) {
		if (%obj.isCustomizing) {
			stopPlayerHairCustomization(%obj);
		}
		return parent::onDisabled(%this, %obj, %enabled);
	}

	////////////////////

	function serverCmdHat(%cl, %na, %nb, %nc, %nd, %ne) {
		if (!%cl.hatTest) {
			messageClient(%cl, '', "You need to go to the barber to change your hair!");
			return;
		}
		//messageClient(%cl, '', "You need to go to the janitor to change your bucket!");
		return parent::serverCmdHat(%cl, %na, %nb, %nc, %nd, %ne);
	}

	function serverCmdHair(%cl) {
		messageClient(%cl, '', "You need to go to the barber to change your hair!");
		return;
	}

	function serverCmdPlantBrick(%cl) {
		if (%cl.isCustomizing) {
			%currentHair = $HairData::currentHair[%cl.bl_id];
			%cl.centerPrint("<br><br><br><br><br>\c2 Hairdo saved!" @ getBarberCenterprint(%cl, %currentHair));
			$HairData::savedHair[%cl.bl_id] = $HairData::currentHair[%cl.bl_id];
			return;
		}
		return parent::serverCmdPlantBrick(%cl);
	}

	function serverCmdLight(%cl) {
		if (%cl.isCustomizing && isObject(%cl.player)) {
			stopPlayerHairCustomization(%cl.player);
			return;
		}
		return parent::serverCmdLight(%cl);
	}
};
activatePackage(CPB_Support_Barber);


////////////////////


function serverCmdBarber(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	} else if ($CPB::PHASE != $CPB::LOBBY) {
		return;
	} else if (vectorLen(%pl.getVelocity()) > 0.1) {
		messageClient(%cl, '', "You need to stop moving to use the barber!");
		return;
	}
	%pl.setVelocity("0 0 0");
	schedule(1, %pl, startBarber, %pl);
}

function startBarber(%cl, %brick) {
	if (!isObject(%pl = %cl.player)) {
		return;
	}
	
	%mount = new AIPlayer(Customization) {
		datablock = EmptyHoleBot;
	};
	MissionCleanup.add(%mount);

	%pl.chairBot = %mount;
	if (isObject(%brick) && %brick.getClassName() $= "fxDTSBrick") {
		%mount.setTransform(%brick.getTransform());
		%pl.setTransform(%pl.getPosition());
	} else {
		%mount.setTransform(%pl.getTransform());
	}

	%mount.mountObject(%pl, 0);
	%pl.canDismount = 0;

	%pl.isCustomizing = 1;
	%cl.isCustomizing = 1;
	%cl.customizingMode = "Barber";
	%cl.lastCustomizeTime = getSimTime();

	%cl.spectateObject(%pl, 0);
}

function stopBarber(%cl) {

}

function toggleBarberSelection(%cl) {

}


//////////////////// Support functions



function getBarberCenterprint(%cl, %index) {
	%list = $HairData::Unlocked[%cl.bl_id];
	%hairs = (%index + 1) @ " / " @ getWordCount(%list) + 0;
	%currHairName = getHairName(getWord(%list, %index));

	%final = "<br><font:Arial Bold:24>\c3Left Click       <font:Palatino Linotype:24>\c3[\c6" @ %hairs @ "\c3]<font:Arial Bold:24>\c3       Right Click <br><font:Palatino Linotype:24><just:center>\c6" @ %currHairName @ " ";
	return %final;
}

function getHairName(%id) {
	return strReplace($Hair[%index], "Hair", "");
}

function Player::equipHair(%pl, %hair) {
	if ((%hair $= "" || %hair $= "None") && %pl.getMountedImage(2) != 0) {
		%pl.unMountImage(2);
	} else {
		%pl.mountImage("Hat" @ stripchars(%hair, " -1234567890_+=.,;':?!@#$%&*()[]{}\"/<>") @ "Data", 2);

		%i = -1;
		while((%node = $hat[%i++]) !$= "")

		for (%i = 0; $hat[%i] !$= ""; %i++) {
			%pl.hideNode(%node);
		}

		%i = -1;
		for (%i = 0; $accent[%i] !$= ""; %i++) {
			%pl.hideNode(%node);
		}
	}
}

function getHairCamPosition(%pl, %obj) {
	if (isObject(%obj) && %obj.getClassName() $= "fxDTSBrick") {
		%id = %obj.getAngleID();
		switch (%id) {
			case 0: %vec = "0 2 2.2";
			case 1: %vec = "-2 0 2.2";
			case 2: %vec = "0 -2 2.2";
			case 3: %vec = "2 0 2.2";
		}
		%start = %obj.getPosition();
		%end = vectorAdd(%vec, %start);
		
		%pos = vectorAdd(%obj.getPosition(), %vec);
	} else if (!isObject(%obj)) {
		%pos = vectorAdd(%pl.getEyeTransform(), vectorAdd(vectorScale(%pl.getForwardVector(), 1.2), "0 0 0.5"));
	} else {
		messageAdmins("!!! \c7Invalid object specified in getHairCamPosition");
		return;
	}
	%delta = vectorSub(getWords(vectorAdd(%pl.getEyeTransform(), "0 0 -0.4"), 0, 2), %pos);
	%deltaX = getWord(%delta, 0);
	%deltaY = getWord(%delta, 1);
	%deltaZ = getWord(%delta, 2);
	%deltaXYHyp = vectorLen(%deltaX SPC %deltaY SPC 0);

	%rotZ = mAtan(%deltaX, %deltaY) * -1; 
	%rotX = mAtan(%deltaZ, %deltaXYHyp);

	%aa = eulerRadToMatrix(%rotX SPC 0 SPC %rotZ); //this function should be called eulerToAngleAxis...
	return %pos SPC %aa;
}