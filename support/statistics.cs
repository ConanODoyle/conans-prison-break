//Functions:
//Created:
//	getStat
//	setStat
//	GameConnection::getStat
//	GameConnection::setStat
//	Player::getStat
//	Player::setStat


function getStat(%stat) {
	return $Stats_[%stat];
}

function setStat(%stat, %val) {
	$Stats_[%stat] = %val;
}

function GameConnection::getStat(%cl, %stat) {
	return getStat(%cl.bl_id @ "_" @ %stat);
}

function GameConnection::setStat(%cl, %stat, %val) {
	setStat(%cl.bl_id @ "_" @ %stat, %val);
}

function Player::setStat(%pl, %stat) {
	%cl = %pl.client;
	if (!isObject(%cl)) {
		echo("Can't get stat " @ %stat @ " for " @ %pl @ " - no client!");
		return;
	}

	return %cl.getStat("PL_" @ %stat, %val);
}

function Player::setStat(%pl, %stat, %val) {
	%cl = %pl.client;
	if (!isObject(%cl)) {
		echo("Can't record stat " @ %stat @ " for " @ %pl @ " - no client!");
		return;
	}

	%cl.setStat("PL_" @ %stat, %val);
}