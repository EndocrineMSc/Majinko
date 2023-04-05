using UnityEngine;
using UnityEngine.EventSystems;
using EnumCollection;

namespace PeggleWars.Cards
{
    internal class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        private Vector3 _normalScale;
        private readonly float _zoomOffset = 90;
        private readonly float _zoomSize = 1.5f;
        private int _index;
        private Vector3 _startPosition;
        private CardZoomEventHandler _otherCardsMovement;
        private Vector3 _initialEulerAngles;

        #endregion

        #region Private Functions

        private void Start()
        {
            _normalScale = new Vector3(0.75f, 0.75f, 0.75f);
            _startPosition = transform.position;
            _initialEulerAngles = transform.eulerAngles;

            //index in hierarchy determines which UI element is in front, so _index is the default state of the card
            _index = transform.GetSiblingIndex();
            _otherCardsMovement = Hand.Instance.GetComponent<CardZoomEventHandler>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.Instance.GameState != GameState.LevelWon)
            {
                _initialEulerAngles = transform.eulerAngles;
                transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize);
                transform.position = new Vector3(transform.position.x, transform.position.y + _zoomOffset, transform.position.z);
                transform.eulerAngles = new Vector3(0, 0, 0);

                _otherCardsMovement.InvokeCardZoomIn(transform.position);

                //the last sibling will be in front of the other cards
                transform.SetAsLastSibling();
            }
            else
            {
                transform.localScale = new Vector3(_zoomSize, _zoomSize, _zoomSize); 
                transform.SetAsLastSibling();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (GameManager.Instance.GameState != GameState.LevelWon)
            {
                transform.localScale = _normalScale;
                transform.SetSiblingIndex(_index);
                transform.eulerAngles = _initialEulerAngles;

                _otherCardsMovement.InvokeCardZoomOut(_startPosition);

                if (transform.position != _startPosition)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - _zoomOffset, transform.position.z);
                }
            }
            else
            {
                transform.localScale = _normalScale;
            }
        }

        #endregion
    }
}
