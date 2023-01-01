using System.Collections.Generic;
using UnityEngine;
using PeggleWars;
using UnityEngine.UI;
using System;
using PeggleWars.TurnManagement;

namespace Cards.DeckManagement.HandHandling
{
    //This class will:
    //Instantiate the Cards in hand and destroy the gameobjects after playing them or at turn end
    public class HandManager : MonoBehaviour
    {
        #region Fields and Properties
        public static HandManager Instance { get; private set; }

        [SerializeField] private List<Card> _handCards = new List<Card>();
        public List<Card> HandCards { get => _handCards; set => _handCards = value; }

        private List<GameObject> _handObjects = new List<GameObject>();

        private DeckManager _deckManager;
        private CardTurnManager _cardTurnManager;
        [SerializeField] private GameObject _cardCanvas;
        private Transform _parentTransform;

        #endregion

        #region Public Functions

        public void DrawHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Card card = _deckManager.DrawCard();
                Instance.HandCards.Add(card);
            }
            Instance.DisplayHand();
        }

        public void DisplayHand()
        {
            foreach (GameObject cardObject in _handObjects)
            {
                Destroy(cardObject);
            }

            _handObjects.Clear();

            foreach (Card card in _handCards)
            {
                GameObject cardObject = Instantiate(card.CardPrefab, _parentTransform);
                _handObjects.Add(cardObject);
            }
        }

        public void OnCardTurnStart()
        {
            Instance.DrawHand(DeckManager.Instance.DrawAmount);
        }

        public void OnCardTurnEnd()
        {
            foreach (Card card in _handCards)
            {
                _deckManager.DiscardPile.Add(card);
            }

            Instance._handCards.Clear();
            Instance.DisplayHand();
        }

        public void RemoveCardFromHand(Card card)
        {
            _handCards.Remove(card);
        }

        #endregion

        #region Private Functions

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
            Instance._deckManager = DeckManager.Instance;
            Instance._parentTransform = _cardCanvas.transform;
            Instance._cardTurnManager = GameManager.Instance.GetComponent<CardTurnManager>();
            Instance._cardTurnManager.EndCardTurn += OnCardTurnEnd;
            Instance._cardTurnManager.StartCardTurn += OnCardTurnStart;
        }

        private void OnDisable()
        {
            Instance._cardTurnManager.EndCardTurn -= OnCardTurnEnd;
            Instance._cardTurnManager.StartCardTurn -= OnCardTurnStart;
        }

        #endregion
    }
}
