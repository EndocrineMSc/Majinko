using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.DragDrop
{
    public class CardDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields

        private Canvas _canvas;
        private Card _card;

        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private readonly float _cardEffectBorderY = -5;

        #endregion

        #region Private Functions

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            GameObject cardCanvas = GameObject.FindGameObjectWithTag("CardCanvas");
            _canvas = cardCanvas.GetComponent<Canvas>();
            _card = GetComponent<Card>();
            _startPosition = gameObject.transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = gameObject.transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_rectTransform.anchoredPosition.y >= _cardEffectBorderY)
            {
                _card.CardDropEffect();
            }
            else
            {
                gameObject.transform.position = _startPosition;
            }
        }        

        #endregion
    }
}
