using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class Icicle : ProjectileAttack
    {
        public override string Bark { get; } = "Icicle!";

        protected override void OnHitPolish()
        {
            base.OnHitPolish();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void AdditionalEffectsOnImpact(GameObject target)
        {
            if (target.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ApplyFreezing(_attackValues.FreezingStacks);

                int randomChance = UnityEngine.Random.Range(0, 101);
                if (randomChance < _attackValues.PercentToFreeze)
                    enemy.ApplyFrozen();
            }
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }
    }
}
