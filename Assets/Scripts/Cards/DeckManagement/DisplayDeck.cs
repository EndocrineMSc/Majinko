using TMPro;
using UnityEngine;

namespace Cards.DeckManagement
{
    public class DisplayDeck : MonoBehaviour
    {
        #region Fields

        private TextMeshProUGUI _remainingCardsInDeck;
        private TextMeshProUGUI _remainingCardsInDiscardPile;

        private DeckManager _deckManager;

        #endregion

        #region Private Functions

        private void Start()
        {
            _deckManager = DeckManager.Instance;
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
