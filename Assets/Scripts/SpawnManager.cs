using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // For Waves
    [SerializeField] private Wave[] waves;
    [Tooltip("Keep common enemy at element 0")]
    [SerializeField] private GameObject[] enemies;

    // For Single Enemy Spawning
    [SerializeField] private float singleEnemySpawnRate;
    [SerializeField] private Transform[] singleSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        // Spawns enemies
        StartCoroutine(SpawnSingleEnemies());
        StartCoroutine(SpawnWaveOfThree());
        StartCoroutine(SpawnWaves());

        TimeBar.TimeBarFinished += TimeBar_TimeBarFinished;
    }

    private void TimeBar_TimeBarFinished()
    {
        StopAllCoroutines();
    }

    // Spawns single enemies from any direction
    IEnumerator SpawnSingleEnemies()
    {
        while (true)
        {
            int randIndex = Random.Range(0, singleSpawnPos.Length);
            Vector2 randSpawnPos = singleSpawnPos[randIndex].position;

            yield return new WaitForSeconds(singleEnemySpawnRate);

            if (PlayerController.isPlayerAlive)
            {
                Instantiate(enemies[RandEnemy()] , randSpawnPos, enemies[RandEnemy()].transform.rotation);
            }
        }
    }

    // Spawns waves of three from any direction
    IEnumerator SpawnWaveOfThree()
    {
        while (true)
        {
            int randWaveOfThreeDir = Random.Range(0, 4);
            int randSmallWaveSpawnTime = Random.Range(5, 7);

            yield return new WaitForSeconds(randSmallWaveSpawnTime);

            if (PlayerController.isPlayerAlive)
            {
                for (int x = 0; x < 4; x++)
                {
                    Instantiate(enemies[RandEnemy()], waves[randWaveOfThreeDir].spawnArea[x].position, enemies[RandEnemy()].transform.rotation);
                }
            }
        }
    }

    // Spawn large enemy waves from any direction
    IEnumerator SpawnWaves()
    {
        while (true)
        {
            int randWaveDir = Random.Range(0, 4);
            int numOfEnemiesToSpawn = Random.Range(4, 7);
            int randLargeWaveSpawnTime = Random.Range(13, 15);

            yield return new WaitForSeconds(randLargeWaveSpawnTime);

            if (PlayerController.isPlayerAlive)
            {
                for (int i = 0; i < numOfEnemiesToSpawn; i++)
                {
                    Instantiate(enemies[RandEnemy()], waves[randWaveDir].spawnArea[i].position, enemies[RandEnemy()].transform.rotation);
                }
            }
        }
    }

    private int RandEnemy()
    {
        int randEnemyNum = Random.Range(0, enemies.Length);

        if (randEnemyNum == 1)
        {
            int otherRandNum = Random.Range(0, enemies.Length);
            return otherRandNum;
        }

        else
        {
            return randEnemyNum;
        }
    }
}

// Wave object
[System.Serializable]
public class Wave
{
    public string waveDir;
    public Transform[] spawnArea;
}
