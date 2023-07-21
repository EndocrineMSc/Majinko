using EnumCollection;
using Audio;
using Orbs;
using Utility;

namespace Characters.Enemies
{
    internal class WraithCaster : RangedEnemy, ICanBeIntangible
    {
        public int IntangibleStacks { get; set; } = 0;

        private int _amountIntangibleOrbs = 0;

        IntangibleController _intangibleController;

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            _deathDelayForAnimation = 1.5f;
            _intangibleController = new(this);
        }

        protected override void OnEndEnemyPhase()
        {
            base.OnEndEnemyPhase();
            OrbManager.Instance.SwitchOrbs(OrbType.IntangibleEnemyOrb, transform.position);
            _amountIntangibleOrbs += 1;
            HandleIntangibleStacks();
        }

        protected override void OnDeathEffect()
        {            
            int orbsToBeSwitched = _amountIntangibleOrbs;

            for (int i = 0; i < orbsToBeSwitched; i++)
            {
                OrbManager.Instance.ReplaceOrbOfType(OrbType.IntangibleEnemyOrb);
            }           
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A minor wraith. Most likely created out of one of your former apprentice compatriots." +
                "Will attack every other turn. Spawns intangible orbs in the arena granting them intagible when the orbs are hit.";
        }

        #region Intangible

        public void SetIntangible(int intangibleStacks = 1)
        {
            _intangibleController.SetIntangible(intangibleStacks);
        }

        public void RemoveIntangible()
        {
            _intangibleController.RemoveIntangible();
        }

        public void HandleIntangibleStacks()
        {
            _intangibleController.HandleIntangibleStacks();
        }

        #endregion

        #region Animation

        protected override void TriggerSpawnAnimation()
        {
            //Not necessary
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
        internal override void StartMovementAnimation()
        {
            //same as idle
        }

        internal override void StopMovementAnimation()
        {
            //same as idle
        }

        protected override void TriggerAttackAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(ATTACK_TRIGGER);
        }

        #endregion

        #region Sounds

        protected override void PlaySpawnSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0503_Wraith_Spawn);
        }

        protected override void PlayDeathSound()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0504_Wraith_Death);
        }


        protected override void PlayHurtSound()
        {
            //ToDo
        }

        public void DisplayOnScroll()
        {
            //ToDo
        }

        public void StopDisplayOnScroll()
        {
            //ToDo
        }

        #endregion

        #endregion
    }
}
