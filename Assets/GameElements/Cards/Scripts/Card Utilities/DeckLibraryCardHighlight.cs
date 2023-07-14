using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    internal class DeckLibraryCardHighlight : MonoBehaviour, IPointerEnterHandler
    {
        private RectTransform _rectTransform;
        private bool _isPunching;

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPunching)
            {
                _isPunching = true;
                _rectTransform.DOBlendablePunchRotation(Vector3.one * 2, 0.2f, 1, 1f);
                StartCoroutine(PunchTimer());
            }
        }

        private IEnumerator PunchTimer()
        {
            yield return new WaitForSeconds(0.1f);
            _isPunching = false;
        }
    }
}
