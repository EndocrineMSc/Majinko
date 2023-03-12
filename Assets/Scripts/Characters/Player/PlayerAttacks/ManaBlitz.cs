using EnumCollection;
using PeggleWars.Audio;

namespace PeggleWars.PlayerAttacks.ManaBlitz
{
    internal class ManaBlitz : PlayerAttack
    {
        //Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0003_ManaBlitz);
        }

        protected override void OnHitPolish()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0010_BluntSpellImpact);
        }
    }
}
