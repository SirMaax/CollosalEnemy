using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GObject : MonoBehaviour
{
    public enum typeObjects
    {
        EnergyCell,
        AmmoCrate
    }
    
    [Header("Status")] 
    private bool isInPickUpRange;
    public bool canPickUp;
    private bool isCarried = false;
    public typeObjects type;
    
    [Header("Refs")] 
    private Rigidbody2D rb;
    private BoxCollider2D physicalBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        transform.parent.GetComponent<BoxCollider2D>();
        canPickUp = true;
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.SetGOjbect(this);
        isInPickUpRange = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.RemoveGObject(this);
        isInPickUpRange = false;
    }

    public bool PickUp()
    {
        if (!canPickUp) return false;
        //TODO random rotation when picking stuff up?
        canPickUp = false;
        isInPickUpRange = false;
        isCarried = true;
        DisableInfluence();
        return true;
    }

    public void Drop()
    {
        canPickUp = true;
        isCarried = false;
        EnableInfluence();
    }

    private void DisableInfluence()
    {
        transform.parent.gameObject.isStatic = true;
        physicalBoxCollider.enabled = false;
    }

    private void EnableInfluence()
    {
        transform.parent.gameObject.isStatic = false;
        physicalBoxCollider.enabled = true;

    }

    public void SetPosition(Vector2 position)
    {
        transform.parent.position = position;
    }

    public void UsedWithConsole(typeObjects typeConsole)
    {
        switch (typeConsole)
        {
            case typeObjects.EnergyCell:
                //Nothing changes
                break;
            case typeObjects.AmmoCrate:
                ConsumeThis();
                break;
        }
    }

    private void ConsumeThis()
    {
        Destroy(transform.parent);
        Destroy(gameObject);
    }
    
    
}

