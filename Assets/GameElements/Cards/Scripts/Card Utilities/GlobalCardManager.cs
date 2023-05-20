using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;

namespace Cards
{
    internal class GlobalCardManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GlobalCardManager Instance {  get; private set; }

        internal Dictionary<CardRarity, float> CardRarityThreshold { get; private set; } = new();
        internal List<Card> AllCards { get; private set; } = new();
        internal List<Card> CommonCards { get; private set; } = new();
        internal List<Card> RareCards { get; private set; } = new();
        internal List<Card> EpicCards { get; private set; } = new();
        internal List<Card> LegendaryCards { get; private set; } = new();

        [SerializeField] private AllCardsCollection _allCardsCollection;
       
        private const string EXODIA_CHECK = "Forbidden";

        private int _remainingAmountExodiaCards = 6;
        private bool _isFirstInit = true;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            
            if (_isFirstInit)
            {
                AllCards = _allCardsCollection.AllCards.ToList();
                AllCards = AllCards.OrderBy(card => card.ScriptableCard.CardName).ToList();
                SetRarityThresholds();
                BuildRarityLists();
                _isFirstInit = false;
            }
        }

        private void SetRarityThresholds()
        {
            CardRarityThreshold.Add(CardRarity.Common, 0);
            CardRarityThreshold.Add(CardRarity.Rare, 50);
            CardRarityThreshold.Add(CardRarity.Epic, 80);
            CardRarityThreshold.Add(CardRarity.Legendary, 95);
        }

        private void BuildRarityLists()
        {
            foreach (Card card in AllCards)
            {
                ScriptableCard scriptCard = card.ScriptableCard;
                switch (scriptCard.Rarity)
                {
                    case CardRarity.Common:
                        CommonCards.Add(card);
                        break;
                    case CardRarity.Rare:
                        RareCards.Add(card);
                        break;
                    case CardRarity.Epic:
                        EpicCards.Add(card);
                        break;
                    case CardRarity.Legendary:
                        LegendaryCards.Add(card);
                        break;
                    default:
                        break;
                }
            }
        }

        internal void BoughtExodiaCard(Card exodiaCard)
        {
            _remainingAmountExodiaCards--;
            CardType cardType = exodiaCard.CardType;
            RemoveExodiaCardsFromShop(cardType);
            SortExodiaCardsIntoNewShopList();
        }

        private void RemoveExodiaCardsFromShop(CardType cardType)
        {           
            foreach (Card card in LegendaryCards)
            {
                if (cardType == card.CardType)
                {
                    LegendaryCards.Remove(card);
                    break;
                }
            }
            foreach (Card card in EpicCards)
            {
                if (cardType == card.CardType)
                {
                    EpicCards.Remove(card);
                    break;
                }
            }
            foreach (Card card in RareCards)
            {
                if (cardType == card.CardType)
                {
                    RareCards.Remove(card);
                    break;
                }
            }
            foreach (Card card in CommonCards)
            {
                if (cardType == card.CardType)
                {
                    CommonCards.Remove(card);
                    break;
                }
            }
        }

        private void SortExodiaCardsIntoNewShopList()
        {
            if (_remainingAmountExodiaCards == 1)
            {
                List<Card> newRareCards = new();
                foreach (Card card in RareCards)
                {
                    if (card.name.Contains(EXODIA_CHECK))
                    {
                        CommonCards.Add(AllCards[(int)card.CardType]);
                    }
                    else
                    {
                        newRareCards.Add(card);
                    }
                    RareCards = newRareCards;
                }
            }
            else if (_remainingAmountExodiaCards <= 3)
            {
                List<Card> newEpicCards = new();
                foreach (Card card in EpicCards)
                {
                    if (card.name.Contains(EXODIA_CHECK))
                    {
                        RareCards.Add(AllCards[(int)card.CardType]);
                    }
                    else
                    {
                        newEpicCards.Add(card);
                    }
                }
                EpicCards = newEpicCards;
            }
            else if (_remainingAmountExodiaCards <= 5)
            {
                List<Card> newLegendaryList = new();
                foreach (Card card in LegendaryCards)
                {
                    Debug.Log(card.name);
                    if (card.name.Contains(EXODIA_CHECK))
                    {
                        EpicCards.Add(AllCards[(int)card.CardType]);
                    }
                    else
                    {
                        newLegendaryList.Add(card);
                    }
                }
                LegendaryCards = newLegendaryList;
            }
        }

        #endregion
    }
}
