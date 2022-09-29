using UnityEngine;

namespace Controllers
{
    public class HUDController : MonoBehaviour
    {
        private GameController _gameController;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void GameStart()
        {
            _gameController = FindObjectOfType<GameController>();
            if (_gameController != null)
            {
                transform.SetParent(_gameController.transform);
            }
        }

        public void RestartGame()
        {
            if (_gameController != null)
            {
                _gameController.ResetGame();
            }
        }
    }
}