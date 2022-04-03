using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnDataPoint
{
    public List<EnemyData> enemiesToSpawn;
    public float time;

}

public enum SpawningType
{
    Random,
    Wave
}

public class EnemySpawner : Singleton<EnemySpawner>
{

    [Header("General")]
    public GameObject spawnAround;
    public float spawnRangeFrom = 5f;
    public float spawnRangeTo = 15f;
    public SpawningType spawningType = SpawningType.Random;


    [Header("Random Spawning")]
    public List<EnemyData> randomSpawnEnemies = new List<EnemyData>();
    public float spawnInterval = 1f;


    [Header("Wave Spawning")]
        public float enemySpawnDelay = 0.25f;
    public List<EnemySpawnDataPoint> waveSpawnDataPoints = new List<EnemySpawnDataPoint>();

    public void Restart()
    {
        StopAllCoroutines();
        if (spawningType == SpawningType.Random)
        {
            StartCoroutine(SpawnEnemiesRandom());
        }
        else if (spawningType == SpawningType.Wave)
        {
            StartCoroutine(SpawnEnemiesWave());
        }
    }

    IEnumerator SpawnEnemiesRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomEnemy();
        }
    }

    IEnumerator SpawnEnemiesWave()
    {
        foreach (EnemySpawnDataPoint espd in waveSpawnDataPoints)
        {
            yield return StartCoroutine(SpawnEnemies(espd.enemiesToSpawn));
            yield return new WaitForSeconds(espd.time);
        }

        // Start spawning random enemies
        StartCoroutine(SpawnEnemiesRandom());
    }

    IEnumerator SpawnEnemies(List<EnemyData> enemiesToSpawn)
    {
        foreach (EnemyData enemyData in enemiesToSpawn)
        {
            SpawnEnemy(enemyData.prefab);

            // play spawn sound
            if (enemyData.spawnSound != null)
            {
                AudioManager.Instance.Play(enemyData.spawnSound);
            }
            // wait between spawning each enemy
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector2 spawnPosition = new Vector2(spawnAround.transform.position.x, spawnAround.transform.position.y);
        spawnPosition += Random.insideUnitCircle.normalized * Random.Range(spawnRangeFrom, spawnRangeTo);
        Vector3 spawnPosition3D = new Vector3(spawnPosition.x, spawnPosition.y, ZPositions.enemy);

        GameObject enemy = Instantiate(prefab, spawnPosition3D, Quaternion.identity);

        // Unused, sound was moved to enemy prefabs instead of a central script
        //EnemySoundManager.AddEnemy(enemy);
    }

    void SpawnRandomEnemy()
    {
        SpawnEnemy(randomSpawnEnemies[Random.Range(0, randomSpawnEnemies.Count)].prefab);
    }


}
