using HUD;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private bool isInvincible;
        [SerializeField] private Sanctuary sanctuary;
        public bool GameRunning { get; set; }

        [HideInInspector] 
        public GameTimer Timer;

        private void Awake()
        {
            Timer = gameObject.AddComponent<GameTimer>();
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
        
            if (Timer)
                Timer.StartTimer();
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
    }
}