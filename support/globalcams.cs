//Functions:
//Created:
//	setAllCamerasView
//	setCameraViewLoop
//	returnAllPlayerControl
//	resetAllClientsFOV
//	setAllCameraControlPlayers
//	setAllCameraControlSelf
//	setAllCameraControlNone


function setAllCamerasView(%camPos, %targetPos, %noControl, %FOV) {
	//calculate the position and rotation of camera
	%pos = %camPos;
	%delta = vectorSub(%targetPos, %pos);
	%deltaX = getWord(%delta, 0);
	%deltaY = getWord(%delta, 1);
	%deltaZ = getWord(%delta, 2);
	%deltaXYHyp = vectorLen(%deltaX SPC %deltaY SPC 0);

	%rotZ = mAtan(%deltaX, %deltaY) * -1; 
	%rotX = mAtan(%deltaZ, %deltaXYHyp);

	%aa = eulerRadToMatrix(%rotX SPC 0 SPC %rotZ); //this function should be called eulerToAngleAxis...
	%camTransform = %pos SPC %aa;

	//apply this on everyone
	setCameraViewLoop(%camTransform, 0, %noControl, %FOV);
}

function setCameraViewLoop(%transform, %i, %noControl, %FOV) {
	if (%i >= ClientGroup.getCount()) {
		return;
	}

	%cl = ClientGroup.getObject(%i);
	%camera = %cl.camera;
	
	%cl.setControlObject(%camera);
	%camera.setTransform(%transform);

	%camera.setFlyMode();
	%camera.mode = "Observer";
	if (%noControl) {
		%camera.setControlObject(%cl.dummyCamera);
	}

	if (%FOV > 0) {
		%cl.setControlCameraFOV(%FOV);
		%cl.originalFOV = %cl.getControlCameraFOV();
	} else if (%cl.originalFOV > 0) {
		%cl.setControlCameraFOV(%cl.originalFOV);
	}
	schedule(0, 0, setCameraViewLoop, %transform, %i+1, %noControl, %FOV);
}

function returnAllPlayerControl() {
	for(%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (isObject(%cl.player)) {
			%cl.setControlObject(%cl.player);
		}
	}
}

function resetAllClientsFOV() {
	for(%i = 0; %i < ClientGroup.getCount(); %i++) {
		if (%cl.originalFOV !$= "") {
			%cl.setControlCameraFOV(%cl.originalFOV);
		}
	}
}

function setAllCameraControlPlayers() {
	for(%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		if (isObject(%cl.player)) {
			%cl.camera.setControlObject(%cl.player);
		}
	}
}

function setAllCameraControlSelf() {
	for(%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		%cl.camera.setControlObject(0);
	}
}

function setAllCameraControlNone(){
	for(%i = 0; %i < ClientGroup.getCount(); %i++) {
		%cl = ClientGroup.getObject(%i);
		%cl.camera.setControlObject(%cl.dummyCamera);
	}
}

// function allCameraPan(%pos1, %pos2, %speed, %targetPos)
// {
// 	if (vectorDist(%pos1, %pos2) < 0.01)
// 	{
// 		talk(vectorDist(%pos1, %pos2));
// 		return;
// 	}
// 	//calculate normal vector from pos 1 to pos 2, then vectorscale by speed/(1000/33)
// 	%moveVector = vectorNormalize(vectorSub(%pos2, %pos1));
// 	%moveVector = vectorScale(%moveVector, %speed*20/10000);

// 	setAllCamerasView(%pos1, %targetPos);
// 	if (isEventPending($allCameraPan)) {
// 		cancel($allCameraPan);
// 	}
// 	$allCameraPan = schedule(0, 0, allCameraPan, vectorAdd(%pos1, %moveVector), %pos2, %speed/1.00005, %targetPos);
// }