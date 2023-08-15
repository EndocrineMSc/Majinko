using Audio;
using UnityEngine;

namespace Attacks
{
    internal class WraithCasterAttack : ProjectileAttack
    {
        public override string Bark { get; } = "Whoh...";

        protected override void AdditionalDamageEffects(GameObject target)
        {
            //none
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0108_WraithCaster_Shot);
        }

        protected override void PlayHitSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

    }
}
