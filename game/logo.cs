datablock StaticShapeData(LogoClosedShape)
{
    shapeFile = "./shapes/logo/LogoClosed.dts";
};

datablock StaticShapeData(LogoOpenShape)
{
    shapeFile = "./shapes/logo/LogoOpen.dts";
};

datablock StaticShapeData(LogoDishShape)
{
    shapeFile = "./shapes/logo/LogoDish.dts";
};

//Functions:
//Created:
//	displayLogo

function displayLogo(%camPos, %targetPos, %logo, %bg) {
    if (!isObject(%logo)) {
        return;
    }

    if (isObject($LogoShape)) {
        $LogoShape.delete();
    }
    
    if (isObject($LogoDish) && %bg) {
        $LogoDish.delete();
    } 

    %pos = %targetPos;
    %delta = vectorSub(%camPos, %pos);
    %deltaX = getWord(%delta, 0);
    %deltaY = getWord(%delta, 1);
    %deltaZ = getWord(%delta, 2);
    %deltaXYHyp = vectorLen(%deltaX SPC %deltaY SPC 0);

    %rotZ = mAtan(%deltaX, %deltaY) * -1; 
    %rotX = mAtan(%deltaZ, %deltaXYHyp) - $tilt;

    %aa = eulerRadToMatrix(%rotX SPC 0 SPC %rotZ); //this function should be called eulerToAngleAxis...
    %camTransform = %pos SPC %aa;
    %dishTransform = vectorSub(%pos, vectorScale(vectorNormalize(%delta), 1.5)) SPC %aa;

    $LogoShape = new StaticShape(Logo) {
        datablock = %logo;
        scale = "2.0 2.0 2.0";
    };
    if (%bg) {
        if (isObject($LogoDish)) {
            $LogoDish.delete();
        }
        $LogoDish = new StaticShape(Logo) {
            datablock = LogoDishShape;
            scale = "0.7 0.7 0.7";
        };
        MissionCleanup.add($LogoDish);
        $LogoDish.startFade(0, 0, 1);
        $LogoDish.setTransform(%dishTransform);
        $LogoDish.setNodeColor("main", "1 0.92 0 1");
        $LogoDish.setNodeColor("Accent", "0.95 0.86 0 1");
        $LogoDish.playThread(0, rotate);
    }
    MissionCleanup.add($LogoShape);

    $LogoShape.startFade(0, 0, 1);
    $LogoShape.setTransform(%camTransform);
    doLogoFadeIn($LogoShape, 0.99);
}