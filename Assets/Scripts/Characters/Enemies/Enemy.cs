using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleAttacks.AttackVisuals.PopUps;
using PeggleWars;
using PeggleWars.Characters.Interfaces;
using PeggleWars.TurnManagement;
using UnityEngine.Events;
using System;

namespace PeggleWars.Enemies
{
    /// <summary>
    /// Parent class for all enemies in the game. Will be inherited by singular Enemy classes.
    /// The PopUpSpawner class spawns DamagePopUps when the enemy takes damage.
    /// </summary>

    [RequireComponent(typeof(PopUpSpawner))]
    [RequireComponent(typeof(EnemyAttack))]
    public abstract class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;
        protected TurnManager _turnManager;
        protected EnemyManager _enemyManager;

        protected string ATTACK_PARAM = "Attack";
        protected string HURT_PARAM = "Hurt";
        protected string DEATH_PARAM = "Death";
        protected string SPAWN_PARAM = "Spawn";
        protected float _deathDelayForAnimation = 1f;

        [SerializeField] protected EnemyAttackType _enemyAttackType; //melee, ranged
        public EnemyAttackType AttackType
        {
            get { return _enemyAttackType; }
            private set { _enemyAttackType = value; }
        }

        [SerializeField] protected int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        [SerializeField] protected int _health = 20;
        public int Health
        {
            get { return _health; }
            private set { _health = value; }
        }

        [SerializeField] private bool _isFlying;

        public bool IsFlying
        {
            get { return _isFlying; }
            private set { _isFlying = value; }
        }

        [SerializeField] private bool _isInAttackPosition;

        public bool IsInAttackPosition
        {
            get { return _isInAttackPosition; }
            private set { _isInAttackPosition = value; }
        }

        [SerializeField] protected int _attackFrequency; // in X turns
        public int AttackFrequency
        {
            get { return _attackFrequency; }
            private set { _attackFrequency = value; }
        }

        protected int _fireStacks;

        public int FireStacks
        {
            get { return _fireStacks; }
            private set { _fireStacks = value; }
        }

        protected int _iceStacks;

        public int IceStacks
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

        private void OnEnable()
        {
            TurnManager.Instance.EndEnemyTurn += OnEndEnemyTurn;
            TurnManager.Instance.StartEnemyTurn += OnStartEnemyTurn;
        }

        private void OnDisable()
        {
            TurnManager.Instance.EndEnemyTurn -= OnEndEnemyTurn;
            TurnManager.Instance.StartEnemyTurn -= OnStartEnemyTurn;
        }
        protected virtual void SetReferences()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(SPAWN_PARAM);
            _popUpSpawner = GetComponent<PopUpSpawner>();
            _enemyManager = EnemyManager.Instance;
        }

        protected void OnEndEnemyTurn()
        {
            if (_enemyAttackType == EnemyAttackType.Distance)
            {
                _isInAttackPosition = true;
            }
            else
            {
                Vector2 walkerMeleeAttackPosition = _enemyManager.EnemyPositions[0, 0];
                Vector2 flyerMeleeAttackPosition = _enemyManager.EnemyPositions[1, 0];
                if (transform.position.Equals(walkerMeleeAttackPosition) 
                    || transform.position.Equals(flyerMeleeAttackPosition))
                {
                    _isInAttackPosition = true;
                }
                else
                {
                    _isInAttackPosition = false;
                }
            }
        }

        protected void OnStartEnemyTurn()
        {
            if (_fireStacks > 0)
            {
                TakeDamage(_fireStacks);
                _fireStacks--;
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            if (_iceStacks > 0)
            {
                _health -= _iceStacks;
                _iceStacks = 0;
                _popUpSpawner.SpawnPopUp(_iceStacks);
            }
            _popUpSpawner.SpawnPopUp(damage);
            _animator.SetTrigger(HURT_PARAM);
            try
            {
                PlayHurtSound();
            }
            catch (NotImplementedException)
            {
                Debug.Log("Hurt Sound not implemented yet.");
            }

            if (_health <= 0)
            {
                HandleDeath();
                StartCoroutine(DestroyThisEnemyWithDelay());
            }
        }
        public void Attack()
        {
            _animator.SetTrigger(ATTACK_PARAM);
            Player.Instance.TakeDamage(_damage);

            AdditionalAttackEffects();
        }

        private void HandleDeath()
        {
            _enemyManager.EnemyDeathEvent?.Invoke();
            _animator.SetTrigger(DEATH_PARAM);

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            EnemyManager.Instance.EnemiesInScene.Remove(this);

            PlayDeathSound();
            OnDeathEffect();
        }

        public void SetOnFire(int fireStacks)
        {
            _fireStacks += fireStacks;
        }

        public void Freeze(int iceStacks)
        {
            _iceStacks += iceStacks;
        }

        protected abstract void PlayDeathSound();

        protected abstract void PlaySpawnSound();

        protected abstract void PlayHurtSound();

        protected abstract void OnDeathEffect();

        protected abstract void AdditionalAttackEffects();

        protected virtual IEnumerator DestroyThisEnemyWithDelay()
        {
            yield return new WaitForSeconds(_deathDelayForAnimation);
            Destroy(gameObject);
        }

        #endregion
    }
}
