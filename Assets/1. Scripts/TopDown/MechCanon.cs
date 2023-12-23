using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MechCanon : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turnSpeedIncrease;
    [SerializeField] private float maxTurnSpeed;
    
    public Vector2[] move;
    private float baseTurnSpeed;  
    private Quaternion startRotation;
    private int lastDirection;
    
    [Header("Refs")] 
    [SerializeField] private GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector2[4];
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float sum = 0;
        foreach (var ele in move)
        {
            sum+= ele.x;
        }

        Debug.Log(sum);
        if (sum == 0)
        {
            turnSpeed = 0;
            return;
        }
        
        int direction = sum > 0 ? 1 : -1;
        if (lastDirection == direction) turnSpeed = Mathf.Clamp(turnSpeed + turnSpeedIncrease, 0, maxTurnSpeed);
        else turnSpeed = 0;
        
        lastDirection = direction;
        TurnLeftOrRight(sum * -1);
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
