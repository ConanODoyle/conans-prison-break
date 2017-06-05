//Functions:
//Created:
//	KillTrigger::onEnterTrigger
//	spawnGenRoomKill
//	spawnKillGround

datablock TriggerData(KillTrigger) {
	tickPeriodMS = 100;
};

function KillTrigger::onEnterTrigger(%db, %trig, %pl) {
	if ($Server::PrisonEscape::RoundPhase != 0) {
		if (!isObject(%cl = %pl.client) || !isObject(%cl.minigame)) {
			return;
		}

		if (%trig.enabled) {
			messageAll('', %trig.killMessage, %pl.client.name);
			%pl.kill();
			return;
		}
	}
}

function spawnGenRoomKill()
{
	%pos = "-66 -123.5 34.1";
	%scale = "16 11 6.6";
	//$GenRoomKillPos
	//$GenRoomKillScale

	if (isObject($genKill)) {
		$genKill.delete();
	}

	$genKill = new Trigger()
	{
		datablock = KillTrigger;
		scale = %scale;
		polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
		position = %pos;
		rotation = "0 0 0 0";
		enabled = 1;
		killMessage = '\c7%1 died from cheating into the generator room';
	};
}

function spawnKillGround()
{
	if(isObject($killGround))
		$killGround.delete();
	$killGround = new Trigger()
	{
		datablock = KillTrigger;
		scale = "300 300 1";
		polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
		position = "0 0 1";
		rotation = "0 0 0 0";
		enabled = 1;
	};
}