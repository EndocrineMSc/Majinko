using EnumCollection;
using PeggleWars.TurnManagement;
using UnityEngine;

namespace PeggleWars.Enemies
{
    internal class EnemySpawnManager : MonoBehaviour
    {
        #region Fields and Properties

        private EnemyManager _enemyManager;
        private Vector2[,] _enemyPositions;

        private int _rightMostEnemyPosition;
        readonly int _enemyBottomRow = 0;
        readonly int _enemyTopRow = 1;

        Vector2 _flyingEnemySpawnPosition;
        Vector2 _walkingEnemySpawnPosition;

        [SerializeField] private int _overworldIndex = 1;
        private EnemyType[] _enemiesForLevel;
        private readonly string WORLD_ONE_PARAM = "ScriptableEnemySets/World 1 Levels";
        private int _amountOfEnemiesInLevel;
        internal int AmountOfEnemiesInLevel { get { return _amountOfEnemiesInLevel; } } 
        private int _enemySpawnCounter;
        internal int EnemySpawnCounter { get { return _enemySpawnCounter; } }

        #endregion

        #region Functions

        private void Start()
        {
            _enemyManager = EnemyManager.Instance;
            _enemyPositions = _enemyManager.EnemyPositions;

            _rightMostEnemyPosition = _enemyPositions.GetLength(1) - 1;
            _flyingEnemySpawnPosition = _enemyPositions[_enemyTopRow, _rightMostEnemyPosition];
            _walkingEnemySpawnPosition = _enemyPositions[_enemyBottomRow, _rightMostEnemyPosition];

            _enemiesForLevel = GetEnemyArrayByWorld();
            _amountOfEnemiesInLevel = _enemiesForLevel.Length;
            EnemyEvents.Instance.EnemyMoveEndEvent?.AddListener(OnEndOfEnemyMovement);
        }

        private void OnDisable()
        {
            EnemyEvents.Instance.EnemyMoveEndEvent?.RemoveListener(OnEndOfEnemyMovement);
        }

        private EnemyType[] GetEnemyArrayByWorld()
        {
            EnemyType[] tempArray = null;

            switch (_overworldIndex)
            {
                case 1:
                    ScriptableEnemySet[] tempAllEnemySets = Resources.LoadAll<ScriptableEnemySet>(WORLD_ONE_PARAM);
                    int randomSet = UnityEngine.Random.Range(0, tempAllEnemySets.Length);
                    ScriptableEnemySet tempEnemySet = tempAllEnemySets[randomSet];
                    tempArray = tempEnemySet.EnemyArray;
                    break;
            }
            return tempArray;
        }

        private void OnEndOfEnemyMovement()
        {
            if (_enemySpawnCounter < _amountOfEnemiesInLevel)
            {
                EnemyType tempEnemy = _enemiesForLevel[_enemySpawnCounter];
                SpawnEnemy(tempEnemy);
            }
            TurnManager.Instance.EndEnemyTurn?.Invoke();
        }

        internal void SpawnEnemy(EnemyType enemyType)
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
                _enemySpawnCounter++;
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
