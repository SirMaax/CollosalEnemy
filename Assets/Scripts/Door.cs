using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Attrbites")] 
    [SerializeField] private float doorSpeed;
    [SerializeField] private float closedDoorPosition;
    [SerializeField] private float openDoorPosition;
    
    [Header("Status")] 
    public bool isClosed;

    [Header("Refs")] 
    [SerializeField] private Lever lever;
     
    // Start is called before the first frame update
    void Start()
    {
        openDoorPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lever.status == 1) OpenDoor();
        else if (lever.status == 0) CloseDoor();
    }

    private void OpenDoor()
    {
        if (!isClosed) return;
        Vector2 dir = doorSpeed * Time.deltaTime * Vector2.up;
        if ((dir.y + transform.position.y) > openDoorPosition)
        {
            dir.y = openDoorPosition;
            transform.position = dir;
        }
        else
        {
            transform.Translate(dir);    
        }
    }

    private void CloseDoor()
    {
        if (isClosed) return;
        Vector2 dir = doorSpeed * Time.deltaTime * Vector2.down;
        if ((dir.y + transform.position.y) < closedDoorPosition)
        {
            dir.y = closedDoorPosition;
            transform.position = dir;
        }
        else
        {
            transform.Translate(dir);    
        }
    }
    
    
}
