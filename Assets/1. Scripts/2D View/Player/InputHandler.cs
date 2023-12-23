using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class InputHandler : MonoBehaviour
{
    [Header("Own Logic")] 
    private bool PlayerIsControllingMech;
    private bool playerIsTurningMech;
    
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
    private MechMovement _mechMovement;
    private MechCanon _mechCanon;
    [SerializeField]private MovementController _movement;
    private PlayerInput _playerInput;
    public InputAction inputAction;
    [SerializeField]private Player _player;
    
    
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
        GameObject mech = GameObject.Find("Mech");
        _mechMovement = mech.GetComponentInChildren<MechMovement>();
        _mechCanon = mech.GetComponentInChildren<MechCanon>();
        _playerInput = GetComponent<PlayerInput>();
    }
    
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnUse(InputValue value)
    {
        _player.Use();
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed) _movement.jumpButtonPressed = true;
        else _movement.jumpButtonPressed = false;
    }
    public void MoveInput(Vector2 newMoveDirection)
    {
        
        if (_playerInput.currentControlScheme != "Gamepad")
        {
            if (newMoveDirection.y > 0) _movement.jumpButtonPressed = true;
            else _movement.jumpButtonPressed = false;
        }
        
        if (PlayerIsControllingMech)
        {
            
            // _mechMovement.move[_player.GetPlayerId()] = newMoveDirection;
            _mechMovement.move = newMoveDirection;
            _movement.move = Vector2.zero;
            _movement.jumpButtonPressed = false;
            return;
        }
        if (playerIsTurningMech)
        {
            if (newMoveDirection.x > 0.9) newMoveDirection.x = 1;
            else if(newMoveDirection.x < -0.9)newMoveDirection.x = -1;
            
            if (_movement.jumpButtonPressed)
            {
                TogglePlayerIsTurningMech();
                return;
            }
            _mechCanon.move[_player.GetPlayerId()-1] = newMoveDirection;
            _movement.jumpButtonPressed = false;
        }
        
        //Remove Y component
        newMoveDirection.y = 0;
        
        _movement.move = newMoveDirection;
    }
    
    public void OnEscape(InputValue value)
    {
        GameMaster.ToggleGui();
    }

    public void TogglePlayerIsControllingMech()
    {
        if (PlayerIsControllingMech)
        {
            PlayerIsControllingMech = false;
            _mechMovement.move = Vector2.zero;
        }
        else
        {
            PlayerIsControllingMech = true;
        }
    }

    public void TogglePlayerIsTurningMech(bool removePlayerSpeed = false)
    {
        if (playerIsTurningMech)
        {
            playerIsTurningMech = false;
            _mechCanon.move[_player.GetPlayerId()-1] = Vector2.zero;
            _player.gameObject.GetComponent<MovementController>().canMove = true;
        }
        else
        {
            playerIsTurningMech = true;
            if(removePlayerSpeed)_player.rb.velocity = Vector2.zero;
            _player.gameObject.GetComponent<MovementController>().canMove = false;
            _movement.jumpButtonPressed = false;
            MoveInput(_movement.move);
        }
    }
}
