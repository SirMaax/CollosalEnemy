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
    private Resource holdedObject;
    
    public override void Interact(Player player)
    {
        if (holdedObject != null) return;
        SoundManager.Play(SoundManager.Sounds.Interact);
        if (player.carriedObject.type != typeConsole) return;

        AcceptResource(player);
        holdedObject = (Resource) player.TakeResource();
        holdedObject.SetPosition(resourcePlace.transform.position);
        isLoaded = true;
        holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }
     
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (canNotBeInteractedWith) return;
        if (col.gameObject.CompareTag("Player")) PlayerEntersConsole();
        else if (col.gameObject.CompareTag("Object") &&
                 !isLoaded &&
                 col.transform.parent.GetComponentInChildren<Object>().type==typeConsole &&
                 !col.transform.parent.GetComponentInChildren<Object>().isCarried &&
                 col.gameObject.layer!=LayerMask.NameToLayer("ObjectLogic"))
        {
            holdedObject = col.transform.parent.GetComponentInChildren<Resource>();
            holdedObject.SetPosition(resourcePlace.transform.position);
            isLoaded = true;
            holdedObject.transform.parent.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            holdedObject.PickUpObject(true);
            holdedObject.transform.parent
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