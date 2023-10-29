using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private bool canSpawnShieldedEnemies;
    [SerializeField] private bool disableEvents;
    
    [Header("IncomingAttack")] public bool incomingAttack;
    [SerializeField] private float minTimeBetweenAttacks;
    [SerializeField] private float maxTimeBetweenAttacks;
    [SerializeField] private float timeTillAttackArriveMin;
    [SerializeField] private float timeTillAttackArriveMax;
    [SerializeField] private float maxAttackCoordinateX;
    [SerializeField] private float minAttackCoordinateX;
    [SerializeField] private float minAttackCoordinateY;
    [SerializeField] private float maxAttackCoordinateY;


    // public bool continueMarching;
    // public bool retreatMarching;

    [Header("Targets")] 
    public bool targetAvailable;
    [SerializeField] private float timeTillTargetLeaves;
    [SerializeField] private float minShieldRecharge;
    [SerializeField] private float maxShieldRecharge;
    [SerializeField] private float timeTillTargetLeavesMax;
    [SerializeField] private float timeTillTargetLeavesMin;
    [SerializeField] private float timeTillNextTargetMin;
    [SerializeField] private float timeTillNextTargetMax;
    private int health;
    private bool shieldActive;
    private int targetLevel;
    
    [Header("Scoree")] 
    [SerializeField] private float scoreHitDeflected;
    [SerializeField] private float scoreHitNotDeflected;
    [SerializeField] private float scoreTargetLeft;
    [SerializeField] private float scoreTargetKilled;
    
    [Header("Refs")]
    [SerializeField] private ShieldSystems shield;
    [SerializeField] private EnvironmentController _environmentController;

    [Header("ScreensRefs")] 
    [SerializeField] private TMP_Text incomingAttackText;
    [SerializeField] TMP_Text targetText;
    [SerializeField] private TMP_Text targetHealth;
    [SerializeField] private TMP_Text targetLeavestext;
    // Start is called before the first frame update
    void Start()
    {
        if (disableEvents) return;
        //Start first Target
        
        SpawnNewTargetInRandomTime();
        float timeNew = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        incomingAttackText.SetText("Currently not targeted");
        StartCoroutine(StartIncomingAttacks(timeNew));
        targetHealth.SetText("-");   
        targetLeavestext.SetText("-");
    }
    
    private void SpawnNewTargetInRandomTime()
    {
        float time = Random.Range(timeTillNextTargetMin, timeTillNextTargetMax);
        UpdateTargetText(-1, 0);
        StartCoroutine(CoutdownTillNewTarget(time));
    }
    private IEnumerator CoutdownTillNewTarget(float time)
    {
        yield return new WaitForSeconds(time);
        NewTarget();
    }
    public void NewTarget()
    {
        int targetLevel = Random.Range(1, 4);
        int shields = Random.Range(0, 2);
        if (shields == 1 && canSpawnShieldedEnemies) shieldActive = true;
        else shieldActive = false; 
        int timeTillTargetLeaves =(int) Random.Range(timeTillTargetLeavesMin, timeTillTargetLeavesMax);
        StartCoroutine(CountDownTargetLeave(timeTillTargetLeaves));
        health = targetLevel;
        UpdateTargetText(targetLevel,shields);
        UpdateTargetHealth();
    }
    private void UpdateTargetText(int level, int shield)
    {
        if (level == -1)
        {
            targetText.SetText("No Target available");
        }
        else
        {
            string shieldActiveText;
            if (shieldActive) shieldActiveText = "Yes";
            else shieldActiveText = "False";
            targetText.SetText("Target Level: " + level.ToString() + "    Shield Active: " + shieldActiveText);
            
        }
    }
    private void UpdateTargetHealth()
    {
        targetHealth.SetText("Remaining Health: " + health.ToString());
    }
    IEnumerator CountDownTargetLeave(int time)
    {
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(1);
            targetLeavestext.SetText("Targets leaves in " + (time - i).ToString() + " seconds");
        }

        targetLeavestext.SetText("-");
        TargetLeft();
    }

    private void TargetLeft()
    {
        GameMaster.ChangeScoreBy(scoreTargetLeft);
        SpawnNewTargetInRandomTime();
    }
    private IEnumerator ShieldCooldown()
    {
        float time = Random.Range(minShieldRecharge, maxShieldRecharge);
        yield return new WaitForSeconds(time);
        shieldActive = true;
    }

    private void DefeatedTarget()
    {
        GameMaster.ChangeScoreBy(targetLevel * scoreTargetKilled);
        targetHealth.SetText("-");
        StopCoroutine(CountDownTargetLeave(0));
        StopCoroutine(ShieldCooldown());
        SpawnNewTargetInRandomTime();
    }

    //TARGETED AREA/////////////////////////////
    
    private void Targeted()
    {
        int time = (int ) Random.Range(timeTillAttackArriveMin, timeTillAttackArriveMax);
        StartCoroutine(Attacked(time));
    }

    private IEnumerator Attacked(int time)
    {
        //TODO SOUND of attack
        for (int i = 0; i < time; i++)
        {
            UpdateIncomingAttackText(time - i);
            yield return new WaitForSeconds(1);
        }
        //Check shields
        if (shield.shieldActive) GameMaster.ChangeScoreBy(scoreHitDeflected);
        else
        {
            GameMaster.ChangeScoreBy(scoreHitNotDeflected);
            float x = Random.Range(minAttackCoordinateX, maxAttackCoordinateX);
            float y = Random.Range(minAttackCoordinateY, maxAttackCoordinateY);
            _environmentController.ApplyEffectFrom(new Vector2(x, y));
        }
        incomingAttackText.SetText("Currently not targeted");
        float timeNew = Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks);
        StartIncomingAttacks(timeNew);
    }

    private void UpdateIncomingAttackText(int time)
    {
        incomingAttackText.SetText("INCOMING ATTACK IN " + time.ToString() + " seconds");
        
    }

    private IEnumerator StartIncomingAttacks(float time)
    {
        yield return new WaitForSeconds(time);
        Targeted();
    }
    // AREA INTERACTION WITH OUTSIDE
    
    public void TargetHit(int dmg)
    {
        if (shieldActive) return;
        health -= dmg;
        if (health == 0)
        {
            DefeatedTarget();
        }

        UpdateTargetHealth();
    }

 
    
    public void ShieldHit()
    {
        if (!shieldActive) return;
        shieldActive = false;
        StartCoroutine(ShieldCooldown());
    }
}