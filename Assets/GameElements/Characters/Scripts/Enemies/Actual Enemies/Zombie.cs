using EnumCollection;
using Audio;
using Orbs;
using Utility;

namespace Characters.Enemies
{
    internal class Zombie : MeleeEnemy
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
            OrbManager.Instance.SwitchOrbs(OrbType.RottedOrb, transform.position, 2);          
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

        internal override void StartMovementAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(WALK_TRIGGER);
        }

        internal override void StopMovementAnimation()
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
            //_animator.SetTrigger(SPAWN_PARAM);
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
