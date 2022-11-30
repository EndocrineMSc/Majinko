using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Enemies;
using PeggleOrbs;

namespace PeggleAttacks.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        #region Fields

        protected EnemyManager _enemyManager;

        #endregion

        #region Properties

        #endregion

        #region Public Functions

        public virtual void ShootAttack(Orb orb, PlayerAttackTarget target, int damage)
        {
            Enemy enemy = null;
            Vector2 targetPosition = new();
            Vector2 startPosition = orb.transform.position;

            switch (target)
            {
                case PlayerAttackTarget.FirstEnemy:
                    enemy = _enemyManager.Enemies[0];
                    targetPosition = enemy.transform.position;                   
                    break;
            }

            //ToDo: Calculate Euler Angle for Instantiation of the Attack
            //ToDo: Then instantiate and shoot in direction of the angle towards the intended target            
        }

        #endregion

        #region Protected Functions

        // Start is called before the first frame update
        protected virtual void Start()
        {
            _enemyManager = EnemyManager.Instance;
        }

        #endregion
    }
}
