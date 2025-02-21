using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldSystems : MechSystem
{
    [Header("Attributes")] 
    [SerializeField] private float activeEnergyDrain;
    [SerializeField] private bool _usesOnlyOneButton;
    
    [Header("State")] 
    public bool shieldActive;
    private int howManyButtonsPressed;
    
    
    [Header("Refs")] 
    [SerializeField] private EnergyCore core;
    [SerializeField] private TMP_Text shieldStatus;
    [SerializeField] private ParticleSystem _particleSystem;
    private GameMaster GM;
    private Coroutine timeTillButtonsReset;
    private List<GameObject> buttons;
    private void Awake()
    {
        shieldActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldActive) core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain);
    }

    private void ToggleShield()
    {
        if (_isBroken) return; 
        if (shieldActive)
        {
            if(shieldStatus!=null) shieldStatus.SetText("Shield off");
            shieldActive = false;
            _particleSystem.Stop();
        }
        else
        {
            if(shieldStatus!=null) shieldStatus.SetText("Shield on");
            shieldActive = true;
            _particleSystem.Play();
        }
        
    }
    
    public void Trigger(GameObject gameObject)
    {
        if (_usesOnlyOneButton)
        {
            ToggleShield();
            return;
        }
        if (buttons.Contains(gameObject)) return;
        buttons.Add(gameObject);
        if (timeTillButtonsReset != null)
        {
            timeTillButtonsReset = StartCoroutine(StartCountdownForAllButtons());
        }

        howManyButtonsPressed += 1;

        if (howManyButtonsPressed == GameMaster.AMOUNT_PLAYER)
        {
            ToggleShield();
            StopCoroutine(timeTillButtonsReset);
            howManyButtonsPressed = 0;
            buttons.Clear();
        }
    }

    /**
     * 
     */
    private IEnumerator StartCountdownForAllButtons()
    {
        yield return new WaitForSeconds(1);
        howManyButtonsPressed = 0;
        buttons.Clear();
    }

    protected override void StopAllAction()
    {
        shieldActive = false;
        if(shieldStatus!=null) shieldStatus.SetText("Shield off");
        _particleSystem.Stop();
    }
}
