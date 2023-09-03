using System.Collections;
using UnityEngine;
using PeggleWars.Characters.Interfaces;
using Utility.TurnManagement;
using Utility;
using DG.Tweening;
using Characters.UI;

namespace Characters.Enemies
{
    [RequireComponent(typeof(PopUpSpawner))]
    [RequireComponent(typeof(ScrollDisplayer))]
    [RequireComponent(typeof(EnemyStatusTooltipSpawner))]
    public abstract class Enemy : MonoBehaviour, IDamagable, IHaveDisplayDescription
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
        public bool IsFrozen { get; private set; }
        public int FrozenForTurns { get; private protected set; } = 0;
        public int BurningStacks { get; private set; } = 0;
        public int FreezingStacks { get; private set; } = 0;
        public int TemperatureSicknessStacks { get; private protected set; } = 0;
        public int EnragedStacks { get; private protected set;} = 0;

        protected float _takenDamageModifier = 1f;
        protected float _dealingDamageModifier = 1f;
        protected float _temperatureSicknessModifier = 0.05f;
        protected float _enragedModifier = 0.05f;

        //Stats
        [SerializeField] ScriptableEnemy _scriptableEnemy;
        protected int _attackFrequency;
        public EnemyAttackType AttackType { get; private set; }
        public int Damage { get; private protected set; }
        public int MaxHealth { get; private protected set; }
        public int Health { get; private set; } = 20;
        public bool IsFlying { get; private set; }
        public bool IsInAttackPosition { get; private set; }
        public int TurnsTillNextAttack { get; set; }
        public bool IsStationary { get; private set; }

        //Other
        protected bool _isDead;

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
            IsStationary = _scriptableEnemy.IsStationary;
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
                TakeDamage(BurningStacks, false);
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
                IsInAttackPosition = (Vector2)transform.position == walkerMeleeAttackPosition || (Vector2)transform.position == flyerMeleeAttackPosition;
            }

            if (EnragedStacks > 0)
            {
                EnragedStacks--;
                _dealingDamageModifier = 1 + (EnragedStacks * _enragedModifier);
                Damage = Mathf.CeilToInt(_scriptableEnemy.Damage * _dealingDamageModifier);
            }
        }

        public void TakeDamage(int damage, bool sourceIsAttack = true)
        {
            if (TemperatureSicknessStacks > 0)
                _takenDamageModifier += (TemperatureSicknessStacks * _temperatureSicknessModifier);

            if (sourceIsAttack)
            {
                damage = Mathf.CeilToInt(damage * _takenDamageModifier); //round to the next higher int in players favor
                Health -= damage;
                TemperatureSicknessStacks = 0;
                _takenDamageModifier = 1; //reset to default after using it once
            }
            else
                Health -= damage;

            if (damage > 0)
                _popUpSpawner.SpawnPopUp(damage);

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
            if(!_isDead)
            {
                _isDead = true;
                transform.DOKill(); //stop all tweens
                EnemyManager.Instance.EnemiesInScene.Remove(this);
                EnemyEvents.RaiseOnEnemyDeath();

                Collider2D collider = GetComponent<Collider2D>();
                collider.enabled = false;

                TriggerDeathAnimation();
                PlayDeathSound();
                OnDeathEffect();
            }
        }

        protected virtual IEnumerator DestroyThisEnemyWithDelay()
        {
            yield return new WaitForSeconds(_deathDelayForAnimation);
            Destroy(gameObject);
        }

        protected abstract void TriggerDeathAnimation();

        protected abstract void PlayDeathSound();

        protected abstract void OnDeathEffect();

        public virtual void Attack()
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

        public void ApplyBurning(int burningStacks, bool sourceIsRelic = false)
        {
            if (FreezingStacks > 0)
            {
                if (FreezingStacks >= burningStacks)
                {
                    FreezingStacks -= burningStacks;
                    ApplyTemperatureSickness(burningStacks);
                }
                else
                {
                    int sicknessStacks = burningStacks - FreezingStacks;
                    int remainingBurningStacks = burningStacks - sicknessStacks;
                    BurningStacks += remainingBurningStacks;
                    ApplyTemperatureSickness(sicknessStacks);
                }
            }
            else
                BurningStacks += burningStacks;

            if (!sourceIsRelic)
                EnemyEvents.RaiseAppliedBurning(this);
        }

        public void ApplyFreezing(int freezingStacks, bool sourceIsRelic = false)
        {
            if (BurningStacks > 0)
            {
                if (BurningStacks >= freezingStacks)
                {
                    BurningStacks -= freezingStacks;
                    ApplyTemperatureSickness(freezingStacks);
                }
                else
                {
                    int sicknessStacks = freezingStacks - BurningStacks;
                    int remainingFreezingStacks = freezingStacks - sicknessStacks;
                    BurningStacks += remainingFreezingStacks;
                    ApplyTemperatureSickness(sicknessStacks);
                }
            }
            else
                FreezingStacks += freezingStacks;

            CheckForFreezingKill();

            if (!sourceIsRelic)
                EnemyEvents.RaiseAppliedFreezing(this);
        }

        public void ApplyTemperatureSickness(int temperatureSicknessStacks = 1, bool sourceIsRelic = false)
        {
            TemperatureSicknessStacks += temperatureSicknessStacks;

            if (!sourceIsRelic)
                EnemyEvents.RaiseAppliedTemperatureSickness(this);
        }

        public void ApplyEnraged(int enragedStacks = 1)
        {
            EnragedStacks += enragedStacks;
            _dealingDamageModifier = 1 + (EnragedStacks * _enragedModifier);
            Damage = Mathf.CeilToInt(_scriptableEnemy.Damage * _dealingDamageModifier);
        }

        protected void CheckForFreezingKill()
        {
            if (FreezingStacks >= Health && Health != 0)
            {
                FreezingStacks = 0;
                TakeDamage(Health);
            }
        }

        public void ApplyFrozen(int frozenStacks = 1, bool sourceIsRelic = false)
        {
            IsFrozen = true;
            FrozenForTurns += frozenStacks;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue;

            if (!sourceIsRelic)
                EnemyEvents.RaiseAppliedFrozen(this);
        }

        protected void OnDestroy()
        {
            transform.DOKill();
        }

        public abstract void StartMovementAnimation();
        public abstract void StopMovementAnimation();

        #endregion
    }
}
