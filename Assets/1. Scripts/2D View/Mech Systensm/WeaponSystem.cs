using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MechSystem
{
    
    [Header("Attributes")] 
    [SerializeField] protected float activeEnergyDrain;
    [SerializeField] protected Bullet.BulletType _whichBulletUsed;
    [SerializeField] protected bool _playAnimationAndParticleSystem;
    
    [Header("Refs")] 
    [SerializeField] private string[] _allAnimations;
    [SerializeField] private ResourceConsole[] allConsoles;
    [SerializeField] private TMP_Text[] consoleText;
    [SerializeField] private TMP_Text cannonReadyText;
    [SerializeField] protected MechCanon mechCanon;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;
    protected EnergyCore core;
    
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<EnergyCore>();
    }

    public override void Trigger(int whichMethod=-1)
    {
        if (_isBroken) return;
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

    protected virtual void Shot()
    {
        if (!AtleastOneAmmoBoxLoaded()) return;
        if (!core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain, playSound:true)) return;
        
        UnloadAllConsoles();
        
        mechCanon.Shoot(_whichBulletUsed);
        PlayAnimation();
    }

    protected virtual bool AtleastOneAmmoBoxLoaded()
    {
        bool atleastAmmoBoxLoaded = false;
        foreach (var console in allConsoles)
        {
            atleastAmmoBoxLoaded = atleastAmmoBoxLoaded || console.isLoaded;
        }
        return atleastAmmoBoxLoaded;
    }

    protected virtual void UnloadAllConsoles()
    {
        foreach (var console in allConsoles)
        {
            if (!console.isLoaded) continue;
            console.DepleteResource();
            console.EjectObject();
        }
    }

    protected virtual void PlayAnimation()
    {
        if (_playAnimationAndParticleSystem)
        {
            _animator.Play(_allAnimations[(int)_whichBulletUsed]); 
            _particleSystem.Play();
        }
    }
    
}
