using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHoldingPlace : Console
{
    [Header("Attributes")] 
    [SerializeField] private GObject.typeObjects typeConsole;
    
    [Header("Refs")]
    [SerializeField] private GameObject resourcePlace;
    private GObject holdedObject; 
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        switch (typeConsole)
        {
            case GObject.typeObjects.EnergyCell:
                EnergyConsole();
                break;
            case GObject.typeObjects.AmmoCrate:
                AmmoConsole();
                break;

        }
    }

    private void EnergyConsole()
    {
        if (_player.carriedObject.type != GObject.typeObjects.EnergyCell) return;
           
        AcceptResource();
        holdedObject = _player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
    }

    private void AmmoConsole()
    {
        if (_player.carriedObject.type != GObject.typeObjects.AmmoCrate) return;
        
        holdedObject = _player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        AcceptResource();
    }
    
    private void AcceptResource()
    {
        _player.carriedObject.UsedWithConsole(typeConsole);

    }
}
