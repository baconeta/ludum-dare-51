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

            CurrentRound = 1;
        }

        private void LastEnemyDestroyed() // This may need to be renamed or rewritten
        {
            // Tell the GameController the round ended so it can inform the elements in the world and the timer
            // Which should also then show the sanctuary
        }
    }
}