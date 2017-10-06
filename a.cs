
function Player::setLargeScale(%this,%iter)
{
	%iter = mClamp(%iter,0,13);
	if(%iter <= 0)
	{
		switch(restWords(%this.setLargeScale))
		{
			case 13: %this.scale = "100.9 100.9 100.9";
			case 12: %this.scale = "62.4 62.4 62.4";
			case 11: %this.scale = "38.6 38.6 38.6";
			case 10: %this.scale = "23.9 23.9 23.9";
			case 9: %this.scale = "14.8 14.8 14.8";
			case 8: %this.scale = "9.2 9.2 9.2";
			case 7: %this.scale = "5.7 5.7 5.7";
			case 6: %this.scale = "3.6 3.6 3.6";
			case 5: %this.scale = "2.2 2.2 2.2";
			case 4: %this.scale = "1.5 1.5 1.5";
			case 3 or 2: %this.scale = "0.8 0.8 0.8";
			case 1: %this.scale = "0.1 0.1 0.1";
		}
		%this.setLargeScale = 0;
		if(isObject(%this.aiplayerscale))
			%this.aiplayerscale.delete();
		if(isObject(%this.client))
			%this.client.setControlObject(%this);
		return;
	}
	if(!firstword(%this.setLargeScale))
	{
		if(!isObject(%this.getObjectMount()))
		{
			%this.aiplayerscale = new AIPlayer() { dataBlock = PlayerStandardArmor; position = %this.getPosition(); };
			%this.aiplayerscale.mountObject(%this,8);
		}
		if(isObject(%client = %this.client) || isObject(%client = %this.getControllingClient()))
		{
			%client.setControlObject(%client.Camera);
			%client.camera.setOrbitMode(%this, %this.getTransform(), 0, 8*1.6*%iter, 8*1.6*%iter);
		}
		%this.setLargeScale = 1 SPC %iter;
		%this.setLargeScaleSchedule = %this.schedule(1500,setLargeScale,%iter);
	} else {
		%this.setPlayerScale(0);
		%this.setLargeScaleSchedule = %this.schedule(100,setLargeScale,%iter--);
	}
}
