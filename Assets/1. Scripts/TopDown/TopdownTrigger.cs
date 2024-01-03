using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownTrigger : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private TopdownEvent _topdownEvent;
    [SerializeField] private TopdownEvent.EEvent _whatIsTriggered;
    [SerializeField] private bool _activatesOnce;
    [SerializeField] private bool _deleteEvent;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Mech")) return;
        _topdownEvent.TriggerEvent(_whatIsTriggered ,_deleteEvent);
        if (_activatesOnce) Destroy(gameObject);
    }
}
