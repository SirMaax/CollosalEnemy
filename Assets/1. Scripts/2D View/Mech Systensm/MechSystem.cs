using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSystem : MonoBehaviour
{
    [Header("Attributes")] 
    protected bool _isBroken;
    
    [Header("References")]
    [SerializeField]private Sign _sign;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void Trigger(int whichMethod = -1){}
    
    public void SetIsBrokenStatus(bool newStatus)
    {
        _isBroken = newStatus; 
        if (_isBroken)
        {
            _sign.ShowSign(Sign.SignType.IsBroken,flashing:true);
            StopAllAction();
        }
        else
        {
            _sign.HideSign();   
        }
    }

    protected virtual void StopAllAction(){}

    public bool GetIsBroken()
    {
        return _isBroken;
    }
}
