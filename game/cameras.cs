//Functions:
//Created:
//	fxDTSBrick::doorToggleLoop


function fxDTSBrick::doorToggleLoop(%this, %time) {
   if (isEventPending(%this.doorToggleLoop)) {
      cancel(%this.doorToggleLoop);
   }
   if (%time == 0) {
      return;
   }

   if (%this.defaultState $= "") {
      if (%this.getDatablock().isOpen) {
         if (%this.getDatablock().getName() $= %this.getDatablock().openCCW) {
            %this.defaultState = 3;
         } else {
            %this.defaultState = 2;
         }
      } else {
         %this.defaultState = 4;
      }
   }

   if (%this.getDatablock().isOpen) {
      %this.door(4);
   } else {
      %name = strlwr(%this.getName());
      if (strPos(%name, "left") >= 0 || (!%this.nextTurnRight && strPos(%name, "right") < 0 && strPos(%name, "left") < 0)) {
         %this.door(2);
         %this.nextTurnRight = 1;
      } else {//if (strPos(%name, "right") < 0 || !%this.nextTurnRight) {
         %this.door(3);
         %this.nextTurnRight = 0;
      }
   }
   %this.doorToggleLoop = %this.schedule(%time / 4, doorToggleLoop, %time);
}