using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechCanon : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float turnSpeed;

    [SerializeField] public int turnTest;
    public bool turning;
    public bool test;

    private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (turning)
        {
            TurnLeftOrRight(turnTest == -1);
        }

        if (test)
        {
            test = false;
            MechBodyRotated();
        }
    }

    public void TurnLeftOrRight(bool left)
    {
        int multiplier = left ? -1 : 1;

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
