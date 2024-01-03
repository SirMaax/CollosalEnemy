using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Mech")) return;
        PickedUp(col.gameObject);
        Destroy(gameObject);
    }

    protected virtual void PickedUp(GameObject mech)
    {
        
    }
    
}
