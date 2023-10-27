using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float activeEnergyDrain;
    
    [Header("Settings")] 

    [Header("Refs")] 
    [SerializeField] GroundButton buttonShot;
    [SerializeField] protected ResourceHoldingPlace console1;
    [SerializeField] protected ResourceHoldingPlace console2;
    [SerializeField] protected ResourceHoldingPlace console3;
    private EnergyCore core;
    [SerializeField] private Door door;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
    }

    // Update is called once per frame
    void Update()
    {
        Shot();
    }

    private void Shot()
    {
        if (!buttonShot.buttonWasPressed) return;
        buttonShot.buttonWasPressed = false;
        if (!door.isClosed && !(console1.isLoaded||console2.isLoaded||console3.isLoaded)) return;
        if (!core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain)) return;
        if (console1.isLoaded)
        {
            console1.EjectShell();
            console1.DepleteResource();
            //TODO Effect Enemy Mech
        }if (console2.isLoaded)
        {
            console2.EjectShell();
            console2.DepleteResource();
            //TODO Effect Enemy Mech
        }
        if (console3.isLoaded)
        {
            console3.EjectShell();
            console3.DepleteResource();
            //TODO Effect Enemy Mech
        }
        //TODO Animation
        
    }
}
