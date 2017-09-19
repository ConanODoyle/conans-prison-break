$LEFTCLICK = 0;
$RIGHTCLICK = 4;
$ClientVariableCount = 0; //use to add client vars for resetting
$CurrMap = "SkillPrison";

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

exec("./support/funclib.cs");
exec("./game/spawn.cs"); //needs lowest package priority

exec("./support/barber.cs");
exec("./support/bottomprint.cs");
exec("./support/EmptyHoleBot.cs");
exec("./support/globalcams.cs");
exec("./support/items.cs");
exec("./support/killzones.cs");
exec("./support/locations.cs");
exec("./support/messaging.cs");
exec("./support/spectate.cs");
exec("./support/stun.cs");
exec("./support/timer.cs");

exec("./game/breakables.cs");
exec("./game/cameras.cs");
exec("./game/classes.cs");
exec("./game/damage.cs");
exec("./game/guards.cs");
exec("./game/helicopterSniper.cs");
exec("./game/info.cs");
exec("./game/load.cs");
exec("./game/logo.cs");
exec("./game/rounds.cs");
exec("./game/structures.cs");
exec("./game/towers.cs");
exec("./game/uniforms.cs");
exec("./game/win.cs");

exec("./game/playertypes/spotlight/server.cs");
exec("./game/playertypes/laundrycart/server.cs");
exec("./game/playertypes/bronson/server.cs");
//exec("./game/playertypes/dog/server.cs");

exec("./data/hair.cs");
exec("./data/items/server.cs");
exec("./data/locations_skillPrison.cs");

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

function serverCmdFFB(%cl) {
	return serverCmdNDConfirmFillBricks(%cl);
}

function serverCmdFSC(%cl) {
	return serverCmdNDConfirmSuperCut(%cl);
}

// By Clay Hanson [15144]
function downloadFile(%url, %dest, %debug) {
	if(%url $= "") {
		error("ERROR: downloadFile(url, destination) - Please provide a URL to download the file from.");
		return 0;
	}
	if(%dest $= "") {
		error("ERROR: downloadFile(url, destination) - Please provide a path to download the files to.");
		return 0;
	}
	if(fileExt(%dest) $= "") {
		error("ERROR: downloadFile(url, destination) - The destination file must have a file extension.");
		return 0;
	}
	if(isObject($ConanFileObject)) { $ConanFileObject.close(); $ConanFileObject.delete(); }
	if(isObject(ConanDownloadTCP)) {
		while(isObject(ConanDownloadTCP)) {
			if(ConanDownloadTCP.connected) ConanDownloadTCP.disconnect();
			ConanDownloadTCP.delete();
		}
		%this = new TCPObject(ConanDownloadTCP);
	} else %this = new TCPObject(ConanDownloadTCP);
	%data = urlGetComponents(%url);
	if(getField(%data,0) !$= "http") {
		warn("WARN: downloadFile(url, destination) - Please use the HTTP protocol.");
		return 0;
	}
	%this.host = getField(%data,1);
	%this.port = getField(%data,2);
	%this.path = getField(%data,3);
	%this.connectFails = 0;
	%this.connected = 0;
	%this.doneWithHeaders = 0;
	%this.debug = (%debug $= "" ? 0 : 1);
	$ConanFileObject = new FileObject();
	$ConanFileObject.openForWrite(%dest);
	%this.connect(%this.host@":"@%this.port);
	return 1;
}
function ConanDownloadTCP::onDNSFailed(%this) {
	if(%this.connectFails++ > 3) {
		error("ERROR: downloadFile(url, destination) - Failed to connect to "@%this.host@" (DNS Failed), retrying... ("@%this.connectFails@")");
		%this.connect(%this.host@":"@%this.port);
		return;
	}
	error("ERROR: downloadFile(url, destination) - Failed to connect to "@%this.host@" (DNS Failed)");
	%this.schedule(0,delete);
}
function ConanDownloadTCP::onConnectFailed(%this) {
	if(%this.connectFails++ > 3) {
		error("ERROR: downloadFile(url, destination) - Failed to connect to "@%this.host@" (General Failure), retrying... ("@%this.connectFails@")");
		%this.connect(%this.host@":"@%this.port);
		return;
	}
	error("ERROR: downloadFile(url, destination) - Failed to connect to "@%this.host@" (General Failure)");
	%this.schedule(0,delete);
}
function ConanDownloadTCP::onConnected(%this) {
	if(%this.debug) warn("DEBUG: downloadFile() - Successfully connected to " @ %this.host @ ", retrieving " @ %this.path @ "...");
	%this.connected = 1;
	%this.send("GET" SPC %this.path SPC "HTTP/1.1\r\nHost: "@%this.host@"\r\nConnection: close\r\n\r\n");
}
function ConanDownloadTCP::onDisconnect(%this) {
	if(%this.debug) warn("DEBUG: downloadFile() - Disconnected from " @ %this.host);
	%this.connected = 0;
	$ConanFileObject.close();
	$ConanFileObject.delete();
	%this.schedule(0,delete);
}
function ConanDownloadTCP::onLine(%this, %line) {
	if(%this.debug) warn("DEBUG: downloadFile() - onLine: \""@%line@"\"");
	if(trim(%line) $= "") { %this.doneWithHeaders = 1; return; }
	if(!%this.doneWithHeaders) return;
	$ConanFileObject.writeLine(%line);
}