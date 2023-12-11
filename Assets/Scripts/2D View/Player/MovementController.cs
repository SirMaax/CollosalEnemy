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
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float maxDegree;

    [Header("Fields")] 
    public Vector2 move;
    private bool grounded = false;
    private bool canJump = true;
    public bool jumpButtonPressed;
    private bool facingRight;
    private float speed;
    public bool canMove = true;
    private Vector2 lastPosition;
    private Quaternion baseRotation;
    
    [Header("Refs")] 
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private GameObject rotatingPoint;
    [SerializeField]private ParticleSystem particleSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        facingRight = true;
        baseRotation = rotatingPoint.transform.rotation;
        rb = transform.parent.GetComponent<Rigidbody2D>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = (lastPosition - (Vector2)transform.parent.position).magnitude * Time.deltaTime;
        lastPosition = rb.position;

        ParticleSystemUpdate();
        CheckGrounded();
        Move();
        Jump();
        Clamp();
        RotateSprite();
    }

    public void Jump()
    {
        
        
        if (!CanJump()) return;
        canJump = false;
        StartCoroutine(JumpAction());

    }

    public void Move()
    {
        if (move == Vector2.zero || !canMove ) return;

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
        SoundManager.Play(SoundManager.Sounds.Jump);
        rb.AddForce(Vector2.up * initialJumpForce);
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void TouchedGround()
    {
        grounded = true;
    }

    private void RotateSprite()
    {
        if (!grounded) return;
        
        // Debug.Log(speed);
        // if (speed == 0 && canMove)
        // {
        //     rotatingPoint.transform.rotation = baseRotation;
        //     Debug.Log("Reset Rotation");
        //     return;
        // }
        
        float angle = Quaternion.Angle(Quaternion.Euler(Vector3.up), rotatingPoint.transform.rotation);
        if (move.x == 0)
        {
            if (angle < 1.5)
            {
                rotatingPoint.transform.rotation = baseRotation;
                return;
            }

            float dotProduct = Vector3.Dot(Vector3.right, rotatingPoint.transform.up);
            int direction = 0;
            if (dotProduct < 0) direction = -1;
            if (dotProduct > 0) direction = 1;
            //If on treadmill return faster to natural pose
            if(!canMove) rotatingPoint.transform.Rotate(new Vector3(0,0,direction), rotatingSpeed*2);
            else rotatingPoint.transform.Rotate(new Vector3(0,0,direction), rotatingSpeed/3);
            return;
        }
        if (angle > maxDegree) return;
        
        int dir = facingRight ? -1 : 1;
        rotatingPoint.transform.Rotate(new Vector3(0,0,dir), rotatingSpeed);
        Debug.Log("Rotated");
    }

    public void TranslatePlayer(float x, float y, float z)
    {
        transform.parent.Translate(x,y,z);
    }

    public Vector2 GetPosition()
    {
        return rb.position;
    }

    private void ParticleSystemUpdate()
    {
        float particleAmount = Mathf.Lerp(0, 25, rb.velocity.magnitude);
        var emission = particleSystem.emission;
        if(!canMove && move.x != 0) emission.rateOverTime = 25;
        else if (!grounded) emission.rateOverTime = 0;
        else  emission.rateOverTime = (int)particleAmount;
        
        if (facingRight && move.x < 0)
        {
            facingRight = false;
            particleSystem.transform.rotation =new Quaternion(-0.594704449f, 0.382526636f, -0.382526636f, 0.594704449f);
        }
        
        if (!facingRight && move.x > 0)
        {
            facingRight = true;
            particleSystem.transform.rotation  = new Quaternion(-0.597565055f, -0.37804234f, 0.382526636f, 0.594704449f);
        }
    }
}
