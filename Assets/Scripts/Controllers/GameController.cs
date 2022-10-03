using System.Collections;
using HUD;
using Player;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private bool isInvincible;

        [SerializeField] private Sanctuary sanctuary;

        // This is used to submit and receive scores to and from the server.
        [SerializeField] private GlobalScoreManager _globalScoreManager;
        [SerializeField] private EndGame endGame;
        [SerializeField] GameUI gameUI;

        public RoundController _rc;
        public bool GameRunning { get; set; }

        public static bool IsPlayerInputEnabled = true;

        [HideInInspector] public GameTimer timer;


        private void Awake()
        {
            timer = gameObject.GetComponent<GameTimer>();
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

            if (gameUI != null)
                gameUI.ShowRoundClearedText(false);
        }

        public void ResetGame()
        {
            BroadcastMessage("onGameReset");
            GameRunning = true;
        }

        public void EndGame(bool victory = false, float delay = 0f)
        {
            Debug.Log("End of Game!");

            if (victory)
            {
                Debug.Log("You Win! You conquered CORN!");
                //Show Scoreboard now
            }
            else
            {
                Debug.Log("You Lost! CORN conquered You!");
                //Show loss/death screen
            }

            // Not sure what this does Currently errors
            // if (!isInvincible)
            // {
            //     Invoke(nameof(BroadcastGameOver), delay);
            // }

            // Submit the score to the score server.
            PlayerStats stats = FindObjectOfType<PlayerStats>();
            stats.CalculateScore();
            var finalScore = stats.GetScore();
            _globalScoreManager.SubmitScore(stats.GetName(), finalScore);

            gameUI.HideUI();
            endGame.ShowEndGameUI(finalScore);
        }

        private void BroadcastGameOver()
        {
            BroadcastMessage("onGameEnd");
            GameRunning = false;
        }

        public void RoundEnded(int currentRound)
        {
            Debug.Log("Round " + currentRound + " completed successfully.");

            if (gameUI != null)
                gameUI.ShowRoundClearedText(true);

            StartCoroutine(RoundTransition(currentRound));
        }

        public IEnumerator RoundTransition(int currentRound)
        {
            timer.JumpToLightPhase();
            float phaseTimer = timer.GetTimer();
            //TODO Wait until end of NEXT light mode.
            yield return new WaitForSeconds(phaseTimer);

            if (gameUI != null)
                gameUI.ShowRoundClearedText(false);

            //End and move to sanctuary
            timer.StopTimer();
            IsPlayerInputEnabled = false;
            sanctuary.ShowSanctuary(currentRound);
            yield return null;
        }


        public void Continue()
        {
            IsPlayerInputEnabled = true;
            timer.JumpToLightPhase();
            timer.StartTimer();
            //If before boss wave
            if (!_rc.isBossRound) _rc.StartNextWave();
            else //if after boss wave
            {
                EndGame(true);
            }
        }
    }
}