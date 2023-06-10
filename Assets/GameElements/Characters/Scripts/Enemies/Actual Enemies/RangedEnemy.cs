using UnityEngine;
using Attacks;

namespace Characters.Enemies
{
    internal abstract class RangedEnemy : Enemy
    {
        #region Fields and Properties

        [SerializeField] protected Attack _enemyAttack;

        #endregion

        #region Functions

        protected override void AdditionalAttackEffects()
        {
            _enemyAttack.ShootAttack(gameObject.transform.position, _dealingDamageModifier);
        }

        #endregion
    }
}
