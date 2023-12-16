using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float speed;
    private float timeAlive;
    public BulletType type;
    private Vector2 direction;
    private bool isAboutToBeRemoved = false;
    [SerializeField] private float aoeAttackRadius;
    
    public enum BulletType
    {
        enemy,
        player,
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

    public void SetAttributes(Vector2 dir, BulletType newType,float time)
    {
        direction = dir;
        type = newType;
        timeAlive = time;
        if (newType == BulletType.enemy)
        {
            transform.localScale /= 2;
            GetComponentInChildren<ParticleSystem>().transform.localScale /= 3;
        }
    }

    public void HitSomething()
    {
        if (isAboutToBeRemoved) return;
        isAboutToBeRemoved = true;
        if (type == BulletType.player) AOEAttack();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        SoundManager.Play(SoundManager.Sounds.MechGotHit);
        Destroy(gameObject,3);
    }

    public void AOEAttack()
    {
        CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = aoeAttackRadius;
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
