using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Status")] 
    public bool isCarrying;
    public bool consoleNearby;
    public bool objectNearby;
    public GObject carriedObject;
    
    [Header("Interaction")] 
    private Console nearestConsole;
    private List<GObject> nearestObjects;

    [Header("Settings")] 
    [SerializeField] private float heavyMass;
    private float startMass;
    
    [Header("Refs")]
    [SerializeField] private GameObject positionForCarryObject;
    private MovementController _movement;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        nearestObjects = new List<GObject>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        startMass = rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        objectNearby = nearestObjects.Count > 0;

        if (isCarrying) UpdateObjectPosition();
    }
    /**
     * Is called via event
     */
    public void Interact()
    {
        //Which action?
        //    3.Pick Up
        //    2.Drop
        //    1.Interact with Console/etc
        if (isCarrying && consoleNearby) InteractWithConsole();
        else if (isCarrying) Drop();
        else TryToPickUp();
        
    }

    private void Drop()
    {
        carriedObject.Drop();
        isCarrying = false;
        ResetWeight();
        carriedObject = null;
    }
    public GObject TakeResource()
    {
        isCarrying = false;
        ResetWeight();
        var temp = carriedObject;
        carriedObject = null;
        return temp;
    }
    public void SetConsole(Console console)
    {
        nearestConsole = console;
        consoleNearby = true;
    }
    public void RemoveConsole()
    {
        consoleNearby = false;
        nearestConsole = null;
    }

    public void SetGOjbect(GObject gObject)
    {
        //Shoudlnt happen but still
        if (nearestObjects.Contains(gObject)) return; 
        nearestObjects.Add(gObject);
    }

    public void RemoveGObject(GObject gObject)
    {
        nearestObjects.Remove(gObject);
    }

    private void TryToPickUp()
    {
        if (!objectNearby) return;
        
        //Get closest interactable object
        GObject cloestObject = null;
        float smallestDistance = 10000;
        foreach (var ele in nearestObjects)
        {
            float distance = (ele.transform.position - transform.position).magnitude;
            if (smallestDistance > distance)
            {
                smallestDistance = distance;
                cloestObject = ele;
            } 
        }
        
        //PickUp
        if (!cloestObject.canPickUp) return;
        if (!cloestObject.PickUp()) return;
        carriedObject = cloestObject;
        isCarrying = true;
        ChangeWeight();
    }

    private void InteractWithConsole()
    {
        nearestConsole.Interact();
    }

    private void ChangeWeight()
    {
        rb.mass = heavyMass;
    }

    private void ResetWeight()
    {
        rb.mass = startMass;
    }

    private void UpdateObjectPosition()
    {
        carriedObject.transform.parent.position = positionForCarryObject.transform.position;
    }

    
} 
