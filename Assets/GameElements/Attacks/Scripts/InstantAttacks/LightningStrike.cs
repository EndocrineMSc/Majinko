using Audio;
using UnityEngine;

namespace Attacks
{
    internal class LightningStrike : InstantAttack
    {
        public override string Bark { get; } = "Lightning Strike!";

        protected override void AdditionalDamageEffects(GameObject target)
        {
            //none
        }

        protected override void PlayAwakeSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0110_LightningStrike);
        }

        protected override void PlayHitSound()
        {
            //none
        }
    }
}
