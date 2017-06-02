$MessagingEnabled = 0;
$DONATOR::CHATMODIFIER = "<color:ffaaaa>";
$PRISONER::CHATCOLOR = "ff8724";
$GUARD::CHATCOLOR = "8ad88d";

//Object properties:
//Client
//	lastMessage
//	lastMessageTime
//	cpPriority
//	cpPrioritySchedule

//Functions:
//Packaged:
//	serverCmdMessageSent
//	serverCmdTeamMessageSent
//Created:
//	getFormattedMessage
//	CPB_SpamFilter
//	GameConnection::canMessage
//	serverCmdMessageAdmins
//	messageAdmins
//	GameConnection::priorityCenterprint
//	GameConnection::setCPPriority
//	priorityCenterprintAll

package CPB_Support_Messaging {
	function serverCmdMessageSent(%cl, %msg) {
		if ($MessagingEnabled) {
			if (CPB_SpamFilter(%cl, %msg)) {
				return;
			}

			%finalMsg = getFormattedMessage(%cl, %msg);
			for (%i = 0; %i < ClientGroup.getCount; %i++) {
				if (%cl.canMessage((%targ = ClientGroup.getObject(%i).messageGroup))) {
					messageClient(%targ, '', %finalMsg);
				}
			}

			echo(getDateTime() SPC %cl.getTeam() SPC %cl.name @ ": " @  %msg);
			%cl.lastMessageTime = getRealTime();
			%cl.lastMessage = %msg;
		} else {
			return parent::serverCmdMessageSent(%cl, %msg);
		}
	}

	function serverCmdTeamMessageSent(%cl, %msg) {
		if ($MessagingEnabled) {
			messageClient(%cl, '', "\c1Team chat disabled - normal chat is already team-specific unless outside.");
		} else {
			return parent::serverCmdTeamMessageSent(%cl, %msg);
		}
	}
};
activatePackage(CPB_Support_Messaging);

function getFormattedMessage(%cl, %msg) {
	%name = %cl.name;
	%location = %cl.getLocation();

	%msg = stripMLControlChars(%msg);
	if (%cl.isDonator) {
		%msg = $DONATOR::CHATMODIFIER @ %msg;
	}

	if (%cl.isPrisoner) {
		%color = "<color:" @ $PRISONER::CHATCOLOR @ ">";
	} else if (%cl.isGuard) {
		%color = "<color:" @ $GUARD::CHATCOLOR @ ">";
	} else {
		%color = "<color:ffff00>";
	}

	if (%location $= "") {
		return %color @ %name @ "\c6: " @ %msg;
	} else {
		return "\c6[\c3" @ %location @ "\c6] " @ %color @ %name @ "\c6: " @ %msg;
	}
}

function CPB_SpamFilter(%cl, %msg) {
	if (%msg $= %cl.lastMessage && getRealTime() - %cl.lastMessageTime < 3000) {
		if (%cl == %targ) {
			messageClient(%cl, '', "Do not repeat yourself");
		}
		return 1;
	} else if (getRealTime() - %cl.lastMessageTime < 100) {
		if (%cl == %targ) {
			messageClient(%cl, '', "Do not spam");
		}
		%cl.lastMessageTime = getRealTime();
		return 1;
	} else if (getRealTime() - %cl.lastMessageTime < 20) {
		%cl.delete("Do not message spam");
		return 1;
	}
	return 0;
}

function GameConnection::canMessage(%cl, %targ, %msg) {
	if (%cl.isMuted) {
		if (%cl == %targ) {
			messageClient(%cl, '', "You are muted. Use /messageAdmins to report problems.");
		}
		return 0;
	}

	if (%cl == %targ) {
		return 1;
	}

	if ($CPB::PHASE == $CPB::INGAME) {
		if (%cl.isAlive) {
			if (%cl.isPrisoner && %targ.isPrisoner) return 1;
			if (!%targ.hasSpawnedOnce) return 1;
			if (%targ.isDead) return 1;

			%isOutside = %cl.isOutside();
			if (%isOutside && %targ.isOutside()) return 1;
		} else {
			if (!%cl.hasSpawnedOnce) return 1;
			if (%targ.isPrisoner) return 1;
			if (%targ.isDead) return 1;
		}

		return 0;
	} else if ($CPB::PHASE == $CPB::LOBBY) {
		%location = %cl.getLocation();
		if (%location == %targ.getLocation()) return 1;
		if (%targ.BL_ID == 4928) return 1;
	} else {
		return 1;
	}
}


////////////////////


function serverCmdMessageAdmins(%cl, %msg) {
	messageAdmins("\c6[\c0ADMIN\c6] \c2" @ %cl.name @ "\c6: " @ %msg);
	messageClient(%cl, '', "\c6[\c0ADMIN\c6] \c2" @ %cl.name @ "\c6: " @ %msg);
}

function messageAdmins(%msg, %level) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%targ = ClientGroup.getObject(%i);
		if ((%targ.isAdmin && !%level) || %targ.isSuperAdmin) {
			messageClient(%targ, '', %msg);
		}
	}
}


////////////////////


function GameConnection::priorityCenterprint(%cl, %cp, %time, %priority) {
	%priority = %priority + 0; //makes %priority = 0 if it is anything but an integer
	if (%cl.cppriority <= %priority) {
		%cl.centerprint(%cp, %time);
		%cl.cpPriority = %priority;
		cancel(%cl.cpprioritySchedule);
		%cl.cpPrioritySchedule = %cl.schedule(%time * 1000, setCPPriority, 0);
	}
}

function GameConnection::setCPPriority(%cl, %priority) {
	%cl.cpPriority = %priority;
}

function priorityCenterprintAll(%cp, %time, %priority) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		ClientGroup.getObject(%i).priorityCenterprint(%cp, %time, %priority);
	}
}