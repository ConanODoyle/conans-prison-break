//Functions:
//Created:
//	despawnAll
//	GameConnection::clearVariables

function despawnAll() {
	for (%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (isObject(%pl = (%cl = ClientGroup.getObject(%i)).player)) {
			%pl.delete();
			%cl.setControlObject(%cl.camera);
			%cl.camera.setControlObject(%cl.dummyCamera);
		}
	}
}

function GameConnection::clearVariables(%cl) {
	%cl.setScore(0);
	//other vars
}