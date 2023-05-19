using UnityEngine;
using Enemies;
using PeggleWars.Orbs;
using PeggleWars.Characters.Interfaces;
using PeggleWars.Utilities;
using Characters;

namespace Attacks
{
    internal abstract class Attack : MonoBehaviour, IHaveBark
    {
        #region Fields and Properties

        protected PlayerAttackDamageManager _playerAttackManager;
        protected Collider2D _collider;

        [SerializeField] protected AttackOrigin _attackOrigin;
        [SerializeField] protected int _damage;

        protected readonly string NOTARGET_BARK = "No enemy!";
        protected readonly string ATTACK_ANIMATION = "Attack";

        internal int Damage { get; private protected set; }

        public abstract string Bark { get; }

        #endregion

        #region Functions

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackDamageManager.Instance;
            
            if (_attackOrigin == AttackOrigin.Player)
            {
                _damage = Mathf.RoundToInt(_damage * _playerAttackManager.DamageModifierTurn);
                Player.Instance.GetComponentInChildren<Animator>().SetTrigger(ATTACK_ANIMATION);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            _collider = collision;

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

        internal abstract void ShootAttack(Vector3 instantiatePosition);

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
