//Functions:
//Packaged:
//	GameConnection::onConnectRequest
//Created:
//	updateClientsMaxPlayerCount

package CPB_Support_CustomJoin {
	function GameConnection::onConnectRequest(%cl, %netAddress, %LANname, %netName, %clanPrefix, %clanSuffix, %clNonce, %a, %b, %c, %d, %e, %f, %g, %h, %i, %j) {
		if ($Server::PlayerCount >= $Pref::Server::maxPlayers) {
			$Pref::Server::maxPlayers++;
		} else if (ClientGroup.getCount() > $Pref::Server::maxPlayers) {
			$Pref::Server::maxPlayers = $Server::PlayerCount + 1;
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

	function GameConnection::onDrop(%client) {
		if ($Pref::Server::maxPlayers > $CPB::maxPlayers) {
			$Pref::Server::maxPlayers--;
			updateClientsMaxPlayerCount();
		} else if ($Pref::Server::maxPlayers > ClientGroup.getCount() && ClientGroup.getCount() > $Server::PrisonEscape::maxPlayers) {
			$Pref::Server::maxPlayers = $Server::PrisonEscape::maxPlayers;
			updateClientsMaxPlayerCount();
		}
		return parent::onDrop(%client);
	}
};
activatePackage(CPB_Support_CustomJoin);


function updateClientsMaxPlayerCount() {
	commandToAll('NewPlayerListGui_UpdateWindowTitle', $Pref::Server::Name, $Pref::Server::maxPlayers);
	// for(%i = 0; %i< ClientGroup.getCount(); %i++) {
	// 	%cl = ClientGroup.getObject(%i);
	// 	%cl.sendPlayerListUpdate();
	// }
}