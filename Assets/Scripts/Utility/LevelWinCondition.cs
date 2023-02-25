using PeggleWars.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;


namespace PeggleWars.Utilities
{
    public class LevelWinCondition : MonoBehaviour
    {
        #region Fields and Properties

        private int _amountOfEnemiesInLevel;
        private EnemySpawnManager _enemySpawnManager;
        private EnemyManager _enemyManager;
        private GameManager _gameManager;

        #endregion

        #region Functions

        private void Start()
        {
            SetReferences();
            _enemyManager.EnemyDeathEvent.AddListener(OnEnemyDeath);
        }

        private void OnDisable()
        {
            _enemyManager.EnemyDeathEvent?.RemoveListener(OnEnemyDeath);
        }

        private void SetReferences()
        {
            _enemyManager = EnemyManager.Instance;
            _enemySpawnManager = _enemyManager.GetComponent<EnemySpawnManager>();
            _amountOfEnemiesInLevel = _enemySpawnManager.AmountOfEnemiesInLevel;
            _gameManager = GameManager.Instance;
        }

        private void OnEnemyDeath()
        {
            int currentEnemiesInScene = _enemyManager.EnemiesInScene.Count;
            int enemySpawnCounter = _enemySpawnManager.EnemySpawnCounter;

            if (currentEnemiesInScene == 0 && enemySpawnCounter >= _amountOfEnemiesInLevel)
            {
                StartCoroutine(_gameManager.SwitchState(GameState.LevelWon));
            }
        }
        #endregion
    }
}
