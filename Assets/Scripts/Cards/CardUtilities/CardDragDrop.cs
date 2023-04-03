using UnityEngine;
using UnityEngine.EventSystems;
using EnumCollection;
using PeggleWars.Audio;

namespace PeggleWars.Cards
{
    internal class CardDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields

        private Canvas _canvas;
        private Card _card;

        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private readonly float _cardEffectBorderY = -150; //Y above which the game will try to use the card

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
           _startPosition = transform.position;
        }

        #endregion

        #region Functions

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0006_CardDrag);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_rectTransform.anchoredPosition.y >= _cardEffectBorderY)
            {
                bool enoughMana = _card.CardEndDragEffect();
                if (!enoughMana)
                {
                    HandleNotEnoughMana();
                }
            }
            else
            {
                HandleNotEnoughMana();
            }
        }

        private void HandleNotEnoughMana()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0007_CardDragReturn);
            Hand.Instance.DisplayHand();
        }

        #endregion
    }
}
