using EnumCollection;
using Audio;
using UnityEngine;

namespace Attacks
{
    internal class ManaBlitz : ProjectileAttack
    {
        public override string Bark { get; } = "Mana Blitz!";

        protected override void AdditionalDamageEffects(GameObject target)
        {
            //none yet
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }
    }
}
