$BUILDINGS::GENERATORALERTCPPRIORITY = 5;
$BUILDINGS::EWSALERTCPPRIORITY = 5;

//Functions:
//Created:
//	fxDTSBrick::disableSpotlights //event
//	destroyGeneratorWindow
//	destroyGenerator
//	destroyEWS


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
	Tower0.guard.player.setDamageFlash(30);
	Tower1.guard.player.setDamageFlash(30);
	Tower2.guard.player.setDamageFlash(30);
	Tower3.guard.player.setDamageFlash(30);

	//messaging and recording
	%type = $DamageType::Satellite;

	if (!isObject(%cl)) {
		priorityCenterprintAll("<font:Impact:30>\c4The cameras and spotlight auto tracking have been disabled!", 20, $BUILDINGS::EWSALERTCPPRIORITY);
		%msg = $DamageType::SuicideBitmap[%type];
	} else {
		priorityCenterprintAll("<font:Impact:30>\c4The cameras and spotlight auto tracking have been disabled by \c3" @ %cl.name @ "\c4!", 20, $BUILDINGS::EWSALERTCPPRIORITY);
		%msg = $DamageType::MurderBitmap[%type];
	}

	messageAll('MsgStartUpload', %cl.name @ " <bitmap:" @ %msg @ "> [" @ getTimeString($CPB::CurrRoundTime) @ "]");
}