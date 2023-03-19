using EnumCollection;
using PeggleWars.Audio;
using PeggleWars.Orbs;
using PeggleWars.ScrollDisplay;
using UnityEngine;

namespace PeggleWars.Enemies
{
    internal class Zombie : MeleeEnemy
    {
        #region Functions

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
            //ToDo
        }

        protected override void AdditionalAttackEffects()
        {
            //ToDo
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A regular old zombie. Will spawn two Rotten Mana orbs on death.";
        }

        protected override void StartMovementAnimation()
        {
            _animator.SetFloat(SPEED_PARAM, 1);
        }

        protected override void StopMovementAnimation()
        {
            _animator.SetFloat(SPEED_PARAM, 0);
        }

        protected override void TriggerAttackAnimation()
        {
            _animator.SetTrigger(ATTACK_PARAM);
        }

        protected override void TriggerSpawnAnimation()
        {
            _animator.SetTrigger(SPAWN_PARAM);
        }

        protected override void TriggerHurtAnimation()
        {
            _animator.SetTrigger(HURT_PARAM);
        }

        protected override void TriggerDeathAnimation()
        {
            _animator.SetTrigger(DEATH_PARAM);
        }

        #endregion
    }
}
