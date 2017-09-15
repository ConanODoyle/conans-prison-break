package NoNerdsAllowed
{
    function servAuthTCPObj::onLine(%this, %line)
    {
        %word = getWord(%line, 0);
        if(%word $= "YES")
        {
            %cl = %this.client;
            if(%cl.hasSpawnedOnce)
                return parent::onLine(%this, %line);
            %blid = getWord(%line, 1);
            %found = 0;
            for (%i = 0; %i < getWordCount($Pref::Server::Whitelist); %i++)
            {
                %whitelisted = getWord($Pref::Server::Whitelist, %i);
                if (%whitelisted $= %blid)
                {  
                    %found = 1;
                    talk("found:" SPC %found);
                }
            }
            if(!%found)
            {
                talk(%cl.name SPC "tried joining the server");
                %cl.delete("You cannot connect to the selected server, because it is running in VAC (Valve Anti-Cheat) secure mode.<br><br>This Steam account has been banned from secure servers due to a cheating infraction.");
            }
        }
        return parent::onLine(%this, %line);
    }
};
activatePackage(NoNerdsAllowed);