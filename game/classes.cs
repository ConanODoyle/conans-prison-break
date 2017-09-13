$CPB::Classes = "NONE";				$CPB::Classes0 = "NONE";
$CPB::Classes::Stun = 1;			$CPB::Classes[1] = "Stun";
$CPB::Classes::Shrapnel = 2;		$CPB::Classes[2] = "Shrapnel";
$CPB::Classes::LMG = 3;				$CPB::Classes[3] = "LMG";
$CPB::Classes::TearGas = 4;			$CPB::Classes[4] = "Tear Gas";
$CPB::Classes::List = "Stun Shrapnel LMG TearGas";

$CPB::Equipment = "NONE";			$CPB::Equipment0 = "NONE";
$CPB::Equipment::Shotgun = 1;		$CPB::Equipment[1] = "Shotgun";
$CPB::Equipment::Flash = 2;			$CPB::Equipment[2] = "Flash";
$CPB::Equipment::Heli = 3;			$CPB::Equipment[3] = "Heli";
$CPB::Equipment::List = "Shotgun Flash Heli";

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
		%cl.pickedTowerBrick.item.setShapeName(%cl.name @ " - " @ $CPB::Classes[%cl.guardClass] @ ", " @ $CPB::Equipment[%cl.guardEquipment]);
	}

	if (isObject(%cl.tower)) {
		%cl.tower.guardOption = %cl.guardClass;
	}
}

function GameConnection::setGuardEquipment(%cl, %name) {
	if (containsWord($CPB::Equipment::List, %name)) {
		%cl.guardEquipment = $CPB::Equipment["::" @ %name];
	}

	if (isObject(%cl.pickedTowerBrick)) {
		%cl.pickedTowerBrick.item.setShapeName(%cl.name @ " - " @ $CPB::Classes[%cl.guardClass] @ ", " @ $CPB::Equipment[%cl.guardEquipment]);
	}

	if (isObject(%cl.tower)) {
		%cl.tower.guardEquipment = %cl.guardEquipment;
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

	switch (%cl.tower.guardEquipment) {
		case $CPB::Equipment::Shotgun: %pl.addItem(PumpShotgunItem);
		case $CPB::Classes::Flash: %pl.addItem(tierFragGrenadeItem); %pl.addItem(tierFragGrenadeItem); %pl.addItem(tierFragGrenadeItem);
	}
}