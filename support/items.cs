$OneUseItemCount = 3;
$OneUseItem[0] = KeyYellowItem;
$OneUseItem[1] = SmokeGrenadeItem;
$OneUseItem[2] = FireAxeItem;

if (!isObject(DroppedItems)) {
	new SimSet(DroppedItems) {};
}

package CPB_Support_Items {
  function collectAllPrisonBricks(%bg, %i) {
    %b = %bg.getObject(%i);
    if (isObject(%b.oneUseItem)) {
      %b.setItem(%b.oneUseItem.getID());
    }
    parent::collectAllPrisonBricks(%bg, %i);
  }
  
  function Armor::onCollision(%this, %obj, %col, %pos) { //check parameters
    if (isOneUseItem(%col).getName() $= $OneUseItem[%i]) {
      %ret = parent::onCollision(%this, %obj, %col, %pos);
      if (isEventPending(%col.respawnSchedule)) {
        return %ret;
      }
      %col.spawnBrick.oneUseItem = %col.getDatablock().getName();
      %col.delete();
      return %ret;
    }
    return parent::onCollision(%this, %obj, %col, %pos);
  }
  
  function Armor::onDisabled(%this, %obj, %col, %pos) {
    for (%i = 0; %i < %this.maxTools; %i++) {
      if (isOneUseItem(%obj.tool[%i])) {
        %it = new Item() {
          datablock = %obj.tool[%i];
          position = %obj.getPosition();
          client = %obj.client;
        };
        %it.setVelocity(vectorAdd("0 0 10", %obj.getVelocity()));
        DroppedItems.add(%it);
      }
    }
    return parent::onDisabled(%this, %obj, %col, %pos);
  }
};
activatePackage(CPB_Support_Items);

function isOneUseItem(%col) {
  if (!isObject(%col) {
    return 0
  }
  for (%i = 0; %i < $OneUseItemCount; %i++) {
    if (%col.getName() $= $OneUseItem[%i]) {
      return 1;
    }
  }
  return 0;
}

function clearAllDroppedItems() {
  for (%i = DroppedItems.getCount() - 1; %i >= 0; %i--) {
    DroppedItems.getObject(%i).delete();
  }
}