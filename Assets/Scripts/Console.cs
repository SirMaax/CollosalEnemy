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

    [Header("Refs")] 
    protected Player _player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.SetConsole(this);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.RemoveConsole();
    }

     public virtual void Interact()
    {
        Debug.Log("Was interacted with");
    }
    /**
     * Puts resource in the right place 
     */
     
}
