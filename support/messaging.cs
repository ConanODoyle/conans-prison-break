$MessagingEnabled = 0;
$DONATOR::CHATMODIFIER = "<color:ffaaaa>";
$PRISONER::CHATCOLOR = "ff8724";
$GUARD::CHATCOLOR = "8ad88d";

$ClientVariable[$ClientVariableCount++] = "cpPriority";

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
//	GameConnection::getTeam
//	serverCmdMessageAdmins
//	messageAdmins
//	messagePrisoners
//	messageGuards
//	GameConnection::priorityCenterprint
//	GameConnection::setCPPriority
//	priorityCenterprintAll


package CPB_Support_Messaging {
	function serverCmdMessageSent(%cl, %msg) {
		if ($MessagingEnabled) {
			if (CPB_SpamFilter(%cl, %msg) && !%cl.isAdmin) {
				messageClient(%cl, '', "\c5Do not repeat yourself");
				return;
			}

			%finalMsg = getFormattedMessage(%cl, %msg);
			for (%i = 0; %i < ClientGroup.getCount(); %i++) {
				if (%cl.canMessage(%targ = ClientGroup.getObject(%i))) {
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
			if ($DebugMode) {
				messageAll('', getFormattedMessage(%cl, %msg));
			} else {
				messageClient(%cl, '', "\c1Team chat disabled - normal chat is team-only when inside");
			}
		} else {
			return parent::serverCmdTeamMessageSent(%cl, %msg);
		}
	}
};
activatePackage(CPB_Support_Messaging);

function getFormattedMessage(%cl, %msg) {
	%name = %cl.name;
	%location = getLocation(%cl.player);

	%msg = stripMLControlChars(%msg);

	%text = %msg;
	%protocol = "http://";
	%protocolLen = strlen(%protocol);
	%urlStart = strpos(%text, %protocol);
	if (%urlStart == -1.0) {
		%protocol = "https://";
		%protocolLen = strlen(%protocol);
		%urlStart = strpos(%text, %protocol);
	}
	if (%urlStart == -1.0) {
		%protocol = "ftp://";
		%protocolLen = strlen(%protocol);
		%urlStart = strpos(%text, %protocol);
	}
	if (%urlStart != -1.0) {
		%urlEnd = strpos(%text, " ", %urlStart + 1.0);
		%skipProtocol = 0;
		if (%protocol $= "http://") {
			%skipProtocol = 1;
		}
		if (%urlEnd == -1.0) {
			%fullUrl = getSubStr(%text, %urlStart, strlen(%text) - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, strlen(%text) - %urlStart - %protocolLen);
		} else {
			%fullUrl = getSubStr(%text, %urlStart, %urlEnd - %urlStart);
			%url = getSubStr(%text, %urlStart + %protocolLen, %urlEnd - %urlStart - %protocolLen);
		}
		if (strlen(%url) > 0.0) {
			%url = strreplace(%url, "<", "");
			%url = strreplace(%url, ">", "");
			if (%skipProtocol) {
				%newText = strreplace(%text, %fullUrl, "<a:" @ %url @ ">" @ %url @ "</a>\c7");
			} else {
				%newText = strreplace(%text, %fullUrl, "<a:" @ %protocol @ %url @ ">" @ %url @ "</a>\c7");
			}
			echo(%newText);
			%text = %newText;
		}
	}
	%msg = %text;


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
		return "\c6[\c7" @ %location @ "\c6] " @ %color @ %name @ "\c6: " @ %msg;
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

	if (!isObject(%cl.minigame) || !isObject(%targ.minigame)) {
		return 1;
	}

	if ($CPB::PHASE == $CPB::GAME) {
		if (%cl.isAlive) {
			if (%cl.isPrisoner && %targ.isPrisoner) return 1;
			if (%cl.isGuard && %targ.isGuard) return 1;
			if (!%targ.hasSpawnedOnce) return 1;
			if (%targ.isDead && !%cl.isGuard) return 1;

			if (%cl.isOutside() && %targ.isOutside()) return 1;
		} else {
			if (!%cl.hasSpawnedOnce) return 1;
			if (%targ.isPrisoner) return 1;
			if (%targ.isDead) return 1;
		}

		return 0;
	} else if ($CPB::PHASE == $CPB::LOBBY) {
		if (%cl.isGuard == %targ.isGuard) return 1;

		if (getLocation(%cl.player) $= getLocation(%targ.player)) return 1;
		if (%targ.BL_ID == 4928) return 1;

		return 0;
	} else {
		return 1;
	}
}

function GameConnection::getTeam(%cl) {
	if (%cl.isPrisoner) {
		return "[Prisoner]";
	} else if (%cl.isGuard) {
		return "[Guard]";
	}
	return "[No Team]";
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

function messagePrisoners(%msg) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%targ = ClientGroup.getObject(%i);
		if (%targ.isPrisoner || %targ.isPrisonerSnoop) {
			messageClient(%targ, '', %msg);
		}
	}
}

function messageGuards(%msg) {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		%targ = ClientGroup.getObject(%i);
		if (%targ.isGuard || %targ.isGuardSnoop) {
			messageClient(%targ, '', %msg);
		}
	}
}


////////////////////


function GameConnection::priorityCenterprint(%cl, %cp, %time, %priority) {
	%priority = %priority + 0; //makes %priority = 0 if it is anything but an integer
	if (%cl.cpPriority <= %priority) {
		%cl.centerprint(%cp, %time);
		%cl.cpPriority = %priority;
		cancel(%cl.cpPrioritySchedule);
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