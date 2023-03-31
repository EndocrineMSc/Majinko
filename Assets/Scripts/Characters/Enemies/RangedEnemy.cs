using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Attacks;

namespace PeggleWars.Enemies
{
    internal abstract class RangedEnemy : Enemy
    {
        #region Fields and Properties

        [SerializeField] protected Attack _enemyAttack;

        #endregion

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            EnemyEvents.Instance.EnemyMoveEndEvent?.AddListener(OnEndMove);
            _enemyAttack.SetAttackInstantiatePosition(transform);
        }

        protected void OnEndMove()
        {
            _enemyAttack.SetAttackInstantiatePosition(transform);
        }

        protected override void AdditionalAttackEffects()
        {
            Debug.Log("Pew pew!");
            _enemyAttack.ShootAttack();
        }

        #endregion

    }
}
