//Functions:
//Created:
//	spawnEmittersLoop

function spawnEmittersLoop(%i) {
	if (isEventPending($infoEmitterLoop) || $Server::PrisonEscape::roundPhase < 1) {
		return;
	}

	%brick = $Server::PrisonEscape::InfoBricks.getObject(%i);
	%name = strLwr(getSubStr(%brick.getName(), 1, strLen(%brick.getName())));
	%type = getSubStr(%name, strPos(%name, "info") + 4, strLen(%name));
	switch$ (%type) {
		case "Bronson": %data = InfoBronsonProjectile;
		case "Bucket": %data = InfoBucketProjectile;
		case "Tray": %data = InfoTrayProjectile;
		case "LaundryCart": %data = InfoLaundryCartProjectile;
		case "Generator": %data = InfoGeneratorProjectile;
		case "SniperRifle": %data = InfoSniperRifleProjectile;
		case "Cameras": %data = InfoCamerasProjectile;
		case "SmokeGrenade": %data = InfoSmokeGrenadeProjectile;
		case "SatDish": %data = InfoSatDishProjectile;
		case "Soap": %data = InfoSoapProjectile;
		case "Burger": %data = InfoBurgerProjectile;
		default: %data = "";
	}
	if (isObject(%data)) {
		%proj = new Projectile(Info) {
			datablock = %data;
			initialPosition = %brick.getPosition();
			initialVelocity = "0 0 1";
		};
		%proj.explode();
		$infoEmitterLoop = schedule(500, 0, spawnEmittersLoop, (%i + 1) % $Server::PrisonEscape::InfoBricks.getCount());
	} else {
		$infoEmitterLoop = schedule(1, 0, spawnEmittersLoop, (%i + 1) % $Server::PrisonEscape::InfoBricks.getCount());
	}
}
