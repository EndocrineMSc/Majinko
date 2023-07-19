using Characters.Enemies;
using UnityEngine;
using UnityEngine.Events;


namespace Utility
{
    internal class WinLoseConditionManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static WinLoseConditionManager Instance { get; private set; } 

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
            EnemyEvents.OnEnemyDied += OnEnemyDeath;
            UtilityEvents.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyDied -= OnEnemyDeath;
            UtilityEvents.OnPlayerDeath -= OnPlayerDeath;
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
                    UtilityEvents.RaiseLevelVictory();
                    StartCoroutine(GameManager.Instance.SwitchState(GameState.LevelWon));
                }
            }
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(GameManager.Instance.SwitchState(GameState.GameOver));
        }

        public void CheatButton()
        {
            UtilityEvents.RaiseLevelVictory();
        }

        #endregion
    }
}
