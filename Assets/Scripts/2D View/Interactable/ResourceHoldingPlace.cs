using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceHoldingPlace : Console
{
    [Header("Attributes")] [SerializeField]
    private GObject.typeObjects typeConsole;

    public bool isLoaded;
    public bool isEjectingResource = true;
    public bool sameLoadingSpace;
    [SerializeField] private Vector2 ejectDirection;

    [Header("Refs")] [SerializeField] public GameObject resourcePlace;

    private GObject holdedObject;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void Interact(Player player)
    {
        SoundManager.Play(8);
        switch (typeConsole)
        {
            case GObject.typeObjects.EnergyCell:
                EnergyConsole(player);
                break;
            case GObject.typeObjects.AmmoCrate:
                AmmoConsole(player);
                break;
        }
    }

    private void EnergyConsole(Player _player)
    {
        if (_player.carriedObject.type != GObject.typeObjects.EnergyCell) return;

        AcceptResource(_player);
        holdedObject = _player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }

    private void AmmoConsole(Player _player)
    {
        if (_player.carriedObject.type != GObject.typeObjects.AmmoCrate) return;

        AcceptResource(_player);
        holdedObject = _player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }

    private void AcceptResource(Player _player)
    {
        // _player.carriedObject.UsedWithConsole(typeConsole);
        _player.carriedObject.transform.parent
            .transform.rotation = quaternion.Euler(Vector3.zero);
    }

    public bool DepleteResource()
    {
        return holdedObject.UseAndCheckIfDepleted();
    }

    public void EjectShell()
    {
        if (isEjectingResource)
        {
            SoundManager.Play(4);
            if (ejectDirection != Vector2.zero) holdedObject.Eject(ejectDirection);
            else holdedObject.Eject();
        }
        else
        {
            holdedObject.ConsumeThis(0);
        }

        NotHoldingAnymore();
    }

    private void NotHoldingAnymore()
    {
        isLoaded = false;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        holdedObject = null;
    }

    public void PlaceResource(GObject newObject)
    {
        isLoaded = true;
        holdedObject = newObject;
        newObject.PickUp(true);
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }
}