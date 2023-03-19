using PeggleWars.Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollection;
using System.Collections;

namespace PeggleWars.Utilities
{
    internal class CardShop : MonoBehaviour
    {
        #region Fields

        private Canvas _shopCanvas;
        private GlobalDeckManager _globalDeckManager;
        private GameManager _gameManager;
        private HorizontalLayoutGroup _shopCardLayout;
        [SerializeField] private Button _buyButtonPrefab;

        private float _rareThreshold;
        private float _epicThreshold;
        private float _legendaryThreshold;

        private bool _isFirstUpdate = true;

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

        private void Update()
        {
            if (_isFirstUpdate)
            {
                _isFirstUpdate = false;
                _rareThreshold = GlobalCardManager.Instance.CardRarityThreshold.GetValueOrDefault(CardRarity.Rare);
                _epicThreshold = GlobalCardManager.Instance.CardRarityThreshold.GetValueOrDefault(CardRarity.Epic);
                _legendaryThreshold = GlobalCardManager.Instance.CardRarityThreshold.GetValueOrDefault(CardRarity.Legendary);
            }
        }

        private void OnLevelVictory()
        {
            _shopCanvas.enabled = true;
            List<Card> shopCards = SetRandomShopCards();
            List<Card> cardObjects = InstantiateShopCards(shopCards);
            DisableCardComponents(cardObjects);
            BuildBuyButtons(cardObjects);
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        private List<Card> SetRandomShopCards(int amountCardChoices = 3)
        {
            List<Card> shopCardList = new();
            int cardPoolSize = GlobalCardManager.Instance.AllCards.Count;

            var retries = 0;

            while (shopCardList.Count < amountCardChoices)
            {
                if (retries > 50)
                    break;

                List<Card> rarityList = DetermineCardRarityList();
                Card card = GetRandomCardFromList(rarityList);
                
                if (shopCardList.Contains(card))
                {
                    retries++;
                    continue;
                }

                shopCardList.Add(card);
            }
    
            return shopCardList;
        }

        private List<Card> DetermineCardRarityList()
        {
            int randomRarityThreshold = UnityEngine.Random.Range(0, 101);

            if (randomRarityThreshold > _legendaryThreshold && GlobalCardManager.Instance.LegendaryCards.Count > 0)
            {
                return GlobalCardManager.Instance.LegendaryCards;
            }
            else if(randomRarityThreshold > _epicThreshold && GlobalCardManager.Instance.EpicCards.Count > 0)
            {
                return GlobalCardManager.Instance.EpicCards;
            }
            else if(randomRarityThreshold > _rareThreshold && GlobalCardManager.Instance.RareCards.Count > 0)
            {
                return GlobalCardManager.Instance.RareCards;
            }
            else
            {
                return GlobalCardManager.Instance.CommonCards;
            }
        }

        private Card GetRandomCardFromList(List<Card> cardList) 
        {
            int randomCardIndex = UnityEngine.Random.Range(0, cardList.Count);
            Card randomCard = cardList[randomCardIndex];
            return randomCard;
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
            Card cardToBeAddedToDeck = GlobalCardManager.Instance.AllCards[cardIndex];

            if (cardToBeAddedToDeck.name.Contains("Forbidden"))
            {
                Debug.Log(cardToBeAddedToDeck.name);
                GlobalCardManager.Instance.BoughtExodiaCard(cardToBeAddedToDeck);
            }
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
