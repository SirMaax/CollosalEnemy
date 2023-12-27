using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    
    public enum EnumWhatIsTriggered
    {
        MechForwardsAndBackwards,
        MechTurnLeftAndRight
    }
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Trigger(EnumWhatIsTriggered which, float status)
    {
        switch ((int)which)
        {
            case 0: MechForwardsAndBackwards(status);
                break;
            case 1: TurnMechRotation(status);
                break;
            
        }
    }

    private void MechForwardsAndBackwards(float status)
    {
        MechMovement movement = GameObject.FindWithTag("Mech").GetComponentInChildren<MechMovement>();
        if (status == 0)movement.StopMovement();
        else movement.StartMovement(new Vector2(status,0),useTranslate:true);
    }

    private void TurnMechRotation(float status)
    {
        MechMovement movement = GameObject.FindWithTag("Mech").GetComponentInChildren<MechMovement>();
        movement.Rotate(status);
    }
}
