using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;

namespace Characters.Enemies
{
    internal class StoneGolem : MeleeEnemy
    {
        private OrbManager _orbManager;
        private readonly int _abilityCountdown = 2;
        private int _abilityCountdownCounter = 0;

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            _orbManager = OrbManager.Instance;
        }

        protected override void OnEndEnemyPhase()
        {
            base.OnEndEnemyPhase();

            if (_abilityCountdownCounter <= 0)
                ActivateAbility();
            else
                _abilityCountdownCounter--;
        }

        private void ActivateAbility()
        {
            _abilityCountdownCounter = _abilityCountdown;
            _orbManager.SwitchOrbsWrap(OrbType.StoneOrb, transform.position, 3);
        }

        protected override void PlaySpawnSound()
        {
            //ToDo: individual sound
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0501_Zombie_Spawn);
        }

        protected override void PlayDeathSound()
        {
            //ToDo: individual sound
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0502_Zombie_Death);
        }

        protected override void OnDeathEffect()
        {
            //none          
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
            displayOnScroll.DisplayDescription = "<size=120%><b>Stone Golem</b><size=20%>\n\n<size=100%>Either awakend rock or petrified adventurers - you hope for the first. Will spawn three <b>Stone Orbs</b> every third turn.";
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
            /*
            if (_animator != null)
                _animator.SetTrigger(ATTACK_TRIGGER);
            */
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
            /*
            if (_animator != null)
                _animator.SetTrigger(DEATH_TRIGGER);
            */
        }

        #endregion
    }
}
