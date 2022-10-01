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

        private int CurrentRound { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            if (enemyController == default)
            {
                Debug.Log("Have you attached the enemy controller ref to the round controller?");
            }

            CurrentRound = 0;
        }

        public void LastEnemyDestroyed() // could this be an event listener?
        {
            CurrentRound += 1; // round data is indexed by 0 and display round numbers indexes by 1 so this is safe

            // Tells the game controller which round just finished and to handle that
            gameController.RoundEnded(CurrentRound);
        }

        public WaveData NextWave()
        {
            return waves[CurrentRound];
        }
    }
}