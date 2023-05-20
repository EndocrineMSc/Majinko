using TMPro;
using UnityEngine;

namespace Cards
{
    internal class DisplayDeck : MonoBehaviour
    {
        #region Fields and Properties

        private TextMeshProUGUI _remainingCardsInDeck;
        private TextMeshProUGUI _remainingCardsInDiscardPile;
        private TextMeshProUGUI _remainingCardsInExhaustPile;

        private const string DECKNUMBER_TAG = "DeckNumber";
        private const string DISCARDNUMBER_TAG = "DiscardNumber";
        private const string EXHAUSTNUMBER_TAG = "ExhaustNumber";

        private Deck _deckManager;

        #endregion

        #region Functions

        private void Start()
        {
            _deckManager = Deck.Instance;
            _remainingCardsInDeck = GameObject.FindGameObjectWithTag(DECKNUMBER_TAG).GetComponent<TextMeshProUGUI>();
            _remainingCardsInDiscardPile = GameObject.FindGameObjectWithTag(DISCARDNUMBER_TAG).GetComponent<TextMeshProUGUI>();
            _remainingCardsInExhaustPile = GameObject.FindGameObjectWithTag(EXHAUSTNUMBER_TAG).GetComponent <TextMeshProUGUI>();
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
