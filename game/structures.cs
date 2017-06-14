$windowDamage = 10;
$towerDamage = 16;
$towerStages = 4;
$towerColor0 = 60;
$towerColor1 = 59;
$towerColor2 = 57;
$towerColor3 = 55;
$towerColor4 = 56;
$towerColor5 = 55;
$towerColor6 = 54;
$damageFlashColor = 45;

$BUILDINGS::GENERATORALERTCPPRIORITY = 5;
$BUILDINGS::EWSALERTCPPRIORITY = 5;

//Functions:
//Created:
//	fxDTSBrick::disableSpotlights //event
//	destroyGeneratorWindow
//	destroyGenerator
//	destroyEWS
//	fxDTSBrick::killDelete
//	fxDTSBrick::damage


registerOutputEvent("fxDTSBrick", "disableSpotlights", "", 1);

function fxDTSBrick::disableSpotlights(%this, %cl) {
	destroyGenerator(%cl);
}

function destroyGeneratorWindow(%cl, %brick) {
	$GenKill.enabled = 0;
}

function destroyGenerator(%cl, %brick) {
	disableTowerSpotlights();

	//messaging and recording
	%type = $DamageType::Generator;

	if (!isObject(%cl)) {
		priorityCenterprintAll("<font:Impact:30>\c4The spotlights have been disabled!", 20, $BUILDINGS::GENERATORALERTCPPRIORITY);
		%msg = $DamageType::SuicideBitmap[%type];
	} else {
		priorityCenterprintAll("<font:Impact:30>\c4The spotlights have been disabled by \c3" @ %cl.name @ "\c4!", 20, $BUILDINGS::GENERATORALERTCPPRIORITY);
		%msg = $DamageType::MurderBitmap[%type];
	}

	messageAll('MsgStartUpload', %cl.name @ " <bitmap:" @ %msg @ "> [" @ getTimeString($CPB::CurrRoundTime) @ "]");
}

function destroyEWS(%cl) {

	//messaging and recording
	%type = $DamageType::Satellite;

	if (!isObject(%cl)) {
		priorityCenterprintAll("<font:Impact:30>\c4The cameras and spotlight auto tracking have been disabled!", 20, $BUILDINGS::EWSALERTCPPRIORITY);
		%msg = $DamageType::SuicideBitmap[%type];
	} else {
		priorityCenterprintAll("<font:Impact:30>\c4The cameras and spotlight auto tracking have been disabled by \c3" @ %cl.name @ "\c4!", 20, $BUILDINGS::EWSALERTCPPRIORITY);
		%msg = $DamageType::MurderBitmap[%type];
	}

	messageAll('MsgStartUpload', %cl.name @ " <bitmap:" @ %msg @ "> [" @ getTimeString($CPB::CurrRoundTime-1) @ "]");
}

function FxDTSBrick::killDelete(%this) {
	if (%this.getDatablock().getName() $= "brick4x4F_GlassPaneData") {
		$Server::PrisonEscape::GeneratorOpened = 1;
	}
	%this.fakeKillBrick((getRandom()-0.5)*20 SPC (getRandom()-0.5)*20 SPC -1, 2);
	%this.schedule(2000, delete);
	serverPlay3D("brickBreakSound", %this.getPosition());

	if (isObject("Tower" @ %this.tower)) {
		validateTower(%this.tower, %this);
	}
}

function FxDTSBrick::damage(%brick, %damage, %player)
{
	if(isEventPending(%brick.recolorSchedule))
		cancel(%brick.recolorSchedule);

	if(!%brick.hasOriginalData)
	{
		%brick.origColorID = %brick.getColorID();
		%brick.hasOriginalData = 1;
	}
	if(!%brick.maxDamage)
	{
		%db = %brick.getDatablock().getName();
		if (%brick == $Server::PrisonEscape::CommDish) {
			%brick.maxDamage = 18;
		} else if (strPos(%brick.getName(), "tower") >= 0) {
			%brick.maxDamage = $towerDamage * $towerStages;
			%brick.isTowerSupport = 1;
			%brick.colorStage = 0;
		} else if (%brick == $Server::PrisonEscape::Generator) {
			%brick.maxDamage = 10;
		} else if (%brick.getDatablock().uiname $= "4x4f Glass Pane") {
			%brick.maxDamage = 25;
		} else if (strPos(%brick.getName(), "tower") < 0) {
			%brick.maxDamage = $windowDamage;
		}
	}

	%brick.damage += %damage;
	if (%brick == $Server::PrisonEscape::CommDish) {
		setStatistic("CommDishHit", getStatistic("CommDishHit", %player.client) + 1, %player.client);
		setStatistic("CommDishHit", getStatistic("CommDishHit") + 1);
		%brick.playSound(trayDeflect1Sound);
	} else if (%brick.getDatablock().uiname $= "4x4f Glass Pane") {
		setStatistic("GeneratorWindowsHit", getStatistic("GeneratorWindowsHit", %player.client) + 1, %player.client);
		setStatistic("GeneratorWindowsHit", getStatistic("GeneratorWindowsHit") + 1);
		%brick.playSound("glassChip" @ getRandom(1, 3) @ "Sound");
	} else if (strPos(%brick.getName(), "tower") < 0) {
		setStatistic("WindowsHit", getStatistic("WindowsHit", %player.client) + 1, %player.client);
		setStatistic("WindowsHit", getStatistic("WindowsHit") + 1);
		%brick.playSound("glassChip" @ getRandom(1, 3) @ "Sound");
	} else {
		setStatistic("TowerSupportsHit", getStatistic("TowerSupportsHit", %player.client) + 1, %player.client);
		setStatistic("TowerSupportsHit", getStatistic("TowerSupportsHit") + 1);
	}
	if (%brick == $Server::PrisonEscape::CommDish) {
		if (getRandom() > 0.8) {
			%player.electrocute(2);
		}
	}

	if (%brick.damage >= %brick.maxDamage) {
		if (%brick == $Server::PrisonEscape::CommDish) {
			%brick.spawnExplosion(tankShellProjectile, "0.5 0.5 0.5");
		} else if (strPos(%brick.getName(), "tower") < 0) {
			%brick.playSound(glassExplosionSound);
		}
		%brick.killDelete();
		return;
	}

	if (%brick.isTowerSupport) {
		%brick.colorStage = mFloor(%brick.damage / $towerDamage);
		%brick.origColorID = $towerColor[%brick.colorStage];
	}

	%brick.setColor($damageFlashColor);
	%brick.recolorSchedule = %brick.schedule(50, setColor, %brick.origColorID);
}