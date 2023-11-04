using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechCanon : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float turnSpeed;

    [SerializeField] 
    public int turnTest;
    public bool turning;
    public bool test;
    public Vector2 move;

    private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (move != Vector2.zero)
        {
            TurnLeftOrRight(move.x * -1);
        }
    }

    public void TurnLeftOrRight(float multiplier)
    {
        // Quaternion quaternion = Quaternion.AngleAxis(currentAngle + multiplier * turnSpeed, Vector3.forward);
        transform.Rotate(Vector3.forward, turnSpeed * multiplier);
    }

    public void Shoot()
    {
        //Play animation
    }

    public void MechBodyRotated()
    {
        transform.rotation =  Quaternion.Inverse(transform.parent.rotation) * startRotation;
    }
}
