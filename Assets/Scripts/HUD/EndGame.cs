using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class EndGame : MonoBehaviour
    {
        public GameObject endGameUI;

        private GameController _gameController;

        public void Start()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        public void ShowEndGameUI()
        {
            endGameUI.SetActive(true);
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("Scenes/Menu");
        }

        public void Replay()
        {
            _gameController.ResetGame();
            endGameUI.SetActive(false);
        }
    }
}