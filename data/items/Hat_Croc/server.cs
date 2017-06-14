datablock ShapeBaseImageData(CrocHatImage)
{
	shapeFile = "./CrocHat.dts";
	emap = true;
	mountPoint = $HeadSlot;
	canMountToBronson = 1;
	offset = "0 0 -0.022";
	eyeOffset = "0 0 0.01";
	rotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	doColorShift = true;
	colorshiftColor = "0.4 0.36 0.1 1";

	stateName[0]			= "Activate";
	stateTimeoutValue[0]		= 0.5;
	stateTransitionOnTimeout[0]	= "Ready";
	stateEmitter[0]					= GoldenEmitter;
	stateEmitterNode[0]				= "mountPoint";
	stateEmitterTime[0]				= 1000;


	stateName[1]			= "Ready";
	stateTimeoutValue[1]		= 0.5;
	stateTransitionOnTimeout[1]	= "Activate";
	stateEmitter[1]					= GoldenEmitter;
	stateEmitterNode[1]				= "mountPoint";
	stateEmitterTime[1]				= 1000;
};