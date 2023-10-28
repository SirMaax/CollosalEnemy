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
    [SerializeField] private Enemy enemy;
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
        int counter = 0;
        if (console1.isLoaded)
        {
            counter += 1;
            console1.DepleteResource();
            console1.EjectShell();
            //TODO Effect Enemy Mech
        }if (console2.isLoaded)
        {
            counter += 1;
            console2.DepleteResource();
            console2.EjectShell();
            //TODO Effect Enemy Mech
        }
        if (console3.isLoaded)
        {
            counter += 1;
            console3.DepleteResource();
            console3.EjectShell();
            //TODO Effect Enemy Mech
        }
        //TODO Animation
        enemy.DealDamage(counter);
        Debug.Log("fired " + counter);
    }
}
