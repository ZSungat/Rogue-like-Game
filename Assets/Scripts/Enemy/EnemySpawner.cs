using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public TMP_Text waveNumberText; // Reference to the TMP_Text object for wave number
    public EnemyHealthBar healthBarSlider;
    public Transform HealthCanvasTransform; // Reference to the canvas where health bars should be added
    public Transform MinSpawn, MaxSpawn;
    public Transform target;
    public int CheckPerFrame = 5;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<WaveInfo> waves;
    private int currentWave = -1;
    private float waveCounter;
    private int enemyToCheck;
    private float deSpawnDistance;
    private bool isSpawning;
    private Coroutine spawnCoroutine;

    void Start()
    {
        deSpawnDistance = Vector3.Distance(transform.position, MaxSpawn.position) + 4f;
        GoToNextWave();
    }

    void Update()
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            if (currentWave < waves.Count)
            {
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    GoToNextWave();
                }

                if (!isSpawning && waveCounter > 0)
                {
                    spawnCoroutine = StartCoroutine(SpawnEnemies());
                }
            }
        }
        else
        {
            if (isSpawning)
            {
                StopCoroutine(spawnCoroutine);
                isSpawning = false;
            }
        }

        transform.position = target.position;
        CheckAndRemoveEnemies();
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        WaveInfo wave = waves[currentWave];

        int totalEnemies = wave.GetTotalEnemyCount();
        int spawnedCount = 0;

        while (spawnedCount < totalEnemies && waveCounter > 0 && PlayerHealthController.instance.gameObject.activeSelf)
        {
            EnemyInfo enemyInfo = wave.GetRandomEnemyInfo();
            Vector3 spawnPoint = SelectSpawnPoint();
            GameObject newEnemy = Instantiate(enemyInfo.enemyPrefab, spawnPoint, Quaternion.identity);
            spawnedEnemies.Add(newEnemy);

            // Create and set up health bar for the new enemy
            EnemyHealthBar newHealthBar = Instantiate(healthBarSlider, HealthCanvasTransform);
            newEnemy.GetComponent<EnemyController>().SetHealthBar(newHealthBar);

            spawnedCount++;
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }

        isSpawning = false;
    }

    private void CheckAndRemoveEnemies()
    {
        int checkTarget = enemyToCheck + CheckPerFrame;
        while (enemyToCheck < checkTarget)
        {
            if (enemyToCheck < spawnedEnemies.Count)
            {
                if (spawnedEnemies[enemyToCheck] != null)
                {
                    if (Vector3.Distance(transform.position, spawnedEnemies[enemyToCheck].transform.position) > deSpawnDistance)
                    {
                        Destroy(spawnedEnemies[enemyToCheck]);
                        spawnedEnemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    }
                    else
                    {
                        enemyToCheck++;
                    }
                }
                else
                {
                    spawnedEnemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            }
            else
            {
                enemyToCheck = 0;
                break;
            }
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;
        bool spawnVerticalEdge = Random.Range(0f, 1f) > .5f;
        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(MinSpawn.position.y, MaxSpawn.position.y);
            spawnPoint.x = Random.Range(0f, 1f) > .5f ? MaxSpawn.position.x : MinSpawn.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(MinSpawn.position.x, MaxSpawn.position.x);
            spawnPoint.y = Random.Range(0f, 1f) > .5f ? MaxSpawn.position.y : MinSpawn.position.y;
        }
        return spawnPoint;
    }

    public void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waves.Count)
        {
            currentWave = waves.Count - 1;
        }
        waveCounter = waves[currentWave].waveLength;

        // Update wave number text
        if (waveNumberText != null)
        {
            waveNumberText.text = "Wave " + (currentWave + 1).ToString();
        }
    }
}

[System.Serializable]
public class WaveInfo
{
    public List<EnemyInfo> enemies;
    public float waveLength = 10f;
    public float timeBetweenSpawns = 1f;

    // Get total count of all enemies in this wave
    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var enemyInfo in enemies)
        {
            total += enemyInfo.count;
        }
        return total;
    }

    // Get a random enemy info from the list, weighted by count
    public EnemyInfo GetRandomEnemyInfo()
    {
        int totalEnemies = GetTotalEnemyCount();
        int randomIndex = Random.Range(0, totalEnemies);
        foreach (var enemyInfo in enemies)
        {
            if (randomIndex < enemyInfo.count)
            {
                return enemyInfo;
            }
            randomIndex -= enemyInfo.count;
        }
        return enemies[0]; // Fallback, should not reach here
    }
}

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemyPrefab;
    public int count;
}
