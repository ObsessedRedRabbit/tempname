using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    private int randEnemy;
    public int maxEnemies;
    public int enemiesSpawned;
    public GameObject[] enemies;

    [Header("SpawnArea")]
    public Vector3 spawnValues;

    [Header("Spawning")]
    public int startTime;
    public bool stopSpawner;
    public float spawnRate;
    public float spawnMostWait;
    public float spawnLeastWait;
    public bool coroutineStarted;

    void Update()
    {
        spawnRate = Random.Range(spawnLeastWait, spawnMostWait);
        
        if (enemiesSpawned >= maxEnemies)
        {
            stopSpawner = true;
            coroutineStarted = false;
            StopCoroutine(Spawning());
        }
        else if (enemiesSpawned < maxEnemies)
        {
            stopSpawner = false;
            if (!coroutineStarted)
            {
                StartCoroutine(Spawning());
            }
        }
    }

    IEnumerator Spawning()
    {
        coroutineStarted = true;

        yield return new WaitForSeconds(startTime);

        while (!stopSpawner)
        {
            randEnemy = Random.Range(0, 2);

            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));

            Instantiate(enemies[randEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);

            enemiesSpawned++;

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
