function displayShapeText(%pos, %rot, %str, %clear) {
	if (!isObject($CPB::TextGroup)) {
		$CPB::TextGroup = new ScriptGroup(TextGroup) { };
	}

	%size = 1;
	%offset = -0.31;

	if (%clear) {
		PPE_MessageAdmins("!!! - \c6Resetting board contents");
		deleteVariables("$displayStr*");
	}

	for (%i = 0; %i < getFieldCount(%str); %i++) {
		%substr = getField(%str, %i);
		if (%substr $= $displayStr[%i]) {
			%pos = vectorAdd(%pos, 0 SPC 0 SPC %offset);
			continue;
		}
		while (isObject(Char @ %i)) {
			(Char @ %i).delete();
		}
		$displayStr[%i] = %substr;
		createTextSignLine(%substr, %pos, %rot, %size, TextGroup, 0, %i);
		%pos = vectorAdd(%pos, 0 SPC 0 SPC %offset);
	}
}

function createTextSignLine(%text, %pos, %rot, %size, %group, %justify, %lineNum) {
	if(%text $= "" || %pos $= "") 	{
		return;
	}

	%text = strlwr(%text);

	// %vec[0] = "0.5 0 0";
	// %vec[1] = "0 -0.5 0";
	// %vec[2] = "-0.5 0 0";
	// %vec[3] = "0 0.5 0";

	%len = strlen(%text);
	%physLen = %len;

	for(%i = 0; %i < %len; %i++) {
		%l = getSubStr(%text, %i, 1);

		%restAfter = getSubStr(%text, %i, strlen(%text) - %i);

		if(strpos( %restAfter, "<color:" ) == 0) {
			%physLen -= 14;
		}
	}

	// switch(%justify) {
	// 	case 1:
	// 		%pos = VectorSub(%pos, VectorScale( %vec[ %rot ], textSignScaleToUnits(%size) * (%physLen-1) ));
	// 	case 2:
	// 		%pos = VectorSub(%pos, VectorScale( %vec[ %rot ], textSignScaleToUnits(%size) * %physLen * 2 ));
	// }

	%axis[0] = "1 0 0 0";
	%axis[1] = "0 0 -1 270";
	%axis[2] = "0 0 -1 180";
	%axis[3] = "0 0 -1 90";

	if (getWordCount(%rot) <= 1) {
		%axis = %axis[%rot + 0];
	} else {
		%axis = %rot;
	}

	%s = textSignScaleToUnits(%size);

	for(%i = 0; %i < %len; %i++) {
		%l = getSubStr(%text, %i, 1);

		%restAfter = getSubStr(%text, %i, strlen(%text) - %i);

		if(strpos( %restAfter, "<color:" ) == 0) {
			%hex = getSubStr( %restAfter, 7, 6 );

			$_currentTextSignColor = strupr(%hex);

			%i += 13;
			continue;
		}

		%id = strpos($TextSign::PosArray, %l);

		if(%id >= 0) {		
			%shape = new StaticShape(Char @ %lineNum) {
				datablock = "TextSignShape";
				position = %pos;
				rotation = %axis;
				scale = %s SPC %s SPC %s;
			};

			%shape.setNodeColor("ALL", getColorF($_currentTextSignColor));
			
			%shape.hideNode("ALL");
			%shape.unhideNode("c" @ %id);
			
			%group.add(%shape);
		}

		%pos = VectorAdd(%pos, VectorScale( %vec[ %rot ], textSignScaleToUnits(%size)*2 ));
	}
}

function displayRoundLoadingInfo(%clear) 
{
	// %guards1 = getGuardNames();
	// %guards2 = getSubStr(%guards1, strPos(%guards1, "Guard 2 <br>") + 12, strLen(%guards1));
	// %guards1 = " \c6" @ getSubStr(%guards1, 0, strPos(%guards1, "Guard 2 <br>") + 12);
	// %centerprintString = "<font:Arial Bold:20>" @ %guards1 @ "<just:center><font:Arial Bold:22>\c3" @ %statisticString @ "<font:Arial Bold:20><just:right>" @ %guards2 @ "<br><br><br>";
	// centerprintAll(%centerprintString);

	if (!isObject($CPB::TextGroup)) {
		$CPB::TextGroup = new ScriptGroup(TextGroup) { };
	}


	//31 chars per lower line, 23 per upper line
	%line0 = 	"<color:ffffff>            ROUND " @ $Statistics::round;
	if (getStatistic("Winner") !$= "Guards") {
		%line1 =	"<color:ff8724>         PRISONERS WIN";
	} else {
		%line1 =	"<color:8AD88D>            GUARDS WIN";
	}
	%line2 =	" ";
	%line3 =	" ";
	// %line4 = 	"<color:8AD88D>MVP Guard<color:ffffff>: " @ $bestGuardname;
	// %line5 =	"<color:ff8724>MVP Prisoner<color:ffffff>: " @ $bestPrisonername;
	// %line6 =	"<color:ffff00>Trays Used<color:ffffff>: " @ getStatistic("TraysPickedUp") + 0;
	// %line7 =	"<color:ffff00>Buckets Used<color:ffffff>: " @ getStatistic("BucketsPickedUp") + 0;
	// %line8 =	"<color:ffff00>Bricks Destroyed<color:ffffff>: " @ getStatistic("BricksDestroyed") + 0;
	// %line9 =	"<color:ffff00>Prisoners Killed<color:ffffff>: " @ getStatistic("Deaths") + 0;
	%line10 =	" ";
	%line11 =	" ";
	%line12 =	" ";
	%line13 =	" ";
	%line14 =	"<color:ffff00>            -Guards-";
	%line15 =	"<color:ffffff>    " @ getWord($CPB::SelectedGuards, 0).name;
	%line16 =	"<color:ffffff>    " @ getWord($CPB::SelectedGuards, 1).name;
	%line17 =	"<color:ffffff>    " @ getWord($CPB::SelectedGuards, 2).name;
	%line18 =	"<color:ffffff>    " @ getWord($CPB::SelectedGuards, 3).name;

	%str = %line0;
	for (%i = 1; %i < 19; %i++) {
		%str = %str TAB %line[%i];
	}

	displayTextOnBoard(_board1.getPosition(), 2, %str, %clear);
	bottomprintAll(generateBottomPrint(), -1, 1);
}

datablock StaticShapeData(TextSignShape)
{
	shapeFile = "./shapes/physicalText/text.dts";
};

function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}

function clearTextSigns()
{
	TextGroup.chainDeleteAll();
}