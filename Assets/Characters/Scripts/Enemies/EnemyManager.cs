using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    [RequireComponent(typeof(EnemySpawnManager))]
    [RequireComponent(typeof(EnemyTurnMovement))]
    internal class EnemyManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static EnemyManager Instance { get; private set; }

        private Enemy[] _enemyLibrary;
        internal Enemy[] EnemyLibrary { get { return _enemyLibrary; } private set { _enemyLibrary = value; } }

        private List<Enemy> _enemiesInScene = new();
        internal List<Enemy> EnemiesInScene { get => _enemiesInScene; set => _enemiesInScene = value; }

        private Vector3[,] _enemyPositions;
        internal Vector3[,] EnemyPositions { get => _enemyPositions; private set => _enemyPositions = value; }

        private readonly int _amountOfXScreenDivisions = 10;
        private readonly int _amountOfCharacterPositionsOnXAxis = 6;
        private readonly int _amountOfEnemyRows = 2;

        [SerializeField] private AllEnemiesCollection _allEnemiesCollection;

        #endregion

        #region Functions

        private void Awake()
        {       
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            EnemyLibrary = _allEnemiesCollection.AllEnemies;
            EnemyLibrary = EnemyLibrary.OrderBy(enemy => enemy.name).ToArray();
            _enemyPositions = new Vector3[_amountOfEnemyRows, _amountOfCharacterPositionsOnXAxis];
            SetEnemyPositions();
        }

        private void SetEnemyPositions()
        {
            Camera camera = Camera.main;
            int cellHeight = Screen.height / 10;
            float yLowerRow = Screen.height - cellHeight;
            float yUpperRow = Screen.height - cellHeight / 2;
            float cellWidth = (float)Screen.width / _amountOfXScreenDivisions;
            float xPositionOffset = 3;

            for (int x = 0; x < _amountOfCharacterPositionsOnXAxis; x++)
            {
                for (int y = 0; y < _amountOfEnemyRows; y++)
                {
                    if (y == 0)
                    {
                        Vector2 possibleCharacterPositionOnScreen = new((x + xPositionOffset) * cellWidth, yLowerRow);
                        Vector2 possibleCharacterPositionAsWorldPoint = camera.ScreenToWorldPoint(possibleCharacterPositionOnScreen);
                        _enemyPositions[y, x] = new Vector3(possibleCharacterPositionAsWorldPoint.x, possibleCharacterPositionAsWorldPoint.y, -1);
                    }
                    else
                    {
                        Vector2 possibleCharacterPositionOnScreen = new((x + xPositionOffset) * cellWidth, yUpperRow);
                        Vector2 possibleCharacterPositionAsWorldPoint = camera.ScreenToWorldPoint(possibleCharacterPositionOnScreen);
                        _enemyPositions[y, x] = new Vector3(possibleCharacterPositionAsWorldPoint.x, possibleCharacterPositionAsWorldPoint.y, -1);
                    }
                }
            }
        }

        #endregion
    }

    internal enum EnemyAttackType
    {
        Melee,
        Ranged,
    }
}
