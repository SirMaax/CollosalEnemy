using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownEvent : MonoBehaviour
{
    public enum EEvent
    {
        DoorTrigger
    }
    

    public void TriggerEvent(EEvent eEvent, bool delteThisAfterwards = false)
    {
        switch ((int)eEvent)
        {
            case 0:
                Debug.Log("Yes u did it. You saved the world!");
                break;        
            
        }
        if (delteThisAfterwards) Destroy(gameObject);


    }
}
