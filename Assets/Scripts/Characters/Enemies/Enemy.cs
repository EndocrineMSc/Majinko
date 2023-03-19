using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleAttacks.AttackVisuals.PopUps;
using PeggleWars.Characters.Interfaces;
using PeggleWars.TurnManagement;
using System;
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Enemies
{
    [RequireComponent(typeof(PopUpSpawner))]
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Enemy : MonoBehaviour, IDamagable, IHaveDisplayDescription
    {
        #region Fields and Properties

        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;
        protected TurnManager _turnManager;
        protected EnemyManager _enemyManager;

        protected readonly string ATTACK_PARAM = "Attack";
        protected readonly string HURT_PARAM = "Hurt";
        protected readonly string DEATH_PARAM = "Death";
        protected readonly string SPAWN_PARAM = "Spawn";
        protected readonly string SPEED_PARAM = "Speed";
        protected float _deathDelayForAnimation = 1f;
        protected float _enemySpeed = 2;
        [SerializeField] protected int _attackFrequency;

        protected int _frozenForTurns = 0;
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
        }

        protected virtual void OnDisable()
        {
            TurnManager.Instance.EndEnemyTurn?.RemoveListener(OnEndEnemyTurn);
            TurnManager.Instance.StartEnemyTurn?.RemoveListener(OnStartEnemyTurn);
        }

        protected virtual void SetReferences()
        {
            _animator = GetComponentInChildren<Animator>();
            TriggerSpawnAnimation();
            _popUpSpawner = GetComponent<PopUpSpawner>();
            _enemyManager = EnemyManager.Instance;
            SetDisplayDescription();
            _baseColor = GetComponentInChildren<SpriteRenderer>().color;
            TurnManager.Instance.EndEnemyTurn?.AddListener(OnEndEnemyTurn);
            TurnManager.Instance.StartEnemyTurn?.AddListener(OnStartEnemyTurn);
            TurnsTillNextAttack = _attackFrequency;
        }

        protected virtual void OnEndEnemyTurn()
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

        protected void OnStartEnemyTurn()
        {
            if(IsFrozen)
            {
                _frozenForTurns--;

                if(_frozenForTurns < 0)
                {
                    IsFrozen = false;
                    _frozenForTurns = 0;
                    GetComponentInChildren<SpriteRenderer>().color = _baseColor;
                }
            }

            if (_fireStacks > 0)
            {
                TakeDamage(_fireStacks);
            }
        }

        internal void ResetTurnsTillNextAttack()
        {
            TurnsTillNextAttack = _attackFrequency;
        }

        internal void ApplyFrozen(int frozenStacks = 1)
        {
            IsFrozen = true;
            _frozenForTurns += frozenStacks;
            GetComponentInChildren<SpriteRenderer>().color = Color.blue; //ToDo: Polish this
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            if (damage > 0)
            {
                _popUpSpawner.SpawnPopUp(damage);
            }
            TriggerHurtAnimation();
            try
            {
                PlayHurtSound();
            }
            catch (NotImplementedException)
            {
                //ToDo: Implement Sound
            }

            if (_health <= 0)
            {
                HandleDeath();
                StartCoroutine(DestroyThisEnemyWithDelay());
            }
        }

        internal virtual void Attack()
        {
            TriggerAttackAnimation();           
            AdditionalAttackEffects();
        }

        protected abstract void TriggerAttackAnimation();

        private void HandleDeath()
        {
            EnemyManager.Instance.EnemiesInScene.Remove(this);
            EnemyEvents.Instance.EnemyDeathEvent?.Invoke();
            TriggerDeathAnimation();

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;

            PlayDeathSound();
            OnDeathEffect();
        }

        internal void SetOnFire(int fireStacks)
        {
            _fireStacks += fireStacks;
        }

        internal void ApplyFreezing(int iceStacks)
        {
            _iceStacks += iceStacks;
        }

        #region SoundEffectTriggers

        protected abstract void PlaySpawnSound();

        protected abstract void PlayHurtSound();

        protected abstract void PlayDeathSound();

        #endregion

        #region Animation

        protected abstract void TriggerSpawnAnimation();
        protected abstract void StartMovementAnimation();
        protected abstract void StopMovementAnimation();
        protected abstract void TriggerHurtAnimation();
        protected abstract void TriggerDeathAnimation();

        #endregion

        protected abstract void OnDeathEffect();

        protected abstract void AdditionalAttackEffects();

        protected virtual IEnumerator DestroyThisEnemyWithDelay()
        {
            yield return new WaitForSeconds(_deathDelayForAnimation);
            Destroy(gameObject);
        }

        public abstract void SetDisplayDescription();

        public void TakeIceDamage()
        {
            TakeDamage(_iceStacks);
        }

        //Moves the enemy one "space" to the left
        internal IEnumerator Move()
        {
            int xIndexOfEnemy = GetEnemyPositionIndex(this);
            Vector2 startPosition = transform.position;
            Vector2 endPosition;
            Vector2 currentPosition = startPosition;
            if (IsFlying)
            {
                endPosition = _enemyManager.EnemyPositions[1, xIndexOfEnemy - 1];
            }
            else
            {
                endPosition = _enemyManager.EnemyPositions[0, xIndexOfEnemy - 1];
            }
            StartMovementAnimation();
            Rigidbody2D _rigidbody = GetComponent<Rigidbody2D>();
            while (endPosition.x < currentPosition.x)
            {
                _rigidbody.velocity = Vector2.left * _enemySpeed;
                yield return new WaitForSeconds(0.2f);
                currentPosition = transform.position;
            }
            StopMovementAnimation();
            _rigidbody.velocity = Vector2.zero;
            transform.position = endPosition;
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

        #endregion
    }
}
