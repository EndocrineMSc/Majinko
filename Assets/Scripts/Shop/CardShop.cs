using PeggleWars.Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollection;

namespace PeggleWars.Utilities
{
    internal class CardShop : MonoBehaviour
    {
        #region Fields

        private Canvas _shopCanvas;
        private GlobalDeckManager _globalDeckManager;
        private GameManager _gameManager;
        private HorizontalLayoutGroup _shopCardLayout;
        private bool _levelIsWon;
        [SerializeField] private Button _buyButtonPrefab;

        #endregion

        #region Functions

        private void Start()
        {
              SetReferences();
        }

        private void SetReferences()
        {
            _shopCanvas = GetComponent<Canvas>();
            _gameManager = GameManager.Instance;
            _shopCanvas.enabled = false;
            _globalDeckManager = GlobalDeckManager.Instance;
            _shopCardLayout = _shopCanvas.GetComponentInChildren<HorizontalLayoutGroup>();
            
            WinLoseConditionManager.Instance.LevelVictory?.AddListener(OnLevelVictory);
        }

        private void OnLevelVictory()
        {
            if (!_levelIsWon)
            {
                _shopCanvas.enabled = true;
                List<Card> shopCards = SetRandomShopCards();
                List<Card> cardObjects = InstantiateShopCards(shopCards);
                DisableCardComponents(cardObjects);
                BuildBuyButtons(cardObjects);
            }
            _levelIsWon = true;
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        private List<Card> SetRandomShopCards(int amountCardChoices = 3)
        {
            List<Card> tempCardList = new();
            int cardPoolSize = _globalDeckManager.AllCards.Count;

            var retries = 0;

            while (tempCardList.Count < amountCardChoices)
            {
                int randomCardIndex = UnityEngine.Random.Range(0, cardPoolSize);

                if (retries > 50)
                    break;

                if (tempCardList.Contains(_globalDeckManager.AllCards[randomCardIndex]))
                {
                    retries++;
                    continue;
                }

                tempCardList.Add(_globalDeckManager.AllCards[randomCardIndex]);
            }
    
            return tempCardList;
        }

        private void DisableCardComponents(List<Card> cardObjects)
        {
            foreach (Card cardObject in cardObjects)
            {
                cardObject.GetComponent<CardDragDrop>().enabled = false;
                cardObject.GetComponent<CardZoom>().enabled = false;
            }
        }

        private List<Card> InstantiateShopCards(List<Card> cards)
        {
            List<Card> instantiatedCards = new();


            foreach (Card card in cards)
            {
                Card cardObject = Instantiate(card, Vector2.zero, Quaternion.identity);
                cardObject.GetComponent<RectTransform>().SetParent(_shopCardLayout.transform, false);             
                instantiatedCards.Add(cardObject);
            }

            return instantiatedCards;
        }

        private void BuildBuyButtons(List<Card> cardObjects)
        {
            foreach (Card cardObject in cardObjects)
            {
                BuildBuyButtonUnderObject(cardObject.gameObject);
            }
        }

        private void BuildBuyButtonUnderObject (GameObject gameObject)
        {
            RectTransform rectTransform = (RectTransform)gameObject.transform;
            float ySpawnPosition = gameObject.transform.position.y - (rectTransform.rect.height * 0.75f);
            float xSpawnPosition = gameObject.transform.position.x;
            Card buyableCard = gameObject.GetComponent<Card>();
            int cardIndex = (int)buyableCard.CardType;

            Vector2 instantiatePosition = new(xSpawnPosition, ySpawnPosition);
            
            Button buyButton = Instantiate (_buyButtonPrefab, instantiatePosition, Quaternion.identity);

            buyButton.transform.SetParent(gameObject.transform, true);
            buyButton.onClick.AddListener(delegate { BuyCard(cardIndex); });
            buyButton.onClick.AddListener(delegate { ChangeGameState(); });
        }

        private void BuyCard(int cardIndex)
        {
            Card cardToBeAddedToDeck = _globalDeckManager.AllCards[cardIndex];
            _globalDeckManager.GlobalDeck.Add(cardToBeAddedToDeck);
            StartCoroutine(_gameManager.SwitchState(GameState.NewLevel));
        }

        private void ChangeGameState()
        {
            StartCoroutine(GameManager.Instance.SwitchState(GameState.NewLevel));
        }

        #endregion
    }
}
