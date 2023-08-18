using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;

namespace Characters.Enemies
{
    internal class Zombie : MeleeEnemy
    {
        [SerializeField] private ParticleSystem _particleSystem;

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
            StartCoroutine(OrbManager.Instance.SwitchOrbs(OrbType.RottedOrb, transform.position, 2));          
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
            displayOnScroll.DisplayDescription = "<size=120%><b>Corrupted Shroombie</b><size=20%>\n\n<size=100%>Normally protectors of the forest, these corrupted sentinels now attack on sight. Will spawn two <b>Rotten Mana Spheres</b> on death.";
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

            _particleSystem.Play();
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
