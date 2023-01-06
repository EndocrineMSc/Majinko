using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using TMPro;
using Cards.DeckManagement.Global;
using Cards.DeckManagement.HandHandling;

namespace Cards.DeckManagement
{
    public class DeckManager : MonoBehaviour
    {
        #region Fields

        public static DeckManager Instance { get; private set; }

        private List<Card> _localDeck = new();
        private List<Card> _discardPile = new();
        private List<Card> _exhaustPile = new();

        public int DrawAmount { get; set; } = 5;
        public List<Card> DiscardPile { get => _discardPile; set => _discardPile = value; }
        public List<Card> LocalDeck { get => _localDeck; set => _localDeck = value; }
        public List<Card> DiscardPile1 { get => _discardPile; set => _discardPile = value; }

        private HandManager _hand;

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
            _localDeck = GlobalDeckManager.Instance.GlobalDeck;
            ShuffleDeck();
            _hand = HandManager.Instance;
        }

        #endregion

        #region Public Functions

        public Card DrawCard()
        {
            if (_localDeck.Count == 0)
            {
                _localDeck.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }

            Card card = _localDeck[0];
            _localDeck.RemoveAt(0);
            return card;
        }

        public void DiscardCard(Card card)
        {
            _discardPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        public void ExhaustCard(Card card)
        {
            _exhaustPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        //Shuffles the deck using the Fisher-Yates shuffle algortihm
        public void ShuffleDeck()
        {
            for (int i = _localDeck.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Card temp = _localDeck[i];
                _localDeck[i] = _localDeck[j];
                _localDeck[j] = temp;
            }
        }

        #endregion
    }
}
