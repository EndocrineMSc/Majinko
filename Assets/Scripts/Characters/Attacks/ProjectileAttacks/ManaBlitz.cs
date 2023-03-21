using EnumCollection;
using PeggleWars.Audio;

namespace PeggleWars.Attacks
{
    internal class ManaBlitz : ProjectileAttack
    {
        public override string Bark { get; } = "Mana Blitz!";

        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0003_ManaBlitz);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0010_BluntSpellImpact);
        }

        protected override void AdditionalEffectsOnImpact()
        {
            //empty
        }
    }
}
