using TMPro;
using UnityEngine;

namespace PeggleWars.Cards.DeckManagement
{
    /// <summary>
    /// This class displays necessary deck information on screen.
    /// </summary>
    public class DisplayDeck : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshProUGUI _remainingCardsInDeck;
        private TextMeshProUGUI _remainingCardsInDiscardPile;

        private Deck _deckManager;

        #endregion

        #region Private Functions

        private void Start()
        {
            _deckManager = Deck.Instance;
            _remainingCardsInDeck = GameObject.FindGameObjectWithTag("DeckNumber").GetComponent<TextMeshProUGUI>();
            _remainingCardsInDiscardPile = GameObject.FindGameObjectWithTag("DiscardNumber").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _remainingCardsInDeck.text = _deckManager.LocalDeck.Count.ToString();
            _remainingCardsInDiscardPile.text = _deckManager.DiscardPile.Count.ToString();
        }

        #endregion
    }
}
