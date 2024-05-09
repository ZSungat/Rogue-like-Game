using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyToSpawn;
    public Transform MinSpawn, MaxSpawn;
    public Transform target;
    public float TimeToSpawn;
    private float SpawnCounter;
    private float deSpawnDistance;
    public int CheckPerFrame;
    private int EnemyToCheck;

    private List<GameObject> SpawnedEnemies = new List<GameObject>();
    public List<WaveInfo> Waves;

    private int CurrentWave;
    private float WaveCounter;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnCounter = TimeToSpawn;

        // target = PlayerHealthController.instance.transform;

        deSpawnDistance = Vector3.Distance(transform.position, MaxSpawn.position) + 4f;

        CurrentWave = -1;
        GoToNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            if (CurrentWave < Waves.Count)
            {
                WaveCounter -= Time.deltaTime;
                if (WaveCounter <= 0)
                {
                    GoToNextWave();
                }
                SpawnCounter -= Time.deltaTime;
                if (SpawnCounter <= 0)
                {
                    SpawnCounter = Waves[CurrentWave].TimeBetweenSpawns;
                    GameObject NewEnemy = Instantiate(Waves[CurrentWave].EnemyToSpawn, SelectSpawnPoint(), Quaternion.identity);
                    SpawnedEnemies.Add(NewEnemy);
                }
            }
        }

        transform.position = target.position;
        int CheckTarget = EnemyToCheck + CheckPerFrame;
        while (EnemyToCheck < CheckTarget)
        {
            if (EnemyToCheck < SpawnedEnemies.Count)
            {
                if (SpawnedEnemies[EnemyToCheck] != null)
                {
                    if (Vector3.Distance(transform.position, SpawnedEnemies[EnemyToCheck].transform.position) > deSpawnDistance)
                    {
                        Destroy(SpawnedEnemies[EnemyToCheck]);
                        SpawnedEnemies.RemoveAt(EnemyToCheck);
                        CheckTarget--;
                    }
                    else
                    {
                        EnemyToCheck++;
                    }
                }
                else
                {
                    SpawnedEnemies.RemoveAt(EnemyToCheck);
                    CheckTarget--;
                }
            }
            else
            {
                EnemyToCheck = 0;
                CheckTarget = 0;
            }
        }
    }

    public Vector3 SelectSpawnPoint()
    {
        Vector3 SpawnPoint = Vector3.zero;
        bool SpawnVerticalEdge = Random.Range(0f, 1f) > .5f;
        if (SpawnVerticalEdge)
        {
            SpawnPoint.y = Random.Range(MinSpawn.position.y, MaxSpawn.position.y);
            if (Random.Range(0f, 1f) > .5f)
            {
                SpawnPoint.x = MaxSpawn.position.x;
            }
            else
            {
                SpawnPoint.x = MinSpawn.position.x;
            }
        }
        else
        {
            SpawnPoint.x = Random.Range(MinSpawn.position.x, MaxSpawn.position.x);
            if (Random.Range(0f, 1f) > .5f)
            {
                SpawnPoint.y = MaxSpawn.position.y;
            }
            else
            {
                SpawnPoint.y = MinSpawn.position.y;
            }
        }

        return SpawnPoint;
    }
    public void GoToNextWave()
    {
        CurrentWave++;

        if (CurrentWave >= Waves.Count)
        {
            CurrentWave = Waves.Count - 1;
        }
        WaveCounter = Waves[CurrentWave].WaveLength;
        SpawnCounter = Waves[CurrentWave].TimeBetweenSpawns;
    }
}


[System.Serializable]
public class WaveInfo
{
    public GameObject EnemyToSpawn;
    public float WaveLength = 10f;
    public float TimeBetweenSpawns = 1f;
}
