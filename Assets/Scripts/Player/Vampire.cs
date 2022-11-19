using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampeggle;
using EnumCollection;

namespace Player
{
    public class Vampire : MonoBehaviour
    {

        #region Fields

        private SpriteRenderer _spriteRenderer;
        private Color _color;
        private bool _eatsBlood;

        [SerializeField] private VampireBat _bat;
        public static Vampire Nosferatu;
        private Animator _animator;

        #endregion

        #region Properties

        [SerializeField] private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        private float _damage;

        public float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        #endregion

        #region Public Functions

        public void ShootBat()
        {
            _bat.Damage = _damage;
            if (_damage > 0)
            {
                VampireBat _shotBat = Instantiate(_bat, new Vector3(transform.position.x + 1.2f, transform.position.y + 0.75f, transform.position.z), Quaternion.identity);
                _shotBat.Damage = _damage;
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            StartCoroutine(nameof(ColorShiftDamage));
            _animator.SetTrigger("Hurt");
        }

        #endregion

        #region Private Functions

        private void Start()
        {
            Nosferatu = this;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = _spriteRenderer.color;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Blood"))
            {
                Destroy(collision.gameObject);
                _damage += 1;

                if (!_eatsBlood)
                {
                    _eatsBlood = true;
                    StartCoroutine(nameof(ColorShiftBlood));
                }
            }
        }
        #endregion

        #region IEnumerators

        private IEnumerator ColorShiftBlood()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _color;
            _eatsBlood = false;
        }

        private IEnumerator ColorShiftDamage()
        {
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = _color;
            _eatsBlood = false;
        }

        #endregion
    }
}
