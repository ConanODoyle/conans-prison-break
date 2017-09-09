//Functions:
//Packaged:
//	minigameCanDamage


package CPB_Game_Damage {
	function minigameCanDamage(%obj1, %obj2) {
		if (%obj1.getClassName() !$= "GameConnection") {
			%cl1 = %obj1.client;
			%db1 = %obj1.getDatablock().getName();
		} else {
			%cl1 = %obj1;
		}

		if (%obj2.getClassName() !$= "GameConnection") {
			%db2 = %obj2.getDatablock().getName();
			%cl2 = %obj2.client;
		} else {
			%cl2 = %obj2;
		}

		if (getMinigameFromObject(%obj1) != getMinigameFromObject(%obj2)) {
			return 0;
		}

		if (%db1 $= "ShepherdDogHoleBot" || %db2 $= "ShepherdDogHoleBot") {
			if (%cl1.isGuard || %cl2.isGuard) {
				return 0;
			}
		} else if (%cl1.isGuard == %cl2.isGuard || %cl1.isPrisoner == %cl2.isPrisoner) {
			return 0;
		} else if (%db1 $= "SpotlightArmor" || %db2 $= "SpotlightArmor") {
			return 0;
		}

		return 1;
	}
};
activatePackage(CPB_Game_Damage);