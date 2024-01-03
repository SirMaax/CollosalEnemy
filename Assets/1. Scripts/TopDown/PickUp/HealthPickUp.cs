using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp
{
    [Header("Settings")] 
    [SerializeField] private int _healthIncrease;

    protected override void PickedUp(GameObject mech)
    {
        mech.GetComponent<Mech>().UpdateHealth(_healthIncrease);
    }
}
