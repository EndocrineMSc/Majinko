using PeggleWars.TurnManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;

namespace PeggleWars.Enemies
{
    public class EnemyTurnMovement : MonoBehaviour
    {
        #region Fields and Properties

        private TurnManager _turnManager;
        private EnemyManager _enemyManager;

        private float _enemySpeed = 2;
        private float _gapSpace; //space necessary to show that there is a gap between enemies

        private List<Enemy> _flyingEnemiesInScene = new();
        private List<Enemy> _walkingEnemiesInScene = new();

        private string SPEED_PARAM = "Speed";

        #endregion

        #region Functions

        private void Start()
        {
            _turnManager = TurnManager.Instance;
            _enemyManager = EnemyManager.Instance;
            _gapSpace = _enemyManager.EnemyPositions[0,3].x - _enemyManager.EnemyPositions[0,1].x;

            TurnManager.Instance.StartEnemyTurn += OnStartEnemyTurn;
            TurnManager.Instance.EndEnemyTurn += OnEndEnemyTurn;

            SetLocalEnemyLists();
            SortLocalEnemyLists();
        }

        private void OnDisable()
        {
            _turnManager.StartEnemyTurn -= OnStartEnemyTurn;
            _turnManager.EndEnemyTurn -= OnEndEnemyTurn;
        }

        private void OnStartEnemyTurn()
        {
            foreach (Enemy enemy in _enemyManager.EnemiesInScene)
            {
                if (CheckForMoveNecessity(enemy))
                {
                    StartCoroutine(Move(enemy));
                }
            }

            TurnManager.Instance.RaiseEndEnemyTurn();
        }

        private void OnEndEnemyTurn()
        {
            _flyingEnemiesInScene.Clear();
            _walkingEnemiesInScene.Clear();
            SetLocalEnemyLists();
            SortLocalEnemyLists();
        }

        private void SetLocalEnemyLists()
        {
            foreach (Enemy enemy in _enemyManager.EnemiesInScene)
            {
                if (enemy.IsFlying)
                {
                    _flyingEnemiesInScene.Add(enemy);
                }
                else
                {
                    _walkingEnemiesInScene.Add(enemy);
                }
            }           
        }

        private void SortLocalEnemyLists()
        {
            _flyingEnemiesInScene = SortListByPositionOnXAxis(_flyingEnemiesInScene);
            _walkingEnemiesInScene = SortListByPositionOnXAxis(_walkingEnemiesInScene);
        }

        private List<Enemy> SortListByPositionOnXAxis(List<Enemy> enemyList)
        {
            List<Enemy> tempList = new();

            for (int i = 0; i < enemyList.Count; i++)
            {
                float enemyPositionOnXAxis = enemyList[0].gameObject.transform.position.x;

                if (i == 0)
                {
                    tempList.Add(enemyList[i]);
                }
                else
                {
                    for (int k = 0; k < tempList.Count; k++)
                    {
                        if (tempList[k].gameObject.transform.position.x > enemyPositionOnXAxis)
                        {
                            tempList.Insert(k, enemyList[i]);
                        }
                    }
                }
            }

            return tempList;
        }

        private bool CheckForMoveNecessity(Enemy enemy)
        {
            if(CheckIfInMeleeAttackPosition(enemy))
            {
                return false;
            }
            else if (CheckIfIsLeftMostEnemy(enemy))
            {
                return true;
            }
            else
            {
                return CheckIfGapExistsToLeftEnemy(enemy);
            }
        }

        private bool CheckIfGapExistsToLeftEnemy(Enemy enemy)
        {
            float enemyXPosition = enemy.transform.position.x;

            if (enemy.IsFlying)
            {
                int enemyIndex = _flyingEnemiesInScene.IndexOf(enemy);
                float leftEnemyXPosition = _flyingEnemiesInScene[enemyIndex - 1].transform.position.x;
                float deltaEnemyXPositions = enemyXPosition - leftEnemyXPosition;

                if (deltaEnemyXPositions >= _gapSpace)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                int enemyIndex = _walkingEnemiesInScene.IndexOf(enemy);
                float leftEnemyXPosition = _walkingEnemiesInScene[enemyIndex - 1].transform.position.x;
                float deltaEnemyXPositions = enemyXPosition - leftEnemyXPosition;

                if (deltaEnemyXPositions >= _gapSpace)
                {
                    Debug.Log("There is a gap!");
                    return true;
                }
                else
                {
                    Debug.Log("There is no gap!");
                    return false;
                }
            }
        }

        private bool CheckIfInMeleeAttackPosition(Enemy enemy)
        {
            Vector2 enemyPosition = enemy.transform.position;
            Vector2 walkerMeleeAttackPosition = _enemyManager.EnemyPositions[0, 0];
            Vector3 flyerMeleeAttackPosition = _enemyManager.EnemyPositions[1, 0];

            if (enemyPosition.Equals(walkerMeleeAttackPosition)
                || enemyPosition.Equals(flyerMeleeAttackPosition))
            {
                Debug.Log("I am in attack position!");
                return true;
            }
            else
            {
                Debug.Log("I am not in reach yet!");
                return false;
            }
        }

        private bool CheckIfIsLeftMostEnemy(Enemy enemy)
        {
            if (_flyingEnemiesInScene.IndexOf(enemy) > 0)
            {
                Debug.Log("I am not the leftmost flying enemy");
                return false;
            }
            else if (_walkingEnemiesInScene.IndexOf(enemy) > 0)
            {
                Debug.Log("I am not the leftmost enemy");
                return false;
            }
            else
            {
                Debug.Log("I am the leftmost enemy!");
                return true;
            }
        }

        //Moves the enemy one "space" to the left
        private IEnumerator Move(Enemy enemy)
        {
            int xIndexOfEnemy = GetEnemyPositionIndex(enemy);
            Vector2 startPosition = enemy.transform.position;
            Vector2 endPosition;
            Vector2 currentPosition = startPosition;
            
            if (enemy.IsFlying)
            {
                endPosition = _enemyManager.EnemyPositions[1, xIndexOfEnemy-1];
            }
            else
            {
                endPosition = _enemyManager.EnemyPositions[0, xIndexOfEnemy-1];
            }

            enemy.GetComponent<Animator>().SetFloat(SPEED_PARAM, 1);

            Rigidbody2D _rigidbody = enemy.GetComponent<Rigidbody2D>();

            while (endPosition.x < currentPosition.x)
            {
                _rigidbody.velocity = Vector2.left * _enemySpeed;
                yield return new WaitForSeconds(0.1f);
                currentPosition = enemy.transform.position;
            }

            enemy.GetComponent<Animator>().SetFloat(SPEED_PARAM, 0);
            _rigidbody.velocity = Vector2.zero;
            enemy.transform.position = endPosition;
        }

        private int GetEnemyPositionIndex(Enemy enemy)
        {
            Vector2 enemyPosition = enemy.transform.position;

            if (enemy.IsFlying)
            {
                for (int i = 0; i < _enemyManager.EnemyPositions.Length; i++)
                {
                    Vector2 indexPosition = _enemyManager.EnemyPositions[1, i];
                    if (indexPosition.Equals(enemyPosition))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _enemyManager.EnemyPositions.Length; i++)
                {
                    Vector2 indexPosition = _enemyManager.EnemyPositions[0, i];
                    if (indexPosition.Equals(enemyPosition))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        #endregion
    }
}
