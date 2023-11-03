using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Lever : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float maxDegreeLeftAndRight;
    [SerializeField] private float leverPushSpeedAngle;
    [Tooltip("Int 0 = Off; -1 = left; +1 = right")]
    [SerializeField] public int status;
    [SerializeField] private float angleTillLeverActive;
    [Header("Refs")] 
    [SerializeField] private GameObject leverPart;

    [Header("Others")]
    private float angle;
  

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        MovementController player = col.gameObject.GetComponent<MovementController>();
        LeverIsPushed(player);
    }

    private void LeverIsPushed(MovementController player)
    {
        //Is player left or right
        Vector2 move = player.move;
        bool left = (player.transform.position.x < transform.position.x);
        if (left && move.x > 0) ApplyForce(-1); //Pushing from left
        else if (!left && move.x < 0) ApplyForce(1); //Pushing from right
    }

    private void ApplyForce(int dir)
    {
        // Quaternion quat = Quaternion.AngleAxis(leverPushSpeedAngle, new Vector3(0, 0, dir));
        // Vector3 rot = leverPart.transform.rotation
        bool pushingSameDirectionAsLeverIs = dir != status;
        angle = Quaternion.Angle(Quaternion.Euler(Vector3.up), leverPart.transform.rotation);
        if (angle > maxDegreeLeftAndRight)
        {
            if (status != dir)
            {
            CalculateStatus();
            return;
            }
        }
        leverPart.transform.Rotate(new Vector3(0,0,dir),leverPushSpeedAngle);
        CalculateStatus();
    }

    private void CalculateStatus()
    {
        // bool farEnoughLeftOrRight = (math.abs(transform.position.x - leverPart.transform.position.x) > distanceTillLeverActive);
        bool farEnoughAngle = angle > angleTillLeverActive;
        bool leverIsRightSide = transform.position.x > leverPart.transform.position.x;
        if (!farEnoughAngle) status = 0;
        else if (leverIsRightSide) status = 1;
        else status = -1;
    }
}
