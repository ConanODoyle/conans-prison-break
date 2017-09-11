$CPB::Classes::Stun = 1;			$CPB::Classes[1] = "Stun";
$CPB::Classes::Shrapnel = 2;		$CPB::Classes[2] = "Shrapnel";
$CPB::Classes::LMG = 3;				$CPB::Classes[3] = "LMG";
$CPB::Classes::TearGas = 4;			$CPB::Classes[4] = "Tear Gas";
$CPB::Classes::List = "Stun Shrapnel LMG TearGas";

//Object parameters:
//Client
//	guardClass

//Functions:
//Created:
//	GameConnection::setClass
//	giveGuardItems


registerOutputEvent(GameConnection, "setGuardClass", "string 200 156", 1);

function GameConnection::setGuardClass(%cl, %name) {
	if (containsWord($CPB::Classes::List, %name)) {
		%cl.guardClass = $CPB::Classes["::" @ %name];
	}

	if (isObject(%cl.pickedTowerBrick)) {
		%cl.pickedTowerBrick.item.setShapeName(%cl.name @ " - " @ $CPB::Classes[%cl.guardClass]);
	}

	if (isObject(%cl.tower)) {
		%cl.tower.guardOption = %cl.guardClass;
	}
}

function giveGuardItems(%pl, %item) {
	%cl = %pl.client;
	if (!%cl.isGuard) {
		return;
	} else if (!isObject(%cl.tower) && %cl.tower.guardOption <= 0) {
		messageAdmins("!!! \c6Invalid guard class to give items!");
		return;
	}

	%pl.addItem(SniperRifleSpotlightItem);
	//%pl.addItem(WhistleItem);
	//%pl.addItem(SteakItem);

	switch (%cl.tower.guardOption) {
		case $CPB::Classes::LMG: %pl.addItem(LightMachineGunItem);
		case $CPB::Classes::TearGas: %pl.addItem(TearGasGrenadeItem);
	}
}
