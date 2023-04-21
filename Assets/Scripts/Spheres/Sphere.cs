using PeggleWars.Audio;
using PeggleWars.ScrollDisplay;
using PeggleWars.TurnManagement;
using PeggleWars.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;


namespace PeggleWars.Spheres
{
    [RequireComponent(typeof(ScrollDisplayer))]
    internal abstract class Sphere : MonoBehaviour, IHaveDisplayDescription, IAmSphere
    {
        #region Fields and Properties

        //fields for physicality
        protected Rigidbody2D _rigidbody;
        protected bool _isNotShotYet = true;
        protected bool _isInShootingTurn;
        protected float _shotSpeed = 10.5f;
        protected Collider2D _collider;

        //fields for indicators
        [SerializeField] protected GameObject _shotIndicatorPrefab;
        protected List<GameObject> _indicators = new();
        protected float _indicatorFrequency = 0.15f;
        protected bool _waitingForCoroutine;

        //fields for Rotation to mouse
        protected Camera _mainCam;
        protected Vector2 _direction;
        protected Vector2 _mousePosition;
        protected float _rotationZ;

        protected readonly string PORTAL_PARAM = "Portal";
        protected readonly string ARENA_PARAM = "Arena";

        internal float ShotSpeed
        {
            get { return _shotSpeed; }
            private set { _shotSpeed = value; }
        }

        protected float _gravity = 1;

        internal float Gravity
        {
            get { return _gravity; }
            private set { _gravity = value; }
        }

        #endregion

        #region Functions

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();

            //holds the ball in place until shot per mouseclick
            _rigidbody.gravityScale = 0;
            _mainCam = Camera.main;
            Physics2D.IgnoreLayerCollision(16, 17);
        }

        protected virtual void OnEnable()
        {
            TurnManager.Instance.EndCardTurn?.AddListener(OnCardTurnEnd);
            ShotEvents.Instance.ShotStackedEvent?.AddListener(ShotStackEffect);           
        }

        protected virtual void OnDisable()
        {
            TurnManager.Instance.EndCardTurn?.RemoveListener(OnCardTurnEnd);
            ShotEvents.Instance.ShotStackedEvent?.RemoveListener(ShotStackEffect);
        }

        private void Start()
        {
            SetDisplayDescription();
        }

        private void OnCardTurnEnd()
        {
            StartCoroutine(SetShotAsActive());
        }

        private IEnumerator SetShotAsActive()
        {
            yield return new WaitForSeconds(0.2f);
            _isInShootingTurn = true;
        }

        protected virtual void Update()
        {
            if (_isNotShotYet && _isInShootingTurn && !PauseControl.Instance.GameIsPaused)
            {
                GetDirectionAndRotation();
                RotateShot();

                if (!_waitingForCoroutine)
                {
                    StartCoroutine(ShootIndicators());
                }

                bool hasMoved = HasMouseMoved(); //destroy indicators on moving mouse for crisper visuals

                if (hasMoved)
                {
                    DestroyAllIndicators();
                }
                ShootOnClick();
            }
        }

        internal virtual void SetShotForce(float shotSpeed)
        {
            _shotSpeed = shotSpeed;
        }

        internal virtual void SetShotAsShotAlready()
        {
            _isNotShotYet = false;
        }

        protected IEnumerator ShootIndicators()
        {
            //do stuff only while the ball hasn't been shot yet
            if (_isNotShotYet)
            {
                _waitingForCoroutine = true; //prevent doubling of coroutine

                //Instantiate the indicator
                GameObject indicator = Instantiate(_shotIndicatorPrefab, transform.position, Quaternion.Euler(0, 0, _rotationZ));
                _indicators.Add(indicator);

                //give it the physicality of a shot
                Rigidbody2D tempBody = indicator.GetComponent<Rigidbody2D>();
                SetGravityAndVelocity(tempBody);

                //limit amount of on-screen indicators
                LimitIndicatorNumber(_indicators.Count);
            }
            yield return new WaitForSeconds(_indicatorFrequency);
            _waitingForCoroutine = false;
        }

        protected void LimitIndicatorNumber(int amountIndicators)
        {
            if (amountIndicators > SphereManager.Instance.NumberOfIndicators)
            {
                GameObject doomedIndicator = _indicators[0];
                Destroy(doomedIndicator);
                _indicators.RemoveAt(0);
            }
        }

        protected void DestroyAllIndicators()
        {
            foreach (GameObject indicator in _indicators)
            {
                Destroy(indicator);
            }
            _indicators.Clear();
        }

        protected void RotateShot()
        {
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }

        protected void GetDirectionAndRotation()
        {
            _mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _shotPosition = transform.position;
            _direction = _mousePosition - _shotPosition;

            _rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        }

        protected void ShootOnClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlaySoundEffectWithoutLimit(EnumCollection.SFX._0770_Orb_Spawn_Whoosh);
                DestroyAllIndicators();
                SetGravityAndVelocity(_rigidbody);
                _isNotShotYet = false;
                OnShootAdditions();
            }
        }

        protected abstract void OnShootAdditions();

        protected void SetGravityAndVelocity(Rigidbody2D rigidbody)
        {
            rigidbody.gravityScale = _gravity;
            rigidbody.velocity = new Vector2(_direction.x, _direction.y).normalized * _shotSpeed;
        }

        protected bool HasMouseMoved()
        {
            if ((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition) != _mousePosition)
            {
                return true;
            }
            return false;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains(PORTAL_PARAM))
            {
                AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX._0760_Sphere_In_Portal);
                StartCoroutine(GameManager.Instance.SwitchState(EnumCollection.GameState.PlayerActions));
                Destroy(gameObject);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains(ARENA_PARAM))
            {
                OnWallHitSoundEffect();
            }
        }

        protected virtual void OnWallHitSoundEffect()
        {
            AudioManager audioManager = AudioManager.Instance;
            int randomIndex = Random.Range(0, 8);
            switch (randomIndex)
            {
                case 0:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0762_Sphere_On_Wood_01);
                    break;
                case 1:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0763_Sphere_On_Wood_02); break;
                case 2:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0764_Sphere_On_Wood_03); break;
                case 3:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0765_Sphere_On_Wood_04); break;
                case 4:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0766_Sphere_On_Wood_05); break;
                case 5:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0767_Sphere_On_Wood_06); break;
                case 6:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0768_Sphere_On_Wood_07); break;
                case 7:
                    audioManager.PlaySoundEffectWithoutLimit(SFX._0769_Sphere_On_Wood_08); break;
            }
        }

        internal abstract void ShotStackEffect();

        public abstract void SetDisplayDescription();

        #endregion
    }
}
