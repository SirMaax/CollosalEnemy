using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float disitacneTillAttacking;
    [SerializeField] private float timeBetweenAttacking;
    
    [Header("Behvaior")] 
    private BehaviorState state;
    private bool acting;

    [Header("Transition")]
    [Tooltip("Distance till enemy notices mech and will start to attack")]
    [SerializeField] private float distanceTillNoticing; 
    
    
    [Header("Refs")] 
    private MechMovement _mech; 
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
    /// - Random Movement           40%
    /// - Standing                  40%
    /// - Movement towards target   20%
    /// </summary>
    private void Walking()
    {
        if(CheckTransitionToStateGoingInRange())return;
        
        int whichAction = Random.Range(0, 101);
        switch (whichAction)
        {
            case < 40:
                RandomMovement();
                break;
            case < 80:
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
        float x = Random.Range(0, 1f);
        float y = Random.Range(0f, 1f);
        StartCoroutine(MoveInDirectionForSeconds(new Vector2(x, y), howLongMoving));
    }

    private IEnumerator MoveInDirectionForSeconds(Vector2 dir, float time)
    {
        acting = true;
        int multiplier = 2;
        for (int i = 0; i < time*multiplier; i++)
        {
            yield return new WaitForSeconds(time / multiplier);
            transform.Translate(dir * movementSpeed);
        }
        acting = false;
    }

    private void MoveTowardsTarget()
    {
        Vector2 dir = _mech.position - transform.position;
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
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacking);
            Attack();
        }
    }

    private void Attack()
    {
        
    }
    #endregion
    public void GetHit()
    {
        //Animation
        Debug.Log("Got Hit");
    }
}
