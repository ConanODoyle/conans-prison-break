$Skill4LifePink = "0.963 0.341 0.313 1";
$SwollowColor = ".9 .34 .08 1";

$GuardColor = ".54 .7 .55 1";
$PrisonerColor = ".9 .34 .08 1";
$DonatorGuardColor = "0 0.141 0.333 1";
$DonatorPrisonerColor = ".106 .459 .769 1";
$PantsColor = "0.1 0.1 0.1 1";

$SpecialUniforms = "12307 6531 1768 4382";

//Functions:
//Packaged:
//	GameConnection::applyBodyParts
//	GameConnection::applyBodyColors
//Created:
//	GameConnection::applyUniformParts
//	GameConnection::applyUniformColors


package CPB_Game_Uniforms {
	function GameConnection::applyBodyParts(%cl) {
		%pl = %cl.player;
		if (!isObject(%pl)) {
			return parent::applyBodyParts(%cl);
		}

		%ret = parent::applyBodyParts(%cl);

		if (%cl.isPrisoner || %cl.isGuard) {
			%cl.applyUniformParts();
		}

		if (%pl.getDatablock().getName() $= "BuffArmor") {
			%pl.unhideNode("ALL");

			if (!%cl.isDonator) {
				%pl.equipHair("Saved");
			} else {
				%pl.mountImage(CrocHatImage, 1);
			}
		}
		
		if (!%cl.skipHairEquip) {
			%pl.equipHair("Saved");
		}

		return %ret;
	}

	function GameConnection::applyBodyColors(%cl) {
		%pl = %cl.player;
		if (!isObject(%pl)) {
			return parent::applyBodyColors(%cl);
		}

		%ret = parent::applyBodyColors(%cl);

		if (%pl.getDatablock().getName() $= "BuffArmor") {
			%color = %cl.headColor;
			%hc = %cl.headColor;
			%tint = max(getWord(%hc, 0) - 0.14, 0) SPC max(getWord(%hc, 1) - 0.16, 0) SPC getWords(%hc, 2, 3);

			%cl.player.setNodeColor("ALL", %color);
			%cl.player.setNodeColor("nipples", %tint);
			%cl.player.setNodeColor("face", "0 0 0 1");
			%cl.player.setNodeColor("pants", "0.1 0.1 0.1 1");
			%cl.player.setNodeColor("lShoe", "0.1 0.1 0.1 1");
			%cl.player.setNodeColor("rShoe", "0.1 0.1 0.1 1");
		}

		if (%cl.isPrisoner || %cl.isGuard) {
			%cl.applyUniformColors();
		}

		return %ret;
	}
};
activatePackage(CPB_Game_Uniforms);


////////////////////


function GameConnection::applyUniformParts(%cl) {
	%pl = %cl.player;
	if(!isObject(%pl) || (!%cl.isPrisoner && !%cl.isGuard)) {
		return;
	}

	if (%cl.isPrisoner) {
		for (%i = 0; %i < 15; %i++) {
			if ($pack[%i] !$= "") { %pl.hideNode($pack[%i]); }
			if ($secondPack[%i] !$= "") { %pl.hideNode($secondPack[%i]); }
			if ($hat[%i] !$= "") { %pl.hideNode($hat[%i]); }
			if ($accent[%i] !$= "") { %pl.hideNode($accent[%i]); }
		}

		if (%pl.isNodeVisible(skirtHip)) {
			%pl.hideNode(skirtHip);
			%pl.hideNode(skirtTrimLeft);
			%pl.hideNode(skirtTrimRight);
			%pl.unHideNode(pants);
		}
		%pl.hideNode(lPeg);
		%pl.hideNode(rPeg);
		%pl.unHideNode(lShoe);
		%pl.unHideNode(rShoe);
	} else if (%cl.isGuard) {
		for (%i = 0; %i < 15; %i++) {
			if ($pack[%i] !$= "") { %pl.hideNode($pack[%i]); }
			if ($secondPack[%i] !$= "" && %i != 4) { %pl.hideNode($secondPack[%i]); }
			if ($hat[%i] !$= "") { %pl.hideNode($hat[%i]); }
			if ($accent[%i] !$= "") { %pl.hideNode($accent[%i]); }
		}

		if (%pl.isNodeVisible(skirtHip)) {
			%pl.hideNode(skirtHip);
			%pl.hideNode(skirtTrimLeft);
			%pl.hideNode(skirtTrimRight);
			%pl.unHideNode(pants);
			%pl.unHideNode(lShoe);
			%pl.unHideNode(rShoe);
		}
		%pl.unHideNode(copHat);
	}

	if (containsWord($SpecialUniforms, %cl.bl_id)) {
		eval("applySpecialUniform" @ %cl.bl_id @ "(" @ %cl @ ");");
	}
}

function GameConnection::applyUniformColors(%cl) {
	%pl = %cl.player;
	if(!isObject(%pl) || (!%cl.isPrisoner && !%cl.isGuard)) {
		return;
	}

	if (%cl.isPrisoner) {
		if (%cl.isDonator) {
			%color = $DonatorPrisonerColor;
		} else {
			%color = $PrisonerColor;
		}

		for (%i = 0; %i < 2; %i++) {
			if ($larm[%i] !$= "") { %pl.setNodeColor($larm[%i], %color); }
			if ($rarm[%i] !$= "") { %pl.setNodeColor($rarm[%i], %color); }
			if ($lleg[%i] !$= "") { %pl.setNodeColor($lleg[%i], $PantsColor); }
			if ($rleg[%i] !$= "") { %pl.setNodeColor($rleg[%i], $PantsColor); }
			if ($chest[%i] !$= "") { %pl.setNodeColor($chest[%i], %color); }
		}

		%pl.setNodeColor(pants, %color);
		%pl.setDecalName("Mod-Prisoner");
	} else if (%cl.isGuard) {
		if (%cl.isDonator) {
			%color = $DonatorGuardColor;
		} else {
			%color = $GuardColor;
		}

		for (%i = 0; %i < 2; %i++) {
			if ($larm[%i] !$= "") { %pl.setNodeColor($larm[%i], %color); }
			if ($rarm[%i] !$= "") { %pl.setNodeColor($rarm[%i], %color); }
			if ($lleg[%i] !$= "") { %pl.setNodeColor($lleg[%i], $PantsColor); }
			if ($rleg[%i] !$= "") { %pl.setNodeColor($rleg[%i], $PantsColor); }
			if ($chest[%i] !$= "") { %pl.setNodeColor($chest[%i], %color); }
		}

		%pl.setNodeColor(pants, $PantsColor);
		%pl.setNodeColor(copHat, %color);
		%pl.setDecalName("Mod-Police");
	}

	if (containsWord($SpecialUniforms, %cl.bl_id)) {
		eval("applySpecialUniform" @ %cl.bl_id @ "(" @ %cl @ ");");
	}
}

function applySpecialUniform12307(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	}

	if (%cl.isGuard) {
		%pl.unHideNode($secondPack2);
		%pl.setNodeColor($secondPack2, "1 1 0 1");
		%pl.unHideNode($accent1);
		%pl.setNodeColor($accent1, "0.388235 0 0.117647 1");
		%color = "0.5 0.5 0.5 1";

		for (%i = 0; %i < 2; %i++) {
			if ($larm[%i] !$= "") { %pl.setNodeColor($larm[%i], %color); }
			if ($rarm[%i] !$= "") { %pl.setNodeColor($rarm[%i], %color); }
			if ($chest[%i] !$= "") { %pl.setNodeColor($chest[%i], %color); }
		}

		%pl.setNodeColor(copHat, %color);
		%pl.setDecalName("Mod-Police");
	}
}

function applySpecialUniform6531(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	}
	
	if (%cl.isPrisoner) {
		%color = $SwollowColor;
		%pl.unHideNode($pack[3]);
		%pl.setNodeColor($pack[3], "0.388 0 0.117 1");
		%pl.unHideNode($secondpack[6]);
		%pl.setNodeColor($secondpack[6], "0.388 0 0.117 1");

		for (%i = 0; %i < 2; %i++) {
			if ($larm[%i] !$= "") { %pl.setNodeColor($larm[%i], %color); }
			if ($rarm[%i] !$= "") { %pl.setNodeColor($rarm[%i], %color); }
			if ($chest[%i] !$= "") { %pl.setNodeColor($chest[%i], %color); }
		}
		%pl.setNodeColor(pants, %color);
	} else if (%cl.isGuard) {
		%pl.unHideNode($pack[3]);
		%pl.setNodeColor($pack[3], "0.388 0 0.117 1");
		%pl.unHideNode($secondpack[6]);
		%pl.setNodeColor($secondpack[6], "0.388 0 0.117 1");
	}

}

function applySpecialUniform4382(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	}

	if (%cl.isPrisoner) {
		%color = $Skill4LifePink;
		%pl.unHideNode($secondpack[2]);
		%pl.setNodeColor($secondpack[2], "1 1 0 1");

		for (%i = 0; %i < 2; %i++) {
			if ($larm[%i] !$= "") { %pl.setNodeColor($larm[%i], %color); }
			if ($rarm[%i] !$= "") { %pl.setNodeColor($rarm[%i], %color); }
			if ($chest[%i] !$= "") { %pl.setNodeColor($chest[%i], %color); }
		}
		%pl.setNodeColor(pants, %color);
	}
}

function applySpecialUniform1768(%cl) {
	if (!isObject(%pl = %cl.player)) {
		return;
	}

	if (%cl.isPrisoner) {
		%pl.unHideNode($secondpack[1]);
		%pl.setNodeColor($secondpack[1], "0.2 0.2 0.2 1");
		%pl.unHideNode($hat[3]);
	}
}