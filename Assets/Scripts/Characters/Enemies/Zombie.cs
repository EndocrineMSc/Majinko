using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Orbs;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Enemies
{
    internal class Zombie : Enemy
    {
        #region Public Functions

        protected override void PlaySpawnSound()
        {
            //AudioManager.Instance.PlaySoundEffectOnce(SFX._0009_ZombieSpawn);
        }

        protected override void PlayDeathSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0011_ZombieDeath);
        }

        protected override void OnDeathEffect()
        {
            OrbManager.Instance.SwitchOrbs(OrbType.RottedOrb, 2);          
        }

        protected override void PlayHurtSound()
        {
            throw new System.NotImplementedException();
        }

        protected override void AdditionalAttackEffects()
        {
            throw new System.NotImplementedException();
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A regular old zombie. Will spawn two Rotten Mana orbs on death.";
        }

        #endregion
    }
}
