using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;

namespace Characters.Enemies
{
    public class Shroombie : Enemy
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        protected override void AttackEffect()
        {
            if (_particleSystem != null)
                _particleSystem.Play();
        }

        protected override void StartTurnEffect()
        {
            //no start turn effect
        }

        protected override void EndTurnEffect()
        {
            //no end turn effect
        }

        protected override void OnDeathEffect()
        {
            if (OrbManager.Instance != null)
                OrbManager.Instance.SwitchOrbsWrap(OrbType.RottedOrb, transform.position, 2);
        }

    }
}
