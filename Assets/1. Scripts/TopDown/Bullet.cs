using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int _damage;
    [SerializeField] private float speed;
    [SerializeField] private float _aoeAttackRadius;
    public BulletType type;
    private bool _firedByPlayer;
    private float timeAlive;
    private Vector2 direction;
    private bool isAboutToBeRemoved = false;

    
    
    public enum BulletType
    {
        explosion,
        shieldDisrupting,
        standardEnemyShot,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountdownLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        if (isAboutToBeRemoved) return;
        Vector2 test = speed * Time.deltaTime * direction;
        Vector2 current = transform.position;
        transform.position = current + test;
    }
    
    private IEnumerator CountdownLifetime()
    {
        yield return new WaitForSeconds(timeAlive);
        //ExplosionAnimation
        HitSomething();
    }

    public void SetAttributes(Vector2 dir, BulletType newType,float time, bool playerFired = false)
    {
        type = newType;
        direction = dir;
        timeAlive = time;
        this._firedByPlayer = playerFired;
    }

    public void HitSomething()
    {
        if (isAboutToBeRemoved) return;
        isAboutToBeRemoved = true;
        if (_firedByPlayer && _aoeAttackRadius > 0) AOEAttack();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        SoundManager.Play(SoundManager.Sounds.MechGotHit);
        Destroy(gameObject,3);
    }

    public void AOEAttack()
    {
        CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = _aoeAttackRadius;
        // circleCollider2D.isTrigger = true;        

        ContactFilter2D filter = new ContactFilter2D();
        List<Collider2D> contacts = new List<Collider2D>();
        circleCollider2D.OverlapCollider(filter,contacts);
        
        foreach (var contact in contacts)
        {
            if (!contact.CompareTag("Enemy")) return;
            Enemy enemy = contact.GetComponent<Enemy>();
            enemy.GetHit(this);
        }
    }

    public int GetDamage()
    {
        return _damage;
    }

    public bool WasFiredByPlayer()
    {
        return _firedByPlayer;
    }
}
