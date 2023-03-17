using UnityEngine;
using EnumCollection;
using PeggleAttacks.AttackManager;
using PeggleWars.Enemies;
using PeggleWars.Orbs;
using PeggleWars.Characters.Interfaces;

namespace PeggleWars.Attacks
{
    internal abstract class Attack : MonoBehaviour
    {
        #region Fields and Properties

        protected Vector2 _instantiatePosition;
        protected PlayerAttackManager _playerAttackManager;
        protected Collider2D _collision;

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

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            _collision = collision;

            IDamagable target = collision.GetComponent<IDamagable>();
            target?.TakeDamage(_damage);
            OnHitPolish();
            AdditionalEffectsOnImpact();
            OrbEvents.Instance.OrbEffectEnd?.Invoke();
            DestroyGameObject();
        }

        internal abstract void ShootAttack();

        internal virtual void SetAttackInstantiatePosition(Transform targetTransform)
        {
            _instantiatePosition = targetTransform.position;
        }

        protected abstract void AdditionalEffectsOnImpact();

        protected abstract void OnHitPolish();

        protected virtual void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        #endregion

        internal enum AttackOrigin
        {
            Player,
            Enemy,
        }
    }
}
