using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SlidingDoor : MechSystem
{
    [Header("Attribute")]
    [SerializeField] private bool _isVerticalDoor;
    [SerializeField] private bool _closeAtStart;
    [SerializeField] private float _doorSpeed;
    
    [SerializeField] private float _closePosition;
    private Vector3 _openPosition;
    private bool isClosed = false;
    private int _doorMovementStatus = 0; //0 Is nothing. // 1 is moving to the right // -1 is moving to the left
    private int _closingDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _canBeBroken = false;
        _openPosition = transform.position;
        if(_isVerticalDoor)_closingDirection = _closePosition < _openPosition.y  ? -1 : 1;
        else _closingDirection = _closePosition < _openPosition.x ? -1 : 1;
        
        if (_closeAtStart)
        {
            Vector3 position = transform.position;
            if (_isVerticalDoor) position.y = _closePosition;
            else position.x = _closePosition;
            isClosed = true;
            transform.position = position;
        }
    }

    private void Update()
    {
        if (_doorMovementStatus==0) return;
        Vector3 direction = _isVerticalDoor ? Vector3.up : Vector3.right;
        
        if (_doorMovementStatus == -1)direction = (_doorSpeed * Time.deltaTime * _closingDirection * direction);
        else direction= (_doorSpeed * Time.deltaTime * direction);
        transform.position += direction;

        CheckIfDoorChangesStatus();
    }

    public override void Trigger(int whichMethod = -1)
    {
        if (_doorMovementStatus != 0)_doorMovementStatus *= -1;
        else if (isClosed) _doorMovementStatus = 1;
        else _doorMovementStatus = -1;
    }

    public void CheckIfDoorChangesStatus()
    {
        Vector3 position = transform.position;
        
        if (_doorMovementStatus == 1 && (position.x > _openPosition.x && !_isVerticalDoor )|| (_isVerticalDoor&& position.y > _openPosition.y))
        {
            
            position = _openPosition;
            isClosed = false;
        }
        else if (_doorMovementStatus == -1 && ((position.y < _closePosition && _isVerticalDoor) || (!_isVerticalDoor&& position.x < _closePosition)))
        {
            if (_isVerticalDoor) position.y = _closePosition;
            else position.x = _closePosition;
            isClosed = true;
        }
        else return;

        transform.position = position;
        _doorMovementStatus = 0;
    }
    
}
