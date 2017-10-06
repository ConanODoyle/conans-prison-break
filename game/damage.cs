//Functions:
//Packaged:
//	GameConnection::onDeath
//	minigameCanDamage


AddDamageType("Satellite",	'<bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Satellite> %1',	 '%2 <bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Satellite> %1',0.2,1);
AddDamageType("Generator",	'<bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Generator> %1',	 '%2 <bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Generator> %1',0.2,1);
AddDamageType("Tower",	'<bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Tower> %1',	 '%2 <bitmap:Add-Ons/Gamemode_CPB/data/ci/CI_Tower> %1',0.2,1);
AddDamageType("Dog",	'<bitmap:Add-Ons/Gamemode_CPB/data/ci/Dog> %1',	 '%2 <bitmap:Add-Ons/Gamemode_CPB/data/ci/Dog> %1',0.2,1);

package CPB_Game_Damage {
	function GameConnection::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc) {
		if ($CPB::PHASE == $CPB::GAME) {
			if (!isObject(%sourceCl) || %cl == %sourceCl) {
				messageClient(%cl, '', "<bitmap:" @ $DamageType::SuicideBitmap[%damageType] @ "> " @ %cl.name);
			} else {
				messageClient(%cl, '', %sourceCl.name @ " <bitmap:" @ $DamageType::MurderBitmap[%damageType] @ "> " @ %cl.name);
				messageClient(%sourceCl, '', %sourceCl.name @ " <bitmap:" @ $DamageType::MurderBitmap[%damageType] @ "> " @ %cl.name);
			}
		}

		return parent::onDeath(%cl, %sourceObj, %sourceCl, %damageType, %damLoc);
	}

	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType) {
		%ret = parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
		if (%db.getID() == BuffArmor.getID() && isObject(%obj.client)) {
			%obj.client.bottomPrintInfo();
		}
		return %ret;
	}

	function Player::setDamageLevel(%obj, %val) {
		if (%obj.client.isGuard && %obj.getDamageLevel() > %val && $CPB::PHASE == $CPB::GAME) {
			messageClient(%obj.client, '', "You cannot heal as a guard!");
			return;
		}
		parent::setDamageLevel(%obj, %val);
		if (%obj.getDatablock().getID() == BuffArmor.getID() && isObject(%obj.client)) {
			%obj.client.bottomPrintInfo();
		}
	}

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


		if (%db1 $= "ShepherdDogHoleBot" || %db2 $= "ShepherdDogHoleBot") {
			if (%cl1.isGuard || %cl2.isGuard) {
				return 0;
			}
		} else if (%db1 $= "ShepherdDogArmor" || %db2 $= "ShepherdDogArmor") {
			if (!%cl1.isPrisoner && !%cl2.isPrisoner) {
				return 0;
			}
		} else if (getMinigameFromObject(%obj1) != getMinigameFromObject(%obj2)) {
			return 0;
		} else if (%cl1.isGuard == %cl2.isGuard || %cl1.isPrisoner == %cl2.isPrisoner) {
			return 0;
		} else if (%db1 $= "SpotlightArmor" || %db2 $= "SpotlightArmor") {
			return 0;
		}

		return 1;
	}
};
activatePackage(CPB_Game_Damage);