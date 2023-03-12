using PeggleWars.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Cards
{
    [RequireComponent(typeof(DisplayDeck))]
    internal class Deck : MonoBehaviour
    {
        #region Fields and Properties

        internal static Deck Instance { get; private set; }

        [SerializeField] private List<Card> _localDeck = new();
        [SerializeField] private List<Card> _discardPile = new();
        private List<Card> _exhaustPile = new();

        internal List<Card> DiscardPile { get => _discardPile; set => _discardPile = value; }
        internal List<Card> LocalDeck { get => _localDeck; set => _localDeck = value; }
        internal List<Card> ExhaustPile { get => _exhaustPile; set => _exhaustPile = value; }

        private Hand _hand;

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
            BuildDeckFromGlobalDeck(GlobalDeckManager.Instance.GlobalDeck);
            _hand = Hand.Instance;
            ShuffleDeck();
            WinLoseConditionManager.Instance.LevelVictory?.AddListener(OnLevelVictory);
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        private void BuildDeckFromGlobalDeck(List<Card> globalDeck)
        {
            foreach (Card card in globalDeck)
            {
                _localDeck.Add(card);
            }
        }

        private void OnLevelVictory()
        {
            this.enabled = false;
        }

        internal Card DrawCard()
        {
            if (_localDeck.Count == 0 && _discardPile.Count != 0)
            {
                _localDeck.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }

            if (_localDeck.Count > 0)
            {
                Card card = _localDeck[0];
                _localDeck.RemoveAt(0);
                return card;
            }
            else { return null; }
        }

        internal void DiscardCard(Card card)
        {
            _discardPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        internal void ExhaustCard(Card card)
        {
            _exhaustPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        //Shuffles the deck using the Fisher-Yates shuffle algortihm
        internal void ShuffleDeck()
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
