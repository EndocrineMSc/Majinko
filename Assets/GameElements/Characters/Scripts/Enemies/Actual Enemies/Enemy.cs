using System.Collections;
using UnityEngine;
using PeggleWars.Characters.Interfaces;
using Utility.TurnManagement;
using Utility;
using DG.Tweening;
using Characters.UI;
using Audio;

namespace Characters.Enemies
{
    [RequireComponent(typeof(PopUpSpawner))]
    [RequireComponent(typeof(ScrollDisplayer))]
    [RequireComponent(typeof(EnemyStatusTooltipSpawner))]
    public abstract class Enemy : MonoBehaviour, IDamagable, IHaveDisplayDescription
    {
        #region Fields and Properties

        [SerializeField] protected ScriptableEnemy _enemyObject;

        public ScriptableEnemy EnemyObject
        {
            get { return _enemyObject; }
            private protected set { _enemyObject = value; }
        }

        //External References
        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;
        protected PhaseManager _phaseManager;
        protected EnemyManager _enemyManager;
        protected SpriteRenderer _spriteRenderer;

        //Animation
        protected readonly string ATTACK_TRIGGER = "Attack";
        protected readonly string HURT_TRIGGER = "Hurt";
        protected readonly string DEATH_TRIGGER = "Death";
        protected readonly string SPAWN_TRIGGER = "Spawn";
        protected readonly string SPEED_PARAM = "Speed";
        protected readonly string WALK_TRIGGER = "Walk";
        protected readonly string IDLE_TRIGGER = "Idle";
        protected float _deathDelayForAnimation = 1f;
        protected Color _baseColor = Color.white;

        //Status Effects
        public bool IsFrozen { get; private protected set; }
        public int FrozenForTurns { get; private protected set; } = 0;
        public int BurningStacks { get; private protected set; } = 0;
        public int FreezingStacks { get; private protected set; } = 0;
        public int TemperatureSicknessStacks { get; private protected set; } = 0;
        public int EnragedStacks { get; private protected set;} = 0;
        public int IntangibleStacks { get; private set; } = 0;

        protected float _takenDamageModifier = 1f;
        protected float _dealingDamageModifier = 1f;
        protected float _temperatureSicknessModifier = 0.05f;
        protected float _enragedModifier = 0.05f;

        //Stats
        public int Health { get; private protected set; } = 20;
        public bool IsInAttackPosition { get; private protected set; }
        public int TurnsTillNextAttack { get; private protected set; }
        public int AbilityCooldown { get; private protected set; }

        //Other
        protected bool _isDead;

        #endregion

        #region Methods

        #region Initiatlization

        protected virtual void OnEnable()
        {
            LevelPhaseEvents.OnStartEnemyPhase += OnStartEnemyPhase;
            LevelPhaseEvents.OnEndEnemyPhase += OnEndEnemyPhase;
            EnemyEvents.OnIntangibleTriggered += ApplyIntangible;
        }

        protected virtual void OnDisable()
        {
            LevelPhaseEvents.OnStartEnemyPhase -= OnStartEnemyPhase;
            LevelPhaseEvents.OnEndEnemyPhase -= OnEndEnemyPhase;
            EnemyEvents.OnIntangibleTriggered -= ApplyIntangible;
        }

        protected virtual void Start()
        {
            SetReferences();
            ReadScriptableEnemy();
            TriggerSpawnAnimation();
            PlaySpawnSound();
        }

        protected virtual void SetReferences()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _popUpSpawner = GetComponent<PopUpSpawner>();
            _enemyManager = EnemyManager.Instance;
            _player = Player.Instance;
        }

        protected void ReadScriptableEnemy()
        {
            Health = EnemyObject.MaxHealth;
            AbilityCooldown = EnemyObject.AbilityCooldownMax;
            TurnsTillNextAttack = EnemyObject.AttackFrequency;
            SetDisplayDescription();
            SetDisplayScale();
        }

        public void SetDisplayDescription()
        {
            if (TryGetComponent<IDisplayOnScroll>(out IDisplayOnScroll displayOnScroll))
                displayOnScroll.DisplayDescription = EnemyObject.Description;
        }

        protected void SetDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = EnemyObject.DisplayScale;
        }

        #endregion

        #region Enemy Effects

        protected abstract void StartTurnEffect();
        protected abstract void AttackEffect();
        protected abstract void OnDeathEffect();
        protected abstract void EndTurnEffect();

        #endregion

        #region Enemy Life Cycle

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

        public void TakeDamage(int damage, bool sourceIsAttack = true)
        {
            if (IntangibleStacks <= 0)
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
        }

        public virtual void Attack()
        {
            TriggerAttackAnimation();
            AttackEffect();
            TurnsTillNextAttack = EnemyObject.AttackFrequency;

            if (EnemyObject.AttackType == EnemyAttackType.Melee)
            {
                var damage = Mathf.CeilToInt(EnemyObject.Damage * _dealingDamageModifier);
                _player.TakeDamage(damage);
            }
        }

        protected virtual void OnEndEnemyPhase()
        {
            if (EnemyObject.AttackType == EnemyAttackType.Ranged)
            {
                IsInAttackPosition = true;
            }
            else
            {
                Vector2 walkerMeleeAttackPosition = EnemyManager.Instance.EnemyPositions[0, 0];
                Vector2 flyerMeleeAttackPosition = EnemyManager.Instance.EnemyPositions[1, 0];
                IsInAttackPosition = (Vector2)transform.position == walkerMeleeAttackPosition || (Vector2)transform.position == flyerMeleeAttackPosition;
            }

            if (EnragedStacks > 0)
            {
                EnragedStacks--;
                _dealingDamageModifier = 1 + (EnragedStacks * _enragedModifier);
            }

            if (EnemyObject.CanBeIntangible)
                RemoveIntangibleStack();

            if (AbilityCooldown > 0)
                AbilityCooldown--;

            EndTurnEffect();
        }

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

        #endregion

        #region Accessor Methods

        public void ReduceTurnsTillNextAttack(int amount = 1)
        {
            TurnsTillNextAttack -= amount;
        }

        #endregion

        #region Status Effects

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

        public void ApplyIntangible(int intangibleStacks)
        {
            if (EnemyObject.CanBeIntangible)
            {
                IntangibleStacks += intangibleStacks;

                Color alpha = _spriteRenderer.color;
                alpha.a = 0.5f;
                _spriteRenderer.color = alpha;
            }
        }

        private void RemoveIntangibleStack()
        {
            if (IntangibleStacks > 0)
                IntangibleStacks--;

            if (IntangibleStacks <= 0)
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();

                Color alpha = spriteRenderer.color;
                alpha.a = 1f;
                spriteRenderer.color = alpha;
            }
        }

        protected void OnDestroy()
        {
            transform.DOKill();
        }

        #endregion

        #region Animations

        protected void TriggerSpawnAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(SPAWN_TRIGGER);
        }

        protected void TriggerAttackAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected void TriggerHurtAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(HURT_TRIGGER);
        }

        protected void TriggerDeathAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(DEATH_TRIGGER);
        }

        public void TriggerWalkAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(WALK_TRIGGER);
        }

        public void TriggerIdleAnimation()
        {
            if (_animator != null)
                _animator.SetTrigger(IDLE_TRIGGER);
        }

        #endregion

        #region Sounds

        protected void PlaySpawnSound()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffectOnce(EnemyObject.SpawnSound);
        }

        protected void PlayHurtSound()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffectOnce(EnemyObject.HurtSound);
        }

        protected void PlayDeathSound()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySoundEffectOnce(EnemyObject.DeathSound);
        }

        #endregion

        #endregion
    }
}
