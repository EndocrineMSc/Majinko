using EnumCollection;
using Utility.TurnManagement;
using UnityEngine;
using Utility;
using UnityEngine.SceneManagement;
using System;

namespace Characters.Enemies
{
    public class EnemySpawnManager : MonoBehaviour
    {
        #region Fields and Properties

        private EnemyManager _enemyManager;
        private Vector3[,] _enemyPositions;
        private GameObject[] _enemiesForLevel;

        private readonly int _enemyBottomRow = 0;
        private readonly int _enemyTopRow = 1;

        Vector2 _flyingEnemySpawnPosition;
        Vector2 _walkingEnemySpawnPosition;
        Vector2 _nullVector = Vector2.zero;

        public int AmountOfEnemiesInLevel { get; private set; } 
        public int EnemySpawnCounter { get; private set; }

        private readonly string NORMAL_COMBAT_SCENE_NAME = "NormalCombat";
        private int _overworldIndex = 1;
        [SerializeField] private OverworldSetOfSets _worldOneNormalCombatSets;
        [SerializeField] private OverworldSetOfSets _worldOneEliteCombatSets;
        [SerializeField] private OverworldSetOfSets _worldOneBossCombatSets;

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

        private GameObject[] GetEnemyArrayByWorld()
        {
            GameObject[] tempArray = null;
            Scene currentScene = SceneManager.GetActiveScene();
            ScriptableEnemySet[] tempAllEnemySets = null;

            switch (_overworldIndex)
            {
                case 1:
                    if (currentScene.name.Contains(NORMAL_COMBAT_SCENE_NAME))
                        tempAllEnemySets = _worldOneNormalCombatSets.ViableEnemySets;
                    else
                        tempAllEnemySets = LoadHelper.CurrentlyBossCombat ? _worldOneBossCombatSets.ViableEnemySets : _worldOneEliteCombatSets.ViableEnemySets;
                    break;
            }

            if (tempAllEnemySets != null)
            {
                int randomSet = UnityEngine.Random.Range(0, tempAllEnemySets.Length);
                ScriptableEnemySet tempEnemySet = tempAllEnemySets[randomSet];
                tempArray = tempEnemySet.EnemyArray;
            }
            return tempArray;
        }

        private void OnEndOfEnemyMovement()
        {
            if (EnemySpawnCounter < AmountOfEnemiesInLevel)
            {
                var tempEnemy = _enemiesForLevel[EnemySpawnCounter];
                SpawnEnemy(tempEnemy);
            }
            PhaseManager.Instance.EndEnemyPhase();
        }

        public void SpawnEnemy(GameObject enemyPrefab)
        {
            Enemy spawnEnemy = enemyPrefab.GetComponent<Enemy>();
            Vector2 spawnPosition;            
                     
            if (spawnEnemy.EnemyObject.IsFlying)
                spawnPosition = _flyingEnemySpawnPosition;
            else
                spawnPosition = _walkingEnemySpawnPosition;

            if (!CheckIfSpawnPositionOccupied(spawnPosition))
            {
                Enemy instantiatedEnemy = Instantiate(spawnEnemy, spawnPosition, Quaternion.identity);
                _enemyManager.EnemiesInScene.Add(instantiatedEnemy);
                EnemySpawnCounter++;
                instantiatedEnemy.transform.position = new Vector3(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y, -1); //quick fix for display scroll
            }
        }

        public bool SpawnEnemy(GameObject enemyPrefab, Vector2 spawnPosition)
        {
            Enemy spawnEnemy = enemyPrefab.GetComponent<Enemy>();
            bool spawnPossible = !CheckIfSpawnPositionOccupied(spawnPosition);

            if (spawnPossible)
            {
                Enemy instantiatedEnemy = Instantiate(spawnEnemy, spawnPosition, Quaternion.identity);
                _enemyManager.EnemiesInScene.Add(instantiatedEnemy);
                EnemySpawnCounter++;
                instantiatedEnemy.transform.position = new Vector3(instantiatedEnemy.transform.position.x, instantiatedEnemy.transform.position.y, -1); //quick fix for display scroll
            }

            return spawnPossible;
        }

        private bool CheckIfSpawnPositionOccupied(Vector2 spawnPosition)
        {
            foreach (Enemy sceneEnemy in _enemyManager.EnemiesInScene)
            {
                Vector2 enemyPosition = sceneEnemy.transform.position;
                if (enemyPosition == spawnPosition)
                    return true;
            }
            return false;
        }
        #endregion
    }
}
