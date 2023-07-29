using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    internal class ShopRelicButton : MonoBehaviour, IPointerEnterHandler
    {
        bool _isPunching;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPunching)
                StartCoroutine(PunchButton());
        }

        private IEnumerator PunchButton()
        {
            _isPunching = true;
            transform.DOPunchScale(new(1.01f, 1.01f), 0.5f, 1, 1);
            yield return new WaitForSeconds(0.5f);
            _isPunching = false;
        }
    }
}
