using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private EventSystem eventSystem;
    
    public void GetHit()
    {
        //Animation
        eventSystem.Attacked();
        // if (health <= 0) Die();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Bullet")) return;
        Bullet bullet = col.GetComponent<Bullet>();
        if (bullet.type != Bullet.BulletType.enemy) return;
        bullet.HitSomething();
        GetHit();
    }
}
