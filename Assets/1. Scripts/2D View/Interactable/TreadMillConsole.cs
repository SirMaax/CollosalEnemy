using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreadMillConsole : Console
{
    [Header("TreadMill Settings")]
    private bool playerIsUsingConsole;
    [SerializeField] private float distanceFromCenter;
    [SerializeField] private float speed;
    private MovementController playerMovement;
    private Animator animator;
    // Update is called once per frame
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<MovementController>();
        // playerMovement = player.gameObject.GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        animator.speed = 0;

    }
    void Update()
    {
        if (playerIsUsingConsole)
        {
            Vector3 playerPosition = playerMovement.GetPosition();
            int mulitplier = 0;
            
            if (playerMovement.move.x > 0)
            {
                if (playerPosition.x < transform.position.x + distanceFromCenter ) mulitplier = 1;
                else mulitplier = -1;
            }
            else if(playerMovement.move.x < 0)
            {
                if (playerPosition.x > transform.position.x - distanceFromCenter ) mulitplier = -1;
                else mulitplier = 1;
            }
            playerMovement.TranslatePlayer(mulitplier * speed * Time.deltaTime,0,0 );
            
            if(playerMovement.move.x ==0)animator.speed = 0;
            else animator.speed = 1;
        }
    }

    protected override void PlayerEntersConsole()
    {
        playerIsUsingConsole = true;
        player.inputHandler.TogglePlayerIsTurningMech(true);
        animator.speed = 1;
        //Start animation
    }

    protected override void PlayerLeavesConsole()
    {
        // player.inputHandler.TogglePlayerIsTurningMech();
        animator.speed = 0;
        playerIsUsingConsole = false;
        //end animation
    }
}