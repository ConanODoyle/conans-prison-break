$CPB::Classes::Stun = 1;
$CPB::Classes::Shrapnel = 2;
$CPB::Classes::LMG = 3;
$CPB::Classes::TearGas = 4;
$CPB::Classes::List = "Stun Shrapnel LMG TearGas";

//Object parameters:
//Client
//	guardClass

//Functions:
//Created:
//	GameConnection::setClass


registerOutputEvent(GameConnection, "setGuardClass", "string 200 156", 1);

function GameConnection::setGuardClass(%cl, %name) {
	if (containsWord($CPB::Classes::List, %name)) {
		%cl.guardClass = $CPB::Classes["::" @ %name];
	}
}