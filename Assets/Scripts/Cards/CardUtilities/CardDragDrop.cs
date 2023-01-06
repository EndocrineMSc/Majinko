using PeggleWars;
using UnityEngine;
using UnityEngine.EventSystems;
using EnumCollection;
using PeggleWars.Audio;

namespace Cards.DragDrop
{
    public class CardDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields

        private Canvas _canvas;
        private Card _card;

        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private readonly float _cardEffectBorderY = -270;

        private bool _cardHandlingTurn;

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
           _startPosition = gameObject.transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_cardHandlingTurn)
            {
                _startPosition = gameObject.transform.position;
                AudioManager.Instance.PlaySoundEffect(SFX.SFX_0006_CardDrag);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_cardHandlingTurn)
            {
                _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            }  
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_rectTransform.anchoredPosition.y >= _cardEffectBorderY)
            {
                bool enoughMana = _card.CardEndDragEffect(_startPosition);
                if (!enoughMana)
                {
                    AudioManager.Instance.PlaySoundEffect(SFX.SFX_0007_CardDragReturn);
                    gameObject.transform.position = _startPosition;
                }
            }
            else
            {
                AudioManager.Instance.PlaySoundEffect(SFX.SFX_0007_CardDragReturn);
                gameObject.transform.position = _startPosition;
            }
        }

        private void Update()
        {
            if (GameManager.Instance.GameState == State.CardHandling)
            {
                _cardHandlingTurn = true;
            }
            else
            {
                _cardHandlingTurn = false;
            }
        }

        #endregion
    }
}
