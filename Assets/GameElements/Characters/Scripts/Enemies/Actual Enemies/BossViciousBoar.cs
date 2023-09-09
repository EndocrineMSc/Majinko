using System.Collections;
using UnityEngine;
using Utility;
using DG.Tweening;
using System;
using TMPro;
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
        [SerializeField] private OrbData _pineMouseOrbData;

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
                if (TurnsTillNextAttack != 0 && TurnsTillNextAttack != EnemyObject.AttackFrequency)
                    _attackClockTransform.DOPunchScale(_attackClockTransform.localScale * 1.02f, 0.4f, 1, 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
                enemy.TakeDamage(Mathf.RoundToInt(EnemyObject.Damage * _dealingDamageModifier));
        }

        private void SetOnFire()
        {
            _isOnFire = true;
            _animator.SetTrigger(FIRE_TRIGGER);
            /*
            Damage = Mathf.RoundToInt(EnemyObject.Damage * 1.5f);
            _attackFrequency--;
            */
        }
        
        
        protected override void AttackEffect()
        {
            ChargeLeft();
        }

        private void ChargeLeft()
        {
            TriggerWalkAnimation();
            transform.DOMoveX(_targetPosition.x, _tackleDuration).SetEase(Ease.InCubic).OnComplete(TurnAndChargeRight);
        }

        private void TurnAndChargeRight()
        {
            Player.Instance.TakeDamage(Mathf.RoundToInt(EnemyObject.Damage * _dealingDamageModifier));
            OnBoarAttack?.Invoke();
            OrbManager.Instance.SwitchOrbs(_pineMouseOrbData, transform.position, 3);
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

        protected override void StartTurnEffect()
        {
            //No start turn effect
        }

        protected override void EndTurnEffect()
        {
            //No end turn effect
        }
    }
}
