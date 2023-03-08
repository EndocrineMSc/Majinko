using System.Collections.Generic;
using UnityEngine;
using PeggleWars.TurnManagement;
using PeggleWars.Audio;
using System.Collections;
using EnumCollection;

namespace PeggleWars.Cards.DeckManagement.HandHandling
{
    /// <summary>
    /// This class handles the cards in hand and especially the instantiation and destruction of the respective
    /// GameObject the player will see and interact with.
    /// </summary>
    public class Hand : MonoBehaviour
    {
        #region Fields and Properties
        public static Hand Instance { get; private set; }

        [SerializeField] private List<Card> _handCards = new();
        public List<Card> HandCards { get => _handCards; set => _handCards = value; }

        public int DrawAmount { get; set; } = 5;

        private List<Card> _instantiatedCards = new();
        public List<Card> InstantiatedCards { get => _instantiatedCards; set => _instantiatedCards = value; }

        private Deck _deck;
        private TurnManager _turnManager;

        private Transform _parentTransform;
        private readonly float _spacing = 150f;

        private Canvas _cardCanvas; //The Card Canvas will be a child of the Hand and contain the UI of instantiated Cards
        #endregion

        #region Public Functions

        public void DrawHand(int amount)
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

        public void OnCardTurnStart()
        {
            DrawHand(DrawAmount);
        }

        public void OnCardTurnEnd()
        {
            foreach (Card card in _handCards)
            {
                _deck.DiscardPile.Add(card);
            }

            _handCards.Clear();
            StartCoroutine(DisplayHand());

            DrawAmount = 5;
        }

        //basically makes a new set of displayed instantiated cards for each card in the _handCards list
        public IEnumerator DisplayHand()
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

        public void RemoveCardFromHand(Card card)
        {
            _handCards.Remove(card);
        }

        public void AlignCards()
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
                rectTransform.anchoredPosition = new Vector2(startX + (rectTransform.rect.width / 2f) + (i * _spacing), (-canvasHeight/2 + cardHeight/4));
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
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

            _turnManager = GameManager.Instance.GetComponent<TurnManager>();
            _turnManager.EndCardTurn += OnCardTurnEnd;
            _turnManager.StartCardTurn += OnCardTurnStart;
        }

        private void OnDisable()
        {
            _turnManager.EndCardTurn -= OnCardTurnEnd;
            _turnManager.StartCardTurn -= OnCardTurnStart;
        }

        #endregion
    }
}
