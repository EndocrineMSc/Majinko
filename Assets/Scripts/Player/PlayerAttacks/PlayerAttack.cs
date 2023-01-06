using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Enemies;
using PeggleOrbs;
using PeggleAttacks.AttackManager;

namespace PeggleAttacks.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        #region Fields

        protected Vector2 _insantiatePosition = new Vector2(-7.5f, 5.9f);
        protected PlayerAttackManager _playerAttackManager;

        [SerializeField] protected PlayerAttackTarget _target;
        [SerializeField] protected float _attackFlySpeed = 10;

        #endregion

        #region Properties

        [SerializeField] protected int _damage;

        public int Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        #endregion

        #region Public Functions

        public virtual void ShootAttack(Vector3 startPosition, PlayerAttack playerAttack)
        {
            Enemy enemy = null;
            Vector3 targetPosition = new();

            if (EnemyManager.Instance.Enemies.Count > 0 ) 
            {
                switch (_target)
                {
                    case PlayerAttackTarget.FirstEnemy:
                        enemy = EnemyManager.Instance.Enemies[0];
                        targetPosition = enemy.transform.position;
                        break;
                }

                Vector3 direction = targetPosition - startPosition;

                Debug.Log("StartPosition: " + startPosition + " to " + direction + " to " + targetPosition);

                float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                PlayerAttack tempAttack = Instantiate(playerAttack, startPosition, Quaternion.Euler(0, 0, rotation * 2.5f));
                Rigidbody2D rigidbody = tempAttack.GetComponent<Rigidbody2D>();

                rigidbody.velocity = new Vector2(direction.x, direction.y).normalized * _attackFlySpeed;
            }           
        }

        public virtual void ShootAttack(PlayerAttack playerAttack)
        {
            PlayerAttack tempAttack = Instantiate(playerAttack, _insantiatePosition, Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbody = tempAttack.GetComponent<Rigidbody2D>();

            rigidbody.velocity = Vector3.right * _attackFlySpeed;
        }

        #endregion

        #region Protected Functions

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = null;
            switch (_target)
            {
                case PlayerAttackTarget.FirstEnemy:
                enemy = EnemyManager.Instance.Enemies[0];
                break;                       
            }

            if (enemy != null)
            {
                enemy.LoseHealth(_damage);
            }

            OnHitPolish();
            Destroy(gameObject);
        }

        protected virtual void OnHitPolish()
        {
            //Do polish stuff here 
        }

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackManager.Instance;
            _damage *= Mathf.RoundToInt(_playerAttackManager.DamageModifierTurn);
        }
        #endregion
    }
}
