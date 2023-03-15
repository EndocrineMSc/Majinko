using UnityEngine;
using EnumCollection;
using PeggleAttacks.AttackManager;
using PeggleWars.Enemies;
using PeggleWars.Orbs;

namespace PeggleWars.PlayerAttacks
{
    internal abstract class PlayerAttack : MonoBehaviour
    {
        #region Fields and Properties

        protected Vector2 _instantiatePosition = new(-7.5f, 7f);
        protected PlayerAttackManager _playerAttackManager;

        [SerializeField] protected PlayerAttackTarget _target;
        [SerializeField] protected float _attackFlySpeed = 10;

        [SerializeField] protected int _damage;
       
        internal int Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        #endregion

        #region Functions

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackManager.Instance;
            _damage = Mathf.RoundToInt(_damage * _playerAttackManager.DamageModifierTurn);
        }

        internal virtual void ShootAttack(PlayerAttack playerAttack)
        {
            PlayerAttack tempAttack = Instantiate(playerAttack, _instantiatePosition, Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbody = tempAttack.GetComponent<Rigidbody2D>();
            rigidbody.velocity = Vector3.right * _attackFlySpeed;
        }

        internal virtual void ShootAttack(Vector3 startPosition, PlayerAttack playerAttack)
        {
            Enemy enemy = null;
            Vector3 targetPosition = new();

            if (EnemyManager.Instance.EnemiesInScene.Count > 0 ) 
            {
                switch (_target)
                {
                    case PlayerAttackTarget.FirstEnemy:
                        enemy = EnemyManager.Instance.EnemiesInScene[0];
                        targetPosition = enemy.transform.position;
                        break;
                }

                Vector3 direction = targetPosition - startPosition;

                float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                PlayerAttack tempAttack = Instantiate(playerAttack, startPosition, Quaternion.Euler(0, 0, rotation * 2.5f));
                Rigidbody2D rigidbody = tempAttack.GetComponent<Rigidbody2D>();

                rigidbody.velocity = new Vector2(direction.x, direction.y).normalized * _attackFlySpeed;
            }           
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Despawn"))
            {
                OrbEvents.Instance.OrbEffectEnd?.Invoke();
                DestroyGameObject();
            }
            else
            {
                Enemy enemy = null;
                switch (_target)
                {
                    case PlayerAttackTarget.FirstEnemy:
                        enemy = EnemyManager.Instance.EnemiesInScene[0];
                        break;

                    case PlayerAttackTarget.LastEnemy:
                        enemy = EnemyManager.Instance.EnemiesInScene[EnemyManager.Instance.EnemiesInScene.Count - 1];
                        break;
                }

                if (enemy != null)
                {
                    enemy.TakeDamage(_damage);
                }

                OnHitPolish();
                OrbEvents.Instance.OrbEffectEnd?.Invoke();
                DestroyGameObject();
            }
        }

        protected abstract void OnHitPolish();

        protected virtual void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
