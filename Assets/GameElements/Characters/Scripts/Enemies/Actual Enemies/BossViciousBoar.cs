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
        private readonly float _tackleDuration = 1f;
        private Collider2D _collider;
        private readonly string FIRE_TRIGGER = "Fire";
        private readonly string TURN_TRIGGER = "Turn";
        private bool _isOnFire;
        private float _onFireDamageModifier = 1;
        private readonly int _fireAttackCooldownReduction = 1;
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
                enemy.TakeDamage(Mathf.RoundToInt(EnemyObject.Damage * _dealingDamageModifier * _onFireDamageModifier));
        }

        private void SetOnFire()
        {
            _isOnFire = true;
            _animator.SetTrigger(FIRE_TRIGGER);
            _onFireDamageModifier = 1.5f;
        }

        protected override void ResetAttackCooldown()
        {
            if (!_isOnFire)
                TurnsTillNextAttack = EnemyObject.AttackFrequency;
            else
                TurnsTillNextAttack = EnemyObject.AttackFrequency - _fireAttackCooldownReduction;
        }

        protected override int CalculateAttackDamage()
        {
            //Will cause Enemy.Attack not to trigger damage to the player, since I want this to happen at boar impact with the player instead
            return 0; 
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
            var damage = Mathf.CeilToInt(EnemyObject.Damage * _dealingDamageModifier * _onFireDamageModifier);
            float amplitude = 1 + damage / 2.5f;
            float shakeTime = (float)damage / 50;

            if (Player.Instance != null)
                Player.Instance.TakeDamage(damage);
            else
                Debug.Log("Boss Boar couldn't deal damage. It thinks player is null");

            if (ScreenShaker.Instance != null)
                ScreenShaker.Instance.ShakeCamera(amplitude, shakeTime);
            else
                Debug.Log("Couldn't shake screen. Thinks ScreenShaker is null");

            OnBoarAttack?.Invoke();
            OrbManager.Instance.SwitchOrbs(_pineMouseOrbData, transform.position, 3);
            _collider.enabled = false;
            _animator.SetTrigger(TURN_TRIGGER); //Animation Event Flips Renderer and another event triggers HandleTurnFollowUp
        }

        public void HandleTurnFollowUp()
        {
            if (_spriteRenderer.flipX)
            {
                _animator.SetTrigger(WALK_TRIGGER);
                transform.DOMoveX(_startPosition.x, _tackleDuration).SetEase(Ease.Linear).OnComplete(TriggerTurnLeftAnimation);
            }
            else
            {
                _animator.SetTrigger(IDLE_TRIGGER);
            }
        }

        public void FlipRenderer()
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        private void TriggerTurnLeftAnimation()
        {
            _animator.SetTrigger(TURN_TRIGGER); //Animation Event Flips Renderer and another event triggers HandleTurnFollowUp
            _collider.enabled = true;
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
