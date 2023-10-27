using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystems : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float activeEnergyDrain;


    [Header("State")] 
    private bool shieldActive;
    
    [Header("Refs")] 
    [SerializeField] GroundButton button1;
    [SerializeField] GroundButton button2;
    [SerializeField] GroundButton button3;
    [SerializeField] GroundButton button4;
    private EnergyCore core;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (button1.buttonWasPressed &&
            button2.buttonWasPressed &&
            button3.buttonWasPressed &&
            button4.buttonWasPressed) ToggleShield();

        if (shieldActive) core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain);
    }

    private void ToggleShield()
    {
        button1.buttonWasPressed = false;
        button2.buttonWasPressed = false;
        button3.buttonWasPressed = false;
        button4.buttonWasPressed = false;

        if (shieldActive) shieldActive = false;
        else shieldActive = true;
    }
}
