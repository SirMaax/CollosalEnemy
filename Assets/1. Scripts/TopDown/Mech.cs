using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mech : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float health;

    [Header("Test")] 
    [SerializeField] private bool _infiniteHealth;
    
    [Header("Private")] private float maxHealth;

    [Header("References")] [SerializeField]
    private EventSystem eventSystem;

    [SerializeField] private Image slider;

    private void Start()
    {
        maxHealth = health;
    }

    public void GetHit(Bullet bullet = null, int damage = 0 )
    {
        eventSystem.Attacked();
        if(bullet!=null) UpdateHealth(-bullet.GetDamage());
        else UpdateHealth(damage);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Obstacle")) CollidedWithObstacle(); 
        if (!col.gameObject.CompareTag("Bullet")) return;
        Bullet bullet = col.GetComponent<Bullet>();
        if (bullet.WasFiredByPlayer()) return;
        bullet.HitSomething();
        GetHit(bullet);
    }

    public void UpdateHealth(float value)
    {
        if (_infiniteHealth) return;
        health += value;
        slider.fillAmount = health / maxHealth;
        if (health <= 0) Die();
    }

    private void Die()
    {
        //Restart Level    
        //Sound
        Menu.clearedLevel = false;
        Menu.LoadScoreScene();
    }

    private void CollidedWithObstacle()
    {
        //Necessary Speed?
        //Maybe take health dmg
        eventSystem.Attacked(environmentAttack:true,screenShakeIntensity:1);
    }

    
}