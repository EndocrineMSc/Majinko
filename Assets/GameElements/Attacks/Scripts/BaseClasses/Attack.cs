using UnityEngine;
using Characters.Enemies;
using Orbs;
using PeggleWars.Characters.Interfaces;
using Characters;
using Utility;

namespace Attacks
{
    internal abstract class Attack : MonoBehaviour, IHaveBark
    {
        #region Fields and Properties

        protected PlayerAttackDamageManager _playerAttackManager;
        protected Collider2D _collider;

        [SerializeField] protected EffectValueCollection _attackValues;
        [SerializeField] protected AttackOrigin _attackOrigin;

        protected readonly string NOTARGET_BARK = "No enemy!";
        protected readonly string ATTACK_ANIMATION = "Attack";

        internal int Damage { get; private protected set; }

        public abstract string Bark { get; }

        #endregion

        #region Functions

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackDamageManager.Instance;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            _collider = collision;

            if ((_attackOrigin == AttackOrigin.Player && collision.gameObject.GetComponent<Enemy>() != null)
                || _attackOrigin == AttackOrigin.Enemy && collision.gameObject.GetComponent<Player>() != null)
            {
                IDamagable target = collision.GetComponent<IDamagable>();
                target?.TakeDamage(Damage);
                OnHitPolish();
                AdditionalEffectsOnImpact();
                if (_attackOrigin == AttackOrigin.Player) { OrbEvents.RaiseEffectEnd(); }
                DestroyGameObject();
            }
        }

        internal abstract void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1);

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
