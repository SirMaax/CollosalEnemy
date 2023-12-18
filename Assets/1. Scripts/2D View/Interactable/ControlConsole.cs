using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControlConsole : Console
{
    [Header("Variables")] 
    [SerializeField] private ControlStationType type;
    
    public enum ControlStationType
    {
        movement,
        turning,
    }
    
    
    public override void Interact(Player player)
    {
        switch (type )
        {
            case ControlStationType.movement:
                ControlStation(player);
                break;
            case ControlStationType.turning:
                TurningStation(player);
                break;
        }
    }

    private void ControlStation(Player player)
    {
        player.inputHandler.TogglePlayerIsControllingMech();
    }

    private void TurningStation(Player player)
    {
        player.inputHandler.TogglePlayerIsTurningMech();
    }
}
