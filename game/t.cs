
function PumpShotgunImage::onFire(%this,%obj,%slot) {
	if(%obj.shotgunAmmo > 0) {
		%fvec = %obj.getForwardVector();
		%fX = getWord(%fvec,0);
		%fY = getWord(%fvec,1);

		%evec = %obj.getEyeVector();
		%eX = getWord(%evec,0);
		%eY = getWord(%evec,1);
		%eZ = getWord(%evec,2);

		%eXY = mSqrt(%eX*%eX+%eY*%eY);
  
		%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
		serverPlay3D(PumpShotgunfireSound,%obj.getPosition());
		%obj.playThread(2, plant);

		%obj.shotgunAmmo--;
    	%obj.client.bottomprintInfo();

		%obj.spawnExplosion(TTLittleRecoilProjectile, "2.5 2.5 2.5");
            		

		%projectile = %this.projectile;
		%spread = 0.003;
		%shellcount = 15;

		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);

			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
		}
	
		//shotgun blast projectile: only effective at point blank, sends targets flying off into the distance
		//
		//more or less represents the concussion blast. i can only assume such a thing exists because
		// i've never stood infront of a fucking shotgun before
		///////////////////////////////////////////////////////////

		%projectile = "shotgunBlastProjectile";
	
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
	

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		return %p;
	}
}