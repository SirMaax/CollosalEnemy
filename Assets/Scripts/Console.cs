using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public enum enumResource
    {
        Energy,
        Ammo,

    }
    
    [Header("Settings")]
    [SerializeField] private float interactionRadius;
    [SerializeField] private bool canNotBeInteractedWith;
 
    
    
    // Start is called before the first frame update
    void Start()
    {
        // CircleCollider2D collder;
        if (TryGetComponent(out CircleCollider2D collider2D)) collider2D.radius = interactionRadius;
        if (canNotBeInteractedWith)
        {
            collider2D.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // if (canNotBeInteractedWith) return;
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.SetConsole(this);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // if (canNotBeInteractedWith) return;
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.RemoveConsole();
    }

     public virtual void Interact(Player player)
    {
        Debug.Log("Was interacted with");
    }
    /**
     * Puts resource in the right place 
     */
     
}
