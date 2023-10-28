using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GObject : MonoBehaviour
{
    public enum typeObjects
    {
        EnergyCell,
        AmmoCrate,
        EmptyCrate,
    }

    [Header("Attributes")] [SerializeField]
    private int uses;
    [SerializeField] private float minSpinForce;
    [SerializeField] private float maxSpinForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float minForce;
    

    [Header("Status")] private bool isInPickUpRange;
    public bool canPickUp;
    private bool isCarried = false;
    public typeObjects type;

    [Header("Refs")] public Rigidbody2D rb;
    private BoxCollider2D physicalBoxCollider;

    private Transform parent;

    // Start is called before the first frame update
    public void Start()
    {
        parent = transform.parent;
        rb = parent.GetComponent<Rigidbody2D>();
        physicalBoxCollider = parent.GetComponent<BoxCollider2D>();
        canPickUp = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.SetGOjbect(this);
        isInPickUpRange = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player") || isCarried) return;
        Player player = col.gameObject.GetComponent<Player>();
        player.RemoveGObject(this);
        isInPickUpRange = false;
    }

    public bool PickUp(bool pickedUpByStation)
    {
        if (!pickedUpByStation) if ( !canPickUp) return false;
        //TODO random rotation when picking stuff up?
        canPickUp = false;
        isInPickUpRange = false;
        isCarried = true;
        DisableInfluence();
        return true;
    }

    public void Drop()
    {
        canPickUp = true;
        isCarried = false;
        EnableInfluence();
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
    }

    public void SetPosition(Vector2 position)
    {
        parent.position = position;
    }

    public void UsedWithConsole(typeObjects typeConsole)
    {
        switch (typeConsole)
        {
            case typeObjects.EnergyCell:
                //Nothing changes
                break;
            case typeObjects.AmmoCrate:
                // ConsumeThis(2f);
                //Nothing changes

                break;
        }
    }

    public void ConsumeThis(float time)
    {
        Destroy(parent.gameObject, time);
        Destroy(gameObject, time);
    }

    /**
     * Return yes if depleted else no
     */
    public bool UseAndCheckIfDepleted()
    {
        if (uses <= 0) return false;
        uses -= 1;
        if (uses <= 0)  ChangeCrateTypeToEmpty();
        return true;
    }

    public void Eject()
    {
        Drop();
        ApplyForceInDirection(Vector2.zero);
    }

    private void ChangeCrateTypeToEmpty()
    {
        type = typeObjects.EmptyCrate;
        //TODO change sprite etc.
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
}