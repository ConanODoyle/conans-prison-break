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
$Hair[17]	= "Flat-TopHair";
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
//	customizationMount
//	isCustomizing
//	lastCustomizeTime
//Player
//	isCustomizing
//	canDismount

//Functions:
//Packaged:
//	GameConnection::onDeath
//Created:
//	serverCmdBarber
//	startPlayerHairCustomization

package CPB_Support_Barber {
	function GameConnection::onDeath(%cl, %proj, %killer, %damageType, %part) {
		if (isObject(%cl.player.))
	}
};
activatePackage(CPB_Support_Barber);

function serverCmdBarber(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	} else if ($Server::PrisonEscape::roundPhase > 0 && getRegion(%pl) !$= "Infirmary") {
		return;
	} else if (vectorLen(%pl.getVelocity()) > 0.1) {
		messageClient(%cl, '', "You need to stop moving to use the barber!");
		return;
	}
	%pl.setVelocity("0 0 0");
	schedule(1, %pl, startPlayerHairCustomization, %pl);
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