using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechCanon : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turnSpeedIncrease;
    [SerializeField] private float maxTurnSpeed;
    
    public Vector2 move;
    private float baseTurnSpeed;  
    private Quaternion startRotation;
    private int lastMoveDirection;
    
    [Header("Refs")] 
    [SerializeField] private GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastMoveDirection != move.x || move == Vector2.zero) turnSpeed = 0;
        else  turnSpeed = Mathf.Clamp(turnSpeed + turnSpeedIncrease, 0, maxTurnSpeed);
        
        lastMoveDirection = (int)move.x;
        
        if (move != Vector2.zero)
        { 
            TurnLeftOrRight(move.x * -1);
        }
        
    }

    public void TurnLeftOrRight(float multiplier)
    {
        transform.Rotate(Vector3.forward, turnSpeed * multiplier);
    }

    public void Shoot()
    {
        Vector2 dir = transform.rotation * (new Vector2(0, 1));
        dir.Normalize();
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector2.up, dir))
            .GetComponent<Bullet>();
        bullet.SetAttributes(dir,Bullet.BulletType.player,1.5f);
        
        SoundManager.Play(SoundManager.Sounds.EnemyHit);
    }

    public void MechBodyRotated()
    {
        transform.rotation =  Quaternion.Inverse(transform.parent.rotation) * startRotation;
    }
}
