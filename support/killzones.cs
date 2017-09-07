//Functions:
//Created:
//	KillTrigger::onEnterTrigger
//	createKillZones
//	deleteKillZones
//	spawnGenRoomKill
//	spawnKillGround

datablock TriggerData(KillTrigger) {
	tickPeriodMS = 100;
};

function KillTrigger::onEnterTrigger(%db, %trig, %pl) {
	if ($CPB::Phase == $CPB::GAME) {
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

function createKillZones() {
	spawnGenRoomKill();
	spawnKillGround();
}

function deleteKillZones() {
	if (isObject($genKill)) {
		$genKill.delete();
	}

	if(isObject($killGround)) {
		$killGround.delete();
	}
}

function spawnGenRoomKill() {
	%pos = "-66 -123.5 34.1";
	%scale = "16 11 6.6";
	//$GenRoomKillPos
	//$GenRoomKillScale

	if (isObject($genKill)) {
		$genKill.delete();
	}

	$genKill = new Trigger(killZones) {
		datablock = KillTrigger;
		scale = %scale;
		polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
		position = %pos;
		rotation = "0 0 0 0";
		enabled = 1;
		killMessage = '\c7%1 died from cheating into the generator room';
	};
}

function spawnKillGround() {
	if(isObject($killGround)) {
		$killGround.delete();
	}

	$killGround = new Trigger(killZones) {
		datablock = KillTrigger;
		scale = "300 300 1";
		polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
		position = "0 0 1";
		rotation = "0 0 0 0";
		enabled = 1;
	};
}