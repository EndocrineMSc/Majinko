using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.ComponentModel.Design;

namespace Cards
{
    internal class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDropHandler
    {
        #region Fields and Properties

        //state booleans
        private bool _isDragged;
        private bool _isZoomed;

        //movement values
        private readonly float _zoomSize = 1.5f;
        private readonly float _moveDistance = 100f;
        private readonly float _xMoveSpeed = 650f;
        private readonly float _yMoveSpeed = 2000f;
        private float _targetYPosition;

        //references
        private Card _card;
        private RectTransform _rectTransform;
        private Vector2 _zoomedCardPosition = new();

        //start states
        private Vector3 _normalScale;
        private int _index;
        private Vector3 _initialEulerAngles;

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
            //set references
            _card = GetComponent<Card>();
            _rectTransform = GetComponent<RectTransform>();

            //set start stats
            _normalScale = new Vector3(0.75f, 0.75f, 0.75f);
            _initialEulerAngles = transform.eulerAngles;
            _index = transform.GetSiblingIndex();

            //zoom position to be roughly aligned with bottom of the screen
            _targetYPosition = GetComponentInParent<RectTransform>().rect.yMin - _rectTransform.rect.height / 1.3f;
        }

        private void Update()
        {
            var positionInHand = GetComponent<Card>().PositionInHand;

            //only move as soon as card dealing is finished
            if (!_card.IsBeingDealt)
            {
                //if zoomed in, but not dragged by player, move to zoom position on y axis
                if (_isZoomed && !_isDragged)
                    ZoomUpMovement();

                //if not zoomed in and not dragged by player, move to start position on y axis
                if (!_isZoomed && !_isDragged)
                    ZoomDownMovement();

                //if another card is zoomed in, move right or left depending on position
                if (!_isZoomed && CardEvents.CardIsZoomed && _zoomedCardPosition != Vector2.zero
                    && _zoomedCardPosition != positionInHand)
                    MoveToOtherCardZoomPosition();

                //if no card is zoomed in move on x axis to start position
                if (!CardEvents.CardIsZoomed || _isZoomed)
                    ReturnToXHandPosition();

                if (!_isDragged && _rectTransform.anchoredPosition.y > _targetYPosition)
                    ZoomDownMovement();
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_card.IsBeingDealt && !_isZoomed)
            {
                //the last sibling will be in front of the other cards
                _index = transform.GetSiblingIndex();
                transform.SetAsLastSibling();
                ZoomInCard();              
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_card.IsBeingDealt && _isZoomed)
                ZoomOutCard();
        }

        private void ZoomInCard()
        {
            var positionInHand = GetComponent<Card>().PositionInHand;

            _isZoomed = true;
            _zoomedCardPosition = positionInHand;
            CardEvents.RaiseCardZoomIn(positionInHand);
            _initialEulerAngles = transform.eulerAngles;
            transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        private void ZoomUpMovement()
        {
            if (_rectTransform.anchoredPosition.y < _targetYPosition)
                _rectTransform.Translate(_yMoveSpeed * Time.deltaTime * Vector2.up, Space.World);
        }

        private void ZoomDownMovement()
        {
            float speed; //different speeds depending on distance to bottom position

            if (_rectTransform.anchoredPosition.y > _targetYPosition)
                speed = _yMoveSpeed;
            else
                speed = _yMoveSpeed / 4;

            if (_rectTransform.anchoredPosition.y > _card.PositionInHand.y + 2f)
                _rectTransform.Translate(speed * Time.deltaTime * Vector2.down);
            else if (_rectTransform.anchoredPosition.y < _card.PositionInHand.y - 2f)
                _rectTransform.Translate(speed * Time.deltaTime * Vector2.up);
        }

        private void ZoomOutCard()
        {
            _isZoomed = false;
            _zoomedCardPosition = Vector2.zero;
            CardEvents.InvokeCardZoomOut();
            transform.localScale = _normalScale;
            transform.SetSiblingIndex(_index);
            transform.eulerAngles = _initialEulerAngles;           
        }

        private void OnCardZoomIn(Vector3 otherCardPosition)
        {
            _zoomedCardPosition = otherCardPosition;
        }

        private void MoveToOtherCardZoomPosition()
        {
            var positionInHand = GetComponent<Card>().PositionInHand;

            if (positionInHand.x - _zoomedCardPosition.x > 0
                && _rectTransform.localPosition.x < positionInHand.x + _moveDistance)
            {
                _rectTransform.Translate(_xMoveSpeed * Time.deltaTime * Vector2.right);
            }
            else if (positionInHand.x - _zoomedCardPosition.x < 0
                && _rectTransform.localPosition.x > positionInHand.x - _moveDistance)
            {
                _rectTransform.Translate(_xMoveSpeed * Time.deltaTime * Vector2.left);
            }
        }

        private void ReturnToXHandPosition()
        {
            var positionInHand = GetComponent<Card>().PositionInHand;

            if (_rectTransform.localPosition.x > (positionInHand.x + 2f))
            {
                _rectTransform.Translate(_xMoveSpeed * Time.deltaTime * Vector2.left);
            }
            else if (_rectTransform.localPosition.x < (positionInHand.x - 2f))
            {
                _rectTransform.Translate(_xMoveSpeed * Time.deltaTime * Vector2.right);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragged = true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            _isDragged = false;
        }

        #endregion
    }
}
