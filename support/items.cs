$OneUseItemCount = 3;
$OneUseItem[0] = yellowKeyItem;
$OneUseItem[1] = riotSmokeGrenadeItem;
$OneUseItem[2] = FireAxeItem;

$CPB::OneUseItemPopTime = 60 * 2;

if (!isObject(DroppedItems)) {
	new SimSet(DroppedItems) {};
}

//Functions:
//Packaged:
//	collectAllPrisonBricks
//	Armor::onCollision
//	fxDTSBrick::onKeyMismatch
//	Armor::onDisabled
//	Item::schedulePop
//	Player::changeDatablock
//	serverCmdDropTool
//Created:
//	Player::removeItem
//	Player::addItem
//	isOneUseItem
//	clearAllDroppedItems
//	startTimedPop


package CPB_Support_Items {
	function collectAllPrisonBricks(%bg, %i) {
		%ret = parent::collectAllPrisonBricks(%bg, %i);
		if (!%ret) {
			return %ret;
		}
		
		%b = %bg.getObject(%i);
		if (isObject(%b.oneUseItem)) {
			%b.setItem(%b.oneUseItem.getID());
		}
	}
	
	function Armor::onCollision(%this, %obj, %col, %pos) {
		if (%col.getClassName() $= "Item" && (%obj.getDatablock().cannotPickupItems || %obj.cannotPickupItems)) {
			return;
		} else if (%col.getClassName() $= "Item" && isOneUseItem(%col.getDatablock()) && %obj.getState() !$= "Dead") {
			for (%i = 0; %i < %this.maxTools; %i++) {
				if (%obj.tool[%i] == %col.getDatablock().getID()){
					return;
				}
			}
			messageAll('', "\c3" @ %obj.client.name @ "\c6 picked up a \c7" @ %col.getDatablock().uiName);
			%obj.specialItemString = trim(%obj.specialItemString TAB %col.getDatablock().uiName);

			%name = %obj.client.name SPC "(" @ strReplace(%obj.specialItemString, "\t", ", ") @ ")";
			%obj.setShapeName(%name, 8564862);

			%ret = parent::onCollision(%this, %obj, %col, %pos);
			if (!isObject(%col)) {
				return %ret;
			}
			
			%col.spawnBrick.oneUseItem = %col.getDatablock().getName();
			%col.delete();
			return %ret;
		}
		return parent::onCollision(%this, %obj, %col, %pos);
	}

	function fxDTSBrick::onKeyMismatch(%this, %pl) {
		%pl.removeItem(%pl.currTool);
		return parent::onKeyMismatch(%this, %pl);
	}
	
	function Armor::onDisabled(%this, %obj, %col, %pos) {
		for (%i = 0; %i < %this.maxTools; %i++) {
			if (isOneUseItem(%obj.tool[%i])) {
				%it = new Item() {
					datablock = %obj.tool[%i];
					position = %obj.getPosition();
					client = %obj.client;
					minigame = %obj.client.minigame;
				};
				%it.setVelocity(vectorAdd("0 0 10", %obj.getVelocity()));
				DroppedItems.add(%it);
				startTimedPop(%it, $CPB::OneUseItemPopTime);
				messageAll('', "\c3" @ %obj.client.name @ "\c6 dropped a \c7" @ %obj.tool[%i].uiName);
			}
		}
		return parent::onDisabled(%this, %obj, %col, %pos);
	}	

	function Item::schedulePop(%obj) {
		if (isOneUseItem(%obj.getDatablock())) {
			DroppedItems.add(%obj);

			startTimedPop(%obj, $CPB::OneUseItemPopTime);
			return;
		}
		return parent::schedulePop(%obj);
	}

	function Player::changeDatablock(%pl, %db) {
		if (%db.getID() == BuffArmor.getID()) {
			%pl.addItem(BuffBashItem);
			%pl.unMountImage(0);
			%pl.unMountImage(1);
		}
		return parent::changeDatablock(%pl, %db);
	}

	function serverCmdDropTool(%cl, %slot) {
		%pl = %cl.player;
		if (isOneUseItem(%pl.tool[%slot])) {
			messageAll('', "\c3" @ %cl.name @ "\c6 dropped a \c7" @ %pl.tool[%slot].uiName);
			%pl.specialItemString = trim(strReplace(%pl.specialItemString, %pl.tool[%slot].uiName, ""));

			if (%pl.specialItemString !$= "") {
				%name = %cl.name SPC "(" @ strReplace(%pl.specialItemString, "\t", ", ") @ ")";
			} else {
				%name = %cl.name;
			}

			%pl.setShapeName(%name, 8564862);
		}

		parent::serverCmdDropTool(%cl, %slot);
	}


	////////////////////


	function riotSmokeGrenadeImage::onFire(%this, %obj, %slot) {
		%obj.specialItemString = trim(strReplace(%obj.specialItemString, %obj.tool[%obj.currTool].uiName, ""));
	}

	function riotSmokeGrenadeGoldenImage::onFire(%this, %obj, %slot) {

	}
};
activatePackage(CPB_Support_Items);

function Player::removeItem(%pl, %i) {
	%cl = %pl.client;
	%pl.tool[%i] = 0;
	if (isObject(%cl)) {
		messageClient(%cl, 'MsgItemPickup', "", %i, 0, 1);
		serverCmdUnuseTool(%cl);
	}
}

function Player::addItem(%this, %item) {
	%item = %item.getID();
	%cl = %this.client;
	for(%i = 0; %i < %this.getDatablock().maxTools; %i++) {
		%tool = %this.tool[%i];
		if (%tool == 0) {
			%this.tool[%i] = %item.getID();
			%this.weaponCount++;
			messageClient(%cl, 'MsgItemPickup', '', %i, %item.getID());
			break;
		}
	}
}

function isOneUseItem (%db) {
	if (!isObject(%db)) {
		return 0;
	}

	for (%i = 0; %i < $OneUseItemCount; %i++) {
		if (%db.getName() $= $OneUseItem[%i]) {
			return 1;
		}
	}
	return 0;
}

function clearAllDroppedItems() {
	DroppedItems.deleteAll();
}

function startTimedPop(%obj, %time) {
	cancel(%obj.timedPopLoop);
	
	if (%time < 0) {
		%obj.delete();
		return;
	}

	%obj.setShapeNameDistance(500);
	%obj.setShapeName(%obj.getDatablock().uiName @ " - " @ %time);

	%obj.timedPopLoop = schedule(1000, %obj, startTimedPop, %obj, %time - 1);
}