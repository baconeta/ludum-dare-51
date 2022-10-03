using System;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] public WaveData bossWave;

        public bool isBossRound;

        public int CurrentRound { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            if (enemyController == default)
            {
                Debug.Log("Have you attached the enemy controller ref to the round controller?");
            }

            BossHotwire hotwire = FindObjectOfType<BossHotwire>();
            if (hotwire && hotwire.DoJumpToBoss())
            {
                // Bypass round counter to spawn boss immediately.
                CurrentRound = 999;
            }
            else
            {
                CurrentRound = 0;
            }

            // Start the first wave
            enemyController.SpawnRound();
        }

        public void LastEnemyDestroyed() // could this be an event listener?
        {
            CurrentRound += 1; // round data is indexed by 0 and display round numbers indexes by 1 so this is safe

            // Tells the game controller which round just finished and to handle that
            gameController.RoundEnded(CurrentRound);

            gameController.timer.ExpectedRounds(CurrentRound * 5f * 20f * 0.67f);
        }

        public WaveData GetNextWave()
        {
            if (waves.Length > CurrentRound)
            {
                return waves[CurrentRound];
            }

            //Last Wave/Boss Fight
            isBossRound = true;
            enemyController.SpawnBoss();
            // Show the boss's health bar.
            GameObject bossHealthBar = FindObjectOfType<BossHealthBarHoist>().bossHealthBar;
            bossHealthBar.SetActive(true);
            return bossWave;
        }

        public void StartNextWave()
        {
            enemyController.SpawnRound();
        }
    }
}