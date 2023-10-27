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
    }
    
    [Header("Attributes")] 
    [SerializeField] private float activationForce;
    [SerializeField] private typeGroundButton type;
    [SerializeField] private float timeTillButtonWasNotPressedAnymore;
    
    [Header("Elevator")] 
    [SerializeField] private int level;
    [SerializeField] private Elevator _elevator;
    
    [Header("Status")] 
    private bool buttonCaBePressed;
    public bool buttonWasPressed;
    
    [Header("Refs")] 
    protected Player _player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        buttonCaBePressed = true;
    }

    // Update is called once per frame
    void Update()
    {
          
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        if (-1 *player.rb.velocity.y >= activationForce && buttonCaBePressed) ButtonPressed();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
           
    }

    private void ButtonPressed()
    {
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
}
