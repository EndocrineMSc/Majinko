using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace PeggleAttacks.AttackVisuals.PopUps
{
    public class DamagePopUp : MonoBehaviour
    {
        #region Fields and Properties

        private float _disappearTimer = 1f;
        private readonly float _disappearTimerMax = 1f;
        private TextMeshPro _textMesh;
        private Color _textColor;
        private Color _red = Color.red;
        private float _damageScale;
        private Vector3 _moveVector;

        #endregion

        #region Public Functions

        public void Setup(int damageAmount)
        {
            _textMesh.text = damageAmount.ToString();
            _textMesh.fontSize = 26 + damageAmount;

            _damageScale = Mathf.Clamp01(damageAmount / 100f);

            _textColor = Color.Lerp(_textMesh.color, _red, _damageScale);

            _moveVector = new Vector3(1, 1);
        }

        #endregion

        #region Private Functions

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= 8f * Time.deltaTime * _moveVector;

            _disappearTimer -= Time.deltaTime;

            if (_disappearTimer > _disappearTimerMax * 0.5f) 
            {
                //First half of the popup lifetime
                transform.localScale += _damageScale * Time.deltaTime * Vector3.one;
            }
            else
            {
                //Second half of the popup lifetime
                transform.localScale -= + _damageScale * Time.deltaTime * Vector3.one;
            }

            if (_disappearTimer < 0)
            {
                //Start disappearing
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;

                if (_textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        #endregion

    }
}
