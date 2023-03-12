using TMPro;
using UnityEngine;

namespace PeggleWars.Cards
{
    internal class DisplayDeck : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshProUGUI _remainingCardsInDeck;
        private TextMeshProUGUI _remainingCardsInDiscardPile;
        private TextMeshProUGUI _remainingCardsInExhaustPile;

        private Deck _deckManager;

        #endregion

        #region Private Functions

        private void Start()
        {
            _deckManager = Deck.Instance;
            _remainingCardsInDeck = GameObject.FindGameObjectWithTag("DeckNumber").GetComponent<TextMeshProUGUI>();
            _remainingCardsInDiscardPile = GameObject.FindGameObjectWithTag("DiscardNumber").GetComponent<TextMeshProUGUI>();
            _remainingCardsInExhaustPile = GameObject.FindGameObjectWithTag("ExhaustNumber").GetComponent <TextMeshProUGUI>();
        }

        private void Update()
        {
            _remainingCardsInDeck.text = _deckManager.LocalDeck.Count.ToString();
            _remainingCardsInDiscardPile.text = _deckManager.DiscardPile.Count.ToString();
            _remainingCardsInExhaustPile.text = _deckManager.ExhaustPile.Count.ToString();
        }

        #endregion
    }
}
