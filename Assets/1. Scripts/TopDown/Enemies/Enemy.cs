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
    [SerializeField] protected int _damage;

    [Header("Behvaior")] public BehaviorState state;
    private bool isMoving;
    private bool acting;

    [Header("Behavior Toggling")] 
    [SerializeField] private bool allowRandomWalking;
    [SerializeField] private bool allowStanding;

    [Header("Transition")] 
    [Tooltip("Distance till enemy notices mech and will start to attack")] 
    [SerializeField] private float distanceTillNoticing;
    [SerializeField] protected float disitacneTillAttacking;
    [SerializeField] protected float _attackChargeUpTime;
    [SerializeField] protected float _timeBetweenAttacking;
    
    [Header("Refs")] protected MechMovement _mech;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject gx;

    [Header("Other")] 
    private Vector2 movementDirection;
    private float _startMovementSpeed;
    protected Coroutine _routine;

    public enum BehaviorState
    {
        Walking,
        GoingInRange,
        Attacking,
    }

    // Start is called before the first frame update
    protected void Start()
    {
        _mech = GameObject.FindWithTag("Mech").GetComponentInChildren<MechMovement>();
        _startMovementSpeed = movementSpeed;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        State();
    }

    protected virtual void State()
    {
        if (isMoving)
        {
            transform.Translate(movementSpeed * Time.deltaTime * movementDirection);
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
    protected virtual void Walking()
    {
        if (CheckTransitionToStateGoingInRange()) return;
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

    protected virtual void RandomMovement()
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

    protected virtual void MoveTowardsTarget(bool noTime = false, float percentSpeedReduce = 1)
    {
        Vector2 dir = (_mech.position - transform.position).normalized;
        float howLongMoving = Random.Range(1f, 2f);
        movementDirection = dir;
        RotateGxTowards(movementDirection);
        if(noTime)transform.Translate(movementSpeed/percentSpeedReduce * Time.deltaTime * movementDirection);
        else StartCoroutine(MoveInDirectionForSeconds(howLongMoving));
    }

    /// <summary>
    /// Check if next state is available
    /// </summary>
    /// <returns>If state was changed</returns>
    protected virtual bool CheckTransitionToStateGoingInRange()
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

    protected virtual void GoingInRange()
    {
        if (CheckTransitionToStateAttacking()) return;
        MoveTowardsTarget();
    }

    protected virtual bool CheckTransitionToStateAttacking()
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

    protected virtual void Attacking(bool moveToTarget = false)
    {
        
        if (_routine == null) _routine = StartCoroutine(AttackCooldown());
        float distance = (_mech.transform.position - transform.position).magnitude;
        RotateGxTowards(_mech.transform.position - transform.position);
        if (distance > disitacneTillAttacking && distance < disitacneTillAttacking * 1.2f && moveToTarget)
        {
            MoveTowardsTarget(noTime: true, percentSpeedReduce:2/3);
        }
        else if (distance > disitacneTillAttacking * 1.2f)
        {
            state = BehaviorState.Walking;
            StopCoroutine(_routine);
        }
    }

    /// <summary>
    /// Split into 2 phases
    /// - Attack Cooldown
    /// - Attacking
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackCooldown()
    {
        // acting = true;
        // while (!CheckTransitionToStateGoingInRangeFromAttack())
        // {
        float time = Time.time;
        sign.ShowSign(Sign.SignType.Attacking, _attackChargeUpTime - 0.5f, flashing: true, flashFaster: true);
        yield return new WaitForSeconds(_attackChargeUpTime);
        Debug.Log(Time.time - time );
        Attack();
        _routine = null;

    }

    protected virtual void Attack()
    {
        Vector2 dir = (_mech.transform.position - transform.position).normalized;
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector2.up, dir))
            .GetComponent<Bullet>();
        bullet.SetAttributes(dir, Bullet.BulletType.explosion, 1.5f);
        SoundManager.Play(SoundManager.Sounds.EnemyHit);
        StartCoroutine(Cooldown());
    }

    protected virtual bool CheckTransitionToStateGoingInRangeFromAttack()
    {
        if ((transform.position - _mech.position).magnitude > disitacneTillAttacking)
        {
            state = BehaviorState.GoingInRange;
            return true;
        }

        return false;
    }

    #endregion

    public virtual void GetHit(Bullet bullet = null, int damage = 0)
    {
        if (_isShielded && bullet.type == Bullet.BulletType.shieldDisrupting)
        {
            SetShieldStatus(false);
            return;
        }

        if (bullet != null) health -= bullet.GetDamage();
        else health -= damage;
        
        if (health <= 0) Die();
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Bullet")) return;
        Bullet bullet = col.GetComponent<Bullet>();
        if (!bullet.WasFiredByPlayer()) return;
        bullet.HitSomething();
        GetHit(bullet);
    }

    protected  virtual void Die()
    {
        //TriggerAnimation
        Destroy(gameObject);
    }

    protected void RotateGxTowards(Vector2 rot)
    {
        gx.transform.rotation = Quaternion.FromToRotation(Vector3.down, rot);
    }

    protected void SetShieldStatus(bool status)
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

    public void ApplyEffect(bool resetEffect = false, float slowDownPercent = -1)
    {
        if (resetEffect) ResetEffects();
        if (slowDownPercent != -1)
        {
            movementSpeed = _startMovementSpeed/ slowDownPercent;
        }
    }
    
    public void ResetEffects()
    {
        movementSpeed = _startMovementSpeed;
    }

    protected float GetDistanceToMech()
    {
        return (transform.position - _mech.position).magnitude;
    }

    protected IEnumerator Cooldown(float waitTime = -1)
    {
        acting = true;
        float time = waitTime == -1 ? _timeBetweenAttacking : waitTime;
        yield return new WaitForSeconds(time);
        acting = false;
    }
}