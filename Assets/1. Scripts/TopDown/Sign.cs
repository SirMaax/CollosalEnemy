using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float flashingIntervall;
    private float _flashFasterSpeed = 0.01f;
    private static float _startFlashingIntervall;
    
    [Header("Private")] private SignType type;

    [Header("References")] [SerializeField]
    private Sprite[] signSprites;

    [SerializeField] private Color[] colors;
    private SpriteRenderer spriteRenderer;
    private Coroutine routine;

    public void Start()
    {
        GetSpriteRenderer();
        _startFlashingIntervall = flashingIntervall;
    }

    public enum SignType
    {
        EnemyAppearing,
        Attacking,
    }

    public void ShowSign(SignType type, float time = 0, bool flashing = false, bool destroyAfterwards = false,bool flashFaster = false)
    {
        if (spriteRenderer == null) GetSpriteRenderer();
        if (routine != null) StopCoroutine(routine);

        spriteRenderer.sprite = signSprites[(int)type];
        spriteRenderer.color = colors[(int)type];
        spriteRenderer.enabled = true;
        if (time != 0) routine = StartCoroutine(HideSignIn(time, flashing, flashFaster));
        if (destroyAfterwards) Destroy(gameObject, time);
    }

    public void HideSign()
    {
        spriteRenderer.enabled = true;
    }

    IEnumerator HideSignIn(float time, bool flashing, bool flashFaster)
    {
        flashingIntervall = _startFlashingIntervall;
        spriteRenderer.enabled = true;
        bool spriteActive = true;

        if (!flashing)
        {
            yield return new WaitForSeconds(time);
        }
        else
        {
            for (float i = 0; i < time; i += flashingIntervall)
            {
                yield return new WaitForSeconds(flashingIntervall);
                if(flashFaster) flashingIntervall=Mathf.Lerp(_startFlashingIntervall,0.01f, i / time);
                if (spriteActive) spriteRenderer.enabled = false;
                else spriteRenderer.enabled = true;
                spriteActive = !spriteActive;
            }
        }
        spriteRenderer.enabled = false;
    }

    private void GetSpriteRenderer()
    {
        if (spriteRenderer != null) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    
}
