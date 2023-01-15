using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PeggleWars.Shots
{
    /// <summary>
    /// This class handles what a shot is, how its physics work and implements the shot prediction indicators.
    /// All other shots should derive from this class and inherit it.
    /// </summary>
    public class BasicShot : MonoBehaviour
    {
        #region Fields and Properties

        //fields for physicality
        private Rigidbody2D _rigidbody;
        private bool _isNotShotYet = true;
        [SerializeField] private float _force;
        private ShotManager _shotManager;

        //fields for indicators
        [SerializeField] private GameObject _shotIndicatorPrefab;
        private List<GameObject> _indicators = new();
        private float _indicatorFrequency = 0.2f;
        private bool _waitingForCoroutine;

        //fields for Rotation to mouse
        private Camera _mainCam;
        private Vector2 _direction;
        private Vector2 _mousePosition;
        private float _rotationZ;

        private bool _destroyBall;

        public bool DestroyBall
        {
            get { return _destroyBall; }
            private set { _destroyBall = value; }
        }

        private int _gravity = 1;

        public int Gravity
        {
            get { return _gravity; }
            private set { _gravity = value; }
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            //holds the ball in place until shot per mouseclick
            _rigidbody.gravityScale = 0;
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Physics2D.IgnoreLayerCollision(16, 17);
        }

        private void Start()
        {
            _shotManager = ShotManager.Instance;
        }

        private void Update()
        {
            if (_isNotShotYet)
            {
                GetDirectionAndRotation();
                RotateShot();

                bool hasMoved = HasMouseMoved(); //destroy indicators on moving mouse for crisper visuals
                if (hasMoved)
                {
                    DestroyAllIndicators();
                }

                if (!_waitingForCoroutine)
                {
                    StartCoroutine(ShootIndicators());
                }
                ShootOnClick();
            }
        }

        private IEnumerator ShootIndicators()
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

        private void LimitIndicatorNumber(int amountIndicators)
        {
            if (amountIndicators > _shotManager.NumberOfIndicators)
            {
                GameObject doomedIndicator = _indicators[0];
                Destroy(doomedIndicator);
                _indicators.RemoveAt(0);
            }
        }

        private void DestroyAllIndicators()
        {
            foreach (GameObject indicator in _indicators)
            {
                Destroy(indicator);
            }
            _indicators.Clear();
        }

        private void RotateShot()
        {
            transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
        }

        private void GetDirectionAndRotation()
        {
            _mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _shotPosition = transform.position;
            _direction = _mousePosition - _shotPosition;

            _rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        }

        private void ShootOnClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DestroyAllIndicators();
                SetGravityAndVelocity(_rigidbody);               
                _isNotShotYet = false;
            }
        }

        private void SetGravityAndVelocity(Rigidbody2D rigidbody)
        {
            rigidbody.gravityScale = _gravity;
            rigidbody.velocity = new Vector2(_direction.x, _direction.y).normalized * _force;
        }

        private bool HasMouseMoved()
        {
            if((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition) != _mousePosition)
            {
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Portal"))
            {
                _destroyBall = true;
            }
        }

        #endregion
    }
}
