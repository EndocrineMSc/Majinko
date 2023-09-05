using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;

namespace Characters.Enemies
{
    internal class GenericMeleeAddMonster : MeleeEnemy
    {

        #region Functions

        protected override void PlaySpawnSound()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0501_Zombie_Spawn);
        }

        protected override void PlayDeathSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0502_Zombie_Death);
        }

        protected override void OnDeathEffect()
        {
            //No Death Effect   
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
            displayOnScroll.DisplayDescription = "<size=120%><b>Pine Mouse</b><size=20%>\n\n<size=100%>Shaken violently from their sleep in the trees, these pine mice are out for revenge.";
        }

        public override void StartMovementAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(WALK_TRIGGER);
        }

        public override void StopMovementAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(IDLE_TRIGGER);
        }

        protected override void TriggerAttackAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected override void TriggerSpawnAnimation()
        {
            //not available
        }

        protected override void TriggerHurtAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(HURT_TRIGGER);
        }

        protected override void TriggerDeathAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(DEATH_TRIGGER);
        }

        #endregion
    }
}