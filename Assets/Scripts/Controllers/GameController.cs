using HUD;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private bool isInvincible;
        [SerializeField] private Sanctuary sanctuary;
        public bool GameRunning { get; set; }

        [HideInInspector] public GameTimer timer;

        private void Awake()
        {
            timer = gameObject.AddComponent<GameTimer>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (sanctuary == default)
            {
                Debug.Log("Add a sanctuary ref to the game controller");
            }

            GameRunning = true;
            HUDController hud = FindObjectOfType<HUDController>();
            if (hud != null)
            {
                hud.GameStart();
            }

            if (timer)
                timer.StartTimer();
        }

        public void ResetGame()
        {
            BroadcastMessage("onGameReset");
            GameRunning = true;
        }

        // Update is called once per frame
        public void EndGame(float delay = 0f)
        {
            ;
            if (!isInvincible)
            {
                Invoke(nameof(BroadcastGameOver), delay);
            }
        }

        private void BroadcastGameOver()
        {
            BroadcastMessage("onGameEnd");
            GameRunning = false;
        }

        public void RoundEnded(int currentRound)
        {
            Debug.Log("Round " + currentRound + " completed successfully.");
            sanctuary.ShowSanctuary();
            // Also we should pause the game and all elements here including input!!
        }
    }
}