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
    [SerializeField] private GroundButton[] buttons;
    private GameMaster GM;
    private EnergyCore core;
    
    // Start is called before the first frame update
    void Start()
    {
        shieldActive = false;
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
        for (int i = 0; i < 4; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < GameMaster.AMOUNT_PLAYER; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool wasPressedCombined = true;
        for (int i = 0; i < GameMaster.AMOUNT_PLAYER; i++)
        {
            wasPressedCombined = wasPressedCombined && buttons[i].buttonWasPressed;
        }
        if (wasPressedCombined)ToggleShield();

        if (shieldActive) core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain);
        if(shieldActive)Debug.Log("Shield active");
    }

    private void ToggleShield()
    {
        for (int i = 0; i < GameMaster.AMOUNT_PLAYER; i++)
        {
             buttons[i].buttonWasPressed = false;
        }

        if (shieldActive) shieldActive = false;
        else shieldActive = true;
    }
}
