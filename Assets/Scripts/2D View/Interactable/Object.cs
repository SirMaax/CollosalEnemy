using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    
    [Header("State")] public typeObjects type;
    
    public bool canBePickedUp;
    private bool isInPickUpRange;
    private bool isCarried = false;
    
    [Header("Ejection Force")]
    [SerializeField] private float minSpinForce;
    [SerializeField] private float maxSpinForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float minForce;

    [Header("Refs")] 
    public Rigidbody2D rb;
    private BoxCollider2D physicalBoxCollider;
    private Transform parent;
    
    public enum typeObjects
    {
        EnergyCell,
        AmmoCrate,
        EmptyCrate,
    }
    
    public void Start()
    {
        parent = transform.parent;
        rb = parent.GetComponent<Rigidbody2D>();
        physicalBoxCollider = parent.GetComponent<BoxCollider2D>();
        canBePickedUp = true;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.ThisIsInPlayerPickUpRadius(this);
        isInPickUpRange = true;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.ThisIsNotAnymoreInPlayerPickUpRadius(this);
        isInPickUpRange = false;
    }
    
    public bool PickUpObject(bool isPickedUpByContainer)
    {
        if (!isPickedUpByContainer && !canBePickedUp) return false;
        canBePickedUp = false;
        isInPickUpRange = false;
        isCarried = true;
        DisableInfluence();
        return true;
    }
    
    public void DropObject(Vector2 velcoity)
    {
        canBePickedUp = true;
        isCarried = false;
        EnableInfluence();
        rb.velocity = velcoity * 1.25f;
    }
    
    private void DisableInfluence()
    {
        rb.simulated = false;
        physicalBoxCollider.enabled = false;
    }
    
    private void EnableInfluence()
    {
        rb.simulated = true;
        physicalBoxCollider.enabled = true;
        StartCoroutine(ChangeLayer());
    }
    
    public void SetPosition(Vector2 position)
    {
        parent.position = position;
    }

    public virtual void UsedWithConsole(){ }
    
    public void DestroyIn(float time)
    {
        Destroy(parent.gameObject, time);
        Destroy(gameObject, time);
    }
    
    
    
    public void Eject()
    {
        DropObject(Vector2.zero);
        ApplyForceInDirection(Vector2.zero);
    }
    
    public void Eject(Vector2 direction)
    {
        DropObject(Vector2.zero);
        ApplyForceInDirection(direction);
    }
    
    public void ApplyForceInDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            float x = Random.Range(-100, 100);
            float y = Random.Range(-100, 100);
            Vector2 dir = new Vector2(x, y);
            dir.Normalize();
            rb.AddForce(dir * Random.Range(minForce, maxForce));
        }
        else
        {
            rb.AddForce(direction * Random.Range(minForce, maxForce));
        }

        rb.AddTorque(Random.Range(minSpinForce,maxSpinForce));
    }
    
    private IEnumerator ChangeLayer()
    {
        transform.parent.gameObject.layer = 12;
        yield return new WaitForSeconds(0.3f);
        transform.parent.gameObject.layer = 8;
    }
    
}
