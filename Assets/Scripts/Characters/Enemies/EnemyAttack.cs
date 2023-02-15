using EnumCollection;
using PeggleWars.TurnManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        #region Fields and Properties

        private Enemy _enemy;
        private EnemyManager _enemyManager;
        private TurnManager _turnManager;
        private int _turnsTillNextAttack;

        #endregion

        #region Functions

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            _enemyManager = EnemyManager.Instance;
            _turnManager = TurnManager.Instance;
            _turnsTillNextAttack = _enemy.AttackFrequency;
        }

        private void OnEnable()
        {
            TurnManager.Instance.StartEnemyTurn += OnStartEnemyTurn;
        }

        private void OnDisable()
        {
            _turnManager.StartEnemyTurn -= OnStartEnemyTurn;
        }

        private void OnStartEnemyTurn()
        {
           if(CheckForAttack())
           {
                _enemy.Attack();
                _turnsTillNextAttack = _enemy.AttackFrequency;
           }
        }

        private bool CheckForAttack()
        {
            if (_turnsTillNextAttack > 0)
            {
                _turnsTillNextAttack--;
                return false;
            }
            else
            {
                return _enemy.IsInAttackPosition;
            }
        }

        #endregion
    }
}
