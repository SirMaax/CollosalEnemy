using UnityEngine;


public class MechMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] AnimationCurve movementSpeedBehavir;
    [SerializeField] private AnimationCurve slowDownCurve;
    [SerializeField] private float StepSpeed;
    [SerializeField] private float maxTurningSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private bool _isTurningInMoveDirection;
    private float currentMovementSpeed;
    private float currentStepOnCurve;
    private bool acceleratingOrDecelerating;
    private bool isUsingTranslateForMovement = false;
    
    [Header("Vars")] 
    private Quaternion rotation;
    private Vector2 lastInput;
    private Vector2 slowDownInput;
    public Vector2 move;
    private int slowDown;
    public Vector3 position;
    [Header("Refs")] 
    private MechCanon _mechCanon;
// Start is called before the first frame update
    void Start()
    {
        _mechCanon = transform.parent.GetComponentInChildren<MechCanon>();
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.parent.position;
        MoveInDirection(move);
    }

    public void MoveInDirection(Vector2 input)
    {
        if (input == Vector2.zero && lastInput != Vector2.zero)
        {
             slowDown = 1;
             input = lastInput;
        }
        else
        {
            slowDown = 0;
        }
        CalculateMovementSpeed();
        if(_isTurningInMoveDirection) transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, input)) 
        , maxTurningSpeed); 
        
        if (input != lastInput && input != Vector2.zero && slowDown == 0)
        {
            ChangedDirection(input);
            lastInput = input;
        }
        
        CalculateMovementSpeed();
        if (isUsingTranslateForMovement) input = transform.rotation * input; 
            transform.parent.position = (Vector2)position + (currentMovementSpeed * Time.deltaTime * input);
        // _mechCanon.MechBodyRotated();
    }

    private void CalculateMovementSpeed()
    {
        
        int multiplier = slowDown!= 0 ? -1 : 1;
        currentStepOnCurve = Mathf.Clamp(currentStepOnCurve + (multiplier *StepSpeed), 0, 1);
        
        if (slowDown != 0) currentMovementSpeed = slowDownCurve.Evaluate(currentStepOnCurve) * maxMovementSpeed; 
        else currentMovementSpeed = movementSpeedBehavir.Evaluate(currentStepOnCurve) * maxMovementSpeed;
        // currentMovementSpeed = movementSpeedBehavir.Evaluate(currentStepOnCurve) * maxMovementSpeed;
        
        if (currentStepOnCurve == 1) acceleratingOrDecelerating = false;
        else if (slowDown != 0 && currentStepOnCurve <= maxMovementSpeed - maxMovementSpeed / slowDown)
            ReachedSlowdownPoint();
    }
    /// <summary>
    /// Mech changed direction the higher slowdown the lower the actual slowdown
    /// </summary>
    private void ChangedDirection(Vector2 input)
    {
        float result = Vector2.Dot(input, lastInput);
        if (result <= 0.5 && currentStepOnCurve >= 0.5f) currentStepOnCurve = 0.5f;
        else if (result <= 0.75 && currentStepOnCurve >= 0.75) currentStepOnCurve = 0.75f;
        else if (result <= 0.85 && currentStepOnCurve >= 0.85) currentStepOnCurve = 0.85f;
    }

    private void ReachedSlowdownPoint()
    {
        slowDown = 0;
        
    }

    public void StartMovement(Vector2 newMovement, bool useTranslate=false)
    {
        isUsingTranslateForMovement = useTranslate;
        move = newMovement;
    }

    public void StopMovement()
    {
        move = Vector2.zero;
        isUsingTranslateForMovement = false;
    }

    public void Rotate(float direction)
    {
        direction *= -1;
        float rotationAmount = direction * _rotationSpeed * Time.deltaTime;
        float currentRotation = transform.rotation.eulerAngles.z;
        float newRotation = (currentRotation + rotationAmount) % 360f;
        newRotation = (newRotation + 360f) % 360f;
        // transform.Rotate(Vector3.forward, direction *_rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }
}
