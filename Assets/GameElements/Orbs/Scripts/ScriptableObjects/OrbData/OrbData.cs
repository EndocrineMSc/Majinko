using UnityEngine;
using EnumCollection;
using Attacks;
using Utility;

namespace Orbs
{
    public abstract class OrbData : ScriptableObject
    {
        #region Fields and Properties

        protected Orb _parentOrb;

        //Internally set Values
        [SerializeField] private string _orbName;
		protected string _orbDescription;
        [SerializeField] protected ManaType _manaToSpawn;
		[SerializeField] protected int _amountManaSpawned = 0;
		[SerializeField] protected int _stalwartHits = 0;
		[SerializeField] protected bool _revertsToBasicOrb = true;
        [SerializeField] protected Attack _orbAttackPrefab = null;
        [SerializeField] protected bool _isAddedToActionList;
        [SerializeField] protected RuntimeAnimatorController _animationController;
        [SerializeField] protected bool _isTriggerColliderOrb;
        [SerializeField] protected EffectValueCollection _effectValues;

        //Attack values, null if no attack prefab
        protected int _damage;
        protected int _shieldStacks;
        protected int _burningStacks;
        protected int _freezingStacks;
        protected int _percentToFreeze;

        public string OrbName
        {
            get { return _orbName; }
            private protected set { _orbName = value; }
        }

        public string OrbDescription
        {
            get { return _orbDescription; }
            private protected set { _orbDescription = value; }
        }

        public ManaType ManaToSpawn
        {
            get { return _manaToSpawn; }
            private protected set { _manaToSpawn = value; }
        }

        public int AmountManaSpawned
        {
            get { return _amountManaSpawned; }
            private protected set { _amountManaSpawned = value; }
        }

        public int StalwartHits
        {
            get { return _stalwartHits; }
            private protected set { _stalwartHits = value; }
        }

        public bool RevertsToBasicOrb
		{
			get { return _revertsToBasicOrb; }
			private protected set { _revertsToBasicOrb = value; }
		}

        public Attack OrbAttack
        {
            get { return _orbAttackPrefab; }
            private protected set { _orbAttackPrefab = value; }
        }

        public bool IsAddedToActionList
        {
            get { return _isAddedToActionList; }
            private protected set { _isAddedToActionList = value; }
        }

        public RuntimeAnimatorController AnimationController
        {
            get { return _animationController; }
            private protected set { _animationController = value; }
        }

        public bool IsTriggerColliderOrb
        {
            get { return _isTriggerColliderOrb; }
            private protected set { _isTriggerColliderOrb = value; }
        }

        #endregion

        #region Methods

        protected virtual void OnValidate()
        {
            if (_orbAttackPrefab != null && _orbAttackPrefab.AttackValues != null && _effectValues == null)
            {
                var attackValues = _orbAttackPrefab.AttackValues;
                _damage = attackValues.Damage;
                _burningStacks = attackValues.BurningStacks;
                _freezingStacks = attackValues.FreezingStacks;
                _percentToFreeze = attackValues.PercentToFreeze;
            }
            else if (_effectValues != null)
            {
                _shieldStacks = _effectValues.ShieldStacks;
            }
        }

        public void InitializeOrbData(Orb orb)
        {
            _parentOrb = orb;
            SetDescription();
            //orb.SetOrbActive();
        }

        public virtual void CollisionEffect()
        {
            if (IsAddedToActionList && OrbActionManager.Instance != null)
                OrbActionManager.Instance.AddOrbToActionList(this);
            else
                OrbEffect();
        }

        protected abstract void SetDescription();

        public abstract void OrbEffect();

		#endregion
	}
}
