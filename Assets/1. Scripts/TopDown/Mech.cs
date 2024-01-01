using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mech : MonoBehaviour
{
    [Header("Attributes")] [SerializeField]
    private float health;

    [Header("Private")] private float maxHealth;

    [Header("References")] [SerializeField]
    private EventSystem eventSystem;

    [SerializeField] private Image slider;

    private void Start()
    {
        maxHealth = health;
    }

    private void GetHit(Bullet bullet)
    {
        eventSystem.Attacked();
        UpdateHealth(-bullet.GetDamage());
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

    private void UpdateHealth(float value)
    {
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