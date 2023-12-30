using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairConsole : Console
{
    [Header("Attributes")] 
    [SerializeField] private float _repairTime;
    
    
    [Header("References")] 
    [SerializeField] private MechSystem _mechSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact(Player player)
    {
        base.Interact(player);
    }
}
