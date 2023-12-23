using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool usesInteractionRadius;
    [SerializeField] private float interactionRadius;
    [SerializeField] protected bool canNotBeInteractedWith;
    [SerializeField] public bool buttonConsole;
    [SerializeField] public bool controlConsole;
    public bool wasPressed;
    public enum enumResource
    {
        Energy,
        Ammo,
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider2D = null;
        if(usesInteractionRadius) if (TryGetComponent(out collider2D)) ((CircleCollider2D)collider2D).radius = interactionRadius;
        if (canNotBeInteractedWith && usesInteractionRadius)
        {
            if (collider2D == null) return;
            collider2D.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

    }

    protected virtual void PlayerEntersConsole(Player player)
    {
        player.SetConsole(this);
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerEntersConsole(col.gameObject.GetComponent<Player>());
        
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        PlayerLeavesConsole();
        player.RemoveConsole(this);
    }

     public virtual void Interact(Player player) { }
     
     protected virtual void PlayerLeavesConsole() { }
     protected virtual void PlayerLeavesConsole(Player player) { }

     public virtual void PressButton()
     {
         wasPressed = true;
     }

     
}
