using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleAttacks.AttackVisuals.PopUps;
using PeggleWars.Characters.Interfaces;
using Utility.TurnManagement;
using System;
using PeggleWars.ScrollDisplay;
using DG.Tweening;
using Characters;

namespace Enemies
{
    [RequireComponent(typeof(PopUpSpawner))]
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Enemy : MonoBehaviour, IDamagable, IHaveDisplayDescription
    {
        #region Fields and Properties

        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;
        protected PhaseManager _turnManager;
        protected EnemyManager _enemyManager;

        protected readonly string ATTACK_PARAM = "Attack";
        protected readonly string HURT_PARAM = "Hurt";
        protected readonly string DEATH_PARAM = "Death";
        protected readonly string SPAWN_PARAM = "Spawn";
        protected readonly string SPEED_PARAM = "Speed";
        protected readonly string WALK_PARAM = "Walk";
        protected readonly string IDLE_PARAM = "Idle";
        protected float _deathDelayForAnimation = 1f;
        protected float _enemyWalkDuration = 1.5f;
        [SerializeField] protected int _attackFrequency;

        internal int FrozenForTurns { get; private protected set; } = 0;
        protected Color _baseColor;

        internal bool IsFrozen { get; private set; }

        [SerializeField] protected EnemyAttackType _enemyAttackType; //melee, ranged

        internal Vector2 CurrentPosition { get; set; }

        internal EnemyAttackType AttackType
        {
            get { return _enemyAttackType; }
            private set { _enemyAttackType = value; }
        }

        [SerializeField] protected int _damage;
        internal int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        internal int MaxHealth { get; private protected set; }

        [SerializeField] protected int _health = 20;
        internal int Health
        {
            get { return _health; }
            private set { _health = value; }
        }

        [SerializeField] private bool _isFlying;

        internal bool IsFlying
        {
            get { return _isFlying; }
            private set { _isFlying = value; }
        }

        [SerializeField] private bool _isInAttackPosition;

        internal bool IsInAttackPosition
        {
            get { return _isInAttackPosition; }
            private set { _isInAttackPosition = value; }
        }

        internal int TurnsTillNextAttack { get; set; }

        protected int _fireStacks;

        internal int FireStacks
        {
            get { return _fireStacks; }
            private set { _fireStacks = value; }
        }

        protected int _iceStacks;

        internal int IceStacks
        {
            get { return _iceStacks; }
            private set { _iceStacks = value; }
        }

        #endregion

        #region Functions

        private void Awake()
        {
            SetReferences();
            PlaySpawnSound();
            MaxHealth = Health;
        }

        protected virtual void SetReferences()
        {
            _animator = GetComponentInChildren<Animator>();
            TriggerSpawnAnimation();
            _popUpSpawner = GetComponent<PopUpSpawner>();
            _enemyManager = EnemyManager.Instance;
            SetDisplayDescription();
            _baseColor = GetComponentInChildren<SpriteRenderer>().color;
            TurnsTillNextAttack = _attackFrequency;
            SetDisplayScale();
        }

        protected abstract void PlaySpawnSound();
        protected abstract void TriggerSpawnAnimation();
        public abstract void SetDisplayDescription();

        protected virtual void OnEnable()
        {
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
            LevelPhaseEvents.OnEndEnemyPhase += OnEndEnemyPhase;
        }

        protected virtual void OnDisable()
        {
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
            LevelPhaseEvents.OnEndEnemyPhase -= OnEndEnemyPhase;
        }

        protected void OnStartEnemyPhase()
        {
            if (IsFrozen)
            {
                FrozenForTurns--;

                if (FrozenForTurns < 0)
                {
                    IsFrozen = false;
                    FrozenForTurns = 0;
                    GetComponentInChildren<SpriteRenderer>().color = _baseColor;
                }
            }

            if (_fireStacks > 0)
            {
                TakeDamage(_fireStacks);
            }
        }

        protected virtual void OnEndEnemyPhase()
        {
            if (_enemyAttackType == EnemyAttackType.Ranged)
            {
                _isInAttackPosition = true;
            }
            else
            {
                Vector2 walkerMeleeAttackPosition = _enemyManager.EnemyPositions[0, 0];
                Vector2 flyerMeleeAttackPosition = _enemyManager.EnemyPositions[1, 0];
                _isInAttackPosition = transform.position.Equals(walkerMeleeAttackPosition) || transform.position.Equals(flyerMeleeAttackPosition);
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            if (damage > 0)
            {
                _popUpSpawner.SpawnPopUp(damage);
            }
            TriggerHurtAnimation();
            PlayHurtSound();

            if (_health <= 0)
            {
                HandleDeath();
                StartCoroutine(DestroyThisEnemyWithDelay());
            }
        }

        protected abstract void TriggerHurtAnimation();

        protected abstract void PlayHurtSound();

        protected void HandleDeath()
        {
            transform.DOKill(); //stop all tweens
            EnemyManager.Instance.EnemiesInScene.Remove(this);
            EnemyEvents.RaiseOnEnemyDeath();
            TriggerDeathAnimation();

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;

            PlayDeathSound();
            OnDeathEffect();
        }

        protected virtual IEnumerator DestroyThisEnemyWithDelay()
        {
            yield return new WaitForSeconds(_deathDelayForAnimation);
            Destroy(gameObject);
        }

        protected abstract void TriggerDeathAnimation();

        protected abstract void PlayDeathSound();

        protected abstract void OnDeathEffect();

        internal virtual void Attack()
        {
            TriggerAttackAnimation();
            AdditionalAttackEffects();
        }

        protected abstract void TriggerAttackAnimation();

        protected abstract void AdditionalAttackEffects();

        internal void ResetTurnsTillNextAttack()
        {
            TurnsTillNextAttack = _attackFrequency;
        }

        internal void ApplyBurning(int fireStacks)
        {
            _fireStacks += fireStacks;
        }

        protected void SetDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 2;
        }

        internal void ApplyFreezing(int iceStacks)
        {
            _iceStacks += iceStacks;
        }

        internal void ApplyFrozen(int frozenStacks = 1)
        {
            IsFrozen = true;
            FrozenForTurns += frozenStacks;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue; //ToDo: Polish this
        }

        //Moves the enemy one "space" to the left
        internal IEnumerator Move()
        {
            int xIndexOfEnemy = GetEnemyPositionIndex(this);
            Vector2 endPosition;
            if (IsFlying)
            {
                endPosition = _enemyManager.EnemyPositions[1, xIndexOfEnemy - 1];
            }
            else
            {
                endPosition = _enemyManager.EnemyPositions[0, xIndexOfEnemy - 1];
            }
            StartMovementAnimation();
            transform.DOMoveX(endPosition.x, _enemyWalkDuration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(_enemyWalkDuration);
            StopMovementAnimation();
            CurrentPosition = endPosition;
        }

        private int GetEnemyPositionIndex(Enemy enemy)
        {
            Vector2 enemyPosition = enemy.transform.position;

            if (enemy.IsFlying)
            {
                for (int i = 0; i < _enemyManager.EnemyPositions.Length; i++)
                {
                    Vector2 indexPosition = _enemyManager.EnemyPositions[1, i];
                    if (indexPosition.Equals(enemyPosition))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _enemyManager.EnemyPositions.Length; i++)
                {
                    Vector2 indexPosition = _enemyManager.EnemyPositions[0, i];
                    if (indexPosition.Equals(enemyPosition))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        protected abstract void StartMovementAnimation();
        protected abstract void StopMovementAnimation();

        #endregion
    }
}
