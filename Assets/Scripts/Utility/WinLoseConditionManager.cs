using PeggleWars.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnumCollection;


namespace PeggleWars.Utilities
{
    internal class WinLoseConditionManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static WinLoseConditionManager Instance { get; private set; } 

        public UnityEvent LevelVictory;
        public UnityEvent GameOver;

        #endregion

        #region Functions

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
            EnemyEvents.Instance.EnemyDeathEvent?.AddListener(OnEnemyDeath);
        }

        private void OnDisable()
        {
            EnemyEvents.Instance.EnemyDeathEvent?.RemoveListener(OnEnemyDeath);
        }

        private void OnEnemyDeath()
        {
            if (EnemyManager.Instance != null)
            {               
                int currentEnemiesInScene = EnemyManager.Instance.EnemiesInScene.Count;
                int enemySpawnCounter = EnemyManager.Instance.GetComponent<EnemySpawnManager>().EnemySpawnCounter;
                int totalAmountOfEnemiesInLevel = EnemyManager.Instance.GetComponent<EnemySpawnManager>().AmountOfEnemiesInLevel;

                if (currentEnemiesInScene == 0 && enemySpawnCounter >= totalAmountOfEnemiesInLevel)
                {
                    LevelVictory?.Invoke();
                    StartCoroutine(GameManager.Instance.SwitchState(GameState.LevelWon));
                }
            }
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(GameManager.Instance.SwitchState(GameState.GameOver));
        }

        #endregion
    }
}
