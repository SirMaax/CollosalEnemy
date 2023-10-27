using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
    
    [Header("Refs")] 
    private MovementController _movement;
    public InputAction inputAction;
    private Player _player;
    
    //How does a function to take input look like?
    public void OnNAMEofPart(InputValue value)
    {
        OtherFunction(value.Get<Vector3>());
        
    }
    
    public void OtherFunction(Vector3 vec)
    {
        //In combination with this function you specifiy why input is feed to the next function
    }
    //--------------------------------------------//


    

    
    private void Start()
    {
        _movement = GetComponent<MovementController>();
        _player = GetComponent<Player>();
    }
    
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
        
    }

    public void OnJump(InputValue value)
    {
        _movement.Jump();
    }

    public void OnInteract(InputValue value)
    {
        _player.Interact();
    }    
    
    // public void OnJumpRelease(InputValue value)
    // {
    //     _movement.StopJump();
    // }

    public void MoveInput(Vector2 newMoveDirection)
    {
        if (newMoveDirection[0] == 0 && newMoveDirection[1] == 0)
        {
            // sprintToggle = false;
            // sprint = false;
        }
        
        if (newMoveDirection.y > 0) _movement.jumpButtonPressed = true;
        else _movement.jumpButtonPressed = false;
        
        //Remove Y component
        newMoveDirection.y = 0;
        _movement.move = newMoveDirection;
    }


}
