using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Attacks;

namespace Characters.Enemies
{
    internal class LeafMage : Enemy
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ProjectileAttack _leafMageShot;

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
                OrbManager.Instance.SwitchOrbsWrap(OrbType.IntangibleEnemyOrb, transform.position);
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
