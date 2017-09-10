exec("./Item_Tray/server.cs");
exec("./Item_Bucket/server.cs");
exec("./Item_Smoke_Grenade/server.cs");
exec("./Item_Tape/server.cs");
exec("./Item_Tear_Gas/server.cs");
exec("./Item_Tear_Gas/Item_Gasmask.cs");
exec("./Item_FireAxe/server.cs");
exec("./Item_Chisel/server.cs");
exec("./Item_Soap/server.cs");
exec("./Item_Steak/server.cs");
exec("./Item_PosTool/posTool.cs");
exec("./Weapon_Shotgun/server.cs");
exec("./Weapon_LMG/server.cs");
exec("./Weapon_Sniper_Rifle_Spotlight/server.cs");
exec("./Hat_Croc/server.cs");

datablock ParticleData(goldenParticleA)
{
	textureName			 = "base/lighting/flare";
	dragCoefficient		= 0.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 300;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "1 1 0 0";
	colors[1]	  = "1 1 0 1";
	colors[2]	  = "1 1 0 0";

	sizes[0]		= 1.5;
	sizes[1]		= 3.3;
	sizes[2]		= 1.8;

	times[0]		= 0.0;
	times[1]		= 0.3;
	times[2]		= 1.0;
};

datablock ParticleData(goldenParticleB)
{
	textureName			 = "base/lighting/flare";
	dragCoefficient		= 0.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0; 
	inheritedVelFactor	= 1;
	lifetimeMS			  = 300;
	lifetimeVarianceMS	= 100;
	useInvAlpha = false;
	spinRandomMin = 280.0;
	spinRandomMax = 281.0;

	colors[0]	  = "1 1 0 0";
	colors[1]	  = "1 1 0 1";
	colors[2]	  = "1 1 0 0";

	sizes[0]		= 0;
	sizes[1]		= 1;
	sizes[2]		= 0;

	times[0]		= 0.0;
	times[1]		= 0.5;
	times[2]		= 1.0;
};

datablock ParticleEmitterData(goldenEmitter)
{
	ejectionPeriodMS = 280;
	periodVarianceMS = 110;

	ejectionOffset = 0.8;
	ejectionOffsetVariance = 0.5;
	
	ejectionVelocity = 0;
	velocityVariance = 0;

	thetaMin			= 0.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = "goldenParticleA goldenParticleB";	

	useEmitterColors = false;

	uiName = "Golden Shine";
};

datablock ParticleData(goldenParticleProjectileA : goldenParticleA)
{
	inheritedVelFactor	= 0;
};

datablock ParticleData(goldenParticleProjectileB : goldenParticleB)
{
	inheritedVelFactor	= 0;
};


datablock ParticleEmitterData(projectileGoldenEmitter)
{
	ejectionPeriodMS = 6;
	periodVarianceMS = 5;

	ejectionOffset = 0;
	ejectionOffsetVariance = 0.6;
	
	ejectionVelocity = 0;
	velocityVariance = 0;

	thetaMin			= 0.0;
	thetaMax			= 180.0;  

	phiReferenceVel  = 0;
	phiVariance		= 360;

	particles = "goldenParticleProjectileA goldenParticleProjectileB";

	useEmitterColors = false;

	uiName = "Golden Shine";
};


exec("./Item_Tray/golden/server.cs");
exec("./Item_Bucket/golden/server.cs");
exec("./Item_Soap/golden/server.cs");
exec("./Item_Smoke_Grenade/golden/server.cs");
exec("./Item_Tape/golden/server.cs");
exec("./Item_Tear_Gas/golden/server.cs");
exec("./Item_Tear_Gas/golden/Item_Gasmask.cs");
exec("./Item_Chisel/golden/server.cs");
exec("./Weapon_Shotgun/golden/server.cs");
exec("./Weapon_LMG/golden/server.cs");
exec("./Weapon_Sniper_Rifle_Spotlight/golden/server.cs");