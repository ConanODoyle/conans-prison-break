datablock ItemData(GasmaskGoldenItem)
{
	uiName = "Golden Gasmask";
	iconName = "./icon_gasmaskGolden";
	image = GasmaskGoldenHImage;
	category = "Tools";
	className = "Weapon";
	shapeFile = "./gasmaskitem.dts";
	mass = 0.5;
	density = 0.2;
	elasticity = 0;
	friction = 0.6;
	emap = true;
	doColorShift = true;
	colorShiftColor = "1 0.9 0 1";
	canDrop = true;
};


datablock ShapeBaseImageData(GasmaskGoldenHImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0 0 0.0";
	rotation = eulerToMatrix("0 0 0");
	className = "WeaponImage";
	item = GasmaskGoldenItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "1 0.9 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;
	stateEmitter[0] = "GoldenEmitter";
	stateEmitterNode[0] = "mountPoint";
	stateEmitterTime[0] = 1000;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;
	stateEmitter[1] = "GoldenEmitter";
	stateEmitterNode[1] = "mountPoint";
	stateEmitterTime[1] = 1000;

	stateName[2] = "Fire";
	stateTransitionOnTimeOut[2] = "Ready";
	stateTimeoutValue[2] = "0.2";
	stateFire[2] = true;
	stateAllowImageChange[2] = true;
	stateScript[2] = "onFire";
	stateEmitter[2] = "GoldenEmitter";
	stateEmitterNode[2] = "mountPoint";
	stateEmitterTime[2] = 1000;
};

function GasmaskGoldenHImage::onFire(%this,%obj,%slot)
{
	if(isObject(%obj))
	{
        if (%obj.isWearingBucket) {
            messageClient(%obj.client, '', "You can't wear a gasmask while having a bucket on!");
            return;
        }
		if(%obj.getMountedImage(3) $= nametoID(GasmaskGoldenMaskImage) || %obj.isWearingGasmask)
		{
			%obj.unmountImage(3);
			serverPlay3D(GasmaskuseSound,%obj.getTransform());
		}
		else
		{
			%obj.unmountImage(3);
			%obj.mountImage(GasmaskMaskImage,3);
			serverPlay3D(GasmaskuseSound,%obj.getTransform());
		}
	}
}


datablock ShapeBaseImageData(GasmaskGoldenMaskImage)
{
	shapeFile = "./Gasmask.dts";
	emap = true;
	mountPoint = $HeadSlot;
	offset = "0 0 0";
	eyeOffset = "0 0 -0.05";
	rotation = eulerToMatrix("0 0 0");
	eyeRotation = eulerToMatrix("0 0 0");
	scale = "1 1 1";
	correctMuzzleVector = true;
	doColorShift = false;
	colorShiftColor = "1 0.9 0 1";
	hasLight = false;
	doRetraction = false;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 100;
	stateTransitionOnTimeout[0] = "Ready";
	stateEmitter[0] = "GoldenEmitter";
	stateEmitterNode[0] = "mountPoint";
	stateEmitterTime[0] = 1000;

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 100;
	stateTransitionOnTimeout[1] = "Activate";
	stateEmitter[1] = "GoldenEmitter";
	stateEmitterNode[1] = "mountPoint";
	stateEmitterTime[1] = 1000;
};

function GasmaskGoldenMaskImage::OnMount(%this, %obj, %slot)
{
	%obj.isWearingGasmask = 1;
    return Parent::OnMount(%this,%obj,%slot);
}

function GasmaskGoldenMaskImage::onUnMount(%this, %obj, %slot)
{
	%obj.isWearingGasmask = 0;
    return Parent::onUnMount(%this,%obj,%slot);
}