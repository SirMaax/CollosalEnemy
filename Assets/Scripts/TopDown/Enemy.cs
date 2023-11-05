using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed;
    
    [Header("Behvaior")] 
    public BehaviorState state;
    private bool acting;

    [Header("Transition")]
    [Tooltip("Distance till enemy notices mech and will start to attack")]
    [SerializeField] private float distanceTillNoticing; 
    [SerializeField] private float disitacneTillAttacking;
    [SerializeField] private float timeBetweenAttacking;
    
    
    [Header("Refs")] 
    private MechMovement _mech;
    [SerializeField] private GameObject bulletPrefab;
    public enum BehaviorState
    {
        Walking,
        GoingInRange,
        Attacking,
    }
    // Start is called before the first frame update
    void Start()
    {
        _mech = GameObject.FindWithTag("Mech").GetComponentInChildren < MechMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((_mech.position - transform.position).magnitude);
        if (acting) return;
        
        switch (state)
        {
            case BehaviorState.Walking:
                Walking();
                break;
            case BehaviorState.GoingInRange:
                GoingInRange();
                break;
            case BehaviorState.Attacking:
                Attacking();
                break;
        }        
    }
    
    #region Walking State    
    /// <summary>
    /// Walking state
    /// - Random Movement           20%
    /// - Standing                  40%
    /// - Movement towards target   40%
    /// </summary>
    private void Walking()
    {
        if(CheckTransitionToStateGoingInRange())return;
        
        int whichAction = Random.Range(0, 101);
        switch (whichAction)
        {
            case < 20:
                RandomMovement();
                break;
            case < 60:
                // Standing();
                break;
            case < 101:
                MoveTowardsTarget();
                break;
        }
    }

    private void RandomMovement()
    {
        float howLongMoving = Random.Range(0.5f, 3f);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        StartCoroutine(MoveInDirectionForSeconds(new Vector2(x, y).normalized, howLongMoving));
    }

    private IEnumerator MoveInDirectionForSeconds(Vector2 dir, float time)
    {
        acting = true;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        int multiplier = 2;
        for (int i = 0; i < time*multiplier; i++)
        {
            yield return new WaitForSeconds(time / multiplier);
            transform.Translate( movementSpeed * Time.deltaTime* dir);
        }
        acting = false;
    }

    private void MoveTowardsTarget()
    {
        Vector2 dir = (_mech.position - transform.position).normalized;
        float howLongMoving = Random.Range(1f, 2f);
        StartCoroutine(MoveInDirectionForSeconds(dir, howLongMoving));
    }
    /// <summary>
    /// Check if next state is available
    /// </summary>
    /// <returns>If state was changed</returns>
    private bool CheckTransitionToStateGoingInRange()
    {
        if ((transform.position - _mech.position).magnitude < distanceTillNoticing)
        {
            state = BehaviorState.GoingInRange;
            return true;
        } 
        return false;
    }
    #endregion
    
    #region GoingInRange

    private void GoingInRange()
    {
        if(CheckTransitionToStateAttacking())return;
        MoveTowardsTarget();
    }

    private bool CheckTransitionToStateAttacking()
    {
        if ((transform.position - _mech.position).magnitude < disitacneTillAttacking)
        {
            state = BehaviorState.Attacking;
            return true;
        }
        else if ((transform.position - _mech.position).magnitude > distanceTillNoticing)
        {
            state = BehaviorState.Walking;
            return true;
        }
        return false;
    }
    #endregion
    
    #region Attacking

    private void Attacking()
    {
        StartCoroutine(AttackCooldown());
    }
    /// <summary>
    /// Split into 2 phases
    /// - Attack Cooldown
    /// - Attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCooldown()
    {
        acting = true;
        while (!CheckTransitionToStateGoingInRangeFromAttack())
        {
            yield return new WaitForSeconds(timeBetweenAttacking);
            Attack();
        }

        acting = false;
    }

    private void Attack()
    {
        Vector2 dir = (_mech.transform.position - transform.position).normalized;
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector3.up, dir))
            .GetComponent<Bullet>();
        bullet.SetAttributes(dir,Bullet.BulletType.enemy);
    }
    
    private bool CheckTransitionToStateGoingInRangeFromAttack()
    {
        if ((transform.position - _mech.position).magnitude > disitacneTillAttacking)
        {
            state = BehaviorState.GoingInRange;
            return true;
        }
        return false;
    }
    #endregion
    public void GetHit()
    {
        //Animation
        Debug.Log("Got Hit");
        if (health <= 0) Die();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Bullet")) return;
        if (col.GetComponent<Bullet>().type != Bullet.BulletType.player) return;
        GetHit();
    }

    private void Die()
    {
        //TriggerAnimation
        Destroy(gameObject);
    }
}
