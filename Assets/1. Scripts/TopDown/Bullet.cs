using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float[] speeds;
    [SerializeField] private bool[] _hasAOE;
    [SerializeField] private float[] aoeAttackRadius;
    public BulletType type;
    public bool firedByPlayer;
    private float timeAlive;
    private Vector2 direction;
    private bool isAboutToBeRemoved = false;

    [Header("References")] 
    [SerializeField] private Sprite[] _sprites;
    
    
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
        Vector2 test = speeds[(int)type] * Time.deltaTime * direction;
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
        this.firedByPlayer = playerFired;
        
        GetComponent<SpriteRenderer>().sprite = _sprites[(int)type];
        if (!playerFired)
        {
            transform.localScale /= 2;
            GetComponentInChildren<ParticleSystem>().transform.localScale /= 3;
        }
    }

    public void HitSomething()
    {
        if (isAboutToBeRemoved) return;
        isAboutToBeRemoved = true;
        if (firedByPlayer && _hasAOE[(int)type]) AOEAttack();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        SoundManager.Play(SoundManager.Sounds.MechGotHit);
        Destroy(gameObject,3);
    }

    public void AOEAttack()
    {
        CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = aoeAttackRadius[(int)type];
        // circleCollider2D.isTrigger = true;        

        ContactFilter2D filter = new ContactFilter2D();
        List<Collider2D> contacts = new List<Collider2D>();
        circleCollider2D.OverlapCollider(filter,contacts);
        
        foreach (var contact in contacts)
        {
            if (!contact.CompareTag("Enemy")) return;
            Enemy enemy = contact.GetComponent<Enemy>();
            enemy.GetHit();
        }
    }
}
