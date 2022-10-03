using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public List<Enemy> enemyList;
        public GameObject spawnableArea;
        public float spawnBoundaryOffsetX;
        public float spawnBoundaryOffsetY;
        public List<Enemy> livingEnemies;

        private bool _active;
        private WaveData _currentWave;
        [SerializeField] private RoundController roundController;

        private float boundX;
        private float boundY;

        private void Awake()
        {
            if (roundController == default)
            {
                Debug.Log("Set a ref to the RoundController on the EnemyController");
            }

            livingEnemies = new List<Enemy>();

            if (!spawnableArea)
            {
                spawnableArea = GameObject.FindWithTag("SpawnableArea");
            }

            //Get bounds of spawnable area
            boundX = (spawnableArea.GetComponent<SpriteRenderer>().size.x - spawnBoundaryOffsetX) / 2;
            boundY = (spawnableArea.GetComponent<SpriteRenderer>().size.y - spawnBoundaryOffsetY) / 2;
        }

        public void Update()
        {
            if (!_active) return;
            if (livingEnemies.Count > 0) return;

            Debug.Log("No enemies remaining");
            _active = false;
            roundController.LastEnemyDestroyed();
        }

        private void ClearLivingEnemies()
        {
            // Debug only NOT CURRENTLY WORKING PROPERLY
            for (int i = 0; i < livingEnemies.Count; i++)
            {
                if (livingEnemies[i] != null)
                {
                    livingEnemies[i].Die(true);
                }
            }

            Debug.Log("All enemies destroyed by debug command.");
        }

        public void SpawnEnemy(Vector3 spawnPosition = default, string enemyType = null)
        {
            //Spawn new enemy
            Enemy newEnemy = Instantiate(GetEnemyType(enemyType));
            //Save reference in living enemies
            livingEnemies.Add(newEnemy);

            //Move new enemy to spawn position.
            Vector3 newPosition;
            if (spawnPosition != default)
            {
                newPosition = spawnPosition;
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

        public void SpawnRound()
        {
            _currentWave = roundController.GetNextWave();

            for (int i = 0; i < _currentWave.numberOfPuffballs; i++)
            {
                SpawnEnemy(enemyType: "Puffball");
            }

            for (int i = 0; i < _currentWave.numberOfSeeds; i++)
            {
                SpawnEnemy(enemyType: "Seed");
            }

            for (int i = 0; i < _currentWave.numberOfMushrooms; i++)
            {
                SpawnEnemy(enemyType: "Mushroom");
            }

            _active = true;
        }

        public void SpawnBoss()
        {
            SpawnEnemy(enemyType: "CornBoss", spawnPosition: new Vector3(-0.165872335f, 7.3083086f, 0f));
        }

        public Enemy GetEnemyType(string enemyType = null)
        {
            Enemy newEnemy = null;
            switch (enemyType)
            {
                case null:
                    newEnemy = enemyList[Random.Range(0, enemyList.Count - 1)]; // don't include the boss in rand choice
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
                case "CornBoss":
                    newEnemy = enemyList[3];
                    break;
            }

            if (newEnemy != null) newEnemy.EcRef = this;

            return newEnemy;
        }
    }
}