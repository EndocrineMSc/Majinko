using Audio;
using UnityEngine;
using UnityEngine.Animations;


namespace Characters.Enemies
{
    [CreateAssetMenu(menuName = "Enemies/Enemy/EnemyObject")]
    public class ScriptableEnemy : ScriptableObject
    {
        #region Backing Fields

        [SerializeField] private string _description;
        [SerializeField] private int _attackFrequency;
        [SerializeField] private int _damage;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _displayScale = 2;
        [SerializeField] private int _abilityCooldownMax;
        [SerializeField] private bool _isRanged;
        [SerializeField] private bool _isFlying;
        [SerializeField] private bool _isStationary;
        [SerializeField] private bool _canBeIntangible;
        [SerializeField] private EnemyAttackType _attackType;
        [SerializeField] private SFX _spawnSound;
        [SerializeField] private SFX _deathSound;
        [SerializeField] private SFX _hurtSound;

        #endregion

        #region Properties

        public string Description
        {
            get { return _description; }
            private set { _description = value; }
        }

        public int AttackFrequency
        {
            get { return _attackFrequency; }
            private set { _attackFrequency = value; }
        }

        public int Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            private set { _maxHealth = value; }
        }

        public int DisplayScale
        {
            get { return _displayScale; }
            private set { _displayScale = value; }
        }

        public int AbilityCooldownMax
        {
            get { return _abilityCooldownMax; }
            private set { _abilityCooldownMax = value; }
        }

        public bool IsRanged
        {
            get { return _isRanged; }
            private set { _isRanged = value; }
        }

        public bool IsFlying
        {
            get { return _isFlying; }
            private set { _isFlying = value; }
        }

        public bool IsStationary
        {
            get { return _isStationary; }
            private set { _isStationary = value; }
        }

        public bool CanBeIntangible
        {
            get { return _canBeIntangible; }
            private set { _canBeIntangible = value; }
        }

        public EnemyAttackType AttackType
        {
            get { return _attackType; }
            private set { _attackType = value; }
        }

        public SFX SpawnSound
        {
            get { return _spawnSound; }
            private set { _spawnSound = value; }
        }

        public SFX DeathSound
        {
            get { return _deathSound; }
            private set { _deathSound = value; }
        }

        public SFX HurtSound
        {
            get { return _hurtSound; }
            private set { _hurtSound = value; }
        }

        #endregion
    }
}
