using PeggleWars;
using PeggleWars.Cards;
using PeggleWars.Cards.DeckManagement.Global;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumCollection;

namespace Utilities.Progression
{
    public class CardShop : MonoBehaviour
    {
        #region Fields

        private Canvas _shopCanvas;
        private GlobalDeckManager _globalDeckManager;
        private GameManager _gameManager;
        private HorizontalLayoutGroup _shopCardLayout;
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
            _gameManager.LevelVictory.AddListener(OnLevelVictory);
            _shopCanvas.enabled = false;
            _globalDeckManager = GlobalDeckManager.Instance;
            _shopCardLayout = _shopCanvas.GetComponent<HorizontalLayoutGroup>();
        }

        private void OnDisable()
        {
            _gameManager.LevelVictory.RemoveListener(OnLevelVictory);
        }

        private void OnLevelVictory()
        {
            _shopCanvas.enabled = true;
            List<Card> shopCards = SetRandomShopCards();
            List<GameObject> cardObjects = InstantiateShopCards(shopCards);
            DisableCardComponents(cardObjects);
            BuildBuyButtons(cardObjects);
        }

        private List<Card> SetRandomShopCards(int amountCardChoices = 3)
        {
            List<Card> tempCardList = new();
            int cardPoolSize = _globalDeckManager.AllCards.Count;

            for (int i = 0; i < amountCardChoices; i++)
            {
                int randomCardIndex = Random.Range(0, cardPoolSize);
                
                if (tempCardList == null)
                {
                    tempCardList.Add(_globalDeckManager.AllCards[randomCardIndex]);
                }
                else
                {
                    foreach (Card card in tempCardList)
                    {
                        int cardIndex = _globalDeckManager.AllCards.IndexOf(card);
                        
                        if (cardIndex == randomCardIndex)
                        {
                            i--;
                            break;
                        }
                    }
                    tempCardList.Add(_globalDeckManager.AllCards[randomCardIndex]);
                }
            }
            return tempCardList;
        }

        private void DisableCardComponents(List<GameObject> cardObjects)
        {
            foreach (GameObject cardObject in cardObjects)
            {
                Component[] cardComponents = cardObject.GetComponents(typeof(MonoBehaviour));

                foreach (Component component in cardComponents)
                {
                    if (!component.GetType().Equals(typeof(SpriteRenderer)))
                    {
                        Destroy(component);
                    }
                }
            }
        }

        private List<GameObject> InstantiateShopCards(List<Card> cards)
        {
            List<GameObject> instantiatedCards = new List<GameObject>();

            foreach (Card card in cards)
            {
                GameObject cardObject = Instantiate(card.CardPrefab, Vector2.zero, Quaternion.identity);
                cardObject.transform.parent = _shopCardLayout.transform;              
                instantiatedCards.Add(cardObject);
            }

            return instantiatedCards;
        }

        private void BuildBuyButtons(List<GameObject> cardObjects)
        {
            foreach (GameObject gameObject in cardObjects)
            {
                BuildBuyButtonUnderObject(gameObject);
            }
        }

        private void BuildBuyButtonUnderObject (GameObject gameObject)
        {
            RectTransform rectTransform = (RectTransform)gameObject.transform;
            float ySpawnPosition = gameObject.transform.position.y - (rectTransform.rect.height * 0.75f);
            float xSpawnPosition = gameObject.transform.position.x;
            Card buyableCard = gameObject.GetComponent<Card>();
            int cardIndex = _globalDeckManager.AllCards.IndexOf(buyableCard);

            Vector2 instantiatePosition = new(xSpawnPosition, ySpawnPosition);
            
            Button buyButton = Instantiate (_buyButtonPrefab, instantiatePosition, Quaternion.identity);

            buyButton.transform.SetParent(gameObject.transform, true);
            buyButton.onClick.AddListener(delegate { BuyCard(cardIndex); });
        }

        private void BuyCard(int cardIndex)
        {
            Card cardToBeAddedToDeck = _globalDeckManager.AllCards[cardIndex];
            _globalDeckManager.GlobalDeck.Add(cardToBeAddedToDeck);
            StartCoroutine(_gameManager.SwitchState(GameState.NewLevel));
        }

        #endregion
    }
}
