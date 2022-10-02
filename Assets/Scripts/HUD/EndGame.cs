﻿using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class EndGame : MonoBehaviour
    {
        public GameObject endGameUI;

        private GameController _gameController;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            _gameController = FindObjectOfType<GameController>();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("Scenes/Menu");
        }

        public void Replay()
        {
            _gameController.ResetGame();
        }
    }
}