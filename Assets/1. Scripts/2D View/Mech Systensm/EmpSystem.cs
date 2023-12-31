using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpSystem : WeaponSystem
{
    // Start is called before the first frame update
    void Start()
    {
        _playAnimationAndParticleSystem = false;
    }
    
    protected override void Shot()
    {
        if (!core.CheckIfEnoughEnergyForDrainThenDrain(activeEnergyDrain, playSound:true)) return;
        
        mechCanon.Shoot(_whichBulletUsed);
        // PlayAnimation();
    }
    
    
}
