using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Enemies;
using UnityEngine;

namespace PeggleWars.Attacks
{
    internal class Icicle : ProjectileAttack
    {
        [SerializeField] protected int _freezingStacks = 5;
        [SerializeField] protected int _frozenThreshold = 20;

        public override string Bark { get; } = "Icicle!";

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
            Enemy enemy = _collision.GetComponent<Enemy>();
            enemy.TakeIceDamage();
            enemy.ApplyFreezing(_freezingStacks);

            int randomChance = UnityEngine.Random.Range(0, 101);
            if (randomChance < _frozenThreshold)
            {
                enemy.ApplyFrozen();
            }
        }
    }
}
