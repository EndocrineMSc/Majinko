using Audio;

namespace Attacks
{
    internal class LightningStrike : InstantAttack
    {
        public override string Bark { get; } = "Lightning Strike!";

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
