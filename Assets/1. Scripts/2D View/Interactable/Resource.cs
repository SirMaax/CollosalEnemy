using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Object
{
    [Header("Attributes")]
    [SerializeField] private int uses = 1;
    
    [Header("Refs")] 
    [SerializeField] private Sprite empty;

    public override void UsedWithConsole()
    {
        
    }
    
    private void ChangeCrateTypeToEmpty()
    {
        type = typeObjects.EmptyCrate;
        transform.parent.GetComponent<SpriteRenderer>().sprite = empty;
    }
    
    public bool UseAndCheckIfDepleted()
    {
        if (uses <= 0) return false;
        uses -= 1;
        if (uses <= 0)  ChangeCrateTypeToEmpty();
        return true;
    }
}
