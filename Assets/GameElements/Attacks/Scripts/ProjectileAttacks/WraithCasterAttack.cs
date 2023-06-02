using EnumCollection;
using Audio;

namespace Attacks
{
    internal class WraithCasterAttack : ProjectileAttack
    {
        public override string Bark { get; } = "Whoh...";

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0101_ManaBlitz_Shot);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0103_Blunt_Spell_Impact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //none
        }
    }
}
