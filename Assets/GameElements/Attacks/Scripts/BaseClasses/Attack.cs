using UnityEngine;
using Characters.Enemies;
using Orbs;
using PeggleWars.Characters.Interfaces;
using Characters;
using Utility;
using Audio;

namespace Attacks
{
    public abstract class Attack : MonoBehaviour, IHaveBark
    {
        #region Fields and Properties

        protected PlayerAttackDamageManager _playerAttackManager;

        [SerializeField] protected EffectValueCollection _attackValues;
        [SerializeField] protected AttackOrigin _attackOrigin;

        protected readonly string NOTARGET_BARK = "No enemy!";
        protected readonly string ATTACK_ANIMATION = "Attack";

        public int Damage { get; private protected set; }

        public abstract string Bark { get; }

        public EffectValueCollection AttackValues
        {
            get { return _attackValues; }
            private protected set { _attackValues = value; }
        }

        #endregion

        #region Functions

        protected virtual void Awake()
        {
            Damage = _attackValues.Damage;
            
            if (AudioManager.Instance != null )
                PlayAwakeSound();
        }

        protected virtual void Start()
        {
            _playerAttackManager = PlayerAttackDamageManager.Instance;
        }

        public abstract void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1);

        protected virtual void OnHitPolish(float damage)
        {
            float amplitude = 1 + damage / 10;
            float shakeTime = (float)damage / 100;
            
            if (ScreenShaker.Instance != null)
                ScreenShaker.Instance.ShakeCamera(amplitude, shakeTime);

            PlayHitSound();
        }

        protected abstract void AdditionalDamageEffects(GameObject target);

        protected abstract void PlayAwakeSound();

        protected abstract void PlayHitSound();

        protected virtual void RaiseAttackFinished()
        {
            AttackEvents.RaiseAttackFinished();
        }

        #endregion
    }

    public enum AttackOrigin
    {
        Player,
        Enemy,
    }
}
