using Audio;
using Characters.Enemies;
using UnityEngine;
using PeggleWars.Attacks;

namespace Attacks
{
    internal class HailStorm : AreaAttack
    {
        public override string Bark { get; } = "Hail Storm!";

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

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
    }
}
