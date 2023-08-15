using Audio;
using Characters.Enemies;
using UnityEngine;

namespace Attacks
{
    internal class FireArrow : ProjectileAttack
    {
        public override string Bark { get; } = "Fire Arrow!";

        protected override void AdditionalEffectsOnImpact(GameObject target)
        { 
            if (target.TryGetComponent<Enemy>(out var enemy))
                enemy.ApplyBurning(_attackValues.BurningStacks);
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }
    }
}
