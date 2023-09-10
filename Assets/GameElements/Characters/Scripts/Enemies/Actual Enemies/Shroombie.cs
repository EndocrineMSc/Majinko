using Orbs;
using UnityEngine;

namespace Characters.Enemies
{
    public class Shroombie : Enemy
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private OrbData _rottenOrbData;
        
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
                OrbManager.Instance.SwitchOrbs(_rottenOrbData, transform.position, 2);
        }
    }
}
