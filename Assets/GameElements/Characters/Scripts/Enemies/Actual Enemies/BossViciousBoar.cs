using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using DG.Tweening;
using System;
using Utility.TurnManagement;
using TMPro;
using EnumCollection;
using Orbs;

namespace Characters.Enemies
{
    public class BossViciousBoar : Enemy
    {
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private float _tackleDuration = 1f;
        private Collider2D _collider;
        private readonly string FIRE_TRIGGER = "Fire";
        private bool _isOnFire;
        [SerializeField] private TextMeshProUGUI _attackClock;
        [SerializeField] private RectTransform _attackClockTransform;

        public event Action OnBoarAttack;

        protected override void Start()
        {
            base.Start();
            _startPosition = transform.position;
            _targetPosition = Player.Instance.gameObject.transform.position;
            _collider = GetComponent<Collider2D>();
            _deathDelayForAnimation = 2f;
        }

        private void Update()
        {
            if (!_isOnFire && Health <= 20)
                SetOnFire();

            if (_attackClock.text != TurnsTillNextAttack.ToString())
            {
                _attackClock.text = TurnsTillNextAttack.ToString();
                if (TurnsTillNextAttack != 0 && TurnsTillNextAttack != _attackFrequency)
                    _attackClockTransform.DOPunchScale(_attackClockTransform.localScale * 1.02f, 0.4f, 1, 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
                enemy.TakeDamage(Mathf.RoundToInt(Damage * _dealingDamageModifier));
        }

        private void SetOnFire()
        {
            _isOnFire = true;
            _animator.SetTrigger(FIRE_TRIGGER);
            Damage = Mathf.RoundToInt(Damage * 1.5f);
            _attackFrequency--;
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "<size=120%><b>Vicious Boar</b><size=20%>\n\n<size=100%>Something seems to have enraged this massive beast. " +
                " Morovian traders pay a fortune for the magically infused fur of these boars. Will attack after its counter reaches zero.";
        }

        protected override void AdditionalAttackEffects()
        {
            ChargeLeft();
        }

        private void ChargeLeft()
        {
            StartMovementAnimation();
            transform.DOMoveX(_targetPosition.x, _tackleDuration).SetEase(Ease.InCubic).OnComplete(TurnAndChargeRight);
        }

        private void TurnAndChargeRight()
        {
            Player.Instance.TakeDamage(Mathf.RoundToInt(Damage * _dealingDamageModifier));
            OnBoarAttack?.Invoke();
            StartCoroutine(OrbManager.Instance.SwitchOrbs(OrbType.PineConeOrb, transform.position, 3));
            _collider.enabled = false;
            //ToDo: Trigger turn right animation
            transform.DOMoveX(_startPosition.x, _tackleDuration).SetEase(Ease.InCubic).OnComplete(TriggerTurnLeftAnimation);
        }

        private void TriggerTurnLeftAnimation()
        {
            _animator.SetTrigger(IDLE_TRIGGER);
            _collider.enabled = true;
            //ToDo: Trigger turn left animation
        }

        protected override void OnDeathEffect()
        {
            StartCoroutine(TriggerLevelWon());
        }

        private IEnumerator TriggerLevelWon()
        {
            yield return new WaitForSeconds(_deathDelayForAnimation * 0.9f);
            UtilityEvents.RaiseLevelVictory();
        }

        #region Sounds

        protected override void PlayDeathSound()
        {
            //tbd
        }

        protected override void PlayHurtSound()
        {
            //tbd
        }

        protected override void PlaySpawnSound()
        {
            //tbd
        }

        #endregion

        #region Animation Triggers

        protected override void TriggerAttackAnimation()
        {
            //tbd
        }

        protected override void TriggerDeathAnimation()
        {
            //tbd
        }

        protected override void TriggerHurtAnimation()
        {
            //tbd
        }

        protected override void TriggerSpawnAnimation()
        {
            //tbd
        }

        public override void StartMovementAnimation()
        {
            _animator.SetTrigger(WALK_TRIGGER);
        }

        public override void StopMovementAnimation()
        {
            _animator.SetTrigger(IDLE_TRIGGER);
        }

        #endregion
    }
}
