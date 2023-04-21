using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.Utilities
{
    internal class PauseControl : MonoBehaviour
    {
        internal static PauseControl Instance { get; private set; }
        
        internal bool GameIsPaused { get; private set; }

        public UnityEvent PauseAndUnpauseGame;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PauseAndUnpauseGame.AddListener(OnPauseUnpauseGame);
        }

        private void OnPauseUnpauseGame()
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
    }
}
