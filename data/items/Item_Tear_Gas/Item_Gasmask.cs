datablock AudioProfile(GasmaskUseSound)
{
	filename = "base/data/sound/playermount.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock ItemData(GasmaskItem)
{
	uiName = "Gasmask";
	iconName = "./icon_gasmask";
	image = GasmaskHImage;
	category = "Tools";
	className = "Weapon";
	shapeFile = "./gasmaskitem.dts";
	mass = 0.5;
	density = 0.2;
	elasticity = 0;
	friction = 0.6;
	emap = true;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";
	canDrop = true;
};


datablock ShapeBaseImageData(GasmaskHImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;
	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0 0 0.0";
	rotation = eulerToMatrix("0 0 0");
	className = "WeaponImage";
	item = GasmaskItem;
	melee = false;
	doReaction = false;
	armReady = false;
	doColorShift = true;
	colorShiftColor = "1 1 1 1";

    goldenImage = GasmaskGoldenHImage;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSound[0] = weaponSwitchSound;

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = true;

	stateName[2] = "Fire";
	stateTransitionOnTimeOut[2] = "Ready";
	stateTimeoutValue[2] = "0.2";
	stateFire[2] = true;
	stateAllowImageChange[2] = true;
	stateScript[2] = "onFire";
};

function GasmaskHImage::onFire(%this, %obj, %slot)
{
	if(isObject(%obj))
	{
        if (%obj.isWearingBucket) {
            messageClient(%obj.client, '', "You can't wear a gasmask while having a bucket on!");
            return;
        }
		if(%obj.getMountedImage(3) $= nametoID(GasmaskMaskImage))
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

package GasmaskPackage
{
	function servercmdDropTool(%this, %slot)
	{
        if (!isObject(%pl = %this.player)) {
            return;
        }

		if (isObject(%pl.tool[%slot]) && (%pl.tool[%slot].getName() $= "GasMaskItem" || %pl.tool[%slot].getName() $= "GasMaskGoldenItem"))
		{
			parent::servercmdDropTool(%this,%slot);
			if (isObject(%pl.getMountedImage(3)) && (%pl.getMountedImage(2).getName() $= "GasmaskMaskImage" || %pl.getMountedImage(2).getName() $= "GasmaskGoldenMaskImage")) 
            { 
                %pl.schedule(5,unMountImage,2);
            }
			return;
		}
		parent::servercmdDropTool(%this,%slot);
	}
};
activatepackage(GasmaskPackage);


datablock ShapeBaseImageData(GasmaskMaskImage)
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
	colorShiftColor = "1 1 1 1";
	hasLight = false;
	doRetraction = false;
};

function GasmaskMaskImage::OnMount(%this, %obj, %slot)
{
	%obj.isWearingGasmask = 1;
    return Parent::OnMount(%this,%obj,%slot);
}

function GasmaskMaskImage::onUnMount(%this, %obj, %slot)
{
	%obj.isWearingGasmask = 0;
    return Parent::onUnMount(%this,%obj,%slot);
}