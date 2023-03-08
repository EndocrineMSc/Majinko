using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Cards.DeckManagement.Global;
using PeggleWars.Cards.DeckManagement.HandHandling;

namespace PeggleWars.Cards.DeckManagement
{
    /// <summary>
    /// Handles local Deckmanagement in the scene. Cards are stored in distinct lists according to their state.
    /// Makes a new deck on level start, depending on the deck stored in the global Deck, which stores modifications to the player deck throughout a run.
    /// The Cards will never be destroyed, just switch between lists. Their instantiated objects that are visible on the screen
    /// will be handled and destroyed in the Hand class.
    /// DisplayDeck handles the visuals of necessary information for the player in the scene.
    /// </summary>
    [RequireComponent(typeof(DisplayDeck))]
    public class Deck : MonoBehaviour
    {
        #region Fields and Properties

        public static Deck Instance { get; private set; }

        private List<Card> _localDeck = new();
        [SerializeField] private List<Card> _discardPile = new();
        private List<Card> _exhaustPile = new();

        public List<Card> DiscardPile { get => _discardPile; set => _discardPile = value; }
        public List<Card> LocalDeck { get => _localDeck; set => _localDeck = value; }
        public List<Card> ExhaustPile { get => _exhaustPile; set => _exhaustPile = value; }

        private Hand _hand;

        #endregion

        #region Functions

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
            _hand = Hand.Instance;
            ShuffleDeck();
        }

        public Card DrawCard()
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
