//Functions:
//Packaged:
//	GameConnection::onClientEnterGame
//Created:
//	createDefaultMinigame


package CPB_Game_Minigame {
	function GameConnection::onClientEnterGame(%cl) {
		%mini = $DefaultMinigame;
		if ($CPB::PHASE < 0 && isObject($DefaultMinigame)) {
			$DefaultMinigame = "";
		}
		parent::onClientEnterGame(%cl);
		$DefaultMinigame = %mini;
	}
};
activatePackage(CPB_Game_Minigame);


////////////////////


function createDefaultMinigame(%cl) {
	if (!isObject($DefaultMinigame)) {
		if (!isObject(fcn(Conan)) && !isObject(%cl)) {
			echo("Cannot create default minigame - Conan isn't here!");
			messageAdmins("!!! \c6Cannot create default minigame - Conan isn't here! Call createDefaultMinigame(%cl); to create a minigame...");
			return;
		} else if (isObject(%cl)) {
			CreateMiniGameSO(%cl, "Prison Break", 1, 0);
			$DefaultMinigame = %cl.minigame;
		} else {
			CreateMiniGameSO(fcn(Conan), "Prison Break", 1, 0);
			$DefaultMinigame = fcn(Conan).minigame;
		}
	}

	$DefaultMinigame.schedule(100, Reset, FakeClient);
	$DefaultMinigame.playerDatablock = PlayerNoJet.getID();
	$DefaultMinigame.useAllPlayersBricks = 1;
}