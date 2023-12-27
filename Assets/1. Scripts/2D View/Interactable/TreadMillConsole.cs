using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreadMillConsole : Console
{
    [Header("TreadMill Settings")] private bool playerIsUsingConsole;
    [SerializeField] private float distanceFromCenter;
    [SerializeField] private float speed;
    private Animator animator;

    private List<MovementController> playerOnMill;

    // Update is called once per frame
    void Start()
    {
        // playerMovement = player.gameObject.GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        animator.speed = 0;
        playerOnMill = new List<MovementController>();
    }

    void Update()
    {
        if (playerIsUsingConsole)
        {
            int movementDirection = 0;
            List<int> playerIndividualMultiplier = new List<int>();
            foreach (var player in playerOnMill)
            {
                Vector3 playerPosition = player.GetPosition();
                int mulitplier = 0;

                if (player.move.x > 0)
                {
                    if (playerPosition.x < transform.position.x + distanceFromCenter) mulitplier = 1;
                    else mulitplier = -1;
                    movementDirection += 1;
                }
                else if (player.move.x < 0)
                {
                    if (playerPosition.x > transform.position.x - distanceFromCenter) mulitplier = -1;
                    else mulitplier = 1;
                    movementDirection -= 1;
                }

                playerIndividualMultiplier.Add(mulitplier);
            }
            
            foreach (var player in playerOnMill)
            {
                Vector3 playerPosition = player.GetPosition();
                int mulitplier = 0;
                int playerDiretion = playerIndividualMultiplier[0];
                playerIndividualMultiplier.RemoveAt(0);
                if(player.move.x == 0 && movementDirection!= 0)
                {
                    mulitplier = movementDirection > 0 ? 1 : -1;
                    mulitplier *= -1;
                }
                else if ((playerPosition.x < transform.position.x && movementDirection < 0) ||
                    playerPosition.x > transform.position.x && movementDirection > 0)
                {
                    //Treadmill and player moving same direction
                    mulitplier = playerDiretion;
                }
                else if(movementDirection != 0) 
                {
                    //Treadmill and player are not moving same direction
                    mulitplier = movementDirection > 0 ? 1 : -1;
                }

                
                player.TranslatePlayer(mulitplier * speed * Time.deltaTime, 0, 0);
            }

            if (movementDirection == 0) animator.speed = 0;
            else animator.speed = 1;
        }
    }

    protected override void PlayerEntersConsole(Player player)
    {
        playerOnMill.Add(player.GetComponent<MovementController>());
        playerIsUsingConsole = true;
        player.inputHandler.TogglePlayerIsTurningMech(true);
        animator.speed = 1;
    }

    protected override void PlayerLeavesConsole(Player player)
    {
        playerOnMill.Remove(player.GetComponent<MovementController>());
        if (playerOnMill.Count <= 0)
        {
            animator.speed = 0;
            playerIsUsingConsole = false;
        }
    }

    protected override void OnTriggerExit2D(Collider2D col)
    {
        PlayerLeavesConsole(col.GetComponent<Player>());
        col.GetComponent<Player>().inputHandler.TogglePlayerIsTurningMech(false,newState:-1);
        base.OnTriggerExit2D(col);
    }
}