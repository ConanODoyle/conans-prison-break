//  _______        _      _____ _
// |__   __|      | |    / ____(_)
//    | | _____  _| |_  | (___  _  __ _ _ __  ___
//    | |/ _ \ \/ / __|  \___ \| |/ _` | '_ \/ __|
//    | |  __/>  <| |_   ____) | | (_| | | | \__ \
//    |_|\___/_/\_\\__| |_____/|_|\__, |_| |_|___/
//                                 __/ |
//                                |___/
// Author: Zapk
// Version: 1.0.0 (August 2015)

// Events:
// - clearTextSign
// - doTextSign [text] [size] [justify] [angle]

// Commands:
// - /cleartextsigns - clears your own text signs
// - /clearalltextsigns - clears all text signs on the server (Admin)

$TextSign::PosArray = "bcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()/?\\|-+=\"'a,.<>;:";

datablock StaticShapeData(TextSignShape)
{
	shapeFile = "./text.dts";
	category = "Static Shapes";
};

function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}

function clearTextSigns(%bl_id)
{
	%c = 0;

	%useID = (%bl_id !$= "");

	if(isObject(TextSignCleanup) && TextSignCleanup.getCount())
	{
		for(%i = TextSignCleanup.getCount() - 1; %i >= 0; %i--)
		{
			%sg = TextSignCleanup.getObject(%i);

			if(!%useID || %sg.bl_id $= %bl_id)
			{
				%c++;
				%sg.delete();
			}
		}
	}

	return %c;
}

function serverCmdClearAllTextSigns(%this)
{
	if(!%this.isAdmin)
		return;

	%signs = mFloor(clearTextSigns());

	messageAll('MsgClearBricks', '\c3%1 \c0cleared all text signs. (%2)', %this.getPlayerName(), %signs);
}

function serverCmdClearTextSigns(%this)
{
	%signs = clearTextSigns(%this.bl_id);

	if(%signs)
	{
		messageAll('MsgClearBricks', '\c3%1 \c2cleared \c3%1\c2\'s text signs (%2)', %this.getPlayerName(), %signs);
	}
}

function getTextSignGroup(%brick)
{
	if(!isObject(TextSignCleanup))
	{
		return -1;
	}

	for(%i = 0; %i < TextSignCleanup.getCount(); %i++)
	{
		%o = TextSignCleanup.getObject(%i);
		if(%o.brick $= %brick)
		{
			return %o;
		}
	}

	return -1;
}

function removeTextSignGroup(%brick)
{
	%obj = getTextSignGroup(%brick);

	if(!isObject(%obj))
	{
		return false;
	}

	%obj.delete();
	return true;
}

function createTextSignLine(%text, %pos, %rot, %size, %group, %justify)
{
	if(%text $= "" || %pos $= "")
	{
		return;
	}

	%text = strlwr(%text);

	%vec[0] = "0.5 0 0";
	%vec[1] = "0 -0.5 0";
	%vec[2] = "-0.5 0 0";
	%vec[3] = "0 0.5 0";

	%len = strlen(%text);
	%physLen = %len;

	for(%i = 0; %i < %len; %i++)
	{
		%l = getSubStr(%text, %i, 1);

		%restAfter = getSubStr(%text, %i, strlen(%text) - %i);

		if(strpos( %restAfter, "<color:" ) == 0)
		{
			%physLen -= 14;
		}
	}

	switch(%justify)
	{
		case 1:
			%pos = VectorSub(%pos, VectorScale( %vec[ %rot ], textSignScaleToUnits(%size) * (%physLen-1) ));
		case 2:
			%pos = VectorSub(%pos, VectorScale( %vec[ %rot ], textSignScaleToUnits(%size) * %physLen * 2 ));
	}

	%axis[0] = "1 0 0 0";
	%axis[1] = "0 0 -1 270";
	%axis[2] = "0 0 -1 180";
	%axis[3] = "0 0 -1 90";

	%axis = %axis[ %rot ];

	%s = textSignScaleToUnits(%size);

	for(%i = 0; %i < %len; %i++)
	{
		%l = getSubStr(%text, %i, 1);

		%restAfter = getSubStr(%text, %i, strlen(%text) - %i);

		if(strpos( %restAfter, "<color:" ) == 0)
		{
			%hex = getSubStr( %restAfter, 7, 6 );

			$_currentTextSignColor = strupr(%hex);

			%i += 13;
			continue;
		}

		%id = strpos($TextSign::PosArray, %l);

		if(%id >= 0)
		{		
			%shape = new StaticShape()
			{
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

function createTextSign(%text, %pos, %rot, %size, %brick, %justify)
{
	if(%text $= "" || %pos $= "")
	{
		return;
	}

	%rot = mFloor(mClamp( %rot, 0, 3 ));
	%justify = mFloor(mClamp( %justify, 0, 2 ));

	if(!isObject( TextSignCleanup ))
	{
		MissionGroup.add( new ScriptGroup(TextSignCleanup) );
	}

	%group = getTextSignGroup(%brick);
	if(!isObject(%group))
	{
		%group = new ScriptGroup()
		{
			brick = %brick;
			bl_id = %brick.getGroup().bl_id;
		};

		TextSignCleanup.add(%group);
	}

	%pos = VectorSub(%pos, VectorScale( "0 0 0.5", textSignScaleToUnits(%size) * 0.75 ));

	$_currentTextSignColor = getColorIDTable(%brick.getColorID());

	for(%i = 0; %i < getLineCount(%text); %i++)
	{
		%line = getLine(%text, %i);

		createTextSignLine(%line, %pos, %rot, %size, %group, %justify);

		%pos = VectorSub(%pos, VectorScale( "0 0 0.5", textSignScaleToUnits(%size) * 2 ));
	}
}

function textSignScaleToUnits(%size)
{
	return (%size + 1) * 0.125;
}

registerOutputEvent("fxDTSBrick", "clearTextSign");
function fxDTSBrick::clearTextSign(%this)
{
	removeTextSignGroup(%this);
}

registerOutputEvent("fxDTSBrick", "doTextSign", "string 200 100" TAB "int 1 20 3" TAB "list Left 0 Center 1 Right 2" TAB "list South 0 West 1 North 2 East 3");
function fxDTSBrick::doTextSign(%this, %text, %size, %justify, %orientation)
{
	%this.clearTextSign();

	%text = strreplace(%text, "<br>", "\n");
	createTextSign(%text, %this.getPosition(), %orientation, %size - 1, %this, %justify);
}

package TextSignPackage
{
	function fxDTSBrick::onDeath(%this)
	{
		removeTextSignGroup(%this);
		return Parent::onDeath(%this);
	}

	function fxDTSBrick::onRemove(%this)
	{
		removeTextSignGroup(%this);
		return Parent::onRemove(%this);
	}
};

activatePackage(TextSignPackage);