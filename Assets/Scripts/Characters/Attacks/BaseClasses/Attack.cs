using UnityEngine;
using EnumCollection;
using PeggleAttacks.AttackManager;
using PeggleWars.Enemies;
using PeggleWars.Orbs;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Utilities;

namespace PeggleWars.Attacks
{
    internal abstract class Attack : MonoBehaviour, IHaveBark
    {
        #region Fields and Properties

        protected Vector2 _instantiatePosition;
        protected PlayerAttackManager _playerAttackManager;
        protected Collider2D _collision;
        protected string _noTargetString = "There's no enemy!";

        [SerializeField] protected AttackOrigin _attackOrigin;
        [SerializeField] protected int _damage;
       
        internal int Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        public abstract string Bark { get; }

        #endregion

        #region Functions

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackManager.Instance;
            if (_attackOrigin == AttackOrigin.Player)
            {
                _damage = Mathf.RoundToInt(_damage * _playerAttackManager.DamageModifierTurn);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            _collision = collision;
            Debug.Log(_attackOrigin);
            Debug.Log(collision.gameObject);

            if ((_attackOrigin == AttackOrigin.Player && collision.gameObject.GetComponent<Enemy>() != null)
                || _attackOrigin == AttackOrigin.Enemy && collision.gameObject.GetComponent<Player>() != null)
            {
                IDamagable target = collision.GetComponent<IDamagable>();
                target?.TakeDamage(_damage);
                OnHitPolish();
                AdditionalEffectsOnImpact();
                if (_attackOrigin == AttackOrigin.Player) { OrbEvents.Instance.OrbEffectEnd?.Invoke(); }
                DestroyGameObject();
            }
        }

        internal abstract void ShootAttack();

        internal virtual void SetAttackInstantiatePosition(Transform originTransform)
        {
            _instantiatePosition = originTransform.position;
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
