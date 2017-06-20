//Datablocks:
//	StunImage

//Object properties:
//Player
//	isStunned

//Functions:
//Packaged:
//	Observer::onTrigger
//Created:
//	Player::electrocute
//	stun
//	electrocute
//	spawnRadioWaves


datablock ShapeBaseImageData(StunImage)
{
	shapeFile = "./shapes/stunImage/stun.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 -0.1";
	eyeOffset = "0 0 -0.18";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = true;
	colorshiftColor = "0.5 0.5 0.5 1";

	stateName[0]				= "Activate";
	stateTimeoutValue[0]		= 0.01;
	stateSequence[0]			= "Ready";
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1]				= "Ready";
	stateTimeoutValue[1]		= 1;
	stateSequence[1]			= "Spin";
};


////////////////////


if (isPackage(CPB_Support_Stun)) {
	deactivatePackage(CPB_Support_Stun);
}

package CPB_Support_Stun {
	function Observer::onTrigger(%this, %obj, %trig, %state) {
		%cl = %obj.getControllingClient();
		
		if (%cl.player.isStunned){
			return;
		}
		
		Parent::onTrigger(%this, %obj, %trig, %state);
	}
};
schedule(5000, 0, activatePackage, "CPB_Support_Stun");


////////////////////


registerOutputEvent("Player", "electrocute", "int 0 60", 1);

function Player::electrocute(%pl, %time) 
{
	if (!isObject(%cl = %pl.cl)) {
		return;
	}

	%cl.camera.setMode(Corpse, %pl);
	%cl.setControlObject(%cl.camera);
	%cl.elecrocutedTime += %time;
	
	electrocute(%pl, %time);
}


////////////////////


function stun(%pl, %time) {
	if (!isObject(%cl = %pl.client)) {
		return;
	}

	if (isEventPending(%pl.stunLoop)) {
		cancel(%pl.stunLoop);
	}

	if (%time <= 0)
	{
		if (%pl.isDead || %pl.isSpectating || %pl.isDisabled()) {
			%cl.setControlObject(%cl.camera);
		} else {
			%pl.isStunned = 0;

			%pl.dismount();
			%pl.unmountImage(3);

			%pl.setControlObject(%pl);
			%cl.setControlObject(%pl);

			%pl.playThread(3, root);
		}
		return;
	} else if (!%pl.isStunned && isObject(%cl.player)) {
		%pl.setControlObject(%cl.camera);
		%pl.setVelocity(vectorAdd(%pl.getVelocity(), getRandom() * 2 SPC getRandom() * 2 SPC "3"));
		%pl.mountImage(stunImage, 3);
		
		%cl.camera.setMode(Corpse, %pl);
		%cl.setControlObject(%cl.camera);

		%pl.playThread(3, sit);

		%pl.setVelocity(vectorAdd("0 0 5", %pl.getVelocity()));
	}

	%pl.isStunned = 1;

	%pl.stunLoop = schedule(1000, %pl, stun, %pl, %time - 1);
}

function electrocute(%pl, %time)
{
	if (!isObject(%cl = %pl.client)) {
		return;
	}

	if (isEventPending(%pl.electrocuteLoop)) {
		cancel(%pl.electrocuteLoop);
	}

	if (%time <= 0)
	{
		if (%pl.isDead || %pl.isSpectating || %pl.isDisabled()) {
			%cl.setControlObject(%cl.camera);
		} else {
			%pl.isStunned = 0;

			%cl.applyBodyColors();
			%cl.camera.setMode(Observer);

			%cl.setControlObject(%pl);
		}
		return;
	}

	%pl.isStunned = 1;

	%pl.setNodeColor("ALL", "1 1 1 1");
	%pl.schedule(100, setNodeColor, "ALL", "0 0 0 1");
	%pl.schedule(200, setNodeColor, "ALL", "1 1 1 1");
	%pl.schedule(300, setNodeColor, "ALL", "0 0 0 1");
	%pl.schedule(400, setNodeColor, "ALL", "1 1 1 1");
	%pl.schedule(500, setNodeColor, "ALL", "0 0 0 1");
	%pl.schedule(600, setNodeColor, "ALL", "1 1 1 1");
	%pl.schedule(700, setNodeColor, "ALL", "0 0 0 1");
	%pl.schedule(800, setNodeColor, "ALL", "1 1 1 1");
	%pl.schedule(900, setNodeColor, "ALL", "0 0 0 1");

	%pl.playThread(2, plant);
	%pl.schedule(100, playThread, 2, plant);
	%pl.schedule(200, playThread, 2, plant);
	%pl.schedule(300, playThread, 2, plant);
	%pl.schedule(400, playThread, 2, plant);
	%pl.schedule(500, playThread, 2, plant);
	%pl.schedule(600, playThread, 2, plant);
	%pl.schedule(700, playThread, 2, plant);
	%pl.schedule(800, playThread, 2, plant);
	%pl.schedule(900, playThread, 2, plant);

	spawnRadioWaves(%pl);
	schedule(100, 0, spawnRadioWaves, %pl);
	schedule(200, 0, spawnRadioWaves, %pl);
	schedule(300, 0, spawnRadioWaves, %pl);
	schedule(400, 0, spawnRadioWaves, %pl);
	schedule(500, 0, spawnRadioWaves, %pl);
	schedule(600, 0, spawnRadioWaves, %pl);
	schedule(700, 0, spawnRadioWaves, %pl);
	schedule(800, 0, spawnRadioWaves, %pl);
	schedule(900, 0, spawnRadioWaves, %pl);

	%pl.electrocuteLoop = schedule(1000, 0, electrocute, %pl, %time - 1);
}

function spawnRadioWaves(%pl)
{
	%pos = %pl.getHackPosition();
	%scale = getWord(%pl.getScale(), 2) * 0.5 + getRandom() * 1.5;

	%proj = new Projectile(){
		datablock = radioWaveProjectile;
		initialPosition = %pos;
		initialVelocity = "0 0 0";
		scale = %scale SPC %scale SPC %scale;
	};
	%proj.explode();
}