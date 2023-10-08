using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.ComponentModel.Design;

namespace Cards
{
    internal class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDropHandler
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
        private readonly float _zoomInDuration = 0.25f;
        private readonly float _moveDistance = 150f;
        private Vector2 _onOtherCardZoomPosition = new();
        private bool _isDragged = false;

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
            _targetYPosition += _rectTransform.rect.height / 1.25f;
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
            if (!CardEvents.CardIsZoomed && !_isZoomed && !_isDragged)
                ReturnToHandPosition();

            if (!_isDragged)
                CheckForCorrectYZoom();
        }

        private void ZoomInCard()
        {
            CardEvents.CardIsZoomed = true;
            _isZoomed = true;
            _initialEulerAngles = transform.eulerAngles;
            transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize);
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.DOMoveY(_targetYPosition, _zoomInDuration).OnComplete(CheckForCorrectYZoom);
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
            var deltaXTransform = otherCardPosition.x - _card.PositionInHand.x;
            float zoomOffset = _rectTransform.rect.height / 1.25f;

            if (deltaXTransform > 0)
                _onOtherCardZoomPosition = new(_card.PositionInHand.x - _moveDistance, _card.PositionInHand.y);
            else if (deltaXTransform < 0)
                _onOtherCardZoomPosition = new(_card.PositionInHand.x + _moveDistance, _card.PositionInHand.y);
            else
                _onOtherCardZoomPosition = new(_card.PositionInHand.x, zoomOffset);
            
            MoveToOtherCardZoomPosition();
        }

        private void CheckForCorrectYZoom()
        {
            if (transform.position.y != _targetYPosition && _isZoomed && !_isDragged)
                transform.DOMoveY(_targetYPosition, _zoomInDuration).OnComplete(CheckForCorrectYZoom);
        }

        private void MoveToOtherCardZoomPosition()
        {
            if (!_isZoomed)
                _rectTransform.DOLocalMove(_onOtherCardZoomPosition, 0.5f).SetEase(Ease.OutCubic);
            else
                _rectTransform.DOLocalMoveX(_onOtherCardZoomPosition.x, 0.5f).SetEase(Ease.OutCubic);
        }

        private void ReturnToHandPosition()
        {
            if (!_isDragged)
                _rectTransform.DOAnchorPos(_card.PositionInHand, _zoomInDuration);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragged = true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            _isDragged = false;
            ZoomOutCard();
        }

        #endregion
    }
}
