using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Cards
{
    internal class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler
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
        private readonly float _zoomInDuration = 0.1f;
        private readonly float _moveDistance = 30000f;
        private Vector2 _onOtherCardZoomPosition = new();
        private float _moveDelayTimer = 0;
        private readonly float _moveDelayTimerMax = 0.5f;

        #endregion

        #region Functions

        private void OnEnable()
        {
            CardEvents.OnCardZoomIn += OnCardZoomIn;
        }

        private void OnDisable()
        {
            CardEvents.OnCardZoomIn -= OnCardZoomIn;
        }

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
            if (!_card.IsBeingDealt && !_isZoomed)
            {
                //the last sibling will be in front of the other cards
                _index = transform.GetSiblingIndex();
                transform.SetAsLastSibling();
                ZoomInCard();
                CardEvents.RaiseCardZoomIn(_card.PositionInHand);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_card.IsBeingDealt && _isZoomed)
            {
                ZoomOutCard();
                CardEvents.InvokeCardZoomOut();                           
            }
        }

        private void Update()
        {
            if (_moveDelayTimer > 0)
                _moveDelayTimer--;
            else
                _moveDelayTimer = 0;

            if (!CardEvents.CardIsZoomed && !_isZoomed && _moveDelayTimer == 0)
                ReturnToHandPosition();
        }

        private void ZoomInCard()
        {
            CardEvents.CardIsZoomed = true;
            _isZoomed = true;
            _initialEulerAngles = transform.eulerAngles;
            transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize);
            transform.eulerAngles = new Vector3(0, 0, 0);

            float zoomOffset = _rectTransform.rect.height / 1.25f;
            transform.DOMoveY(_targetYPosition + zoomOffset, _zoomInDuration);
        }

        private void ZoomOutCard()
        {
            CardEvents.CardIsZoomed = false;
            _isZoomed = false;
            transform.localScale = _normalScale;
            transform.SetSiblingIndex(_index);
            transform.eulerAngles = _initialEulerAngles;           
        }

        private void OnCardZoomIn(Vector3 otherCardPosition)
        {
            _moveDelayTimer = _moveDelayTimerMax;
            var deltaXTransform = otherCardPosition.x - _card.PositionInHand.x;
            deltaXTransform = deltaXTransform == 0 ? 0.01f : deltaXTransform;
            var distanceModifier = (1 / (Mathf.Abs(deltaXTransform) * 2));
            var finalMoveDistance = distanceModifier * _moveDistance;

            if (deltaXTransform > 0)
                _onOtherCardZoomPosition = new(_card.PositionInHand.x - finalMoveDistance, _card.PositionInHand.y);
            else if (deltaXTransform < 0)
                _onOtherCardZoomPosition = new(_card.PositionInHand.x + finalMoveDistance, _card.PositionInHand.y);
            else
                _onOtherCardZoomPosition = _card.PositionInHand;

            if (!_isZoomed)
                MoveToOtherCardZoomPosition();
        }

        private void MoveToOtherCardZoomPosition()
        {
            _rectTransform.DOLocalMove(_onOtherCardZoomPosition, 0.5f).SetEase(Ease.OutCubic);
        }

        private void ReturnToHandPosition()
        {
            if (_moveDelayTimer == 0)
                _rectTransform.DOAnchorPos(_card.PositionInHand, _zoomInDuration);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.DOKill();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ZoomOutCard();
        }

        #endregion
    }
}
