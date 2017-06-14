// //we need the gun add-on for this, so force it to load
// %error = ForceRequiredAddOn("Weapon_Gun");
// %error2 = ForceRequiredAddOn("Player_Spotlight");

// if(%error == $Error::AddOn_Disabled)
// {
//    //A bit of a hack:
//    //  we just forced the gun to load, but the user had it disabled
//    //  so lets make it so they can't select it
//    GunItem.uiName = "";
// }

// if(%error == $Error::AddOn_NotFound || %error2 == $Error::AddOn_NotFound)
// {
//    //we don't have the gun, so we're screwed
//    error("ERROR: Weapon_Sniper Rifle Spotlight - required add-on Weapon_Gun/Player_Spotlight not found");
// }
// else
// {
   exec("./Weapon_Sniper_Rifle.cs"); 
// }