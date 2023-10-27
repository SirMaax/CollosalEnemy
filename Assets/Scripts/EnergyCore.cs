using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCore : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float energyLevel;
    [SerializeField] private float energyDrain;
    [SerializeField] private float energyRefillAmount;

    [Header("Settings")] 
    [SerializeField] private float coolDownBetweenEnergyDrain;

    [Header("Refs")] 
    [SerializeField] GroundButton buttonFillEnergy;
    [SerializeField] GroundButton buttonEjectShell;
    [SerializeField] protected ResourceHoldingPlace console;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DrainEnergy());
    }

    // Update is called once per frame
    void Update()
    {   
        if (buttonFillEnergy.buttonWasPressed)
        {
            buttonFillEnergy.buttonWasPressed = false;
            RefilEnergy();
        }
        else if (buttonEjectShell.buttonWasPressed)
        {
            buttonFillEnergy.buttonWasPressed = false;
            EjectShell();
        }

        CheckEnergyLevel();
        UpdateSprite();
    }

    private void RefilEnergy()
    {
        if (!console.isLoaded)
        {
            //TODO bad action 
            return;
        }
        console.DepleteResource();
        energyLevel += energyRefillAmount;
    }

    private void EjectShell()
    {
        if (!console.isLoaded)
        {
            //TODO bad action
            return;
        }
        console.EjectShell();
    }
    
    protected IEnumerator DrainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownBetweenEnergyDrain);
            energyLevel -= energyDrain;
        }
        
    }

    private void CheckEnergyLevel()
    {
        //TODO do stuff when energy empty etc.
    }

    private void UpdateSprite()
    {
        //TODO this
    }
    /**
     * Chcks if enough energy is leved for draining.
     */
    public bool CheckIfEnoughEnergyForDrainThenDrain(float amountEnergy)
    {
        bool enoughEnergy = false;
        if (energyLevel - amountEnergy > 0)
        {
            energyLevel -= amountEnergy;
            enoughEnergy = true;
        }

        return enoughEnergy;
    }
    
    
}
