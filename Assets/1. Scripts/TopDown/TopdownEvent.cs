using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownEvent : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameObject _openDoor; 
    [SerializeField] private GameObject _closeDoor;
    
    
    public enum EEvent
    {
        OpenDoor,
        CloseDoor,
        OpenDoorAndCloseDoor,
    }
    

    public void TriggerEvent(EEvent eEvent, bool deleteThisAfterwards = false)
    {
        switch (eEvent)
        {
            case EEvent.OpenDoor:
                //OpenDoor
                _openDoor.SetActive(false);
                break;        
            case EEvent.CloseDoor:
                //OpenDoor
                _closeDoor.SetActive(true);
                break;            
            case EEvent.OpenDoorAndCloseDoor:
                //OpenDoor
                _openDoor.SetActive(false);
                _closeDoor.SetActive(true);
                break;
            
        }
        if (deleteThisAfterwards) Destroy(gameObject);


    }

    
}
