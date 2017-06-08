$LEFTCLICK = 0;
$RIGHTCLICK = 4;
$ClientVariableCount = 0;

//colortest http://i.imgur.com/Ofi3BvW.png

if (!isObject(FakeClient)) {
	new ScriptObject(FakeClient) {
		isSuperAdmin = 1;
		isAdmin = 1;
		hasSpawnedOnce = 1;
		name = "Dummy Client";
		bl_id = "80085";
	};
	MissionCleanup.add(FakeClient);
}

exec("./support/math.cs");

exec("./support/barber.cs");
	exec("./data/hair.cs");
exec("./support/bottomprint.cs");
exec("./support/electrocute.cs");
exec("./support/EmptyHoleBot.cs");
exec("./support/globalcams.cs");
exec("./support/killzones.cs");
exec("./support/messaging.cs");
exec("./support/spawn.cs");
exec("./support/spectate.cs");
exec("./support/stun.cs");
exec("./support/timer.cs");

exec("./game/buildings.cs");
exec("./game/guards.cs");
exec("./game/rounds.cs");
exec("./game/towers.cs");
exec("./game/playertypes/spotlight/server.cs");