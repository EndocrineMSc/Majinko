using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EnumCollection;
using Unity.VisualScripting;
using Audio;
using Utility;
using DG.Tweening;
using Overworld;

namespace Cards
{
    internal class DeckLibrary : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private TextMeshProUGUI _playerActionOrders;
        [SerializeField] private GridLayoutGroup _cardGrid;
        [SerializeField] private Canvas _libraryCanvas;

        private Vector3 _cardScale = new(1.25f, 1.25f, 1);

        #endregion

        #region Functions

        internal void SetUpCurrentDeckLibrary(string playerActionOrders, DeckLibraryAction action)
        {
            _libraryCanvas.enabled = true;
            _playerActionOrders.text = playerActionOrders;

            if (GlobalDeckManager.Instance != null && GlobalCardManager.Instance != null)
            {
                var cardsToDisplay = new List<Card>();
                foreach (Card card in GlobalDeckManager.Instance.GlobalDeck)
                    cardsToDisplay.Add(card);
                
                var displayedCards = DisplayLibraryCards(cardsToDisplay);
                ManageCardComponents(displayedCards);
                AddCardAction(displayedCards, action);
            }
        }

        internal void SetUpElementDeckLibrary(string playerActionOrders, CardElement element, DeckLibraryAction action)
        {
            _libraryCanvas.enabled = true;
            _playerActionOrders.text = playerActionOrders;

            if (GlobalDeckManager.Instance != null && GlobalCardManager.Instance != null)
            {
                var cardsToDisplay = new List<Card>();
                foreach (Card card in  GlobalDeckManager.Instance.GlobalDeck)
                {
                    if (card.Element == element)
                        cardsToDisplay.Add(card);
                }

                var displayedCards = DisplayLibraryCards(cardsToDisplay);
                ManageCardComponents(displayedCards);
                AddCardAction(displayedCards, action);
            }
        }

        private void ManageCardComponents(List<GameObject> displayedCards)
        {
            foreach (var cardObject in  displayedCards)
            {
                var card = cardObject.GetComponent<Card>();
                card.GetComponent<CardDragDrop>().enabled = false;
                card.GetComponent<CardZoom>().enabled = false;
                card.GetComponent<CardZoomMovement>().enabled = false;
                cardObject.AddComponent<DeckLibraryCardHighlight>();
            }
        }

        private List<GameObject> DisplayLibraryCards(List<Card> cardsToDisplay)
        {
            var displayedCards = new List<GameObject>();
            if (cardsToDisplay.Count > 0)
            {
                foreach (Card card in cardsToDisplay)
                {
                    var currentCard = Instantiate(card, _cardGrid.transform.position, Quaternion.identity).gameObject;
                    currentCard.GetComponent<RectTransform>().localScale = _cardScale;
                    currentCard.transform.SetParent(_cardGrid.transform);
                    displayedCards.Add(currentCard);
                }
            }
            return displayedCards;
        }

        private void AddCardAction(List<GameObject> displayedCards, DeckLibraryAction action)
        {
            foreach (var cardObject in displayedCards)
            {                 
                Button button = cardObject.GetComponent<Button>();
                button.interactable = true;
                button.onClick.AddListener(delegate { ButtonFeedback(button); });
                button.onClick.AddListener(delegate { RemoveAllButtonFunctions(displayedCards); });
                button.onClick.AddListener(LoadNextSceneWrap);

                switch (action)
                {
                    case DeckLibraryAction.AddCard:
                        button.onClick.AddListener(delegate { AddCard(button); });
                        break;

                    case DeckLibraryAction.RemoveCard:
                        button.onClick.AddListener(delegate { RemoveCard(button); });
                        break;

                    case DeckLibraryAction.UpgradeCard:
                        button.onClick.AddListener(delegate { UpgradeCard(button); });
                        break;

                    case DeckLibraryAction.ExchangeCard:
                        button.onClick.AddListener(delegate { ExchangeCard(button); });
                        break;
                }
            }          
        }

        private void AddCard(Button button)
        {
            var cardType = button.GetComponent<Card>().CardType;
            var card = GlobalCardManager.Instance.AllCards[(int)cardType];
            GlobalDeckManager.Instance.GlobalDeck.Add(card);
        }

        private void RemoveCard(Button button)
        {
            var cardToDelete = button.GetComponent<Card>();
            RemoveCardFromGlobalDeck(cardToDelete);
        }

        private void UpgradeCard(Button button)
        {
            //Implementation tbd
        }

        private void ExchangeCard(Button button)
        {
            var card = button.GetComponent<Card>();
            ExchangeRandomCard(button, card);
        }

        private void ExchangeRandomCard(Button button, Card cardToRemove)
        {
            RemoveCardFromGlobalDeck(cardToRemove);
            var tries = 0;
            while (true)
            {
                tries++;
                int randomIndex = UnityEngine.Random.Range(0, GlobalCardManager.Instance.AllCards.Count);
                Card cardCandidate = GlobalCardManager.Instance.AllCards[randomIndex];

                if (cardCandidate.CardType != cardToRemove.CardType)
                {
                    GlobalDeckManager.Instance.GlobalDeck.Add(cardCandidate);
                    ShowExchangedCard(button, cardCandidate);
                    break;
                }

                if (tries > 50)
                    break;
            }
        }

        private void ButtonFeedback(Button button)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.DOPunchScale(rectTransform.localScale * 0.2f, 0.1f, 0, 0.1f);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
            }
        }
  
        private void RemoveAllButtonFunctions(List<GameObject> displayedCards)
        {
            StartCoroutine(RemovalTimer(displayedCards));
        }
        
        private IEnumerator RemovalTimer(List<GameObject> displayedCards)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var card in displayedCards)
            {
                card.GetComponent<Button>().onClick.RemoveAllListeners();
                card.GetComponent<Button>().interactable = false;
            }
        }

        private void LoadNextSceneWrap()
        {
            StartCoroutine(LoadNextScene());
        }

        private void RemoveCardFromGlobalDeck(Card cardToDelete)
        {
            foreach (var card in GlobalDeckManager.Instance.GlobalDeck)
            {
                if (cardToDelete.CardType == card.CardType)
                {
                    GlobalDeckManager.Instance.GlobalDeck.Remove(card);
                    break;
                }
            }
        }

        private IEnumerator LoadNextScene()
        {
            if (FadeCanvas.Instance != null)
                FadeCanvas.Instance.FadeToBlack();

            yield return new WaitForSeconds(LoadHelper.LoadDuration);
            LoadHelper.LoadSceneWithLoadingScreen(SceneName.WorldOne);
        }

        private void ShowExchangedCard(Button button, Card card)
        {
            GameObject cardObject = Instantiate(card, button.transform.position, Quaternion.identity).gameObject;
            cardObject.transform.SetParent(_libraryCanvas.transform);
            cardObject.transform.DOJump(transform.position + Vector3.one, 1, 1, 0.5f);
            cardObject.GetComponent<RectTransform>().localScale = _cardScale;
            cardObject.GetComponent<Image>().raycastTarget = false;
            StartCoroutine(DestroyObjectWithDelay(cardObject));
        }

        private IEnumerator DestroyObjectWithDelay(GameObject gobject)
        {
            yield return new WaitForSeconds(2f);
            Destroy(gobject);
        }

        #endregion
    }

    internal enum DeckLibraryAction
    {
        AddCard,
        ExchangeCard,
        RemoveCard,
        UpgradeCard,
    }
}
