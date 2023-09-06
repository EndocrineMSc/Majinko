using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;

namespace Characters.Enemies
{
    internal class StoneGolem : Enemy
    {
        #region Functions

        protected override void StartTurnEffect()
        {
            //no start turn effect
        }

        protected override void AttackEffect()
        {
            //no attack effect
        }

        protected override void OnDeathEffect()
        {
            //no death effect
        }

        protected override void EndTurnEffect()
        {
            if (AbilityCooldown <= 0 && OrbManager.Instance != null)
            {
                AbilityCooldown = EnemyObject.AbilityCooldownMax;
                OrbManager.Instance.SwitchOrbsWrap(OrbType.StoneOrb, transform.position, 3);
            }
        }

        #endregion
    }
}
