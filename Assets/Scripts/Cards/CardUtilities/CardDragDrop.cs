using UnityEngine;
using UnityEngine.EventSystems;
using Audio;

namespace Cards
{
    /// <summary>
    /// This class is a required component of the class Card.
    /// </summary>
    internal class CardDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields and Properties

        private Canvas _cardCanvas;
        private Card _card;

        private RectTransform _rectTransform;
        private readonly float _cardEffectBorderY = -150; //Y above which the game will try to use the card

        private const string CARDCANVAS_OBJECT = "CardCanvas";

        #endregion

        #region Functions

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _cardCanvas = GameObject.FindGameObjectWithTag(CARDCANVAS_OBJECT).GetComponent<Canvas>();
            _card = GetComponent<Card>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0004_CardDrag);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _cardCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_rectTransform.anchoredPosition.y >= _cardEffectBorderY)
            {
                bool enoughMana = _card.CardEndDragEffect();
                
                if (!enoughMana)
                    ReturnCardToHand();
            }
            else
            {
                ReturnCardToHand();
            }
        }

        private void ReturnCardToHand()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0005_CardDragReturn);
            Hand.Instance.AlignCards();
        }

        #endregion
    }
}
