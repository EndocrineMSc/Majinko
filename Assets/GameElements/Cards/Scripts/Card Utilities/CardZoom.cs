using UnityEngine;
using UnityEngine.EventSystems;
using Utility;
using DG.Tweening;

namespace Cards
{
    internal class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields and Properties

        private Vector3 _normalScale;
        private readonly float _zoomSize = 1.5f;
        private float _targetYPosition;
        private int _index;
        private Vector3 _initialEulerAngles;
        private Card _card;
        private RectTransform _rectTransform;
        private bool _isZoomed;

        #endregion

        #region Functions

        private void Start()
        {
            _card = GetComponent<Card>();
            _normalScale = new Vector3(0.75f, 0.75f, 0.75f);
            _initialEulerAngles = transform.eulerAngles;
            _targetYPosition = Camera.main.ScreenToWorldPoint(new(0, 0, 0)).y;
            _index = transform.GetSiblingIndex();
            _rectTransform = GetComponent<RectTransform>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.Instance.GameState != GameState.LevelWon && !_card.IsBeingDealt && !_isZoomed)
            {
                ZoomInCard();
                CardEvents.RaiseCardZoomIn(transform.position);
            }

            //the last sibling will be in front of the other cards
            _index = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (GameManager.Instance.GameState != GameState.LevelWon && !_card.IsBeingDealt && _isZoomed)
            {
                ZoomOutCard();
                CardEvents.InvokeCardZoomOut();
                
                if (Hand.Instance != null)                
                    Hand.Instance.AlignCardsWrap();                
            }
        }

        private void ZoomInCard()
        {
            _isZoomed = true;
            _initialEulerAngles = transform.eulerAngles;
            transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize);
            transform.eulerAngles = new Vector3(0, 0, 0);

            float zoomOffset = _rectTransform.rect.height / 1.25f;
            transform.DOMoveY(_targetYPosition + zoomOffset, 0.1f);
        }

        private void ZoomOutCard()
        {
            _isZoomed = false;
            transform.localScale = _normalScale;
            transform.SetSiblingIndex(_index);
            transform.eulerAngles = _initialEulerAngles;
        }

        #endregion
    }
}
