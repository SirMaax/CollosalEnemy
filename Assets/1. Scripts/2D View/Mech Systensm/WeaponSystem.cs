using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MechSystem
{
    
    [Header("Attributes")] 
    [SerializeField] private float activeEnergyDrain;

    [Header("Refs")] 
    [SerializeField] private ResourceConsole[] allConsoles;
    [SerializeField] private TMP_Text[] consoleText;
    [SerializeField] private TMP_Text cannonReadyText;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private MechCanon mechCanon;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    private EnergyCore core;
    
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
    }

    public override void Trigger(int whichMethod=-1)
    {
        Shot();
    }

    // Update is called once per frame
    void Update()
    {
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

        bool atleastAmmoBoxLoaded = false;
        foreach (var console in allConsoles)
        {
            atleastAmmoBoxLoaded = atleastAmmoBoxLoaded || console.isLoaded;
        }
        
        if (!atleastAmmoBoxLoaded) return;
        
        if (!core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain))
        {
            SoundManager.Play(SoundManager.Sounds.NoEnergyLeft);
            return;
        }
        
        foreach (var console in allConsoles)
        {
            if (!console.isLoaded) continue;
            console.DepleteResource();
            console.EjectObject();
        }
        mechCanon.Shoot();
        _animator.Play("CannonShoting");
        _particleSystem.Play();
    }
    
    
}
