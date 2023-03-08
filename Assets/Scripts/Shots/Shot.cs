using PeggleWars.Shots;
using PeggleWars.TurnManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


namespace PeggleWars.Shots
{
    public abstract class Shot : MonoBehaviour
    {
        #region Fields and Properties

        //fields for physicality
        protected Rigidbody2D _rigidbody;
        protected bool _isNotShotYet = true;
        protected bool _isInShootingTurn;
        protected float _shotSpeed = 7f;
        protected ShotManager _shotManager;
        protected TurnManager _turnManager;

        //fields for indicators
        [SerializeField] protected GameObject _shotIndicatorPrefab;
        protected List<GameObject> _indicators = new();
        protected float _indicatorFrequency = 0.2f;
        protected bool _waitingForCoroutine;

        //fields for Rotation to mouse
        protected Camera _mainCam;
        protected Vector2 _direction;
        protected Vector2 _mousePosition;
        protected float _rotationZ;

        protected string PORTAL_PARAM = "Portal";

        public float ShotSpeed
        {
            get { return _shotSpeed; }
            protected set { _shotSpeed = value; }
        }

        protected float _gravity = 1;

        public float Gravity
        {
            get { return _gravity; }
            protected set { _gravity = value; }
        }

        #endregion

        #region Functions

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            //holds the ball in place until shot per mouseclick
            _rigidbody.gravityScale = 0;
            _mainCam = Camera.main;
            Physics2D.IgnoreLayerCollision(16, 17);
        }

        protected virtual void Start()
        {
            _shotManager = ShotManager.Instance;
            _turnManager = TurnManager.Instance;
            _turnManager.EndCardTurn += OnCardTurnEnd;
            _shotManager.ShotStackedEvent?.AddListener(ShotStackEffect);
        }

        protected virtual void OnDisable()
        {
            _turnManager.EndCardTurn -= OnCardTurnEnd;
            _shotManager.ShotStackedEvent.RemoveListener(ShotStackEffect);
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
            if (_isNotShotYet && _isInShootingTurn)
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

        public virtual void SetShotForce(float shotSpeed)
        {
            _shotSpeed = shotSpeed;
        }

        public virtual void SetShotAsShotAlready()
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
            if (amountIndicators > _shotManager.NumberOfIndicators)
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
                StartCoroutine(GameManager.Instance.SwitchState(EnumCollection.GameState.PlayerActions));
                Destroy(gameObject);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            OnWallHitSoundEffect();
        }

        protected virtual void OnWallHitSoundEffect()
        {
            //ToDo: implement SoundEffect;
        }

        public abstract void ShotStackEffect();
            

        #endregion
    }
}
