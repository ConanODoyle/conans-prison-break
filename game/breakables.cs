$CPB::BrickType::Support = 1;
$CPB::BrickType::Window = 2;
$CPB::BrickType::SatDish = 3;
$CPB::BrickType::Bars = 4;
$CPB::BrickType::Plates = 5;

$CPB::BrickType::WindowHP = 20;
$CPB::BrickType::SatDishHP = 30;
$CPB::BrickType::BarsHP = 1;
$CPB::BrickType::PlatesHP = 1;

$CPB::BrickType::SupportStageHP = 16;
$CPB::BrickType::SupportStageCount = 4;
$CPB::BrickType::SupportColor0 = 60;
$CPB::BrickType::SupportColor1 = 59;
$CPB::BrickType::SupportColor2 = 57;
$CPB::BrickType::SupportColor3 = 55;
$CPB::BrickType::SupportColor4 = 56;
$CPB::BrickType::SupportColor5 = 55;
$CPB::BrickType::SupportColor6 = 54;

$damageFlashColor = 45;

//Object properties:
//fxDTSBrick
//	hasOriginalColorData

//Functions:
//Packaged:
//	fxDTSBrick::onDeath
//	ChiselProjectile::onCollision
//Created:
//	fxDTSBrick::damage
//	isBreakableBrick
//	fxDTSBrick::killDelete


package CPB_Game_Breakables {
	function fxDTSBrick::onDeath(%obj) {

		$InputTarget_["Self"] = %obj;
		$InputTarget_["Client"] = %cl = findClientByBL_ID(%obj.getGroup().bl_id);
		$InputTarget_["Player"] = %cl.player;
		$InputTarget_["Minigame"] = getMinigameFromObject(%b);

		%obj.processInputEvent("onDeath", %obj.hitClient);

		parent::onDeath(%obj);
	}

	function ChiselProjectile::onCollision(%data, %obj, %col, %fade, %pos, %normal) {
		if (%col.getClassName() $= "FxDTSBrick") {
			if ((%type = %b.type) || (%type = isBreakableBrick(%col, %obj.sourceObject)) {
				%obj.type = %type;
				%obj.client.incScore(1);
				%col.damage();
			}
		}
		return parent::onCollision(%data, %obj, %col, %fade, %pos, %normal);
	}
};
activatePackage(CPB_Game_Breakables);


////////////////////


function fxDTSBrick::damage(%b, %damage, %player) {
	if(isEventPending(%b.recolorSchedule))
		cancel(%b.recolorSchedule);

	if(!%b.hasOriginalColorData) {
		%b.origColorID = %b.getColorID();
		%b.hasOriginalColorData = 1;
	}

	if(!%b.maxDamage) {
		if (%b.type == $CPB::BrickType::Bars) {
			%b.maxDamage = $CPB::BrickType::BarsHP;
		} else if (%b.type == $CPB::BrickType::Window) {
			%b.maxDamage = $CPB::BrickType::WindowHP;
		} else if (%b.type == $CPB::BrickType::SatDish) {
			%b.maxDamage = $CPB::BrickType::SatDishHP;
		} else if (%b.type == $CPB::BrickType::Plates) {
			%b.maxDamage = $CPB::BrickType::PlatesHP;
		} else if (%b.isSupport) {
			%b.maxDamage = $CPB::BrickType::SupportStageHP * $CPB::BrickType::SupportStageCount;
			%b.stage = 0;
		}
	}

	%b.damage += %damage;

	//special behavior
	if (%b.type == $CPB::BrickType::SatDish) {
		if (getRandom() < 0.5) {
			%player.electrocute(2);
		}
	}

	if (%b.damage >= %b.maxDamage) {
		%b.killDelete();
		return;
	}

	if (%b.isSupport) {
		%b.colorStage = mFloor(%b.damage / $CPB::BrickType::SupportStageHP);
		%b.origColorID = $CPB::BrickType::SupportColor[%b.colorStage];
	}

	%b.setColor($damageFlashColor);
	%b.recolorSchedule = %b.schedule(50, setColor, %b.origColorID);
}

function isBreakableBrick(%b, %player) {
	%db = %b.getDatablock().getName();
	
	if (%b.isSupport) {
		return $CPB::BrickType::Support;
	} else if (%db.getID() == brickSatDishData.getID()) {
		return $CPB::BrickType::SatDish;
	} else if (strPos(strLwr(%db.getName()), "pole") >= 0 && !%b.isCosmetic) {
		return $CPB::BrickType::Bars;
	} else if (strPos(strLwr(%db.getName()), "window") >= 0) {
		return $CPB::BrickType::Window;
	}

	return 0;
}

function fxDTSBrick::killDelete(%b) {
	%b.fakeKillBrick((getRandom() - 0.5) * 20 SPC (getRandom() - 0.5) * 20 SPC "-1", -1);
	%b.schedule(2000, delete);
	if (%b.type == $CPB::BrickType::SatDish) {
		%b.spawnExplosion(tankShellProjectile, "0.5 0.5 0.5");
	} else if (%b.type == $CPB::BrickType::Window) {
		%b.playSound(glassExplosionSound);
	} else {
		serverPlay3D("brickBreakSound", %b.getPosition());
	}

	if (%b.tower > 0) {
		validateTower(%b.tower, %b);
	}
}