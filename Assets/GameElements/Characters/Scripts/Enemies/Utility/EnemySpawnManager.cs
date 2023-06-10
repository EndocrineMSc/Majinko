using EnumCollection;
using Utility.TurnManagement;
using UnityEngine;
using Utility;

namespace Characters.Enemies
{
    internal class EnemySpawnManager : MonoBehaviour
    {
        #region Fields and Properties

        private EnemyManager _enemyManager;
        private Vector3[,] _enemyPositions;
        private EnemyType[] _enemiesForLevel;

        private readonly int _enemyBottomRow = 0;
        private readonly int _enemyTopRow = 1;

        Vector2 _flyingEnemySpawnPosition;
        Vector2 _walkingEnemySpawnPosition;

        internal int AmountOfEnemiesInLevel { get; private set; } 
        internal int EnemySpawnCounter { get; private set; }

        private int _overworldIndex = 1;
        [SerializeField] private OverworldSetOfSets _worldOneSets;

        #endregion

        #region Functions

        private void Awake()
        {
            _enemiesForLevel = GetEnemyArrayByWorld();
            AmountOfEnemiesInLevel = _enemiesForLevel.Length;
        }

        private void Start()
        {
            SetReferences();
            SetSpawnPositions();
            SpawnEnemy(_enemiesForLevel[0]);
        }

        private void OnEnable()
        {
            EnemyEvents.OnEnemyFinishedMoving += OnEndOfEnemyMovement;           
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyFinishedMoving -= OnEndOfEnemyMovement;
        }

        private void SetReferences()
        {
            _enemyManager = EnemyManager.Instance;
            _enemyPositions = _enemyManager.EnemyPositions;
            _overworldIndex = GlobalWorldManager.Instance.WorldIndex;
        }

        private void SetSpawnPositions()
        {
            int rightMostEnemyPosition = _enemyPositions.GetLength(1) - 1;
            _flyingEnemySpawnPosition = _enemyPositions[_enemyTopRow, rightMostEnemyPosition];
            _walkingEnemySpawnPosition = _enemyPositions[_enemyBottomRow, rightMostEnemyPosition];
        }

        private EnemyType[] GetEnemyArrayByWorld()
        {
            EnemyType[] tempArray = null;

            switch (_overworldIndex)
            {
                case 1:
                    ScriptableEnemySet[] tempAllEnemySets = _worldOneSets.ViableEnemySets;
                    int randomSet = UnityEngine.Random.Range(0, tempAllEnemySets.Length);
                    ScriptableEnemySet tempEnemySet = tempAllEnemySets[randomSet];
                    tempArray = tempEnemySet.EnemyArray;
                    break;
            }
            return tempArray;
        }

        private void OnEndOfEnemyMovement()
        {
            if (EnemySpawnCounter < AmountOfEnemiesInLevel)
            {
                EnemyType tempEnemy = _enemiesForLevel[EnemySpawnCounter];
                SpawnEnemy(tempEnemy);
            }
            PhaseManager.Instance.EndEnemyPhase();
        }

        internal void SpawnEnemy(EnemyType enemyType)
        {
            Enemy spawnEnemy = _enemyManager.EnemyLibrary[(int)enemyType];
            Vector2 spawnPosition;            
                     
            if (spawnEnemy.IsFlying)
                spawnPosition = _flyingEnemySpawnPosition;
            else
                spawnPosition = _walkingEnemySpawnPosition;

            if (!CheckIfSpawnPositionOccupied())
            {
                Enemy instantiatedEnemy = Instantiate(spawnEnemy, spawnPosition, Quaternion.identity);
                _enemyManager.EnemiesInScene.Add(instantiatedEnemy);
                EnemySpawnCounter++;
                instantiatedEnemy.transform.position = new Vector3(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y, -1); //quick fix for display scroll
            }
        }

        private bool CheckIfSpawnPositionOccupied()
        {
            foreach (Enemy sceneEnemy in _enemyManager.EnemiesInScene)
            {
                Vector2 enemyPosition = sceneEnemy.transform.position;

                if (enemyPosition == _flyingEnemySpawnPosition)
                {
                    return true;
                }
                else if (enemyPosition == _walkingEnemySpawnPosition)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
