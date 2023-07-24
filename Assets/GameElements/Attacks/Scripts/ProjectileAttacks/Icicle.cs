using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class Icicle : ProjectileAttack
    {
        public override string Bark { get; } = "Icicle!";

        //Do special stuff in here
        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            Enemy enemy = _collider.GetComponent<Enemy>();
            enemy.ApplyFreezing(_attackValues.FreezingStacks);

            int randomChance = UnityEngine.Random.Range(0, 101);
            if (randomChance < _attackValues.PercentToFreeze)
            {
                enemy.ApplyFrozen();
            }
        }
    }
}
