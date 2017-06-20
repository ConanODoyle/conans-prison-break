$HairCount = 46;
$Hair[0]	= "None";
$Hair[1]	= "Comb-Over Hair";
$Hair[2]	= "Corn Rows Hair";
$Hair[3]	= "Suav Hair";
$Hair[4]	= "Mohawk Hair";
$Hair[5]	= "Neat Hair";
$Hair[6]	= "Judge Hair";
$Hair[7]	= "Wig";
$Hair[8]	= "BobCut Hair";
$Hair[9]	= "Goatee Hair";
$Hair[10]	= "Old Woman Hair";
$Hair[11]	= "Emo Hair";
$Hair[12]	= "Broad Hair";
$Hair[13]	= "Punk Hair";
$Hair[14]	= "Afro Hair";
$Hair[15]	= "Phoenix Hair";
$Hair[16]	= "Daphne Hair";
$Hair[17]	= "Flat Top Hair";
$Hair[18]	= "Mom Hair";
$Hair[19]	= "Thinning Hair";
$Hair[20]	= "Balding Hair";
$Hair[21]	= "Pompadour Hair";
$Hair[22]	= "Familiar Hair";
$Hair[23]	= "Mullet Hair";
$Hair[24]	= "Slick Hair";
$Hair[25]	= "Turban";
$Hair[26]	= "Ponytail Hair";
$Hair[27]	= "Bowl Hair";
$Hair[28]	= "Bunn Hair";
$Hair[29]	= "Shaggy Hair";
$Hair[30]	= "Saiyan Hair";
$Hair[31]	= "Fabio Hair";
$Hair[32]	= "Freddie Hair";
$Hair[33]	= "Girls Hair";
$Hair[34]	= "Greaser Hair";
$Hair[35]	= "Messy Hair";
$Hair[36]	= "Parted Hair";
$Hair[37]	= "Wavey Hair";
$Hair[38]	= "Layered Hair";
$Hair[39]	= "Macho Hair";
$Hair[40]	= "Long Hair";
$Hair[41]	= "JewFro Hair";
$Hair[42]	= "Fat Hair";
$Hair[43]	= "Dastardly Hair";
$Hair[44]	= "Clown Hair";
$Hair[45]	= "Important Hair";

if(!isObject(CPB_HairSet)) {
	new SimSet(CPB_HairSet);
	missionCleanup.add(CPB_HairSet);
}

//Object properties:
//Client
//	hairTest
//Player
//	customizingMode
//	lastCustomizeTime
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
//	serverCmdGrantHair
//	getBarberCenterprint	//support
//	getHairCamPosition
//	getHairName
//	Player::equipHair
//	GameConnection::hasHair
//	GameConnection::grantFreeHair
//	GameConnection::giveHair
//	GameConnection::giveRandomHair
//	isHair	//datablock definitions
//	registerHair
//	Hatmod_getProperties
//	strLastPos
//	registerAllHairs


package CPB_Support_Barber {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceClient, %damageType, %part) {
		if (isObject(%cl.player.chairBot)) {
			%cl.player.chairBot.delete();
		}

		return parent::onDeath(%cl, %sourceObj, %sourceClient, %damageType, %part);
	}

	function GameConnection::onDrop(%cl) {
		if (%cl.player.isCustomizing) {
			stopBarber(%cl);
		}
		return parent::onDrop(%cl);
	}

	function Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();
		if (%cl.player.isCustomizing && getSimTime() - %cl.player.lastCustomizeTime > 500 && %state) {
			eval("toggle" @ %cl.player.customizingMode @ "Selection(" @ %cl @ ", " @ %trig @ ");");
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
			stopBarber(%obj.client);
		}
		return parent::onDisabled(%this, %obj, %enabled);
	}

	////////////////////

	function serverCmdHat(%cl, %na, %nb, %nc, %nd, %ne) {
		if (!%cl.hairTest) {
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
		if (%cl.player.isCustomizing) {
			%currentHair = $HairData::currentHair[%cl.bl_id];
			%cl.centerPrint("<br><br><br><br><br>\c2 Hairdo saved!" @ getBarberCenterprint(%cl, %currentHair));
			$HairData::savedHair[%cl.bl_id] = $HairData::currentHair[%cl.bl_id];
			return;
		}
		return parent::serverCmdPlantBrick(%cl);
	}

	function serverCmdLight(%cl) {
		if (%cl.player.isCustomizing && isObject(%cl.player)) {
			stopBarber(%cl);
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
	schedule(1, %pl, startBarber, %cl);
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
	%mount.setMaxSideSpeed(0);
	%mount.setMaxForwardSpeed(0);
	%mount.setMaxBackwardSpeed(0);
	%mount.setMaxCrouchSideSpeed(0);
	%mount.setMaxCrouchForwardSpeed(0);
	%mount.setMaxCrouchBackwardSpeed(0);
	%pl.canDismount = 0;

	%pl.isCustomizing = 1;
	%pl.customizingMode = "Barber";
	%pl.lastCustomizeTime = getSimTime();

	%cl.spectateObject(%pl, 0);
	%cl.camera.setMode(Observer);
	%cl.camera.setControlObject(%pl);
	%pl.setControlObject(%pl);
	%cl.camera.setTransform(getHairCamPosition(%pl));

	if ($HairData::Unlocked[%cl.bl_id] $= "") {
		%cl.grantFreeHair();
	}

	%index = $HairData::currentHair[%cl.bl_id] = $HairData::savedHair[%cl.bl_id] + 0;
	%pl.equipHair("Saved");
	centerPrint(%cl, "<br><br><br><br><br>\c6Plant Brick: Confirm \c7||\c6 Light: Exit" @ getBarberCenterprint(%cl, %index));

	serverCmdUnUseTool(%cl);
	%pl.playThread(0, sit);
}

function stopBarber(%cl) {
	if (!isObject(%pl = %cl.player) || !%pl.isCustomizing) {
		return;
	}

	%pl.canDismount = 1;
	%pl.dismount();
	if (!isObject(%pl.customizingBrick)) {
		%pl.schedule(10, setTransform, %pl.chairBot.getPosition() SPC rotFromTransform(%pl.getTransform()));
	}

	%pl.isCustomizing = 0;
	%pl.chairBot.delete();

	%cl.stopSpectatingObject();
	%cl.camera.setControlObject(0);
	%cl.setControlObject(%pl);

	%currentHairID = getWord($HairData::Unlocked[%cl.bl_id], $HairData::currentHair[%cl.bl_id]);
	%pl.equipHair("Saved");

	export("$HairData::*", "Add-ons/Gamemode_CPB/data/hair.cs");
	%cl.centerPrint("");
}

function toggleBarberSelection(%cl, %trig) {
	%list = $HairData::Unlocked[%cl.bl_id];
	%count = getWordCount(%list);

	if (isObject(%pl = %cl.player)) {
		if (%trig == $LEFTCLICK) {
			$HairData::currentHair[%cl.bl_id] += %count - 1;
			$HairData::currentHair[%cl.bl_id] %= %count;
		} else if (%trig == $RIGHTCLICK) {
			$HairData::currentHair[%cl.bl_id] += 1;
			$HairData::currentHair[%cl.bl_id] %= %count;
		}

		%currentHair = $HairData::currentHair[%cl.bl_id];
		%currentHairID = getWord($HairData::Unlocked[%cl.bl_id], %currentHair);

		if (%currentHair >= %count) {
			messageAdmins("!!! - \c3" @ %cl.name @ "\c6's hair index is invalid!");
			return;
		}

		%print = "<br><br><br><br><br>\c6Plant Brick: Confirm \c7||\c6 Light: Exit" @ getBarberCenterprint(%cl, %currentHair);
		centerPrint(%cl, %print);
		%pl.equipHair(getHairName(%currentHairID));
	}
}

function serverCmdGrantHair(%cl, %target, %hair) {
	if (!%cl.isAdmin) {
		return;
	}
	if (!isInteger(%hair) || %hair >= $HairCount || %hair <= 0) {
		messageClient(%cl, '', "!!! Please put in a valid hair ID");
		return;
	}

	%t = fcn(%target);

	if (!isObject(%t)) {
		messageClient(%cl, '', "!!! Cannot find client " @ %target @ "!");
		return;
	}
	if ($HairData::Unlocked[%t.bl_id] $= "") {
		%t.grantFreeHair();
	}	
	if (hasHair(%t, %hair)) {
		messageClient(%cl, '', "!!! \c3" @ %t.name @ "\c6 already has the " @ $Hair[%hair] @ "!");
		return;
	}

	%t.giveHair(%hair);

	messageClient(%cl, '', "\c6You have given \c3" @ %t.name @ "\c6 the \c3" @ $Hair[%hair] @ "\c6.");
	messageClient(%t, '', "\c6You have been given the \c3" @ $Hair[%hair]@ "\c6 by \c3" @ %cl.name);

	export("$HairData::*", "Add-ons/Gamemode_CPB/data/hair.cs");
}


//////////////////// Support functions


function getBarberCenterprint(%cl, %unlockedIndex) {
	%list = $HairData::Unlocked[%cl.bl_id];
	%hairs = (%unlockedIndex + 1) @ " / " @ getWordCount(%list) + 0;
	%currHairName = getHairName(getWord(%list, %unlockedIndex));

	%final = "<br><font:Arial Bold:24>\c3Left Click       <font:Palatino Linotype:24>\c3[\c6" @ %hairs @ "\c3]<font:Arial Bold:24>\c3       Right Click <br><font:Palatino Linotype:24><just:center>\c6" @ %currHairName @ " ";
	return %final;
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

function getHairName(%id) {
	return $Hair[%id];
}

function Player::equipHair(%pl, %hair) {
	%cl = %pl.client;
	if ((%hair $= "" || %hair $= "None") && %pl.getMountedImage(2) != 0) {
		%pl.unMountImage(2);
	} else if (%hair $= "Saved") {
		%list = $HairData::Unlocked[%cl.bl_id];
		%currHairName = getHairName(getWord(%list, $HairData::savedHair[%cl.bl_id]));
		%pl.equipHair(%currHairName);
	} else {
		%pl.mountImage("Hat" @ stripchars(%hair, " -1234567890_+=.,;':?!@#$%&*()[]{}\"/<>") @ "Data", 2);

		for (%i = 0; $hat[%i] !$= ""; %i++) {
			%pl.hideNode($hat[%i]);
		}

		for (%i = 0; $accent[%i] !$= ""; %i++) {
			%pl.hideNode($accent[%i]);
		}
	}
}

function GameConnection::hasHair(%cl, %hairID) {
	%list = $HairData::Unlocked[%cl.bl_id];
	for (%i = 0; %i < getWordCount(%list); %i++) {
		if (getWord(%list, %i) == %hairID) {
			return 1;
		}
	}
	return 0;
}

function GameConnection::grantFreeHair(%cl) {
	if ($HairData::Unlocked[%cl.bl_id] $= "") {
		$HairData::Unlocked[%cl.bl_id] = "0";
		messageClient(%cl, '', "\c2You got two free first hairdos!");
		%cl.giveRandomHair();
		%cl.giveRandomHair();
	}
}

function GameConnection::giveHair(%cl, %hairID) {
	if ($HairData::Unlocked[%cl.bl_id] $= "") {
		$HairData::Unlocked[%cl.bl_id] = "0";
	}
	$HairData::Unlocked[%cl.bl_id] = trim($HairData::Unlocked[%cl.bl_id] SPC %hairID);
}

function GameConnection::giveRandomHair(%cl) {
	%list = getIntegerList(1, $HairCount - 1);
	%count = getWordCount(%list);
	%ownedList = $HairData::Unlocked[%cl.bl_id];
	%ownedCount = getWordCount($HairData::Unlocked[%cl.bl_id]);

	if (%ownedCount == %count) {
		return;
	}

	while (getWordCount(%list) > 0 && %loopCount < 1000) {
		%idx = getRandom(0, %count - 1);
		%hairID = getWord(%list, %idx);

		if (!%cl.hasHair(%hairID)) {
			break;
		} else {
			%list = removeWord(%list, %idx);
			%count--;
		}
		%loopCount++;
	}

	if (%count > 0) {
		messageClient(%cl, '', "\c6You have been given the \c3" @ $Hair[%hairID] @ "\c6!");
		%cl.giveHair(%hairID);
	} else { //all hairs already owned
		messageClient(%cl, '', "\c6You won a hairdo, but you already have all the hairs!");
	}
	export("$HairData::*", "Add-ons/Gamemode_CPB/data/hair.cs");
}


//////////////////// Hatmod Functions


function isHair(%hat) {
	return isObject(strReplace("Hat" @ %hat @ "Data", " ", "_"));
}

function registerHair(%name, %dir, %offset, %eyeOffset) {
	if(isHair(%name)) {
		echo("Error: Hair already exists! (" @ strReplace(%name, "_", " ") @ ")");
		CPB_HairSet.add("Hat" @ %name @ "Data");
		return 0;
	}

	%evalString =	"datablock ShapeBaseImageData(Hat" @ %name @ "Data) {" SPC
						"shapeFile = \"" @ %dir @ "\";" SPC
						"mountPoint = $HeadSlot;" SPC
						"offset = \"" @ %offset @ "\";" SPC
						"eyeOffset = \"" @ %eyeOffset @ "\";" SPC
						"rotation = \"" @ eulerToMatrix("0 0 0") @ "\";" SPC
						"scale = \"0.1 0.1 0.1\";" SPC
						"doColorShift = false;" SPC
						"hatName = \"" @ %name @ "\";" SPC
					"};";

	eval(%evalString);
	CPB_HairSet.add("Hat" @ %name @ "Data");

	return 1;
}

function HatMod_GetProperties(%configDir) {
	%offset = "0 0 0";
	%minVer = "0";
	%eyeOffset = "0 0 -1000";

	if(%configDir !$= "Default") {
		%file = new fileObject();
		%file.openForRead(%configDir);

		while(!%file.isEOF()) {
			%line = trim(%file.readLine());

			if(%line $= "")
				continue;

			%firstWord = getWord(%line, 0);
			%wordCount = getWordCount(%line);

			switch$(%firstWord) {
			case "offset":
				%offset = getWords(%line, %wordCount-3, %wordCount-1);
				%offset = strReplace(%offset, "\"", "");
				%offset = strReplace(%offset, ";", "");
				%offset = vectorAdd(%offset, "0 0 0"); //Forces it to be a 3d vector

			case "minVer":
				%minVer = getWord(%line, getWordCount(%line)-1);

			case "eyeOffset":
				%eyeOffset = getWords(%line, %wordCount-3, %wordCount-1);
				%eyeOffset = strReplace(%eyeOffset, "\"", "");
				%eyeOffset = strReplace(%eyeOffset, ";", "");
				%eyeOffset = vectorAdd(%eyeOffset, "0 0 0"); //Forces it to be a 3d vector
			}
		}

		%file.close();
		%file.delete();
	}

	return %offset TAB %minVer TAB %eyeOffset;
}

function strLastPos(%str, %search, %offset) {
	if(%offset > 0) {
		%pos = %offset;
	} else {
		%str = getSubStr(%str, 0, strLen(%str) + %offset);
		%pos = 0;
	}

	%lastPos = -1;

	while((%pos = strPos(%str, %search, %pos+1)) > 0) {
		%lastPos = %pos;
		if(%break++ >= 500) { //Pretty sure strings have a max length shorter than this, should be fine
			return -2;
		}
	}
	return %lastPos;
}

function registerAllHairs() {
	echo("Searching for Hairs...");

	if(isObject(CPB_HairSet)) {
		CPB_HairSet.clear();
	}

	%loc = "Add-ons/Gamemode_CPB/support/hairs/*/*.dts";
	for(%dir = findFirstFile(%loc); %dir !$= ""; %dir = findNextFile(%loc)) {
		//echo("DIR:" SPC %dir);
		%pos1 = strLastPos(%dir, "/")-1;
		%pos2 = strLastPos(getSubStr(%dir, 0, %pos1), "/");
		%name = getSubStr(%dir, 1+%pos2, %pos1-%pos2);
		//echo("NAME:" SPC %name);

		//echo("   Found hat:" SPC strReplace(%name, "_", " ") SPC "at dir" SPC %dir);

		%configDir = getSubStr(%dir, 0, strLastPos(%dir, "/")+1) @ %name @ ".txt";
		if(isFile(%configDir)) {
			echo("   Reading file for calibration.");

			%config = HatMod_GetProperties(%configDir);
		} else
			%config = HatMod_GetProperties("Default");

		%offset = getField(%config, 0);
		%eyeOffset = getField(%config, 2);

		if(registerHair(%name, %dir, %offset, %eyeOffset)) {
			echo("   Hat" SPC strReplace(%name, "_", " ") SPC "registered successfully.");
			%count++;
		}
	}

	echo("Hat search done. Found" SPC %count + 0 SPC "new hats.");
}

if (!$hasRegisteredHairsOnce) {
	registerAllHairs();
	$hasRegisteredHairsOnce = 1;
}
