using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool _enableBreakdownOverTime;
    [SerializeField] private Transform coordinatesLeftBottom ;
    [SerializeField] private Transform coordiantesRightTop;

    [Header("Test Settings")] 
    [SerializeField] private bool _disableSystemBreaking;
    
    [Header("ScreenShake")] 
    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _screenShakeDuration;
    
    [Header("Refs")]
    [SerializeField] private ShieldSystems shield;
    [SerializeField] private EnvironmentController _environmentController;
    [SerializeField] private CinemachineVirtualCamera _camera;
    private List<GameObject> _allBreakableSystems;


    public void Start()
    {
        GetAllBreakableSystems();
        if(_enableBreakdownOverTime)StartCoroutine(BreakSystemWithChanceAfterEachTimestep());
    }
    
    public void Attacked(bool environmentAttack = false, float screenShakeIntensity = -1, float duration = -1)
    {
        
        if (shield.shieldActive && !environmentAttack)
        {
            // GameMaster.ChangeScoreBy(scoreHitDeflected);
            SoundManager.Play(6) ;
        }
        else
        {
            float x = Random.Range(coordinatesLeftBottom.position.x, coordiantesRightTop.position.x);
            float y = Random.Range(coordinatesLeftBottom.position.y, coordiantesRightTop.position.y);
            _environmentController.ApplyEffectFrom(new Vector2(x, y));
            if(environmentAttack) RollForSystemsBreaking(0.05f);
            else RollForSystemsBreaking();
            StartScreenShake(screenShakeIntensity, duration);
        }
    }

    private void StartScreenShake(float intensity = -1, float duration = -1)
    { 
        StopAllCoroutines();
        StartCoroutine(ScreenShakeCooldown(duration));
        float shakeIntensity = intensity == -1 ? _shakeIntensity : intensity;
        _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeIntensity;
    }

    private void StopScreenShake()
    {
        _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    private IEnumerator ScreenShakeCooldown(float duration = -1)
    {
        float time = duration != -1 ? _screenShakeDuration : duration;
        yield return new WaitForSeconds(_screenShakeDuration);
        StopScreenShake();
    }

    private void RollForSystemsBreaking(float reduceChance = 0)
    {
        float chance = Random.Range(0f, 1f);
        chance -= reduceChance;
        if (chance < 0.3) return;
        else if (chance < 0.85) BreakRandomSystem(1);
        else if (chance < 0.95) BreakRandomSystem(2);
        else if (chance < 1) BreakRandomSystem(3);
    }

    private void BreakRandomSystem(int times)
    {
        if (_disableSystemBreaking) return;
        List<int> unbrokenSystem = Enumerable.Range(0, _allBreakableSystems.Count).ToList();
        Console console;
        MechSystem mechSystem;
        for (int i = 0; i < times; i++)
        {
            while (unbrokenSystem.Count > 0)
            {
                int randomIndex = (int)Random.Range(0, unbrokenSystem.Count);
                GameObject gameObject = _allBreakableSystems[unbrokenSystem[randomIndex]];

                unbrokenSystem.RemoveAt(randomIndex);
                if (gameObject.TryGetComponent<Console>(out console))
                {
                    if (!console.GetBrokenStatus() && gameObject.activeSelf)
                    {
                        console.SetIsBrokenStatus(true);
                        break;
                    }
                }
                else
                {
                    mechSystem = gameObject.GetComponent<MechSystem>();
                    if (!mechSystem.GetIsBroken() && gameObject.activeSelf)
                    {
                        mechSystem.SetIsBrokenStatus(true);
                        break;
                    }
                }
            } 
            if (unbrokenSystem.Count <= 0) break;

        }
    }

    private void GetAllBreakableSystems()
    {
        _allBreakableSystems = GameObject.FindGameObjectsWithTag("MechSystem").ToList();
        _allBreakableSystems.AddRange(GameObject.FindGameObjectsWithTag("Console"));
    }

    private IEnumerator BreakSystemWithChanceAfterEachTimestep()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (Random.Range(0, 1) < 0.1) BreakRandomSystem(1);
        }
    } 
}