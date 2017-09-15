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

datablock ItemData(StunItem : HammerItem) {
	shapeFile = "./shapes/stunVisual/stunVisual.dts";

	isVisual = 1;
	doColorShift = false;
	uiName = "Stun Visual";
};

datablock ItemData(ShrapnelItem : HammerItem) {
	shapeFile = "./shapes/shrapnelVisual/shrapnelVisual.dts";

	isVisual = 1;
	doColorShift = false;
	uiName = "Shrapnel Visual";
};

datablock ItemData(TearGasItem : HammerItem) {
	shapeFile = "./shapes/TearGasVisual/TearGasVisual.dts";

	isVisual = 1;
	doColorShift = false;
	uiName = "TearGas Visual";
};

datablock ItemData(LMGItem : HammerItem) {
	shapeFile = "./shapes/LMGVisual/LMGVisual.dts";

	isVisual = 1;
	doColorShift = false;
	uiName = "LMG Visual";
};

//Object parameters:
//Client
//	guardClass

//Functions:
//Packaged:
//	ItemData::onAdd
//	Armor::onCollision
//Created:
//	GameConnection::setGuardClass
//	GameConnection::setGuardEquipment
//	giveGuardItems
//	colorVisualItem


package CPB_Game_Classes {
	function ItemData::onAdd(%this, %obj) {
		%ret = parent::onAdd(%this, %obj);
		if (%this.isVisual) {
			schedule(33, %obj, colorVisualItem, %obj);
		}
		return %ret;
	}

	function Armor::onCollision(%this, %obj, %col, %pos) {
		if (%col.getDatablock().isVisual) {
			return;
		}
		return parent::onCollision(%this, %obj, %col, %pos);
	}
};
activatePackage(CPB_Game_Classes);


////////////////////


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

function colorVisualItem(%item) {
	echo(%name = %item.getDatablock().getName());
	%alpha = 1;
	%item.startFade(0, 0, 1);
	%skinColor = "0.9 0.712 0.456 " @ %alpha;
	if (%name $= "StunItem") {
		%item.setNodeColor("pHeadSkin", %skinColor);
		%item.setNodeColor("pLHand", %skinColor);
		%item.setNodeColor("pRHand", %skinColor);
		%item.setNodeColor("pPants", $PrisonerColor);
		%item.setNodeColor("pChest", $PrisonerColor);
		%item.setNodeColor("pChestStripes", "0 0 0 " @ %alpha);
		%item.setNodeColor("pLArmSlim", $PrisonerColor);
		%item.setNodeColor("pRArmSlim", $PrisonerColor);
		%item.setNodeColor("Icosphere", "1 1 1 1");
		%item.setNodeColor("sniperBarrel", "0.4 0.4 0.4 1");
	} else if (%name $= "ShrapnelItemB") {
		%item.setNodeColor("Icosphere", "1 1 1 1");
		%item.setNodeColor("tracers", "0.5 0.5 0.5 0.5");
		%item.setNodeColor("bullets", "1 1 0 1");
	} else if (%name $= "TearGasItem") {
		%item.setNodeColor("Icosphere", "1 1 1 1");
		%item.setNodeColor("grey50", "0.15 0.15 0.15 1");
		%item.setNodeColor("grey80", "0.07 0.07 0.07 1");
		%item.setNodeColor("grey95", "0.01 0.01 0.01 1");
		%item.setNodeColor("white", "1 1 1 1");
	} else if (%name $= "LMGItem") {
		%item.setNodeColor("yellow", "1 1 0 1");
		%item.setNodeColor("gray15", "0.15 0.15 0.15 1");
		%item.setNodeColor("gray25", "0.25 0.25 0.25 1");
		%item.setNodeColor("gray50", "0.50 0.50 0.50 1");
		%item.setNodeColor("gray75", "0.75 0.75 0.75 1");
		%item.setNodeColor("greenMagazine", "0.1 0.5 0.1 1");
	}
}