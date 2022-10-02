using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class EndGame : MonoBehaviour
    {
        public GameObject endGameUI;
        public TMP_Text scoreElement;

        private GameController _gameController;

        public void Start()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        public void ShowEndGameUI(int finalScore)
        {
            scoreElement.text = finalScore.ToString();
            endGameUI.SetActive(true);
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("Scenes/Menu");
        }

        //Note: Not being used as Replay functionality has been removed. This doesn't work as-is.
        public void Replay()
        {
            _gameController.ResetGame();
            endGameUI.SetActive(false);
        }
    }
}