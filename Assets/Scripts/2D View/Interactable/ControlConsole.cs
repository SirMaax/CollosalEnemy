using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlConsole : Console
{
    [Header("Variables")] 
    private bool isInUse = false;
    public override void Interact(Player player)
    {
        if (!isInUse)
        {
            player.UsingControlStation(true);
            isInUse = true;
        }
        else
        {
            isInUse = false;
            player.UsingControlStation(false);
        }
    }
    
    
}
