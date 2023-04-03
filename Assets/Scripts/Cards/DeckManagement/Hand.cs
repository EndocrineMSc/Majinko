using System.Collections.Generic;
using UnityEngine;
using PeggleWars.TurnManagement;
using PeggleWars.Audio;
using System.Collections;
using EnumCollection;
using PeggleWars.Utilities;

namespace PeggleWars.Cards
{
    internal class Hand : MonoBehaviour
    {
        #region Fields and Properties

        internal static Hand Instance { get; private set; }

        [SerializeField] private List<Card> _handCards = new();
        internal List<Card> HandCards { get => _handCards; set => _handCards = value; }

        internal int DrawAmount { get; set; } = 5;

        private List<Card> _instantiatedCards = new();
        internal List<Card> InstantiatedCards { get => _instantiatedCards; set => _instantiatedCards = value; }

        private Deck _deck;

        private Transform _parentTransform;
        private readonly float _spacing = 150f;

        private Canvas _cardCanvas; //The Card Canvas will be a child of the Hand and contain the UI of instantiated Cards
        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _deck = Deck.Instance;
            _cardCanvas = GetComponentInChildren<Canvas>();
            _parentTransform = _cardCanvas.transform;

            TurnManager.Instance.EndCardTurn?.AddListener(OnCardTurnEnd);
            TurnManager.Instance.StartCardTurn?.AddListener(OnCardTurnStart);
            WinLoseConditionManager.Instance.LevelVictory?.AddListener(OnLevelVictory);
        }

        internal void OnLevelVictory()
        {
            TurnManager.Instance.StartCardTurn?.RemoveListener(OnCardTurnStart);
            TurnManager.Instance.EndCardTurn?.RemoveListener(OnCardTurnEnd);
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        internal void OnCardTurnStart()
        {
            DrawHand(DrawAmount);
        }

        internal void OnCardTurnEnd()
        {
            foreach (Card card in _handCards)
            {
                _deck.DiscardPile.Add(card);
            }

            _handCards.Clear();
            StartCoroutine(DisplayHand());

            DrawAmount = 5;
        }

        internal void DrawHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Card card = _deck.DrawCard();
                if (card != null)
                {
                    HandCards.Add(card);
                }
            }

            AudioManager.Instance.PlaySoundEffectOnce(SFX._0012_DrawHand);
            StartCoroutine(DisplayHand());
        }

        //basically makes a new set of displayed instantiated cards for each card in the _handCards list
        internal IEnumerator DisplayHand()
        {
            foreach (Card card in _instantiatedCards)
            {
                Destroy(card.gameObject);
            }

            _instantiatedCards.Clear();

            foreach (Card card in _handCards)
            {
                Card cardObject = Instantiate(card, _parentTransform);
                _instantiatedCards.Add(cardObject);
                AlignCards();
                yield return new WaitForSeconds(0.2f);
            }
        }

        internal void AlignCards()
        {
            // Get the width of the canvas in pixels
            float canvasWidth = _cardCanvas.GetComponent<RectTransform>().rect.width;
            float canvasHeight = _cardCanvas.GetComponent<RectTransform>().rect.height;
            float cardWidth = 0;

            if (_instantiatedCards.Count > 0)
            {
                cardWidth = _instantiatedCards[0].GetComponent<RectTransform>().rect.width;
            }

            // Calculate the total width of all Card GameObjects including spacing
            float totalWidth = (_instantiatedCards.Count) * (_spacing + cardWidth);

            // Calculate the starting x position of the game objects
            float startX = -(totalWidth / 4);

            // Set the position of each game object
            for (int i = 0; i < _instantiatedCards.Count; i++)
            {
                RectTransform rectTransform = _instantiatedCards[i].GetComponent<RectTransform>();
                float cardHeight = rectTransform.rect.height;
                rectTransform.anchoredPosition = new Vector2((startX + (rectTransform.rect.width / 2f) + (i * _spacing)), (-canvasHeight/2 + cardHeight/4));
            }
        }

        #endregion
    }
}
