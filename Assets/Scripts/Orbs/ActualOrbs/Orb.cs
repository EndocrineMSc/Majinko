using EnumCollection;
using System.Collections;
using UnityEngine;
using PeggleWars.ManaManagement;
using PeggleWars.Audio;
using PeggleWars.ScrollDisplay;
using PeggleWars.Spheres;
using DG.Tweening;

namespace PeggleWars.Orbs
{
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Orb : MonoBehaviour, IHaveDisplayDescription
    {
        #region Fields and Properties

        [SerializeField] protected ManaType _spawnManaType;
        [SerializeField] protected int _manaAmount = 10;
        [SerializeField] protected GameObject _defaultOrb;
        protected Collider2D _collider;

        protected ManaPool _manaPool;
        protected OrbManager _orbManager;
        protected AudioManager _audioManager;

        protected Vector3 _position;
        protected Vector3 _onHitTweenScale = new(5, 5, 5);
        protected float _tweenDuration = 0.2f;

        [SerializeField] protected OrbType _orbType;

        internal OrbType OrbType
        {
            get { return _orbType; }
            private set { _orbType = value; }
        }

        internal bool IsOrbActive { get; private set; } = true;

        #endregion

        #region Functions

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

        protected virtual void SetReferences()
        {
            _manaPool = ManaPool.Instance;
            _orbManager = OrbManager.Instance;
            _audioManager = AudioManager.Instance;
            _collider = GetComponent<Collider2D>();
            OrbEvents.Instance.SetOrbsActive?.AddListener(OnSetOrbActive);
        }

        public abstract void SetDisplayDescription();

        protected void SetScrollDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 4;
        }

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
                StartCoroutine(DestroyOrb());
            }
            else
            {
                _collider.enabled = true;
            }
        }

        protected abstract void AdditionalEffectsOnCollision();

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
            OrbEvents.Instance.ManaSpawnTrigger?.Invoke(_spawnManaType, _manaAmount);
        }

        protected virtual void ReplaceHitOrb()
        {
            GameObject orbObject = Instantiate(_defaultOrb, transform.position, Quaternion.identity);
            Orb orb = orbObject.GetComponent<Orb>();
            orb.SetOrbInactive();
            _orbManager.SceneOrbList.Remove(this);
            _orbManager.SceneOrbList.Add(orb);
        }

        protected virtual IEnumerator DestroyOrb()
        {
            yield return new WaitForSeconds(_tweenDuration * 1.05f);
            Destroy(gameObject);
        }

        protected void SetOrbInactive()
        {
            _collider.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            IsOrbActive = false;
        }
   
        protected void OnSetOrbActive()
        {
            IsOrbActive = true;
            _collider.enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        internal void SetActionOrbInactive()
        {
            IsOrbActive = false;
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
        #endregion
    }
}

