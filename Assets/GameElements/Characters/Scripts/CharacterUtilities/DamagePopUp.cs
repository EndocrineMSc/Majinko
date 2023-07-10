using TMPro;
using UnityEngine;

namespace Characters.UI
{
    internal class DamagePopUp : MonoBehaviour
    {
        #region Fields and Properties

        private float _disappearTimer = 2f;
        private readonly float _disappearTimerMax = 2f;
        private TextMeshPro _textMesh;
        private Color _textColor;
        private Color _red = Color.red;
        private float _damageScale;
        private Vector3 _moveVector;

        #endregion

        #region Functions

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        internal void Setup(int damageAmount)
        {
            _textMesh.text = damageAmount.ToString();
            _textMesh.fontSize = 26 + damageAmount;

            _damageScale = Mathf.Clamp01(damageAmount / 100f);

            _textColor = Color.Lerp(_textMesh.color, _red, _damageScale);

            _moveVector = new Vector3(1, 1);
        }

        internal void Setup(string barkText)
        {
            _textMesh.text = barkText;
            _moveVector = new Vector3(2, 2);
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= 8f * Time.deltaTime * _moveVector;

            _disappearTimer -= Time.deltaTime;

            if (_disappearTimer > _disappearTimerMax * 0.5f) 
                //First half of the popup lifetime
                transform.localScale += _damageScale * Time.deltaTime * Vector3.one;
            else
                //Second half of the popup lifetime
                transform.localScale -= + _damageScale * Time.deltaTime * Vector3.one;

            if (_disappearTimer < 0)
            {
                //Start disappearing
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;

                if (_textColor.a < 0)
                    Destroy(gameObject);
            }
        }

        #endregion
    }
}
