using UnityEngine;


namespace ManaManagement
{
    internal class FireMana : Mana
    {
        [SerializeField] private bool _hasRested;
        [SerializeField] private bool _isBoiling;
        [SerializeField] private float _currentTime = 0;
        [SerializeField] private float _boilSpeed = 0.0005f;
        [SerializeField] private readonly float _boilCooldown = 0.05f;
        private Rigidbody2D _rigidbody;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = (Vector2.up * 10);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Mana>(out _))
            {
                _currentTime = 0;
            }

            if (collision.gameObject.TryGetComponent<IBoiler>(out _))
            {
                _isBoiling = true;
                _hasRested = true;
                _rigidbody.gravityScale = 0;
                _rigidbody.mass = 1500;
            }
        }

        private void Update()
        {
            if(!_hasRested && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                _hasRested = true;
                _rigidbody.mass = 5;
            }

            if (_isBoiling)
            {
                _rigidbody.velocity = Vector2.up * _boilSpeed;
                _currentTime += Time.deltaTime;
            }

            if (_isBoiling && _currentTime >= _boilCooldown)
            {
                _isBoiling = false;
                _rigidbody.gravityScale = 1;
                _rigidbody.mass = 5;
                _currentTime = 0;
            }
        }
    }
}