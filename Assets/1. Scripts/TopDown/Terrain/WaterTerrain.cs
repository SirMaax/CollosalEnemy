using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTerrain : BaseTerrrain
{
    [SerializeField] private float _slowDownPercent = .5f;
    
    protected override void ApplyEffectToMech(bool isMech,GameObject gameObject)
    {
        if(isMech)gameObject.GetComponentInChildren<MechMovement>().ApplyEffects(true,slowDownPercent:_slowDownPercent);
        else gameObject.GetComponentInChildren<Enemy>().ApplyEffect(slowDownPercent:_slowDownPercent);
    }
    
    
}
