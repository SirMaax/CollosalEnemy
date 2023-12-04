using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Score")] 
    [SerializeField] private float baseScore;
    
    [Header("Attributes")] 
    [SerializeField] private float activeEnergyDrain;
    
    [Header("Settings")] 
    [SerializeField] private bool doorUsed;

    [Header("Refs")] 
    [SerializeField] GroundButton buttonShot;
    [SerializeField] private ResourceHoldingPlace[] allConsoles;
    [SerializeField] private TMP_Text[] consoleText;
    [SerializeField] private TMP_Text cannonReadyText;
    [SerializeField] private Door door;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private MechCanon mechCanon;
    private EnergyCore core;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
    }

    // Update is called once per frame
    void Update()
    {
        Shot();
        if (!cannonReadyText.IsUnityNull())
        {
            bool atleastAmmoBoxLoaded = false;
            for (int i = 0; i < allConsoles.Length; i++)
            {
                atleastAmmoBoxLoaded = atleastAmmoBoxLoaded || allConsoles[i].isLoaded;
                if (consoleText[i].IsUnityNull()) continue;
                if(allConsoles[i].isLoaded)consoleText[i].
                    SetText("Ammoslot " + (i+1).ToString() + " loaded");
                else  consoleText[i].SetText("Ammoslot " + (i+1).ToString() + " not loaded");
            }
            foreach (var console in allConsoles)
            {
                atleastAmmoBoxLoaded = atleastAmmoBoxLoaded || console.isLoaded;
                
            }
            if(atleastAmmoBoxLoaded)cannonReadyText.SetText("Cannon is ready");
            else cannonReadyText.SetText("Cannon not ready");
        }
    }

    private void Shot()
    {
        if (!buttonShot.buttonWasPressed) return;
        buttonShot.buttonWasPressed = false;
        if (doorUsed && !door.isClosed) return;

        bool atleastAmmoBoxLoaded = false;
        foreach (var console in allConsoles)
        {
            atleastAmmoBoxLoaded = atleastAmmoBoxLoaded || console.isLoaded;
        }
        
        if (!atleastAmmoBoxLoaded) return;
        
        if (!core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain))
        {
            SoundManager.Play(7);
            return;
            
        }
        
        int counter = 0;
        foreach (var console in allConsoles)
        {
            if (!console.isLoaded) continue;
            counter += 1;
            console.DepleteResource();
            console.EjectShell();
        }

        mechCanon.Shoot();
    }
}
