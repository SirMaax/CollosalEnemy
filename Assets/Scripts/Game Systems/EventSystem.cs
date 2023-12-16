using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private Transform coordinatesLeftBottom ;
    [SerializeField] private Transform coordiantesRightTop;
    
    [Header("Refs")]
    [SerializeField] private ShieldSystems shield;
    [SerializeField] private EnvironmentController _environmentController;
    
    public void Attacked()
    {
        if (shield.shieldActive)
        {
            // GameMaster.ChangeScoreBy(scoreHitDeflected);
            SoundManager.Play(6) ;
        }
        else
        {
            float x = Random.Range(coordinatesLeftBottom.position.x, coordiantesRightTop.position.x);
            float y = Random.Range(coordinatesLeftBottom.position.y, coordiantesRightTop.position.y);
            _environmentController.ApplyEffectFrom(new Vector2(x, y));
        }
    }
    
}