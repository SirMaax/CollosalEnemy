using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Attribute")] [SerializeField] private float speed;
    [SerializeField] private float[] heightLevel;
    [SerializeField] private float snappingPoint;
    [Tooltip("Every timeBetweenSteps one StepSize is added till the maximum speed is reached")]
    [SerializeField] private float TimeBetweenSteps;
    [SerializeField] private float stepSize;
    
    [Header("Status")] private int currentLevel;
    private bool travelling;
    private int nextLevel;
    private int nextnextLevel;
    private bool comingToStop;
    private float startSpeed;
    
    // [Header("Energy")]

    [Header("Ref")] 
    [SerializeField] private Console upButton;    
    [SerializeField] private Console downButton;    
    
    
    // Start is called before the first frame update
    void Start()
    {
        nextLevel = 0;
        currentLevel = 0;
        startSpeed = speed;
        comingToStop = false;
        travelling = false;
    }

    // Update is called once per frame
    void Update()
    {
        Travel();
        if (upButton.wasPressed || downButton.wasPressed) TravelLevelUpOrDown();
    }

    public void GoToLevel(int level)
    {
        if (currentLevel == level) return;
        if (travelling)
        {
            nextnextLevel = level;
            StopAllCoroutines();
            // StopCoroutine("SlowDown");
            StartCoroutine(SlowDown());
        }
        else
        {
            travelling = true;
            speed = startSpeed;
            nextLevel = level;
            currentLevel = -1;
        }
    }

    public void Travel()
    {
        if (!travelling) return;
        Vector2 currentPos = transform.position;

        int multiplier = 1;
        if (currentPos.y > heightLevel[nextLevel]) multiplier = -1;

        Vector2 direction = speed * Time.deltaTime * multiplier * Vector2.up;
        currentPos += direction;
        if (Mathf.Abs(currentPos.y - heightLevel[nextLevel]) < snappingPoint)
        {
            currentPos.y = heightLevel[nextLevel];
            travelling = false;
            currentLevel = nextLevel;
            nextLevel = -1;
        }
        transform.position = currentPos;
    }

    IEnumerator SlowDown()
    {
        int counter = 0;
        while (speed!=0)
        {
            counter += 1;
            yield return new WaitForSeconds(TimeBetweenSteps);
            speed = Mathf.Lerp(startSpeed, 0,stepSize * counter);
        }
        nextLevel = nextnextLevel;
        counter = 0;
        while (speed!=startSpeed)
        {
            counter += 1;
            yield return new WaitForSeconds(TimeBetweenSteps);
            speed = Mathf.Lerp(0, startSpeed,stepSize * counter);
        }

        
    }

    private void TravelLevelUpOrDown()
    {
        if (upButton.wasPressed && (currentLevel > nextLevel || !travelling))
        {
            upButton.wasPressed = false;
            if (currentLevel == heightLevel.Length-1) return;
            if (travelling) nextLevel = currentLevel;
            else nextLevel = currentLevel + 1;
            travelling = true;
        }
        if (downButton.wasPressed)
        {
            downButton.wasPressed = false;
            if (currentLevel == 0 && !travelling) return;
            if (travelling) nextLevel = currentLevel;
            else nextLevel = currentLevel - 1;
            travelling = true;
        }
    }
}