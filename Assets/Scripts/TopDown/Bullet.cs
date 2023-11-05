using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Attributes")] 
    private Vector2 direction;
    [SerializeField] private float speed;
    public float timeAlive;
    public BulletType type;

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
        Vector2 test = speed * Time.deltaTime * direction;
        // transform.Translate( speed * Time.deltaTime * direction);
        Vector2 current = transform.position;
        transform.position = current + test;
    }
    
    private IEnumerator CountdownLifetime()
    {
        yield return new WaitForSeconds(timeAlive);
        //ExplosionAnimation
        Destroy(gameObject);
    }

    public void SetAttributes(Vector2 dir, BulletType newType)
    {
        direction = dir;
        type = newType;
    }
}
