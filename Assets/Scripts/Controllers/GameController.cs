using System.Collections;
using HUD;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private bool isInvincible;
        [SerializeField] private Sanctuary sanctuary;

        private RoundController _rc;
        public bool GameRunning { get; set; }

        public static bool IsPlayerInputEnabled = true;

        [HideInInspector] public GameTimer timer;

        private void Awake()
        {
            timer = gameObject.AddComponent<GameTimer>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _rc = FindObjectOfType<RoundController>();

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
            
            StartCoroutine(RoundTransition());
        }

        public IEnumerator RoundTransition()
        {
            float lightTimer;
            lightTimer = timer.GetTimeToNextDark();
            Debug.Log(lightTimer);
            //TODO Wait until end of NEXT light mode.
            yield return new WaitForSeconds(lightTimer);
            
            //End and move to sanctuary
            timer.StopTimer();
            IsPlayerInputEnabled = false;
            sanctuary.ShowSanctuary();
            yield return null;
        }
        
        

        public void Continue()
        {
            IsPlayerInputEnabled = true;
            timer.StartTimer();
            //If before boss wave
            _rc.StartNextWave();
            
            //if after boss wave
            //Show scoreboard
        }
    }
}