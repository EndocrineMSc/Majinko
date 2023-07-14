using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Characters;
using Cards;
using EnumCollection;

namespace Overworld
{
    internal class ForestFireMapEvent : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private Button _extractFireButton;
        [SerializeField] private Button _freezeFireButton;
        [SerializeField] private Button _burnCardButton;
        [SerializeField] private CardShop _cardShop;
        [SerializeField] private DeckLibrary _deckLibrary;

        #endregion

        #region Functions

        private void Start()
        {
            _extractFireButton.onClick.AddListener(ExtractFire);
            _freezeFireButton.onClick.AddListener(FreezeFire);
            _burnCardButton.onClick.AddListener(BurnCard);

            CheckForFrozenCardAvailability();

            StartCoroutine(GameManager.Instance.SwitchState(GameState.LevelWon));
        }

        private void CheckForFrozenCardAvailability()
        {
            var hasFrozenCards = false;
            foreach (Card card in GlobalDeckManager.Instance.GlobalDeck)
            {
                if (card.Element == CardElement.Ice)
                {
                    hasFrozenCards = true;
                    break;
                }
            }

            if (!hasFrozenCards)
                _freezeFireButton.interactable = false;
        }

        private void ExtractFire()
        {
            Player.Instance.TakeDamage(5);
            _cardShop.PresentCardChoiceByElement(EnumCollection.CardElement.Fire);
        }

        private void FreezeFire()
        {
            _deckLibrary.SetUpElementDeckLibrary("Choose card to sacrifice", CardElement.Ice, DeckLibraryAction.RemoveCard);
        }

        private void BurnCard()
        {
            Player.Instance.TakeDamage(10);
            _deckLibrary.SetUpCurrentDeckLibrary("Choose card to burn", DeckLibraryAction.RemoveCard);
        }

        #endregion

    }
}
