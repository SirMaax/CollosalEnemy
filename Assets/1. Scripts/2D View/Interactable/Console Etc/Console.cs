using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Console : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private bool usesInteractionRadius;
    [SerializeField] private float interactionRadius;
    [SerializeField] protected bool canNotBeInteractedWith;
    [SerializeField] public bool buttonConsole;
    [SerializeField] public bool controlConsole;
    [SerializeField] public bool isResourceConsole;
    protected bool _isBroken;
    public bool wasPressed;
    private EConsoleType _type;
    private float _repairProgress = 0;
    private float _amountPlayerRepairing = 0;
    private Coroutine _repairRoutine;
    [SerializeField] private bool test;

    [Header("References")] [SerializeField]
    protected Sign _sign;

    public enum enumResource
    {
        Energy,
        Ammo,
    }

    public enum EConsoleType
    {
        buttonConsole,
        controlConsole,
        resourceConsole,
        repairConsole,
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            SetIsBrokenStatus(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider2D = null;
        if (usesInteractionRadius)
            if (TryGetComponent(out collider2D))
                ((CircleCollider2D)collider2D).radius = interactionRadius;
        if (canNotBeInteractedWith && usesInteractionRadius)
        {
            if (collider2D == null) return;
            collider2D.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    protected virtual void PlayerEntersConsole(Player player)
    {
        player.SetConsole(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        PlayerEntersConsole(col.gameObject.GetComponent<Player>());
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();
        PlayerLeavesConsole(player);
        player.RemoveConsole(this);
    }

    public virtual void Interact(Player player)
    {
    }

    protected virtual void PlayerLeavesConsole()
    {
    }

    protected virtual void PlayerLeavesConsole(Player player)
    {
        if (_isBroken && player.GetIsRepairing())
        {
            _amountPlayerRepairing -= 1;
            player.SetIsRepairing(false);
        }
    }

    public virtual void PressButton()
    {
        wasPressed = true;
    }

    public void SetIsBrokenStatus(bool newStatus)
    {
        _isBroken = newStatus;
        if (_isBroken)
        {
            _sign.ShowSign(Sign.SignType.IsBroken, flashing: true);
            StopAllAction();
        }
        else
        {
            _sign.HideSign();
            _repairProgress = 0;
            _amountPlayerRepairing = 0;
        }
    }

    protected virtual void StopAllAction()
    {
    }

    public EConsoleType GetConsoleType()
    {
        return _type;
    }

    public void StartRepair()
    {
        _amountPlayerRepairing += 1;
        if (_repairRoutine != null) return;
        _repairRoutine = StartCoroutine(TimeTillRepaired());
    }

    private IEnumerator TimeTillRepaired()
    {
        while (_repairProgress < 1)
        {
            yield return new WaitForSeconds(0.1f);
            _repairProgress += _amountPlayerRepairing * 0.03f;
        }
        SetIsBrokenStatus(false);
    }

    public void StoppedRepairing()
    {
        _amountPlayerRepairing -= 1;
    }

    public bool GetBrokenStatus()
    {
        return _isBroken;
    }
}