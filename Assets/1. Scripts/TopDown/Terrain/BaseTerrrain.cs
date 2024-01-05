using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseTerrrain : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private bool _canAffectEnemies;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sp;
        if (TryGetComponent<SpriteRenderer>(out sp)) sp.enabled = false;
    }
    
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Mech"))
        {
            ApplyEffectToMech(true,col.gameObject);
            
        }
        else if (_canAffectEnemies && col.gameObject.CompareTag("Enemy"))
        {
            ApplyEffectToMech(false,col.gameObject);
        }
    }
    
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Mech"))
        {
            ApplyExitEffect(true,col.gameObject);
        }
        else if (_canAffectEnemies && col.gameObject.CompareTag("Enemy"))
        {
            ApplyExitEffect(false,col.gameObject);
        }
    }
    
    
    protected virtual void ApplyEffectToMech(bool isMech, GameObject gameObject){}

    protected virtual void ApplyExitEffect(bool isMech, GameObject gameObject)
    {
        if(isMech)gameObject.GetComponentInChildren<MechMovement>().ResetEffects(); 
        else gameObject.GetComponentInChildren<Enemy>().ResetEffects();
    }
    
}
