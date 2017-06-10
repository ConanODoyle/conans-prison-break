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
//  applyLogoColors
//  doLogoFadeIn
//  doLogoFadeOut
//  clearLogo

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
    applyLogoColors($LogoShape, 1);
}

function applyLogoColors(%item, %alpha) {
    if (%alpha <= 0 || !isObject(%item)) {
        %item.delete();
        return;
    }

    %item.setNodeColor("outline", "0 0 0 " @ %alpha);
    %item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
    %item.setNodeColor("skin", "0.9 0.712 0.456 " @ %alpha);
    %item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
    %item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);
}

function doLogoFadeOut(%item, %alpha) {
    if (%alpha <= 0 || !isObject(%item)) {
        %item.delete();
        return;
    }

    %item.setNodeColor("outline", "0 0 0 " @ %alpha);
    %item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
    %item.setNodeColor("skin", "0.9 0.712 0.456 " @ %alpha);
    %item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
    %item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);

    schedule(10, %item, doLogoFadeOut, %item, %alpha-0.01);
}

function doLogoFadeIn(%item, %alpha) {
    if (%alpha >= 1 || !isObject(%item)) {
        return;
    }
    %item.unhideNode("ALL");

    %item.setNodeColor("outline", "0 0 0 " @ %alpha);
    %item.setNodeColor("clothing", "0.9 0.479 0 " @ %alpha);
    %item.setNodeColor("skin", "0.96 0.762 0.486 " @ %alpha);
    %item.setNodeColor("bars", "0.5 0.5 0.5 " @ %alpha);
    %item.setNodeColor("beams", "0.168 0.168 0.168 " @ %alpha);

    schedule(10, %item, doLogoFadeIn, %item, %alpha + 0.01);
}

function clearLogo() {
    if (isObject($LogoShape)) {
        $LogoShape.delete();
    }
    
    if (isObject($LogoDish)) {
        $LogoDish.delete();
    } 
}