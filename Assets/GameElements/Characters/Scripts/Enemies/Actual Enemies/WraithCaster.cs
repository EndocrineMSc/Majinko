using EnumCollection;
using Audio;
using Orbs;
using Utility;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Characters.Enemies
{
    internal class WraithCaster : RangedEnemy, ICanBeIntangible
    {
        public int IntangibleStacks { get; set; } = 0;

        private int _amountIntangibleOrbs = 0;
        private bool _abilityReady = true;

        [SerializeField] private ParticleSystem _particleSystem;

        IntangibleController _intangibleController;

        #region Functions

        protected override void SetReferences()
        {
            base.SetReferences();
            _deathDelayForAnimation = _particleSystem.main.startLifetimeMultiplier;
            _intangibleController = new(this);
        }

        protected override void OnEndEnemyPhase()
        {
            base.OnEndEnemyPhase();
            if (_abilityReady) 
            {
                _abilityReady = false;
                OrbManager.Instance.SwitchOrbs(OrbType.IntangibleEnemyOrb, transform.position);
                _amountIntangibleOrbs += 1;
            }
            else
            {
                _abilityReady = true;
            }
            HandleIntangibleStacks();
        }

        protected override void OnDeathEffect()
        {            
            int orbsToBeSwitched = _amountIntangibleOrbs;

            for (int i = 0; i < orbsToBeSwitched; i++)
                OrbManager.Instance.ReplaceOrbOfType(OrbType.IntangibleEnemyOrb);         
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Leaf Mage</b><size=20%>\n\n<size=100%>These magical guardians of the forest rustle softly in the wind. While usually peaceful, something seems to have agitated them." +
                " Will attack and spawn an <b>Intangible Sphere</b> every other turn. They will become intangible when these orbs are hit.";
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
                _animator.enabled = false;
            StartCoroutine(FadeAndDisableSpriteRenderer());
            _particleSystem.Play();
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

        private IEnumerator FadeAndDisableSpriteRenderer()
        {
            var renderer = GetComponent<SpriteRenderer>();
            renderer.DOFade(0, 0.1f);
            yield return new WaitForSeconds(0.11f);
            renderer.enabled = false;
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
