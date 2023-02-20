using EnumCollection;
using PeggleWars.TurnManagement;
using System.Collections;
using UnityEngine;

namespace PeggleWars.Enemies
{
    public class EnemySpawnManager : MonoBehaviour
    {
        #region Fields and Properties

        private EnemyManager _enemyManager;
        private Vector2[,] _enemyPositions;

        int _rightMostEnemyPosition;
        readonly int _enemyBottomRow = 0;
        readonly int _enemyTopRow = 1;

        Vector2 _flyingEnemySpawnPosition;
        Vector2 _walkingEnemySpawnPosition;

        #endregion

        #region Functions

        private void Start()
        {
            _enemyManager = EnemyManager.Instance;
            _enemyPositions = _enemyManager.EnemyPositions;

            _rightMostEnemyPosition = _enemyPositions.GetLength(1) - 1;
            _flyingEnemySpawnPosition = _enemyPositions[_enemyTopRow, _rightMostEnemyPosition];
            _walkingEnemySpawnPosition = _enemyPositions[_enemyBottomRow, _rightMostEnemyPosition];

            TurnManager.Instance.StartEnemyTurn += OnStartEnemyTurn;
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartEnemyTurn -= OnStartEnemyTurn;
        }

        private void OnStartEnemyTurn()
        {
            SpawnEnemy(EnemyType.CloakedZombie);
        }

        public void SpawnEnemy(EnemyType enemyType)
        {
            Enemy tempEnemy = _enemyManager.EnemyLibrary[(int)enemyType];
            Vector2 spawnPosition;
            
            bool spawnPositionIsOccupied = CheckIfPositionOccupied(tempEnemy);
                     
            if (tempEnemy.IsFlying)
            {
                spawnPosition = _flyingEnemySpawnPosition;
            }
            else
            {
                spawnPosition = _walkingEnemySpawnPosition;
            }

            if (!spawnPositionIsOccupied)
            {
                Enemy instantiatedEnemy = Instantiate(tempEnemy, spawnPosition, Quaternion.identity); ;
                _enemyManager.EnemiesInScene.Add(instantiatedEnemy);
            }
        }

        private bool CheckIfPositionOccupied(Enemy enemy)
        {
            bool flyPositionIsOccupied = false;
            bool landPositionIsOccupied = false;

            foreach (Enemy sceneEnemy in _enemyManager.EnemiesInScene)
            {
                Vector2 enemyPosition = sceneEnemy.transform.position;

                if (enemyPosition == _flyingEnemySpawnPosition)
                {
                    flyPositionIsOccupied = true;
                }
                else if (enemyPosition == _walkingEnemySpawnPosition)
                {
                    landPositionIsOccupied = true;
                }
            }

            if (enemy.IsFlying)
            {
                return flyPositionIsOccupied;
            }
            else
            {
                return landPositionIsOccupied;
            }
        }
        #endregion
    }
}
