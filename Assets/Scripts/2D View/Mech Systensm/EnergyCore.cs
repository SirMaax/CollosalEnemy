using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyCore : MechSystem
{
    [Header("Attributes")] 
    [SerializeField] private float energyLevel;
    [SerializeField] private float energyDrain;
    [SerializeField] private float energyRefillAmount;
    
    private int maxEnergy;
    
    [Header("Settings")] 
    [SerializeField] private float coolDownBetweenEnergyDrain;
    [SerializeField] private bool infiteEnergy;
    
    private bool once = true;
    [SerializeField] private float scoreNoEnergy;
    
    [Header("Refs")] 
    [SerializeField] GroundButton buttonFillEnergy;
    [SerializeField] GroundButton buttonEjectShell;
    [SerializeField] protected ResourceConsole console;
    [SerializeField] private TMP_Text text; 
    [SerializeField] private TMP_Text text2; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        maxEnergy = (int)energyLevel;
        StartCoroutine(DrainEnergy());
    }

    // Update is called once per frame
    void Update()
    {
        if (infiteEnergy) return;
        if(energyLevel==0)
        {
            if (once)
            {
                once = false;
                SoundManager.Play(SoundManager.Sounds.NotEnoughEnergy);
            }
            GameMaster.ChangeScoreBy(scoreNoEnergy);
        }

        if (energyLevel >= 0 && !once)
        {
            SoundManager.Stop(SoundManager.Sounds.NotEnoughEnergy);
            once = true;
        }
        
        CheckEnergyLevel();
        UpdateSprite();
        UpdateText();
    }

    public override void Trigger(int whichMethod)
    {
        if (whichMethod == 0) RefilEnergy();
        else if (whichMethod == 1) EjectShell();
    }

    private void RefilEnergy()
    {
        if (!console.isLoaded)
        {
            //Negative result?
            return;
        }

        if (console.DepleteResource())
        {
            energyLevel += energyRefillAmount;
            if (energyLevel >= maxEnergy) energyLevel = maxEnergy;
        }

    }

    private void EjectShell()
    {
        if (!console.isLoaded)
        {
            //TODO bad action
            return;
        }
        console.EjectObject();
        Debug.Log("Shell ejceted");

    }
    
    protected IEnumerator DrainEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDownBetweenEnergyDrain);
            if (energyLevel - energyDrain > 0) energyLevel -= energyDrain;
            else energyLevel = 0;
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

    public bool CheckIfEnoughEnergyForDrainThenDrain(float amountEnergy)
    {
        if (infiteEnergy) return true;
        bool enoughEnergy = false;
        if (energyLevel - amountEnergy > 0)
        {
            energyLevel -= amountEnergy;
            enoughEnergy = true;
        }

        return enoughEnergy;
    }

    private void UpdateText()
    {
        text.SetText("Energy: " +((int)energyLevel).ToString() + "/" + maxEnergy.ToString());
        text2.SetText("Energy: " +((int)energyLevel).ToString() + "/" + maxEnergy.ToString());
    }
    
}
