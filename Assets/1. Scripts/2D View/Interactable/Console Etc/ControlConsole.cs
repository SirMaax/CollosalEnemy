using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControlConsole : Console
{
    [Header("Variables")] 
    [SerializeField] private ControlStationType type;
    private bool _isInUse = false;
    private Player _whichPlayerUsesConsole;
    public enum ControlStationType
    {
        movement,
        turning,
    }
    
    
    public override void Interact(Player player)
    {
        if (_isBroken) return;
        if (_isInUse && player != _whichPlayerUsesConsole) return;
        _isInUse = !_isInUse;
        _whichPlayerUsesConsole = player;
        if (!_isInUse) _whichPlayerUsesConsole = null;
        
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

    protected override void PlayerLeavesConsole(Player player)
    {
        if (_isInUse && player == _whichPlayerUsesConsole) Interact(player);
        base.PlayerLeavesConsole(player);
    }

    
    
    private void ControlStation(Player player)
    {
        player.inputHandler.TogglePlayerIsControllingMech();
    }
    
    private void TurningStation(Player player)
    {
        player.inputHandler.TogglePlayerIsTurningMech();
    }

    protected override void StopAllAction()
    {
        if (!_isInUse) return;
        if(type==ControlStationType.movement)ControlStation(_whichPlayerUsesConsole);
        else TurningStation(_whichPlayerUsesConsole);
    }
}
