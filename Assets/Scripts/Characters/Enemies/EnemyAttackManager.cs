using EnumCollection;
using Utility.TurnManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    internal class EnemyAttackManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static EnemyAttackManager Instance { get; private set; }
        [SerializeField] private float _attackGapSeconds = 0.5f;

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
            foreach (Enemy enemy in EnemyManager.Instance.EnemiesInScene)
            {
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
                        enemy.ResetTurnsTillNextAttack();
                    }
                }
            }
            EnemyEvents.RaiseOnEnemiesFinishedAttacking();
        }

        #endregion
    }
}
