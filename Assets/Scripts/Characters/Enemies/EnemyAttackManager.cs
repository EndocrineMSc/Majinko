using EnumCollection;
using PeggleWars.TurnManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Enemies
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
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            TurnManager.Instance.StartEnemyTurn?.AddListener(OnStartEnemyTurn);
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartEnemyTurn?.RemoveListener(OnStartEnemyTurn);
        }

        private void OnStartEnemyTurn()
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
            EnemyEvents.Instance.EnemyAttacksEndEvent?.Invoke();
        }

        #endregion
    }
}
