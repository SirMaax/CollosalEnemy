using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{

    
    
    [Header("Status")] 
    public bool isCarrying;
    public bool consoleNearby;
    public bool objectNearby;
    public Object carriedObject;
    private bool _isRepairing;


    [Header("Interaction")] 
    private Console nearestConsole;
    private List<Object> nearestObjects;

    [Header("Settings")] 
    [SerializeField] private float heavyMass;
    [SerializeField] private int playerId;
    private float startMass;
    
    
    [Header("Refs")]
    [SerializeField] private GameObject positionForCarryObject;
    [SerializeField] private Sprite[] sprites;
    [SerializeField]private MovementController _movement;
    public InputHandler inputHandler;
    public Rigidbody2D rb;

    [Header("Other")] 
    private Object _highlightedObject;
    private Console _hightlightedConsole;
    // Start is called before the first frame update
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        nearestObjects = new List<Object>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        startMass = rb.mass;

        playerId = GameMaster.AMOUNT_PLAYER;
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[playerId - 1];
        _movement.SetPlayerPosition(GameObject.FindWithTag("SpawnPoint").transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        objectNearby = nearestObjects.Count > 0;

        if (isCarrying) UpdateObjectPosition();
        HightlightClosestObjectOrConsole();
    }
    
    public void Use()
    {
        if (_isRepairing) return;
        // if (isCarrying) Drop();
        // else TryToPickUp();
        if (isCarrying && carriedObject.type == Object.typeObjects.RepairTool && consoleNearby &&
            nearestConsole.GetBrokenStatus()) StartRepairing();
        else if (consoleNearby && nearestConsole.buttonConsole) nearestConsole.PressButton();
        else if (consoleNearby && nearestConsole.controlConsole && !nearestConsole.isResourceConsole)
            InteractWithConsole();
        else if (consoleNearby && nearestConsole.controlConsole && (nearestConsole.isResourceConsole
                                                                    && !isCarrying)) InteractWithConsole();
        else if (isCarrying && consoleNearby)
        {
            if (nearestConsole.isResourceConsole && !((ResourceConsole)nearestConsole).isLoaded) InteractWithConsole();
            else Drop();

        }
        else if (isCarrying) Drop();
        else TryToPickUp();
    }
    
    private void Drop()
    {
        SoundManager.Play(12);
        carriedObject.DropObject(rb.velocity);
        isCarrying = false;
        ResetWeight();
        carriedObject = null;
    }
    public Object TakeResource()
    {
        isCarrying = false;
        ResetWeight();
        var temp = carriedObject;
        ThisIsNotAnymoreInPlayerPickUpRadius(carriedObject);
        carriedObject = null;
        return temp;
    }
    public void SetConsole(Console console)
    {
        nearestConsole = console;
        consoleNearby = true;
    }
    public void RemoveConsole(Console console)
    {
        if (nearestConsole != console) return;
        consoleNearby = false;
        nearestConsole = null;
    }

    public void ThisIsInPlayerPickUpRadius(Object gObject)
    {
        if (nearestObjects.Contains(gObject)) return; 
        nearestObjects.Add(gObject);
    }

    public void ThisIsNotAnymoreInPlayerPickUpRadius(Object gObject)
    {
        nearestObjects.Remove(gObject);
    }

    private void TryToPickUp()
    {
        if (!objectNearby) return;
        SoundManager.Play(11);
        //Get closest interactable object
        Object cloestObject = null;
        float smallestDistance = 10000;
        foreach (var ele in nearestObjects)
        {
            if (ele == null)
            {
                nearestObjects.Remove(ele);
                TryToPickUp();
                return;
            }
            float distance = (ele.transform.position - transform.position).magnitude;
            if (smallestDistance > distance)
            {
                smallestDistance = distance;
                cloestObject = ele;
            } 
        }
        
        //PickUp
        if (!cloestObject.canBePickedUp) return;
        if (!cloestObject.PickUpObject(false)) return;
        carriedObject = cloestObject;
        isCarrying = true;
        ChangeWeight();
    }

    private void InteractWithConsole()
    {
        nearestConsole.Interact(this);
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

    public void Disable(bool useStatus)
    {
        
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    public void CarryObject(Object newObject)
    {
        carriedObject = newObject;
        isCarrying = true;
        ChangeWeight();
    }

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }

    public void CancelledInput()
    {
        if (!_isRepairing) return;
        _isRepairing = false;
        nearestConsole.StoppedRepairing();
    }

    private void StartRepairing()
    {
        _isRepairing = true;
        nearestConsole.StartRepair();
    }

    public void SetIsRepairing(bool status)
    {
        _isRepairing = status;
    }

    public bool GetIsRepairing()
    {
        return _isRepairing;
    }

    private void HightlightClosestObjectOrConsole()
    {
        Object closestObject = null;
        float smallestDistance = math.INFINITY;
        foreach (var element in nearestObjects)
        {
            float distance = (rb.position - (Vector2)element.transform.position).magnitude;
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestObject = element;
            }
        }
        
        if (_hightlightedConsole != null) _hightlightedConsole.StopHighlight();
        if (_highlightedObject != null) _highlightedObject.StopHighlight();

        
        if (consoleNearby &&((Vector2)nearestConsole.transform.position - rb.position).magnitude < smallestDistance)
        {

            nearestConsole.Highlight();
            _hightlightedConsole = nearestConsole;
        }
        else if(closestObject!=null && !isCarrying)
        {


            closestObject.Highlight();
            _highlightedObject = closestObject;
        }

    }
} 
