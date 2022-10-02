using System;
using UnityEngine;

namespace Controllers
{
    [System.Serializable]
    public class WaveData
    {
        public int numberOfPuffballs;
        public int numberOfSeeds;
        public int numberOfMushrooms;
    }

    public class RoundController : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private EnemyController enemyController;
        [SerializeField] public WaveData[] waves;

        public bool isBossRound;

        private int CurrentRound { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            if (enemyController == default)
            {
                Debug.Log("Have you attached the enemy controller ref to the round controller?");
            }

            CurrentRound = 0;

            // Start the first wave
            enemyController.SpawnRound();
        }

        public void LastEnemyDestroyed() // could this be an event listener?
        {
            CurrentRound += 1; // round data is indexed by 0 and display round numbers indexes by 1 so this is safe
            
            // Tells the game controller which round just finished and to handle that
            gameController.RoundEnded(CurrentRound);
            
        }

        public WaveData GetNextWave()
        {
            if (waves.Length > CurrentRound)
            {
                return waves[CurrentRound];
            }
            
            //Last Wave/Boss Fight
            Debug.Log("Boss Fight!");
            isBossRound = true;
            return new WaveData(); // TODO this should return and handle the boss fight somehow?
        }

        public void StartNextWave()
        {
            enemyController.SpawnRound();
        }
    }
}