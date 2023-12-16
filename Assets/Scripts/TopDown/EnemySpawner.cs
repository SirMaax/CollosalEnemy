using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float firstSpawn;
    [SerializeField] private float spawnSpeedMax;
    [SerializeField] private float spawnSpeedMin;

    [Header("Other")] 
    private int lastIndex;
    
    [Header("References")] 
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyCooldown(firstSpawn));
    }
    
    IEnumerator SpawnEnemyCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        int index = 0;
        do
        {
            index = (int)Random.Range(0, spawnPoints.Length);
        } while (index == lastIndex);
        lastIndex = index;
        
        Enemy enemy = Instantiate(enemyPrefab, 
                            spawnPoints[index].transform.position, 
            quaternion.identity).GetComponent<Enemy>();

        float randomTime = Random.Range(spawnSpeedMin, spawnSpeedMax);
        StartCoroutine(SpawnEnemyCooldown(randomTime));
    }
}
