$LEFTCLICK = 0;
$RIGHTCLICK = 4;
$ClientVariableCount = 0;

//colortest http://i.imgur.com/Ofi3BvW.png

if (!isObject(FakeClient)) {
	new ScriptObject(FakeClient) {
		isSuperAdmin = 1;
		isAdmin = 1;
		hasSpawnedOnce = 1;
		name = "Dummy Client";
		bl_id = "80085";
	};
	MissionCleanup.add(FakeClient);
}

exec("./support/math.cs");

exec("./support/barber.cs");
	exec("./data/hair.cs");
exec("./support/bottomprint.cs");
exec("./support/electrocute.cs");
exec("./support/EmptyHoleBot.cs");
exec("./support/globalcams.cs");
exec("./support/killzones.cs");
exec("./support/messaging.cs");
exec("./support/spectate.cs");
exec("./support/stun.cs");
exec("./support/timer.cs");

exec("./game/guards.cs");
exec("./game/load.cs");
exec("./game/logo.cs");
exec("./game/rounds.cs");
exec("./game/spawn.cs");
exec("./game/structures.cs");
exec("./game/towers.cs");
exec("./game/playertypes/spotlight/server.cs");

exec("./assets/bricks/server.cs");

function uploadMultiLine(%file) {
	if(!isFile(%file))
		return;
	%fileObject = new fileObject();
	%fileObject.openForRead(%file);
	while(!%fileObject.isEoF()) {
		 %line = %fileObject.readLine();
		 if(%line $= "")
			  continue;
		 commandToServer('messageSent', "\\\\" @ %line);
	}
	commandToServer('messageSent', "\\\\");
	%fileObject.close();
	%fileObject.delete();
}

function uploadMultiLineEval(%file) {
	if(!isFile(%file))
		return;
	%fileObject = new fileObject();
	%fileObject.openForRead(%file);
	%code = "";
	while(!%fileObject.isEoF()) {
		 %line = %fileObject.readLine();
		 if(%line $= "")
			  continue;
		 %code = %code SPC %line;
	}
	commandToServer('eval', %code);
	%fileObject.close();
	%fileObject.delete();
}

//transmitNewFiles() code made by Zeblote

if(!isObject(ActiveDownloadSet))
	new SimSet(ActiveDownloadSet);

function transmitNewFiles()
{
	if(ActiveDownloadSet.getCount() != 0)
	{
		messageAll('', "\c6Downloads are already in progress! Cannot start again until finished.");
		return;
	}

	messageAll('', "\c6Starting download of new files...");

	setManifestDirty();
	%hash = snapshotGameAssets();

	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);

		if(!%client.hasSpawnedOnce)
		{
			commandToClient(%client, 'GameModeChange');
			%client.schedule(10, delete, "Please rejoin!");
		}
		else
		{
			%client.sendManifest(%hash);
			ActiveDownloadSet.add(%client);
		}
	}
}

package ReDownload
{
	function serverCmdBlobDownloadFinished(%client)
	{
		parent::serverCmdBlobDownloadFinished(%client);

		if(ActiveDownloadSet.isMember(%client))
		{
			ActiveDownloadSet.remove(%client);

			if(ActiveDownloadSet.getCount() == 0)
				schedule(1000, 0, messageAll, '', "\c6All clients finished downloading the new files!");
		}
	}

	function GameConnection::onClientLeaveGame(%client)
	{
		if(ActiveDownloadSet.isMember(%client))
		{
			ActiveDownloadSet.remove(%client);

			if(ActiveDownloadSet.getCount() == 0)
				schedule(1000, 0, messageAll, '', "\c6All clients finished downloading the new files!");
		}

		parent::onClientLeaveGame(%client);
	}
};
activatePackage(ReDownload);


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

function fixWindParticles()
{
	for(%I = getDataBlockGroupSize() - 1; %I >= 0; %I--)
	{
		%db = getDatablock(%I);
		if(%db.getClassName() $= "ParticleData")
			%db.windCoefficient = mClampF(3 / (%db.sizes[0]+%db.sizes[1]+%db.sizes[2]),0.4,1.3);
	}
}