using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        State();
    }

    protected override void State()
    {
        Attacking();
    }

    protected override void Attacking(bool moveToTarget = false)
    {
        float distance = (_mech.transform.position - transform.position).magnitude;
        if (distance < disitacneTillAttacking)
        {
            if (_routine == null) _routine = StartCoroutine(AttackCooldown());
            RotateGxTowards(_mech.transform.position - transform.position);
        }
        else if (distance > disitacneTillAttacking * 1.1f)
        {
         if(_routine!=null)StopCoroutine(_routine);
        }
    }
}
