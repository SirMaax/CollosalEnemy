using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float _attackRange;
    [SerializeField] private float _degrees;
    [SerializeField] private float _timeBetweenRotation;

    private void Update()
    {
        
        if(GetDistanceToMech() <= _attackRange)Debug.DrawLine(_mech.transform.position,transform.position,Color.green);
        else Debug.DrawLine(_mech.transform.position,transform.position,Color.red);
        State();
    }
    
    protected override void Attacking(bool moveToTarget = false)
    {
        if (_routine == null) _routine = StartCoroutine(AttackCooldown());
        RotateGxTowards(_mech.transform.position - transform.position);
        if (GetDistanceToMech() < _attackRange / 2) return;
        MoveTowardsTarget(noTime: true, percentSpeedReduce:0.75f );
    }

    protected override void Attack()
    {
        if(GetDistanceToMech() < _attackRange)_mech.transform.parent.GetComponent<Mech>().GetHit(damage:_damage);
        StartCoroutine(SpinAnimation());
        StartCoroutine(Cooldown());
    }

    private IEnumerator SpinAnimation()
    {
        float degrees = 0;
        while (degrees < 360)
        {
            yield return new WaitForSeconds(_timeBetweenRotation);
            transform.Rotate(Vector3.forward,_degrees);
            degrees += _degrees;
        }
        
    }
}
