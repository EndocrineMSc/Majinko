using Utility.TurnManagement;
using System.Collections;
using UnityEngine;

namespace Characters.Enemies
{
    internal class EnemyAttackManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static EnemyAttackManager Instance { get; private set; }

        private readonly float _attackGapSeconds = 1f;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
        }

        private void OnStartEnemyPhase()
        {
            StartCoroutine(HandleEnemyAttacks());
        }

        private IEnumerator HandleEnemyAttacks()
        {
            int amountOfEnemies = EnemyManager.Instance.EnemiesInScene.Count;

            for (int i = 0; i < amountOfEnemies; i++)
            {
                if (i < EnemyManager.Instance.EnemiesInScene.Count)
                {
                    Enemy enemy = EnemyManager.Instance.EnemiesInScene[i];

                    if (enemy != null)
                    {
                        if (enemy.TurnsTillNextAttack > 0)
                        {
                            enemy.TurnsTillNextAttack--;
                        }
                        else if (enemy.AttackType == EnemyAttackType.Ranged || enemy.IsInAttackPosition)
                        {
                            enemy.Attack();
                            yield return new WaitForSeconds(_attackGapSeconds);
                        }
                    }
                }    
            }
            EnemyEvents.RaiseOnEnemiesFinishedAttacking();
        }

        #endregion
    }
}
