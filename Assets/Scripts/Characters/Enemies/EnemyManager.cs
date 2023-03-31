using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.Enemies
{
    [RequireComponent(typeof(EnemySpawnManager))]
    [RequireComponent(typeof(EnemyTurnMovement))]
    internal class EnemyManager : MonoBehaviour
    {
        #region Fields

        internal static EnemyManager Instance { get; private set; }

        private Enemy[] _enemyLibrary;
        internal Enemy[] EnemyLibrary { get { return _enemyLibrary; } private set { _enemyLibrary = value; } }

        private List<Enemy> _enemiesInScene = new();
        internal List<Enemy> EnemiesInScene { get => _enemiesInScene; set => _enemiesInScene = value; }

        private Vector2[,] _enemyPositions;
        internal Vector2[,] EnemyPositions { get => _enemyPositions; private set => _enemyPositions = value; }

        private readonly int _amountOfXScreenDivisions = 10;
        private readonly int _amountOfCharacterPositionsOnXAxis = 6;
        private readonly int _amountOfEnemyRows = 2;
        private bool _isFirstInit = true;

        protected string ENEMY_FOLDER_PARAM = "EnemyPrefabs";

        #endregion

        #region Functions

        private void Awake()
        {           
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            if (_isFirstInit)
            {
                _enemyLibrary = Resources.LoadAll<Enemy>(ENEMY_FOLDER_PARAM);
            }
        }

        private void Start()
        {
            if (_isFirstInit)
            {
                _enemyPositions = new Vector2[_amountOfEnemyRows, _amountOfCharacterPositionsOnXAxis];
                SetEnemyPositions();
                _isFirstInit = false;
            }
        }

        private void SetEnemyPositions()
        {
            Camera camera = Camera.main;
            int cellHeight = Screen.height / 10;
            float yLowerRow = Screen.height - cellHeight;
            float yUpperRow = Screen.height - cellHeight / 2;
            float cellWidth = Screen.width / _amountOfXScreenDivisions;
            float xPositionOffset = 3;

            for (int x = 0; x < _amountOfCharacterPositionsOnXAxis; x++)
            {
                for (int y = 0; y < _amountOfEnemyRows; y++)
                {
                    if (y == 0)
                    {
                        Vector2 possibleCharacterPositionOnScreen = new((x + xPositionOffset) * cellWidth, yLowerRow);
                        Vector2 possibleCharacterPositionAsWorldPoint = camera.ScreenToWorldPoint(possibleCharacterPositionOnScreen);
                        _enemyPositions[y, x] = possibleCharacterPositionAsWorldPoint;
                    }
                    else
                    {
                        Vector2 possibleCharacterPositionOnScreen = new((x + xPositionOffset) * cellWidth, yUpperRow);
                        Vector2 possibleCharacterPositionAsWorldPoint = camera.ScreenToWorldPoint(possibleCharacterPositionOnScreen);
                        _enemyPositions[y, x] = possibleCharacterPositionAsWorldPoint;
                    }
                }
            }
        }

        #endregion
    }
}
