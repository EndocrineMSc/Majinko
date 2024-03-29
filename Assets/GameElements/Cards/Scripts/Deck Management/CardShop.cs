using Cards;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollection;
using Audio;
using Unity.VisualScripting;
using DG.Tweening;
using System.Linq;
using System.Collections;

namespace Utility
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

        internal int AmountCardsToChooseFrom { get; private set; } = 3;

        #endregion

        #region Functions

        private void OnEnable()
        {
            UtilityEvents.OnLevelVictory += OnLevelVictory;
        }

        private void OnDisable()
        {
            UtilityEvents.OnLevelVictory -= OnLevelVictory;
        }

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
            StartCoroutine(GameManager.Instance.SwitchState(GameState.LevelWon));
            StartCoroutine(OpenShopWithDelay());
        }

        internal void PresentCardChoiceByElement(CardElement element)
        {
            _shopCanvas.enabled = true;
            List<Card> shopCards = SetRandomShopCardsByElement(element, AmountCardsToChooseFrom);
            List<Card> cardObjects = InstantiateShopCards(shopCards);
            ManageCardComponents(cardObjects);
            BuildBuyButtons(cardObjects);           
        }

        private IEnumerator OpenShopWithDelay()
        {
            yield return new WaitForSeconds(1);
            _shopCanvas.enabled = true;
            List<Card> shopCards = SetRandomShopCards(AmountCardsToChooseFrom);
            List<Card> cardObjects = InstantiateShopCards(shopCards);
            ManageCardComponents(cardObjects);
            BuildBuyButtons(cardObjects);
        }

        private List<Card> SetRandomShopCards(int amountCardChoices = 3)
        {
            List<Card> shopCardList = new();
            var retries = 0;

            while (shopCardList.Count < amountCardChoices)
            {
                if (retries > 50)
                {
                    Debug.Log("Shop Building Failed");
                    break;
                }

                List<Card> rarityList = DetermineCardRarityList();
                if (rarityList.Count == 0)
                {
                    retries++;
                    continue;
                }
                else
                {
                    Card card = GetRandomCardFromList(rarityList);

                    if (shopCardList.Contains(card))
                    {
                        retries++;
                        continue;
                    }
                    shopCardList.Add(card);
                }               
            }

            if (shopCardList.Count > AmountCardsToChooseFrom)
                shopCardList.RemoveAt((AmountCardsToChooseFrom - 1));
                        
            return shopCardList;
        }

        private List<Card> SetRandomShopCardsByElement(CardElement element, int amountCardChoices = 3)
        {
            List<Card> shopCardList = new();
            var retries = 0;

            while (shopCardList.Count < amountCardChoices)
            {
                if (retries > 150)
                {
                    Debug.Log("Shop Building Failed");
                    break;
                }

                List<Card> rarityList = DetermineCardRarityList();
                List<Card> cardsWithElement = new();

                foreach (Card card in rarityList)
                {
                    if (card.Element == element)
                        cardsWithElement.Add(card);
                }

                if (cardsWithElement.Count == 0)
                {
                    retries++;
                    continue;
                }
                else
                {
                    Card card = GetRandomCardFromList(cardsWithElement);

                    if (shopCardList.Contains(card))
                    {
                        retries++;
                        continue;
                    }
                    shopCardList.Add(card);
                }
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

        private void ManageCardComponents(List<Card> cardObjects)
        {
            foreach (Card cardObject in cardObjects)
            {
                cardObject.IsBeingDealt = false;
                cardObject.GetComponent<CardMovement>().enabled = false;
                cardObject.AddComponent<ShopMouseOverSound>();
            }
        }

        private List<Card> InstantiateShopCards(List<Card> cards)
        {
            List<Card> instantiatedCards = new();

            for (int i = 0; i < cards.Count; i++)
            {
                Vector2 instantiatePosition = new(-350 + (i * 350), 50);
                Card cardObject = Instantiate(cards[i], _shopCanvas.transform);
                cardObject.GetComponent<RectTransform>().anchoredPosition = instantiatePosition;
                cardObject.transform.DOScale(new Vector3(1.25f, 1.25f, 1), 0.1f);
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
            float ySpawnPosition = gameObject.transform.position.y - (rectTransform.rect.height * 0.85f);
            float xSpawnPosition = gameObject.transform.position.x;
            Card buyableCard = gameObject.GetComponent<Card>();
            int cardIndex = (int)buyableCard.ScriptableCard.Type;

            Vector2 instantiatePosition = new(xSpawnPosition, ySpawnPosition);
            
            Button buyButton = Instantiate (_buyButtonPrefab, instantiatePosition, Quaternion.identity);

            buyButton.transform.SetParent(_shopCanvas.transform, true);
            buyButton.onClick.AddListener(delegate { BuyCard(cardIndex); });
            buyButton.onClick.AddListener(delegate { ChangeGameState(); });
        }

        private void BuyCard(int cardIndex)
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0007_ShopCard_Picked);
            Card cardToBeAddedToDeck = GlobalCardManager.Instance.AllCards[cardIndex];

            if (cardToBeAddedToDeck.name.Contains("Forbidden"))
            {
                GlobalCardManager.Instance.BoughtExodiaCard(cardToBeAddedToDeck);
            }
            _globalDeckManager.GlobalDeck.Add(cardToBeAddedToDeck);
            StartCoroutine(_gameManager.SwitchState(GameState.NewLevel));
        }

        private void ChangeGameState()
        {
            StartCoroutine(GameManager.Instance.SwitchState(GameState.NewLevel));
        }

        public void SkipCardButton()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
            ChangeGameState();
        }

        #endregion
    }
}
