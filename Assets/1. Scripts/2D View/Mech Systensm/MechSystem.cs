using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSystem : MonoBehaviour
{
    [Header("Attributes")] 
    protected bool _isBroken;
    protected bool _canBeBroken = true;
    
    [Header("References")]
    [SerializeField]private Sign _sign;

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
