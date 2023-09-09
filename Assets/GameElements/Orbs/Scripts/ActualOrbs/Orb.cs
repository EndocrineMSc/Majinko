using UnityEngine;
using Audio;
using Utility;  
using DG.Tweening;
using Spheres;

namespace Orbs
{
    [RequireComponent(typeof(ScrollDisplayer)), RequireComponent(typeof(OrbManaPopUpDisplayer))]
    public class Orb : MonoBehaviour, IHaveDisplayDescription
    {
        #region Fields and Properties

        //References
        private AudioManager _audioManager;
        private Collider2D _collider;
        private Animator _animator;
        private OrbManaPopUpDisplayer _orbManaPopUpDisplayer;

        //Stats
        [SerializeField] private OrbData _orbData;
        public OrbData Data
        {
            get { return _orbData; }
            private protected set {_orbData = value; }
        }

        public bool OrbIsActive { get; private set; } = true;
        public int StalwartStacks { get; private set; } = 0;

        //Tweening
        private Vector3 _onHitTweenScale = new(5, 5, 5);
        private readonly float _tweenDuration = 0.2f;

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
            SetScrollDisplayScale();
            ReadOrbData();
        }

        private void OnEnable()
        {
            OrbEvents.OnSetOrbsActive += SetOrbActive;
        }

        private void OnDisable()
        {
            OrbEvents.OnSetOrbsActive -= SetOrbActive;
        }

        private void SetReferences()
        {
            _orbManaPopUpDisplayer = GetComponent<OrbManaPopUpDisplayer>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _audioManager = AudioManager.Instance;
        }

        private void ReadOrbData()
        {
            SetDisplayDescription();
            StalwartStacks = Data.StalwartHits;
            GetComponent<Animator>().runtimeAnimatorController = Data.AnimationController;
            _collider.isTrigger = Data.IsTriggerColliderOrb;
        }

        public void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = Data.OrbDescription;
        }

        private void SetScrollDisplayScale()
        {
            GetComponent<ScrollDisplayer>().DisplayScale = 4;
        }

        #endregion

        #region Collision Handling

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IAmSphere>(out _))
            {
                ArenaConditionTracker.OrbWasHit();
                OrbEvents.RaiseOrbHit();

                if (StalwartStacks <= 0)
                {
                    _collider.enabled = false;
                    Data.CollisionEffect();

                    OrbManager.Instance.ReturnOrbToDisabledPool(this);

                    PlayOrbOnHitSound();
                    OnCollisionVisualPolish();
                    SpawnMana();
                }
                else
                {
                    StalwartStacks--;
                }
            }
        }

        private void PlayOrbOnHitSound()
        {
            PlayOrbImpactSound();
        }

        private void OnCollisionVisualPolish()
        {
            transform.DOScale(_onHitTweenScale, _tweenDuration).SetEase(Ease.OutBack).OnComplete(SetOrbInactive);
        }

        private void SpawnMana()
        {
            if (Data.AmountManaSpawned != 0)
                OrbEvents.RaiseSpawnMana(Data.ManaToSpawn, Data.AmountManaSpawned);
        }

        public void SetOrbInactive()
        {
            _collider.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            OrbIsActive = false;
        }

        #endregion

        #region Other Functions

        public void SetOrbActive()
        {
            OrbIsActive = true;
            _collider.enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        public void SetActionOrbInactive()
        {
            OrbIsActive = false;
            _collider.enabled = false;
        }

        private void PlayOrbImpactSound()
        {
            if (_audioManager != null)
            {
                int randomSoundIndex = UnityEngine.Random.Range(0, 9);
                switch (randomSoundIndex)
                {
                    case 0:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0751_Orb_Impact_01);
                        break;
                    case 1:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0752_Orb_Impact_02);
                        break;
                    case 2:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0753_Orb_Impact_03);
                        break;
                    case 3:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0754_Orb_Impact_04);
                        break;
                    case 4:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0755_Orb_Impact_05);
                        break;
                    case 5:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0756_Orb_Impact_06);
                        break;
                    case 6:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0757_Orb_Impact_07);
                        break;
                    case 7:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0758_Orb_Impact_08);
                        break;
                    case 8:
                        _audioManager.PlaySoundEffectWithoutLimit(SFX._0759_Orb_Impact_09);
                        break;
                }
            }
        }

        public void SetOrbData(OrbData orbData)
        {
            if (orbData != null)
            {
                orbData.InitializeOrbData(this);
                Data = orbData;
                ReadOrbData();
                GetComponent<OrbManaPopUpDisplayer>().SetManaType(Data.ManaToSpawn);
            }
            else
            {
                Debug.Log("Orb got null reference orbData handed over!");
            }
        }

        #endregion

        #endregion
    }
}

