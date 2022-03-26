using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnAround;
    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 1f;

    public float spawnRangeFrom = 5f;
    public float spawnRangeTo = 15f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(spawnAround.transform.position.x, spawnAround.transform.position.y) + Random.insideUnitCircle * Random.Range(spawnRangeFrom, spawnRangeTo);
        Vector3 spawnPosition3D = new Vector3(spawnPosition.x, spawnPosition.y, ZPositions.enemy);

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        GameObject enemy = Instantiate(prefab, spawnPosition3D, Quaternion.identity);
    }
        

}
