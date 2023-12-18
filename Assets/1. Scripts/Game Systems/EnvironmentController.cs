using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Refs")] 
    private GameMaster gm;

    [Header("Attributes")]
    [SerializeField] private bool disableEnvironment;
    [SerializeField] private bool test;
    [SerializeField] private float minSpinForce;
    [SerializeField] private float maxSpinForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float minForce;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindWithTag("GM").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false; 
            ApplyEffectFrom(new (10,20));
        }
    }

    public void ApplyEffectFrom(Vector2 origin)
    {
        if (disableEnvironment) return;
        SoundManager.Play(SoundManager.Sounds.MechGotHit);
        //Players
        for (int i = 0; i < GameMaster.AMOUNT_PLAYER; i++)
        {
            Vector2 forceDirection = gm.players[i].transform.position; 
            ApplyForceInDirection(forceDirection,
                gm.players[i].GetComponentInChildren<Rigidbody2D>(),false);
        }
        
        //Objects
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject ele in objects)
        {
            Vector2 forceDirection = ele.transform.position;
            ApplyForceInDirection(forceDirection,
                ele.GetComponentInChildren<Rigidbody2D>(),true);
        }
    }
    
    private void ApplyForceInDirection(Vector2 direction,Rigidbody2D rb, bool isObject)
    {
        if (disableEnvironment) return;
        if (direction == Vector2.zero)
        {
            float x = Random.Range(-100, 100);
            float y = Random.Range(-100, 100);
            Vector2 dir = new Vector2(x, y);
            dir.Normalize();
            rb.AddForce(dir * Random.Range(minForce, maxForce));
        }
        else
        {
            direction.Normalize();
            rb.AddForce(direction * Random.Range(minForce, maxForce));
        }
        if(isObject) rb.AddTorque(Random.Range(minSpinForce,maxSpinForce));
    }
}
