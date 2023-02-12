using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleAttacks.AttackVisuals.PopUps;
using PeggleWars;
using PeggleWars.Characters.Interfaces;

namespace PeggleWars.Enemies
{
    /// <summary>
    /// Parent class for all enemies in the game. Will be inherited by singular Enemy classes.
    /// The PopUpSpawner class spawns DamagePopUps when the enemy takes damage.
    /// </summary>

    [RequireComponent(typeof(PopUpSpawner))]
    public class Enemy : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        protected Animator _animator;
        protected PopUpSpawner _popUpSpawner;
        protected Player _player;

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
            set { _isFlying = value; }
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            SetReferences();
            PlaySpawnSound();
        }

        private void HandleDeath()
        {
            _animator.SetTrigger("Death");

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            EnemyManager.Instance.EnemiesInScene.Remove(this);

            PlayDeathSound();
            OnDeathEffect();
        }

        #endregion

        #region Protected Functions

        protected virtual void SetReferences()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger("Spawn");
            _popUpSpawner = GetComponent<PopUpSpawner>();           
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name.Contains("Attack"))
            {
                _animator.SetTrigger("Hurt");
                PlayHurtSound();
            }
        }

        protected virtual void PlayDeathSound()
        {
            //children will implement death sounds here
        }

        protected virtual void PlaySpawnSound()
        {
            //children will implement spawn sounds here
        }

        protected virtual void PlayHurtSound()
        {
            //children will implement hurt sounds here
        }

        protected virtual void OnDeathEffect()
        {
            //children will implement on death effects here
        }

        protected virtual void AdditionalAttackEffects()
        {
            //children will implement additional effects if necessary
        }

        #endregion

        #region Public Functions

        public void TakeDamage(int damage)
        {
            _health -= damage;

            _popUpSpawner.SpawnPopUp(damage);

            if (_health <= 0)
            {
                HandleDeath();
                StartCoroutine(DestroyEnemy());
            }
        }

        public void Attack()
        {
            _animator.SetTrigger("Attack");
            Player.Instance.TakeDamage(_damage);

            AdditionalAttackEffects();
        }

        #endregion

        #region IEnumerators

        protected virtual IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        #endregion
    }
}
