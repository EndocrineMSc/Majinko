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
        private int _turnsTillNextAttack;

        #endregion

        #region Functions

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            _turnsTillNextAttack = _enemy.AttackFrequency;
        }

        private void OnEnable()
        {
            TurnManager.Instance.StartEnemyTurn?.AddListener(OnStartEnemyTurn);
        }

        private void OnDisable()
        {
            TurnManager.Instance.StartEnemyTurn?.RemoveListener(OnStartEnemyTurn);
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
