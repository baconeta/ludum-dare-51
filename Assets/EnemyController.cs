using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public List<Enemy> enemyList;
    public GameObject spawnableArea;
    public List<Enemy> livingEnemies;

    private float boundX;
    private float boundY;

    private void Start()
    {
        livingEnemies = new List<Enemy>();
        
        if (!spawnableArea)
        {
            spawnableArea = GameObject.FindWithTag("SpawnableArea");
        }

        //Get bounds of spawnable area
        boundX = spawnableArea.GetComponent<SpriteRenderer>().size.x / 2;
        boundY = spawnableArea.GetComponent<SpriteRenderer>().size.y / 2;
        
        //Start round - Arbitrary number of enemies to spawn
        SpawnRound(10);
    }

    public void Update()
    {
        if (livingEnemies.Count == 0)
        {
            SpawnRound(10);
        }
    }

    public void ClearLivingEnemies()
    {
        foreach (Enemy enemy in livingEnemies)
        {
            enemy.Die(true);
        }
    }

    public void SpawnEnemy(Transform spawnPosition = null)
    {
        //Spawn new enemy
        Enemy newEnemy = Instantiate(GetEnemyType());
        //Save reference in living enemies
        livingEnemies.Add(newEnemy);
        
        //Move new enemy to spawn position.
        Vector3 newPosition;
        if (spawnPosition)
        {
            newPosition = spawnPosition.position;
        }
        else
        {
            //Get random position
            float randomX = Random.Range(-boundX, boundX);
            float randomY = Random.Range(-boundY, boundY);
            newPosition = new Vector3(randomX, randomY, 0);
        }

        newEnemy.transform.position = newPosition;
    }

    public void SpawnRound(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    public Enemy GetEnemyType(string enemyType = null)
    {
        Enemy newEnemy = null;
        switch (enemyType)
        {
            case null:
                newEnemy = enemyList[Random.Range(0, enemyList.Count)];
                break;
            case "Mushroom":
                newEnemy = enemyList[0];
                break;
            case "Puffball":
                newEnemy = enemyList[1];
                break;
            case "Seed":
                newEnemy = enemyList[2];
                break;
        }

        return newEnemy;
    }
}
