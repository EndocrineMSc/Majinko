using UnityEngine;
using Characters.Enemies;
using Orbs;
using PeggleWars.Characters.Interfaces;
using Characters;
using Utility;
using Audio;

namespace Attacks
{
    internal abstract class Attack : MonoBehaviour, IHaveBark
    {
        #region Fields and Properties

        protected PlayerAttackDamageManager _playerAttackManager;

        [SerializeField] protected EffectValueCollection _attackValues;
        [SerializeField] protected AttackOrigin _attackOrigin;

        protected readonly string NOTARGET_BARK = "No enemy!";
        protected readonly string ATTACK_ANIMATION = "Attack";

        internal int Damage { get; private protected set; }

        public abstract string Bark { get; }

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

        internal abstract void ShootAttack(Vector3 instantiatePosition, float damageModifier = 1);

        protected virtual void OnHitPolish()
        {
            float amplitude = 1 + Damage / 10;
            float shakeTime = (float)Damage / 100;
            
            if (ScreenShaker.Instance != null)
                ScreenShaker.Instance.ShakeCamera(amplitude, shakeTime);

            PlayHitSound();
        }

        protected abstract void PlayAwakeSound();

        protected abstract void PlayHitSound();

        #endregion
    }

    internal enum AttackOrigin
    {
        Player,
        Enemy,
    }
}
