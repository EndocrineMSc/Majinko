using System.Collections;
using UnityEngine;
using Utility.TurnManagement;
using PeggleWars.Characters.Interfaces;
using Audio;

namespace Characters
{
    internal class Player : MonoBehaviour, IDamagable
    {
        #region Fields and Properties

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Color _color;

        internal static Player Instance { get; private set; }
        private Animator _animator;
        private PhaseManager _turnManager;
        private string HURT_ANIMATION = "Hurt";

        [SerializeField] private int _health;

        internal int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        private int _shield;

        internal int Shield
        {
            get { return _shield; }
            set { _shield = value; }
        }

        private int _maxHealth;

        internal int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            _maxHealth = _health;
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
        }

        public void TakeDamage(int damage, bool sourceIsAttack = true)
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0104_Player_Takes_Damage);
            int calcDamage = damage;
            if (_shield > calcDamage)
            {
                _shield -= damage;
            }
            else if (_shield > 0)
            {
                calcDamage = damage - _shield;
                _shield = 0;
                _health -= calcDamage;
                StartCoroutine(nameof(ColorShiftDamage));
                _animator.SetTrigger(HURT_ANIMATION);
            }
            else
            {
                _health -= damage;
                StartCoroutine(nameof(ColorShiftDamage));
                _animator.SetTrigger(HURT_ANIMATION);
            }
        }


        private void OnCardTurnStart()
        {
            Shield = 0;
        }

        private IEnumerator ColorShiftDamage()
        {
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = _color;
        }

        public void TakeIceDamage()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
