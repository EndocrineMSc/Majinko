using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Enemies
{
    [RequireComponent(typeof(EnemySpawnManager))]
    [RequireComponent(typeof(EnemyTurnMovement))]
    internal class EnemyManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static EnemyManager Instance { get; private set; }

        internal Enemy[] EnemyLibrary { get; private set; }
        internal List<Enemy> EnemiesInScene { get; set; } = new();
        internal Vector3[,] EnemyPositions { get; private set; }

        //Fields to calculate EnemyPositions regardless of screen size
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

            //needs to be in Awake for timing issues
            EnemyLibrary = _allEnemiesCollection.AllEnemies;
            EnemyLibrary = EnemyLibrary.OrderBy(enemy => enemy.name).ToArray();
            EnemyPositions = new Vector3[_amountOfEnemyRows, _amountOfCharacterPositionsOnXAxis];
            SetEnemyPositions();
        }

        private void SetEnemyPositions()
        {
            Camera camera = Camera.main;
            int cellHeight = Screen.height / 10;
            float yLowerRow = Screen.height - cellHeight - 15;
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
                        EnemyPositions[y, x] = new Vector3(possibleCharacterPositionAsWorldPoint.x, possibleCharacterPositionAsWorldPoint.y, -1);
                    }
                    else
                    {
                        Vector2 possibleCharacterPositionOnScreen = new((x + xPositionOffset) * cellWidth, yUpperRow);
                        Vector2 possibleCharacterPositionAsWorldPoint = camera.ScreenToWorldPoint(possibleCharacterPositionOnScreen);
                        EnemyPositions[y, x] = new Vector3(possibleCharacterPositionAsWorldPoint.x, possibleCharacterPositionAsWorldPoint.y, -1);
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
