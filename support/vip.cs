$CPB::MaxPlayers = 30;
$CPB::VIPMembers = "4928 4382 1768 12307 6531 0 1 2 50258";
$CPB::Donators = "33935 20419 44383 2127 12027 19101 40725 26663 3306 30139 3636 23751 37331 70245 51892 107022 102220 44334 42088 32202 18569";
$CPB::Debug = 1;

//Functions:
//Packaged:
//	GameConnection::onConnectRequest
//	GameConnection::onDrop
//	WeaponImage::onMount
//Created:
//	updateClientsMaxPlayerCount

package CPB_Support_CustomJoin {
	function GameConnection::onConnectRequest(%cl, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix, %clNonce, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j) {
		if ($Server::PlayerCount >= $Pref::Server::MaxPlayers) {
			$Pref::Server::MaxPlayers++;
		} else if (ClientGroup.getCount() > $Pref::Server::MaxPlayers) {
			$Pref::Server::MaxPlayers = $Server::PlayerCount + 1;
		}
		// for (%i = 0; %i < $NameOverrideCount; %i++) {
		// 	if (%netName $= getField($NameOverride[%i], 0)) {
		// 		%netName = getField($NameOverride[%i], 1);
		// 			%cl.setPlayerName("au^timoamyo7zene", %netName);
		// 	}
		// }
		if (isObject(fcn(Conan))) {
			messageClient(fcn(Conan), '', "\c4Attempt to join by \c3" @ %netName);
		}
		return parent::onConnectRequest(%cl, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix, %clNonce, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j);
	}

	function GameConnection::onDrop(%cl) {
		if ($Server::PlayerCount < $Pref::Server::MaxPlayers) {
			if ($Pref::Server::MaxPlayers > $CPB::MaxPlayers) {
				$Pref::Server::MaxPlayers--;
				updateClientsMaxPlayerCount();

				if ($CPB::Debug) {
					messageClient(fcn(Conan), '', "\c6On leave: Playercount: " @ $Server::PlayerCount @ " MaxPlayers: " @ $Pref::Server::MaxPlayers);
				}
			}
		}
		return parent::onDrop(%cl);
	}

	function servAuthTCPObj::onLine(%this, %line) {
		%word = getWord(%line, 0);
		if(%word $= "YES") {
			%cl = %this.client;
			if (%cl.hasSpawnedOnce) {
				return parent::onLine(%this, %line);
			}

			%blid = getWord(%line, 1);
			if (containsWord($CPB::VIPMembers, %blid)) {
				messageAll('', "<bitmap:base/client/ui/ci/star> \c3" @ %cl.name @ "\c4 is a VIP member!");
			} else if (containsWord($CPB::Donators, %blid)) {
				messageAll('', "<bitmap:base/client/ui/ci/star> \c3" @ %cl.name @ "\c4 is a Donator!");
				%cl.isDonator = 1;
			}
        }
        return parent::onLine(%this, %line);
	}

	function WeaponImage::onMount(%this, %obj, %slot) {
		if (isObject(%cl = %obj.client) && %cl.isDonator && isObject(%this.goldenImage)) {
			%obj.mountImage(%this.goldenImage, %slot);
			return;
		}
		return parent::onMount(%this, %obj, %slot);
	}
};
activatePackage(CPB_Support_CustomJoin);


function updateClientsMaxPlayerCount() {
	commandToAll('NewPlayerListGui_UpdateWindowTitle', $Pref::Server::Name, $Pref::Server::MaxPlayers);
	// for(%i = 0; %i< ClientGroup.getCount(); %i++) {
	// 	%cl = ClientGroup.getObject(%i);
	// 	%cl.sendPlayerListUpdate();
	// }
}