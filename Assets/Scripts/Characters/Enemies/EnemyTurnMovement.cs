using PeggleWars.TurnManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PeggleWars.Enemies
{
    public class EnemyTurnMovement : MonoBehaviour
    {
        #region Fields and Properties

        private EnemyManager _enemyManager;

        private float _gapSpace; //space necessary to show that there is a gap between enemies

        private List<Enemy> _flyingEnemiesInScene = new();
        private List<Enemy> _walkingEnemiesInScene = new();

        #endregion

        #region Functions

        private void Start()
        {
            _enemyManager = EnemyManager.Instance;
            _gapSpace = (_enemyManager.EnemyPositions[0, 3].x - _enemyManager.EnemyPositions[0, 2].x) * 1.2f;

            EnemyEvents.Instance.EnemyAttacksEndEvent?.AddListener(OnEndAttacks);
            TurnManager.Instance.EndEnemyTurn?.AddListener(OnEndEnemyTurn);

            SetLocalEnemyLists();
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartEnemyTurn?.RemoveListener(OnEndAttacks);
            TurnManager.Instance.EndEnemyTurn?.RemoveListener(OnEndEnemyTurn);
        }

        private void OnEndAttacks()
        {
            SetLocalEnemyLists();
            if (_enemyManager.EnemiesInScene.Count > 0)
            {
                SortLocalEnemyLists();
            }
            StartCoroutine(HandleEnemyMovement());
        }

        private IEnumerator HandleEnemyMovement()
        {
            for (int i = 0; i < _enemyManager.EnemiesInScene.Count; i++)
            {
                if (CheckForMoveNecessity(_enemyManager.EnemiesInScene[i]))
                {
                    yield return StartCoroutine(_enemyManager.EnemiesInScene[i].Move());
                }
            }
            EnemyEvents.Instance.EnemyMoveEndEvent?.Invoke();
        }


        private void OnEndEnemyTurn()
        {
            _flyingEnemiesInScene.Clear();
            _walkingEnemiesInScene.Clear();
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

            if (enemyList.Count == 0)
            {
                return tempList;
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (tempList.Count == 0)
                {
                    tempList.Add(enemyList[0]);
                }
                else
                {
                    float currentEnemyPositionOnXAxis = enemyList[i].gameObject.transform.position.x;
                    int sortedEnemies = tempList.Count;

                    for (int k = 0; k < sortedEnemies; k++)
                    {
                        float tempListEnemyXPosition = tempList[k].gameObject.transform.position.x;

                        if (currentEnemyPositionOnXAxis < tempListEnemyXPosition)
                        {
                            tempList.Insert(k, enemyList[i]);
                            break;
                        }

                        if (k == (sortedEnemies - 1))
                        {
                            tempList.Add(enemyList[i]);
                        }
                    }
                }
            }
            return tempList;
        }

        private bool CheckForMoveNecessity(Enemy enemy)
        {
            if(enemy.IsFrozen)
            {
                return false;
            }
            else if(CheckIfInMeleeAttackPosition(enemy))
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

                if (deltaEnemyXPositions > _gapSpace)
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

                if (deltaEnemyXPositions > _gapSpace)
                {
                    return true;
                }
                else
                {
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
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfIsLeftMostEnemy(Enemy enemy)
        {
            if (_flyingEnemiesInScene.IndexOf(enemy) > 0)
            {
                return false;
            }
            else if (_walkingEnemiesInScene.IndexOf(enemy) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
