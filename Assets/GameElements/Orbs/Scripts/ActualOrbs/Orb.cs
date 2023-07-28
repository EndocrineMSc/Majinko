using EnumCollection;
using System.Collections;
using UnityEngine;
using ManaManagement;
using Audio;
using Utility;  
using DG.Tweening;
using Spheres;

namespace Orbs
{
    [RequireComponent(typeof(ScrollDisplayer)), RequireComponent(typeof(OrbManaPopUpDisplayer))]
    internal abstract class Orb : MonoBehaviour, IHaveDisplayDescription
    {
        #region Fields and Properties

        //References
        protected ManaPool _manaPool;
        protected OrbManager _orbManager;
        protected AudioManager _audioManager;
        protected Collider2D _collider;

        //Stats
        [SerializeField] protected ManaType _spawnManaType;
        [SerializeField] protected int _manaAmount = 10;

        internal ManaType SpawnManaType { get { return _spawnManaType; } private protected set { _spawnManaType = value; } }
        internal int ManaAmount { get { return _manaAmount; } private protected set { _manaAmount = value; }}
        [SerializeField] protected GameObject _defaultOrb;
        [SerializeField] internal OrbType OrbType;
        internal bool OrbIsActive { get; private set; } = true;

        //Tweening
        protected Vector3 _position;
        protected Vector3 _onHitTweenScale = new(5, 5, 5);
        protected float _tweenDuration = 0.2f;

        #endregion

        #region Functions

        #region Initialization

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            SetReferences();
            SetDisplayDescription();
            SetScrollDisplayScale();
        }

        protected void OnEnable()
        {
            OrbEvents.OnSetOrbsActive += OnSetOrbActive;
        }

        protected void OnDisable()
        {
            OrbEvents.OnSetOrbsActive -= OnSetOrbActive;
        }

        protected virtual void SetReferences()
        {
            _manaPool = ManaPool.Instance;
            _orbManager = OrbManager.Instance;
            _audioManager = AudioManager.Instance;
            _collider = GetComponent<Collider2D>();
        }

        public abstract void SetDisplayDescription();

        protected void SetScrollDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 4;
        }

        #endregion

        #region Collision Handling

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            _collider.enabled = false;
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _))
            {
                AdditionalEffectsOnCollision();
                ReplaceHitOrb();               
                PlayOrbOnHitSound();
                OnCollisionVisualPolish();
                SpawnMana();
                StartCoroutine(DestroyOrbWithDelay());
            }
            else if (collision.gameObject.TryGetComponent<Orb>(out _))
            {
                Debug.Log("Destroyed " + gameObject.name + " due to collision with orb");
                Destroy(gameObject);
            }
            else
            {
                _collider.enabled = true;
            }
        }

        protected abstract void AdditionalEffectsOnCollision();

        protected virtual void ReplaceHitOrb()
        {
            Orb orb = Instantiate(_defaultOrb, transform.position, Quaternion.identity).GetComponent<Orb>();
            orb.SetOrbInactive();
            _orbManager.SceneOrbList.Remove(this);
            _orbManager.SceneOrbList.Add(orb);
        }

        protected virtual void PlayOrbOnHitSound()
        {
            PlayOrbImpactSound();
        }

        protected virtual void OnCollisionVisualPolish()
        {
            transform.DOScale(_onHitTweenScale, _tweenDuration).SetEase(Ease.OutBack);
        }

        protected virtual void SpawnMana()
        {
            if (ManaAmount != 0)
                OrbEvents.RaiseSpawnMana(SpawnManaType, ManaAmount);
        }

        protected virtual IEnumerator DestroyOrbWithDelay()
        {
            yield return new WaitForSeconds(_tweenDuration * 1.05f);
            Destroy(gameObject);
        }

        internal void SetOrbInactive()
        {
            _collider.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            OrbIsActive = false;
        }

        #endregion

        #region Other Functions

        protected void OnSetOrbActive()
        {
            OrbIsActive = true;
            _collider.enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        internal void SetActionOrbInactive()
        {
            OrbIsActive = false;
            _collider.enabled = false;
        }

        internal abstract IEnumerator OrbEffect();

        private void PlayOrbImpactSound()
        {
            int randomSoundIndex = UnityEngine.Random.Range(0, 9);
            AudioManager audioManager = AudioManager.Instance;
            switch (randomSoundIndex)
            {
                case 0:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0751_Orb_Impact_01);
                    break;                      
                case 1:                        
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0752_Orb_Impact_02);
                    break;                     
                case 2:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0753_Orb_Impact_03);
                    break;                      
                case 3:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0754_Orb_Impact_04);
                    break;                      
                case 4:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0755_Orb_Impact_05);
                    break;                      
                case 5:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0756_Orb_Impact_06);
                    break;                      
                case 6:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0757_Orb_Impact_07);
                    break;                      
                case 7:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0758_Orb_Impact_08);
                    break;                      
                case 8:                         
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0759_Orb_Impact_09);
                    break;
            }
        }

        protected void OnDestroy()
        {
            transform.DOKill();
        }

        #endregion

        #endregion
    }
}

