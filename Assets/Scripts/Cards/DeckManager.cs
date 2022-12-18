using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using TMPro;
using Cards.DeckManagement.Global;

namespace Cards.DeckManagement
{
    public class DeckManager : MonoBehaviour
    {
        #region Fields

        public static DeckManager Instance { get; private set; }

        private List<Card> _localDeck = new();
        private List<Card> _discardPile = new();
        private List<Card> _abolishedPile = new();

        public StartDeck StartDeck { get; private set; }

        private List<Card> _handCards = new();
        public List<Card> HandCards { get => _handCards; }

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
        }

        #endregion


        #region Private Functions

        public void DrawCards(int amount = 5)
        {
            for (int i = 0; i < amount; i++)
            {
                int randomIndex = Random.Range(0, _localDeck.Count);
                Card randomCard = _localDeck[randomIndex];
                _localDeck.Remove(randomCard);
                _handCards.Add(randomCard);
            }
        }

        public void EndTurnDiscard()
        {
            foreach (Card card in _handCards)
            {
                _handCards.Remove(card);
                _discardPile.Add(card);
            }
        }

        #endregion
    }
}
