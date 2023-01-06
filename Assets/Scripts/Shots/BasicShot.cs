using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PeggleWars.Shots
{
    public class BasicShot : MonoBehaviour
    {
        #region Fields

        private Rigidbody2D _rigidbody;
        private bool _unshotBall = true;
        [SerializeField] private float _force;
        private ShotManager _shotManager;


        //fields for indicators
        [SerializeField] private GameObject _shotIndicatorPrefab;
        private List<GameObject> _indicators = new();
        private float _indicatorFrequency = 0.2f;
        private bool _waitingForCoroutine;

        //fields for Rotation
        private Camera _mainCam;
        private Vector2 _shotPosition;
        private Vector2 _direction;
        private Vector2 _mousePosition;
        private float _rotationZ;

        #endregion

        #region Properties

        private bool _destroyBall;

        public bool DestroyBall
        {
            get { return _destroyBall; }
            private set { _destroyBall = value; }
        }


        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            //holds the ball in place until shot per mouseclidc
            _rigidbody.gravityScale = 0;

            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Physics2D.IgnoreLayerCollision(16, 17);
        }

        private void Start()
        {
            _shotManager = ShotManager.Instance;
        }

        private IEnumerator ShootIndicators()
        {
            if (_unshotBall)
            {
                _waitingForCoroutine = true;
                GameObject indicator = Instantiate(_shotIndicatorPrefab, transform.position, Quaternion.Euler(0, 0, _rotationZ));
                _indicators.Add(indicator);
                Rigidbody2D tempBody = indicator.GetComponent<Rigidbody2D>();
                tempBody.gravityScale = 1;
                tempBody.velocity = new Vector2(_direction.x, _direction.y).normalized * _force;
               
                if (_indicators.Count > _shotManager.NumberOfIndicators)
                {
                    GameObject doomedIndicator = _indicators[0];
                    Destroy(doomedIndicator);
                    _indicators.RemoveAt(0);
                }
                
            }
            yield return new WaitForSeconds(_indicatorFrequency);
            _waitingForCoroutine = false;
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
            if (_unshotBall)
            {
                transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
            }
        }

        private void GetDirectionAndRotation()
        {
            _mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            _shotPosition = transform.position;
            _direction = _mousePosition - _shotPosition;

            _rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        }

        private void ShootOnClick()
        {
            if (Input.GetMouseButtonDown(0) && _unshotBall)
            {
                DestroyAllIndicators();

                _rigidbody.gravityScale = 1;
                _rigidbody.velocity = new Vector2(_direction.x, _direction.y).normalized * _force;

                _unshotBall = false;
            }
        }

        private void HasMouseMoved()
        {
            if((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition) != _mousePosition)
            {
                DestroyAllIndicators();
            }

        }

        private void Update()
        {
            GetDirectionAndRotation();
            RotateShot();

            if(!_waitingForCoroutine)
            {
                StartCoroutine(ShootIndicators());
            }
            
            HasMouseMoved(); //destroy indicators on moving mouse for crisper visuals
            ShootOnClick();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Portal"))
            {
                _destroyBall = true;
            }
        }
    }
}
