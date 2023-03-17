using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Enemies;
using UnityEngine;

namespace PeggleWars.PlayerAttacks
{
    internal class HailStorm : PlayerAttack
    {
        [SerializeField] protected int _freezingStacks = 5;
        [SerializeField] protected int _frozenThreshold = 30;
        private Enemy _enemy;

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0003_ManaBlitz);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0010_BluntSpellImpact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            _enemy = _collision.GetComponent<Enemy>();
            _enemy.TakeIceDamage();
            _enemy.ApplyFreezing(_freezingStacks);

            int randomChance = UnityEngine.Random.Range(0, 101);
            if (randomChance < _frozenThreshold)
            {
                _enemy.ApplyFrozen();
            }
        }
    }
}
