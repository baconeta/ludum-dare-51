using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] public bool isInvincible;
        public bool GameRunning { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            GameRunning = true;
            HUDController hud = FindObjectOfType<HUDController>();
            if (hud != null)
            {
                hud.GameStart();
            }
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