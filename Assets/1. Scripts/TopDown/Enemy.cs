using System.Collections;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : BaseMech
{
    [Header("Attributes")]
    [SerializeField] private int health;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool _isShielded;
    
    [Header("Behvaior")] 
    public BehaviorState state;
    private bool isMoving;
    private bool acting;

    [Header("Behavior Toggling")] 
    [SerializeField] private bool allowRandomWalking;
    [SerializeField] private bool allowStanding;

    [Header("Transition")]
    [Tooltip("Distance till enemy notices mech and will start to attack")]
    [SerializeField] private float distanceTillNoticing; 
    [SerializeField] private float disitacneTillAttacking;
    [SerializeField] private float timeBetweenAttacking;
    
    [Header("Refs")] 
    private MechMovement _mech;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gx;
    
    [Header("Other")] 
    private Vector2 movementDirection;
    public enum BehaviorState
    {
        Walking,
        GoingInRange,
        Attacking,
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _mech = GameObject.FindWithTag("Mech").GetComponentInChildren < MechMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate( movementSpeed * Time.deltaTime* movementDirection);
        }
        
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
        int baseValue = 0;
        baseValue += !allowRandomWalking ? 20 : 0;
        baseValue += !allowStanding ? 40 : 0;
        
        int whichAction = Random.Range(baseValue, 101);
        switch (whichAction)
        {
            case < 20:
                RandomMovement();
                break;
            case < 60:
                //Standing();
                break;
            case < 101:
                MoveTowardsTarget();
                break;
        }
    }

    private void RandomMovement()
    {
        float howLongMoving = Random.Range(3f, 3f);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        movementDirection = new Vector2(x, y).normalized;
        RotateGxTowards(movementDirection);
        StartCoroutine(MoveInDirectionForSeconds(howLongMoving));
    }

    private IEnumerator MoveInDirectionForSeconds(float time)
    {
        acting = true;
        isMoving = true; 
        yield return new WaitForSeconds(time);
        acting = false;
        isMoving = false;
    }

    private void MoveTowardsTarget()
    {
        Vector2 dir = (_mech.position - transform.position).normalized;
        float howLongMoving = Random.Range(1f, 2f);
        movementDirection = dir;
        RotateGxTowards(movementDirection);
        StartCoroutine(MoveInDirectionForSeconds(howLongMoving));
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
            sign.ShowSign(Sign.SignType.Attacking,timeBetweenAttacking,flashing:true,flashFaster:true);
            yield return new WaitForSeconds(timeBetweenAttacking);
            Attack();
        }

        acting = false;
    }

    private void Attack()
    {
        Vector2 dir = (_mech.transform.position - transform.position).normalized;
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector2.up, dir))
            .GetComponent<Bullet>();
        bullet.SetAttributes(dir,Bullet.BulletType.explosion,1.5f);
        SoundManager.Play(SoundManager.Sounds.EnemyHit);
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
    public void GetHit(Bullet bullet)
    {
        
        if (_isShielded && bullet.type == Bullet.BulletType.shieldDisrupting)
        {
            SetShieldStatus(false);
            return;
        }
        health -= bullet.GetDamage();
        if (health <= 0) Die();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Bullet")) return;
        Bullet bullet = col.GetComponent<Bullet>();
        if (!bullet.WasFiredByPlayer()) return;
        bullet.HitSomething();
        GetHit(bullet);
    }

    private void Die()
    {
        //TriggerAnimation
        Destroy(gameObject);
    }

    private void RotateGxTowards(Vector2 rot)
    {
        gx.transform.rotation = Quaternion.FromToRotation(Vector3.down, rot);
    }

    private void SetShieldStatus(bool status)
    {
        if (status)
        {
            _isShielded = true;
        }
        else
        {
            _isShielded = false;
        }
        
    }
}
