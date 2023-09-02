using System.Collections;
using UnityEngine;
using Utility.TurnManagement;
using PeggleWars.Characters.Interfaces;
using Audio;
using Utility;

namespace Characters
{
    public class Player : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Color _color;

        public static Player Instance { get; private set; }
        private Animator _animator;
        private PhaseManager _turnManager;
        private readonly string HURT_ANIMATION_PARAM = "Hurt";
        private readonly string ATTACK_ANIMATION_PARAM = "Attack";

        public int Health { get; private set; }
        public int Shield { get; private set; }
        public int MaxHealth { get; private set; }

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            MaxHealth = PlayerConditionTracker.MaxPlayerHealth;
            Health = PlayerConditionTracker.PlayerHealth;
        }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _color = _spriteRenderer.color;
            _turnManager = PhaseManager.Instance;    
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartCardPhase += OnCardTurnStart;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartCardPhase -= OnCardTurnStart;
            PlayerConditionTracker.ResetBuffsAndDebuffs();
        }

        public void TakeDamage(int damage, bool sourceIsAttack = true)
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0104_Player_Takes_Damage);
            int calcDamage = damage;
            if (Shield > calcDamage)
            {
                Shield -= damage;
            }
            else if (Shield > 0)
            {
                calcDamage = damage - Shield;
                Shield = 0;
                Health -= calcDamage;
                StartCoroutine(nameof(ColorShiftDamage));
                _animator.SetTrigger(HURT_ANIMATION_PARAM);
            }
            else
            {
                Health -= damage;
                StartCoroutine(nameof(ColorShiftDamage));
                _animator.SetTrigger(HURT_ANIMATION_PARAM);
            }

            PlayerConditionTracker.SetPlayerHealth(Health);
            
            if (Health <= 0)
                UtilityEvents.RaisePlayerDeath();         
        }

        private void OnCardTurnStart()
        {
            if(!PlayerConditionTracker.HasShieldBeetle)
                Shield = 0;
        }

        private IEnumerator ColorShiftDamage()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = _color;
        }

        public void TakeIceDamage()
        {
            throw new System.NotImplementedException();
        }

        public void StartAttackAnimation()
        {
            _animator.SetTrigger(ATTACK_ANIMATION_PARAM);
        }

        public void GainShield(int amount = 1, bool sourceIsOrb = true)
        {
            Shield += amount;

            if (sourceIsOrb)
                PlayerEvents.RaiseGainedShield();
        }

        #endregion
    }
}
