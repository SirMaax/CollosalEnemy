using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool _instantlyStart;
    [SerializeField] private int _damage;
    [SerializeField] private float _timeTillStriking;
    [SerializeField] private float _collisionRadius;
    
    [Header("References")]
    [SerializeField] private Sign _sign;
    private ParticleSystem _particleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_instantlyStart)StartCoroutine(TimeTillAirStrike());
    }

    public void StartAirStrike(float time = -1, float radius = -1, int dmg = -1)
    {
        _damage = dmg == -1 ? _damage : dmg;
        _timeTillStriking = time == -1 ? _timeTillStriking : time;
        _collisionRadius = radius == -1 ? _collisionRadius : radius;

        StartCoroutine(TimeTillAirStrike());
    }
 

    private IEnumerator TimeTillAirStrike()
    {
        _sign.ShowSign(Sign.SignType.AirStrike,_timeTillStriking,flashFaster:true,destroyAfterwards:true);
        yield return new WaitForSeconds(_timeTillStriking + 0.1f);
        if(_particleSystem!=null)_particleSystem.Play();
        CheckCollision();
    }

    private void CheckCollision()
    {
        bool hitMechOnce = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _collisionRadius);
        foreach (var element in colliders)
        {
            if (element.gameObject.CompareTag("Mech") && !hitMechOnce)
            {
                hitMechOnce = true;
                element.gameObject.GetComponent<Mech>().GetHit(damage:_damage);       
            }
            else if(element.gameObject.CompareTag("Enemy"))
            {
                element.gameObject.GetComponent<Enemy>().GetHit(damage:_damage);
            }
        }
    }
}
