using PeggleWars.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using EnumCollection;

namespace Cards.Zoom
{
    public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        private Vector3 _normalScale;
        private readonly float _zoomOffset = 90;
        private readonly float _zoomSize = 1.5f;
        private int _index;
        private Vector3 _startPosition;

        #endregion

        #region Private Functions

        private void Start()
        {
            _normalScale = transform.localScale;
            _index = transform.GetSiblingIndex();
            _startPosition = transform.position;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(_zoomSize,_zoomSize,_zoomSize);
            transform.position = new Vector3(transform.position.x, transform.position.y + _zoomOffset, transform.position.z);
            transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _normalScale;
            transform.SetSiblingIndex(_index);

            if (transform.position != _startPosition)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - _zoomOffset, transform.position.z);
            }
        }

        #endregion
    }
}
