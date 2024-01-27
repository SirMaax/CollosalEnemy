using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : Console
{


    [Header("Attributes")] 
    [SerializeField] private float _buttonAnimationSpeed;
    [SerializeField] private int whichTriggeredMethod = -1;
    [SerializeField] private float activationForce;
    [SerializeField] private float buttonCooldown;
    [SerializeField] private float buttonMovement;
    
    [SerializeField] private typeGroundButton type;
    [SerializeField] private float timeTillButtonWasNotPressedAnymore;
    [SerializeField] private float jumpPadForce;


    [Header("Status")] 
    private bool buttonCaBePressed = true;
    public bool buttonWasPressed;

    [Header("Refs")] 
    [SerializeField] private GameObject _buttonHead;
    [SerializeField] private MechSystem activatedSystem;
    protected Player _player;
    
    public enum typeGroundButton
    {
        Elevator,
        Energy,
        Shield,
        JumpPad,
        Activation,
        ResourceAmmo,
        ResourceEnergy,
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (_isBroken) return;
        if (col.gameObject.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (-1 * player.rb.velocity.y >= activationForce && buttonCaBePressed) ButtonPressed(player);
        }
        else if (col.gameObject.CompareTag("Object"))
        {
            Jump(col.gameObject.GetComponentInChildren<Object>());
        }
    }
    
    protected override void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
    }

    private void ButtonPressed(Player player)
    {
        SoundManager.Play(2);
        StartCoroutine(ButtonAnimation());
        if (type == typeGroundButton.JumpPad)
        {
            Jump(player);
        }
        else if (type == typeGroundButton.Shield)
        {
            ShieldSystems shields = (ShieldSystems)activatedSystem;
            shields.Trigger(transform.gameObject);
        }
        else activatedSystem.Trigger(whichTriggeredMethod);
        
    }
    
    private void Shield()
    {
        StopCoroutine(ButtonReset());
        buttonWasPressed = true;
        StartCoroutine(ButtonReset());
    }

    private IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(timeTillButtonWasNotPressedAnymore);
        buttonWasPressed = false;
    }

    private void Jump(Player player)
    {
        Vector2 vel = player.rb.velocity;
        vel.y = 0;
        player.rb.velocity = vel;
        player.rb.AddForce(Vector2.up * jumpPadForce);
    }

    private void Jump(Object gObject)
    {
        Vector2 vel = gObject.rb.velocity;
        vel.y = 0;
        gObject.rb.velocity = vel;
        gObject.rb.AddForce(Vector2.up * jumpPadForce);
    }

    IEnumerator ButtonCooldown()
    {
        buttonCaBePressed = false;
        yield return new WaitForSeconds(buttonCooldown);
        buttonCaBePressed = true;
    }
    
    private IEnumerator ButtonAnimation()
    {
        _buttonHead.transform.Translate(Vector3.down * buttonMovement);
        yield return new WaitForSeconds(_buttonAnimationSpeed);
        _buttonHead.transform.Translate(Vector3.up * buttonMovement);
    }
}