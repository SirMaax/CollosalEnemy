using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteSystem : MonoBehaviour
{
    [Header("Refs")] 
    [SerializeField]private ResourceGiver giver;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Object"))
        {
            giver.IncreaseNumberEmptyCrates();
            col.GetComponentInChildren<GObject>().ConsumeThis(1f);
        }
        
    }
}
