using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.ComponentModel.Design;

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
        private readonly float _zoomInDuration = 0.25f;
        private readonly float _moveDistance = 150f;
        private Vector2 _onOtherCardZoomPosition = new();
        private bool _isZoomable = true;

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
            if (!_card.IsBeingDealt && !_isZoomed && _isZoomable)
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
            if (!CardEvents.CardIsZoomed && !_isZoomed)
                ReturnToHandPosition();

            CheckForCorrectYZoom();
        }

        private void ZoomInCard()
        {
            CardEvents.CardIsZoomed = true;
            _isZoomed = true;
            SetUnzoomable();
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
            
            if (!_isZoomed)
                _isZoomable = false;
            
            MoveToOtherCardZoomPosition();
        }

        private void CheckForCorrectYZoom()
        {
            if (transform.position.y != _targetYPosition && _isZoomed)
                transform.DOMoveY(_targetYPosition, _zoomInDuration).OnComplete(CheckForCorrectYZoom);
        }

        private void MoveToOtherCardZoomPosition()
        {
            if (!_isZoomed)
                _rectTransform.DOLocalMove(_onOtherCardZoomPosition, 0.5f).SetEase(Ease.OutCubic);
            else
                _rectTransform.DOLocalMoveX(_onOtherCardZoomPosition.x, 0.5f).SetEase(Ease.OutCubic);
        }

        private void SetUnzoomable()
        {
            //_isZoomable = false;
        }

        private void SetZoomable()
        {
            _isZoomable = true;
        }

        private void ReturnToHandPosition()
        {
            _rectTransform.DOAnchorPos(_card.PositionInHand, _zoomInDuration).OnComplete(SetZoomable);
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
