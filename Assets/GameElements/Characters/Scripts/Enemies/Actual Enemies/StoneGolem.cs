using Orbs;
using UnityEngine;

namespace Characters.Enemies
{
    public class StoneGolem : Enemy
    {
        [SerializeField] private OrbData _stoneOrbData;

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
                OrbManager.Instance.SwitchOrbs(_stoneOrbData, transform.position, 3);
            }
        }

        #endregion
    }
}
