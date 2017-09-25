$CPB::TutorialCount0 = 5;

$CPB::Tutorial0_0_0 = "\c6";
$CPB::Tutorial0_0_1 = "<font:Courier New Bold:32><color:E65714>Prison Break Tutorial<font:Palatino Linotype:24>";
$CPB::Tutorial0_0_2 = "\c3=Overview=";
$CPB::Tutorial0_0_3 = "";
$CPB::Tutorial0_0_4 = "END";

$CPB::Tutorial0_1_0 = "\c3Overview";
$CPB::Tutorial0_1_1 = "\c6The goal of the gamemode is for the prisoners to break out";
$CPB::Tutorial0_1_2 = "\c6while the guards keep them in until time runs out.";

$CPB::Tutorial0_2_0 = "";
$CPB::Tutorial0_2_1 = "\c6Guards are placed in 4 towers surrounding the prison, and";
$CPB::Tutorial0_2_2 = "\c6prisoners must destroy their towers or break through the wall.";

$CPB::Tutorial0_3_0 = "\c3Tools";
$CPB::Tutorial0_3_1 = "\c6Guards get sniper rifles and equipment to stop the prisoners";
$CPB::Tutorial0_3_2 = "\c6while prisoners use trays and buckets for protection.";


$CPB::TutorialCount1 = 7;

$CPB::Tutorial1_0_0 = "\c6";
$CPB::Tutorial1_0_1 = "<font:Courier New Bold:32><color:E65714>Prison Break Tutorial<font:Palatino Linotype:24>";
$CPB::Tutorial1_0_2 = "\c6=Objectives=";
$CPB::Tutorial1_0_3 = "";
$CPB::Tutorial1_0_4 = "END";

$CPB::Tutorial1_1_0 = "\c3Objectives";
$CPB::Tutorial1_1_1 = "\c6Prisoners can go for multiple objectives to help them";
$CPB::Tutorial1_1_2 = "\c6in their escape.";

$CPB::Tutorial1_2_0 = "\c3Satellite Dish";
$CPB::Tutorial1_2_1 = "\c6Located on the roof, this allows guards to use cameras";
$CPB::Tutorial1_2_2 = "\c6and auto-tracking spotlights.";

$CPB::Tutorial1_3_0 = "\c3Generator";
$CPB::Tutorial1_3_1 = "\c6Accessible through roof, this opens up a new prison exit";
$CPB::Tutorial1_3_2 = "\c6and a key to unlock bonuses.";

$CPB::Tutorial1_4_0 = "\c3Bronson (Requires Key)";
$CPB::Tutorial1_4_1 = "\c6Super buff prisoner that hits fast and has high HP.";

$CPB::Tutorial1_5_0 = "\c3Smoke Grenades (Requires Key)";
$CPB::Tutorial1_5_1 = "\c6Shrouds an area in smoke for " @ $smoketime / 1000 @ " seconds.";

$CPB::Tutorial1_6_0 = "\c3Fire Axe";
$CPB::Tutorial1_6_1 = "\c6Special item that does high damage to tower supports and windows.";

$CPB::TutorialCount2 = 7;

$CPB::Tutorial2_0_0 = "\c6";
$CPB::Tutorial2_0_1 = "<font:Courier New Bold:32><color:E65714>Prison Break Tutorial<font:Palatino Linotype:24>";
$CPB::Tutorial2_0_2 = "\c6=Tips=";
$CPB::Tutorial2_0_3 = "";
$CPB::Tutorial2_0_4 = "END";

$CPB::Tutorial2_1_0 = "\c3Tip #1";
$CPB::Tutorial2_1_1 = "\c6Group up and push together in waves, while others";
$CPB::Tutorial2_1_2 = "\c6go for side objectives.";

$CPB::Tutorial2_2_0 = "\c3Tip #2";
$CPB::Tutorial2_2_1 = "\c6Tape trays to prisoner's backs with the tape item";
$CPB::Tutorial2_2_2 = "\c6in your inventory.";

$CPB::Tutorial2_3_0 = "\c3Tip #3";
$CPB::Tutorial2_3_1 = "\c6Buckets block a single headshot, but do not stun if";
$CPB::Tutorial2_3_2 = "\c6the bullet isn't a stun bullet.";

$CPB::Tutorial2_4_0 = "\c3Tip #4";
$CPB::Tutorial2_4_1 = "\c6Click as a driver of a laundry cart to push it forward.";

$CPB::Tutorial2_5_0 = "\c3Tip #5";
$CPB::Tutorial2_5_1 = "\c6You can unlock a reward by talking to the cat.";

$CPB::Tutorial2_6_0 = "\c3Tip #6";
$CPB::Tutorial2_6_1 = "\c6Pick up dropped special items before they despawn.";

//Functions:
//Packaged:
//	Armor::onRemove
//	Armor::onTrigger
//	serverCmdLight
//Created:
//	GameConnection::showTutorial
//	toggleTutorialSelection
//	getTutorialCenterprint


package CPB_Support_Tutorial {
	function Armor::onRemove(%this, %obj) {
		if (%obj.isViewingTutorial) {
			%obj.client.centerprint("", 0);
		}
		return parent::onRemove(%this, %obj);
	}

	function Armor::onTrigger(%this, %obj, %trig, %state) {
		if (%obj.isViewingTutorial && %state == 1) {
			toggleTutorialSelection(%obj.client, %trig);
		}
		return parent::onTrigger(%this, %obj, %trig, %state);
	}

	function serverCmdLight(%cl) {
		if (%cl.player.isViewingTutorial) {
			%cl.player.isViewingTutorial = 0;
			%cl.centerprint("");
			return;
		}
		return parent::serverCmdLight(%cl);
	}
};
activatePackage(CPB_Support_Tutorial);


////////////////////



registerOutputEvent("GameConnection", "showTutorial", "int 0 100 0");

function GameConnection::showTutorial(%cl, %int) {
	if (%cl.player.isViewingTutorial && %cl.currGroup == %int) {
		return;
	}
	%cl.currGroup = %int;
	%cl.currTutorial = "";
	if (!isObject(%cl.player)) {
		return;
	}
	%cl.player.isViewingTutorial = 1;
	toggleTutorialSelection(%cl, 0);
}

function toggleTutorialSelection(%cl, %trig) {
	%group = %cl.currGroup + 0;
	if (%cl.currTutorial $= "") {
		%cl.currTutorial = 0;
	} else if (%trig == $LEFTCLICK) {
		%cl.currTutorial = ($CPB::TutorialCount[%group] + %cl.currTutorial - 1) % $CPB::TutorialCount[%group];
	} else if (%trig == $RIGHTCLICK) {
		%cl.currTutorial = ($CPB::TutorialCount[%group] + %cl.currTutorial + 1) % $CPB::TutorialCount[%group];
	}

	%cl.centerprint(getTutorialCenterprint(%cl.currGroup, %cl.currTutorial));
}

function getTutorialCenterprint(%group, %id, %bool) {
	%group = %group + 0;
	%id = %id + 0;
	%left = " <br><br><just:left>\c3Left Click<just:center>\c6Light - Exit";
	%right = "               <just:right>\c3Right Click ";

	%result = "<just:center>";
	for (%i = 0; %i < 5; %i++) {
		if ($CPB::Tutorial[%group @ "_" @ %id @ "_" @ %i] $= "END") {
			break;
		}
		%result = %result @ " <br>" @ $CPB::Tutorial[%group @ "_" @ %id @ "_" @ %i];
	}

	%result = %result @ %left @ %right;
	return %result;
}
