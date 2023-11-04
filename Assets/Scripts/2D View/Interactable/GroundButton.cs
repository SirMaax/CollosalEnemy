using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{
    public enum typeGroundButton
    {
        Elevator,
        Energy,
        Shield,
        JumpPad,
        Activation,
        
    }

    [Header("Attributes")] 
    [SerializeField] private float activationForce;
    [SerializeField] private float buttonCooldown;
    [SerializeField] private float buttonMovement;
    
    [SerializeField] private typeGroundButton type;
    [SerializeField] private float timeTillButtonWasNotPressedAnymore;
    [SerializeField] private float jumpPadForce;

    [Header("Elevator")] [SerializeField] private int level;
    [SerializeField] private Elevator _elevator;

    [Header("Status")] private bool buttonCaBePressed;
    public bool buttonWasPressed;

    [Header("Refs")] protected Player _player;
    

    // Start is called before the first frame update
    void Start()
    {
        buttonCaBePressed = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (-1 * player.rb.velocity.y >= activationForce && buttonCaBePressed) ButtonPressed(player);
        }
        else if (col.gameObject.CompareTag("Object"))
        {
            Jump(col.gameObject.GetComponentInChildren<GObject>());
        }
    }



    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
    }

    private void ButtonPressed(Player player)
    {
        SoundManager.Play(2);
        ButtonAnimation();
        switch (type)
        {
            case typeGroundButton.Elevator:
                ElevatorCall();
                break;
            case typeGroundButton.Energy:
                EnergyLevel();
                break;
            case typeGroundButton.Shield:
                Shield();
                break;
            case typeGroundButton.JumpPad:
                Jump(player);
                break;
            case typeGroundButton.Activation:
                
                StopCoroutine(ButtonReset());
                buttonWasPressed = true;
                StartCoroutine(ButtonReset());
                StartCoroutine(ButtonCooldown());
                break;
        }
    }

    public void ElevatorCall()
    {
        _elevator.GoToLevel(level);
    }

    private void EnergyLevel()
    {
        buttonWasPressed = true;
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

    private void Jump(GObject gObject)
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

    private void ButtonAnimation()
    {
        StartCoroutine(buttonAnimatio());
    }

    private IEnumerator buttonAnimatio()
    {
        transform.Translate(Vector3.down * buttonMovement);
        yield return new WaitForSeconds(0.3f);
        transform.Translate(Vector3.up * buttonMovement);
    }
}