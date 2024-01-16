using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public GameObject enemy;
    public Transform spawnPoint;

    public float spawnInterval = 2f;
    private float nextSpawnTime = 0f;

    void Update()
    {

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {

        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

}
