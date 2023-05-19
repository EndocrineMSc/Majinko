using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleAttacks.AttackVisuals.PopUps;
using PeggleWars.Characters.Interfaces;
using Utility.TurnManagement;
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

        //External References
        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;
        protected PhaseManager _phaseManager;
        protected EnemyManager _enemyManager;

        //Animation
        protected readonly string ATTACK_TRIGGER = "Attack";
        protected readonly string HURT_TRIGGER = "Hurt";
        protected readonly string DEATH_TRIGGER = "Death";
        protected readonly string SPAWN_TRIGGER = "Spawn";
        protected readonly string SPEED_PARAM = "Speed";
        protected readonly string WALK_TRIGGER = "Walk";
        protected readonly string IDLE_TRIGGER = "Idle";
        protected float _deathDelayForAnimation = 1f;
        protected Color _baseColor;

        //Status Effects
        internal bool IsFrozen { get; private set; }
        internal int FrozenForTurns { get; private protected set; } = 0;
        internal int BurningStacks { get; private set; } = 0;
        internal int FreezingStacks { get; private set; } = 0;

        //Stats
        [SerializeField] ScriptableEnemy _scriptableEnemy;
        protected int _attackFrequency;
        internal EnemyAttackType AttackType { get; private set; }
        internal int Damage { get; private set; }
        internal int MaxHealth { get; private protected set; }
        internal int Health { get; private set; } = 20;
        internal bool IsFlying { get; private set; }
        internal bool IsInAttackPosition { get; private set; }
        internal int TurnsTillNextAttack { get; set; }

        #endregion

        #region Functions

        protected virtual void Start()
        {
            PlaySpawnSound();
            TriggerSpawnAnimation();
            SetDisplayDescription();
            SetDisplayScale();
        }

        protected virtual void SetReferences()
        {
            _animator = GetComponentInChildren<Animator>();
            _popUpSpawner = GetComponent<PopUpSpawner>();
            _enemyManager = EnemyManager.Instance;
            _baseColor = GetComponentInChildren<SpriteRenderer>().color;
        }

        protected virtual void SetEnemyStats()
        {
            _attackFrequency = _scriptableEnemy.AttackFrequency;
            AttackType = _scriptableEnemy.AttackType;
            Damage = _scriptableEnemy.Damage;
            IsFlying = _scriptableEnemy.IsFlying;
            MaxHealth = _scriptableEnemy.MaxHealth;
            Health = MaxHealth;
            TurnsTillNextAttack = _attackFrequency;
        }

        protected abstract void PlaySpawnSound();
        protected abstract void TriggerSpawnAnimation();
        public abstract void SetDisplayDescription();

        protected virtual void OnEnable()
        {
            //in OnEnable to prevent timing null reference issues
            SetReferences(); 
            SetEnemyStats();

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

            if (BurningStacks > 0)
            {
                TakeDamage(BurningStacks);
            }
        }

        protected virtual void OnEndEnemyPhase()
        {
            if (AttackType == EnemyAttackType.Ranged)
            {
                IsInAttackPosition = true;
            }
            else
            {
                Vector2 walkerMeleeAttackPosition = _enemyManager.EnemyPositions[0, 0];
                Vector2 flyerMeleeAttackPosition = _enemyManager.EnemyPositions[1, 0];
                IsInAttackPosition = transform.position.Equals(walkerMeleeAttackPosition) || transform.position.Equals(flyerMeleeAttackPosition);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (damage > 0)
            {
                _popUpSpawner.SpawnPopUp(damage);
            }
            TriggerHurtAnimation();
            PlayHurtSound();

            if (Health <= 0)
            {
                HandleDeath();
                StartCoroutine(DestroyThisEnemyWithDelay());
            }

            CheckForFreezingKill();
        }

        protected abstract void TriggerHurtAnimation();

        protected abstract void PlayHurtSound();

        protected void HandleDeath()
        {
            transform.DOKill(); //stop all tweens
            EnemyManager.Instance.EnemiesInScene.Remove(this);
            EnemyEvents.RaiseOnEnemyDeath();

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;

            TriggerDeathAnimation();
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
            TurnsTillNextAttack = _attackFrequency;
        }

        protected abstract void TriggerAttackAnimation();

        protected abstract void AdditionalAttackEffects();


        protected void SetDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 2;
        }

        internal void ApplyBurning(int burningStacks)
        {
            BurningStacks += burningStacks;
        }

        internal void ApplyFreezing(int freezingStacks)
        {
            FreezingStacks += freezingStacks;
            CheckForFreezingKill();
        }

        protected void CheckForFreezingKill()
        {
            if (FreezingStacks >= Health && Health != 0)
            {
                FreezingStacks = 0;
                TakeDamage(Health);
            }
        }

        internal void ApplyFrozen(int frozenStacks = 1)
        {
            IsFrozen = true;
            FrozenForTurns += frozenStacks;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }

        internal abstract void StartMovementAnimation();
        internal abstract void StopMovementAnimation();

        #endregion
    }
}
