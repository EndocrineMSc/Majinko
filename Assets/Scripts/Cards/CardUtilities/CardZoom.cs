using UnityEngine;
using UnityEngine.EventSystems;

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

        #endregion

        #region Private Functions

        private void Start()
        {
            _normalScale = transform.localScale;
            _startPosition = transform.position;

            //index in hierarchy determines which UI element is in front, so _index is the default state of the card
            _index = transform.GetSiblingIndex();
            _otherCardsMovement = Hand.Instance.GetComponent<CardZoomEventHandler>();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(_zoomSize,_zoomSize,_zoomSize);
            transform.position = new Vector3(transform.position.x, transform.position.y + _zoomOffset, transform.position.z);
            
            _otherCardsMovement.InvokeCardZoomIn(transform.position);
            
            //the last sibling will be in front of the other cards
            transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _normalScale;
            transform.SetSiblingIndex(_index);

            _otherCardsMovement.InvokeCardZoomOut(_startPosition);

            if (transform.position != _startPosition)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - _zoomOffset, transform.position.z);
            }
        }

        #endregion
    }
}
