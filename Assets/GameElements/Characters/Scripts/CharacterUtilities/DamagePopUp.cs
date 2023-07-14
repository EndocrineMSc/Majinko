using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Characters.UI
{
    internal class DamagePopUp : MonoBehaviour
    {
        #region Fields and Properties

        private readonly float _disappearTimerMax = 2f;
        private TextMeshPro _textMesh;

        #endregion

        #region Functions

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
        }

        internal void Setup(int amount, string hexColor = "#FF0A01")
        {
            _textMesh.text = amount.ToString();
            _textMesh.fontSize = 26 + amount;

            var sizeScale = Mathf.Clamp01(amount / 50f);
            var colorScale = Mathf.Clamp01(amount / 10f);

            ColorUtility.TryParseHtmlString(hexColor, out Color basicColor);
            _textMesh.color = Color.Lerp(_textMesh.color, basicColor, colorScale);

            transform.DOMove(transform.position + Vector3.one, _disappearTimerMax);
            transform.DOScale(sizeScale * Vector3.one, _disappearTimerMax);
            _textMesh.DOFade(0, _disappearTimerMax);
            StartCoroutine(DestroyAfterDelay());
        }

        internal void Setup(string barkText, string hexColor = "#FF0A01")
        {
            _textMesh.text = barkText;

            ColorUtility.TryParseHtmlString(hexColor, out Color basicColor);
            _textMesh.color = basicColor;

            transform.DOJump(transform.position + new Vector3(1, 0.5f, 0), 1, 1, _disappearTimerMax);
            _textMesh.DOFade(0, _disappearTimerMax);
            StartCoroutine(DestroyAfterDelay());
        }
        
        private IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(_disappearTimerMax * 1.2f);
            Destroy(gameObject);
        }

        #endregion
    }
}
