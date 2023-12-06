using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceConsole : Console
{
    [Header("Attributes")] [SerializeField]
    private Object.typeObjects typeConsole;
    public bool isLoaded;
    public bool isEjectingResource = true;
    public bool sameLoadingSpace;
    [SerializeField] private Vector2 ejectDirection;

    [Header("Refs")] 
    [SerializeField] public GameObject resourcePlace;
    private Resource holdedObject;
    
    public override void Interact(Player player)
    {
        if (holdedObject != null) return;
        SoundManager.Play(SoundManager.Sounds.Interact);
        switch (typeConsole)
        {
            case Object.typeObjects.EnergyCell:
                EnergyConsole(player);
                break;
            case Object.typeObjects.AmmoCrate:
                AmmoConsole(player);
                break;
        }
    }
    private void EnergyConsole(Player player)
    {
        if (player.carriedObject.type != Object.typeObjects.EnergyCell) return;

        AcceptResource(player);
        holdedObject = (Resource) player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }

    private void AmmoConsole(Player player)
    {
        if (player.carriedObject.type != Object.typeObjects.AmmoCrate) return;

        AcceptResource(player);
        holdedObject = (Resource) player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }

    private void AcceptResource(Player player)
    {
        player.carriedObject.transform.parent
            .transform.rotation = quaternion.Euler(Vector3.zero);
    }

    public bool DepleteResource()
    {
        return holdedObject.UseAndCheckIfDepleted();
    }

    public void EjectObject()
    {
        if (holdedObject == null) return;
        if (isEjectingResource)
        {
            SoundManager.Play(4);
            if (ejectDirection != Vector2.zero) holdedObject.Eject(ejectDirection);
            else holdedObject.Eject();
        }
        else
        {
            holdedObject.DestroyIn(0);
        }

        NotHoldingAnymore();
    }

    private void NotHoldingAnymore()
    {
        isLoaded = false;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        holdedObject = null;
    }

    public void PlaceResource(Resource newObject)
    {
        isLoaded = true;
        holdedObject = newObject;
        newObject.PickUpObject(true);
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }
}