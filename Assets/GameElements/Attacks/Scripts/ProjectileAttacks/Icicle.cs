using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class Icicle : ProjectileAttack
    {
        public override string Bark { get; } = "Icicle!";

        protected override void AdditionalDamageEffects(GameObject target)
        {
            if (target.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ApplyFreezing(_attackValues.FreezingStacks);

                int randomChance = UnityEngine.Random.Range(0, 101);
                if (randomChance < _attackValues.PercentToFreeze)
                    enemy.ApplyFrozen();
            }
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0106_Icicle_Shot);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0107_Icicle_Impact);
        }

    }
}
