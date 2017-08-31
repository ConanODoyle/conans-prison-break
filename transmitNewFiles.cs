// package vNoPickup {
// 	function Armor::onCollision(%this, %obj, %col, %vel, %speed) {
// 		if (%col.getDatablock().getName() $= "vapeNoColorItem") {
// 			return;
// 		}
// 		return parent::onCollision(%this, %obj, %col, %vel, %speed);
// 	}
// };
// activatePackage(vNoPickup);

// package Bobme {
// 	function serverCmdMessageSent(%cl, %msg) {
// 		if (%cl.isBob) {
// 			if (%cl.lastMessage $= %msg) {
// 				spamAlert(%cl);
// 				return;
// 			}
// 			messageAll('', "\c3bob\c6: " @ stripMLControlChars(%msg));
// 			return;
// 		} else {
// 			return parent::serverCmdMessageSent(%cl, %msg);
// 		}
// 	}

// 	function GameConnection::spawnPlayer(%cl) {
// 		%ret = parent::spawnPlayer(%cl);

// 		if (%cl.isBob) {
// 			%cl.player.setShapeName("bob", 8564862);
// 		}
// 		return %ret;
// 	}
// };
// activatePackage(Bobme);

// function serverCmdBob(%cl, %targ) {
// 	if (!%cl.isAdmin) {
// 		return;
// 	} else if (!isObject(%obj = findClientByName(%targ))) {
// 		messageClient(%cl, '', "Cannot find client by name of " @ %targ);
// 	}

// 	%obj.isBob = 1;
// 	if (isObject(%obj.player))
// 		%obj.player.setShapeName("bob", 8564862);
// }

// function serverCmdUnBob(%cl, %targ) {
// 	if (!%cl.isAdmin) {
// 		return;
// 	} else if (!isObject(%obj = findClientByName(%targ))) {
// 		messageClient(%cl, '', "Cannot find client by name of " @ %targ);
// 	}

// 	%obj.isBob = 0;
// 	if (isObject(%obj.player))
// 		%obj.player.setShapeName(%obj.name, 8564862);
// }

// function serverCmdBobAll(%cl) {
// 	if (!%cl.isAdmin) {
// 		return;
// 	}

// 	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
// 		%obj = ClientGroup.getObject(%i);

// 		%obj.isBob = 1;
// 		if (isObject(%obj.player))
// 			%obj.player.setShapeName("bob", 8564862);
// 	}
// }

// function serverCmdUnBobAll(%cl) {
// 	if (!%cl.isAdmin) {
// 		return;
// 	}

// 	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
// 		%obj = ClientGroup.getObject(%i);

// 		%obj.isBob = 0;
// 		if (isObject(%obj.player))
// 			%obj.player.setShapeName(%obj.name, 8564862);
// 	}
// }

package DisableDatablockChange {
	function serverCmdAddEvent(%cl, %en, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
	{
		if (!%cl.isAdmin && (%par1.getID() == DragonArmor.getID() || %par1.getID() == MadmanPlArmor.getID()) ) {
			messageClient(%cl, '', "You cannot set events with Dragon or Madman as the datablock!");
			return;
		}
		else 
		{
			return parent::serverCmdAddEvent(%cl, %en, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		}
	}
};
activatePackage(DisableDatablockChange);

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
