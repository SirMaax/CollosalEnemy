using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float maximumFallSpeed;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float initialJumpForce;

    
    [Header("Fields")] 
    public Vector2 move;
    private bool grounded = false;
    private bool canJump = true;
    public bool jumpButtonPressed;
    private bool facingRight;
    
    [Header("Refs")] 
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckTransform;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        Move();
        Jump();
        Clamp();
    }

    public void Jump()
    {
        if (facingRight && move.x < 0)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;   
        }

        if (!facingRight && move.x > 0)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;   
        }
        
        if (!CanJump()) return;
        canJump = false;
        StartCoroutine(JumpAction());

    }

    public void Move()
    {
        if (move == Vector2.zero) return;

        rb.AddForce( Time.deltaTime *  movementSpeed *  move );
    }

    public void Clamp()
    {
        Vector2 vel = rb.velocity;
        if (math.abs(rb.velocity.x) >= maximumSpeed)  vel.x = maximumSpeed;
        if (math.abs(rb.velocity.y) >= maximumFallSpeed) vel.y = maximumFallSpeed;

        rb.velocity = vel;

    }

    private void CheckGrounded()
    {
        if (Physics2D.Raycast(groundCheckTransform.transform.position,
                Vector2.down, groundCheckDistance, groundMask)) TouchedGround();
        else grounded = false;
    }

    private bool CanJump()
    {
        bool eval = grounded && jumpButtonPressed && canJump;


        return eval;
    }

    private IEnumerator JumpAction()
    {
        grounded = false;
        SoundManager.Play(13);
        rb.AddForce(Vector2.up * initialJumpForce);
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void TouchedGround()
    {
        grounded = true;
        
    }

    
}
