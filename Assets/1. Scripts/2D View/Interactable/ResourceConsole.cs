using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private Resource _holdedObject;


    public override void Interact(Player player)
    {
        if (isEjectingResource)
        {
            player.CarryObject(_holdedObject);
            _holdedObject.PickUpObject(false,byPassChecks:true);
            isLoaded = false;
            return;
        }
        
        if (_holdedObject != null) return;
        SoundManager.Play(SoundManager.Sounds.Interact);
        if (player.carriedObject.type != typeConsole) return;

        AcceptResource(player);
        _holdedObject = (Resource) player.TakeResource();
        _holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        _holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }
     
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (canNotBeInteractedWith) return;
        if (col.gameObject.CompareTag("Player")) PlayerEntersConsole(col.GetComponent<Player>());
        else if (col.gameObject.CompareTag("Object") &&
                 !isLoaded &&
                 col.transform.parent.GetComponentInChildren<Object>().type==typeConsole &&
                 !col.transform.parent.GetComponentInChildren<Object>().isCarried &&
                 col.gameObject.layer!=LayerMask.NameToLayer("ObjectLogic"))
        {
            _holdedObject = col.transform.parent.GetComponentInChildren<Resource>();
            _holdedObject.SetPosition(resourcePlace.transform.position);
            isLoaded = true;
            _holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            _holdedObject.PickUpObject(true);
            _holdedObject.transform.parent
                .transform.rotation = quaternion.Euler(Vector3.zero);
        }
        
    }
    
    private void AcceptResource(Player player)
    {
        player.carriedObject.transform.parent
            .transform.rotation = quaternion.Euler(Vector3.zero);
    }

    public bool DepleteResource()
    {
        return _holdedObject.UseAndCheckIfDepleted();
    }

    public void EjectObject()
    {
        if (_holdedObject == null) return;
        if (isEjectingResource)
        {
            SoundManager.Play(4);
            if (ejectDirection != Vector2.zero) _holdedObject.Eject(ejectDirection);
            else _holdedObject.Eject();
        }
        else
        {
            _holdedObject.DestroyIn(0);
        }

        NotHoldingAnymore();
    }

    private void NotHoldingAnymore()
    {
        isLoaded = false;
        _holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        _holdedObject = null;
    }

    public void PlaceResource(Resource newObject, bool canBePickedUp = false)
    {
        isLoaded = true;
        _holdedObject = newObject;
        newObject.PickUpObject(true);
        _holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }



}