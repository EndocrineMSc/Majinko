using Audio;
using UnityEngine;

namespace Attacks
{
    internal class WraithCasterAttack : ProjectileAttack
    {
        public override string Bark { get; } = "Whoh...";

        protected override void AdditionalEffectsOnImpact(GameObject target)
        {
            //none
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
