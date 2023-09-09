using Orbs;
using UnityEngine;
using Attacks;

namespace Characters.Enemies
{
    internal class LeafMage : Enemy
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ProjectileAttack _leafMageShot;
        [SerializeField] private OrbData _intangibleOrbData;

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            _deathDelayForAnimation = _particleSystem.main.startLifetimeMultiplier;
        }

        protected override void EndTurnEffect()
        {
            if (AbilityCooldown <= 0 && OrbManager.Instance != null)
            {
                AbilityCooldown = EnemyObject.AbilityCooldownMax;
                OrbManager.Instance.SwitchOrbs(_intangibleOrbData, transform.position);
            }
        }

        protected override void StartTurnEffect()
        {
            //None
        }

        protected override void AttackEffect()
        {
            _leafMageShot.ShootAttack(transform.position, _dealingDamageModifier);
        }

        protected override void OnDeathEffect()
        {
            if (_particleSystem != null)
                _particleSystem.Play();
        }

        #endregion
    }
}
