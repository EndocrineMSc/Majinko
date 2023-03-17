using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Cards;
using EnumCollection;
using System.Linq;
using System;

namespace PeggleWars.Cards
{
    internal class GlobalCardManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GlobalCardManager Instance {  get; private set; }

        internal Dictionary<CardRarity, float> CardRarityThreshold { get; private set; } = new();

        [SerializeField] private List<Card> _allCards; //List of all Cards, built from Resources Folder
        internal List<Card> AllCards { get { return _allCards; } }
        internal List<Card> CommonCards { get; private set; } = new();
        internal List<Card> RareCards { get; private set; } = new();
        internal List<Card> EpicCards { get; private set; } = new();
        internal List<Card> LegendaryCards { get; private set; } = new();
       
        private string RESOURCE_LOAD_PARAM = "CardPrefabVariants";

        private IList<string> _names;
        private int _remainingAmountExodiaCards;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _allCards = Resources.LoadAll<Card>(RESOURCE_LOAD_PARAM).OrderBy(x => x.CardName).ToList();
        }

        private void Start()
        {
            SetRarityThresholds();
            BuildRarityLists();

            if(GlobalDeckManager.Instance.GlobalDeck.Count > 0)
            {

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
            foreach (Card card in _allCards)
            {
                switch (card.Rarity)
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

        internal Card GetCardByType(CardType type)
        {
            _names ??= Enum.GetNames(typeof(CardType)).OrderBy(x => x).ToList();

            return _allCards[_names.IndexOf(type.ToString())];
        }

        internal void BoughtExodiaCard(Card exodiaCard)
        {
            _remainingAmountExodiaCards--;
            CardType cardType = exodiaCard.CardType;
            RemoveExodiaCardFromList(LegendaryCards, cardType);
            RemoveExodiaCardFromList(EpicCards, cardType);
            RemoveExodiaCardFromList(RareCards, cardType);
            RemoveExodiaCardFromList(CommonCards, cardType);
            SortExodiaCardsIntoNewShopList();
        }

        private void RemoveExodiaCardFromList(List<Card> cards, CardType cardType)
        {
            foreach (Card card in cards)
            {
                if (cardType == card.CardType)
                {
                    cards.Remove(card);
                }
            }
        }

        private void SortExodiaCardsIntoNewShopList()
        {
            if (_remainingAmountExodiaCards == 1)
            {
                foreach (Card card in RareCards)
                {
                    if (card.TryGetComponent<IAmExodia>(out _))
                    {
                        CommonCards.Add(card);
                        RareCards.Remove(card);
                    }
                }
            }
            else if (_remainingAmountExodiaCards <= 3)
            {
                foreach (Card card in EpicCards)
                {
                    if (card.TryGetComponent<IAmExodia>(out _))
                    {
                        RareCards.Add(card);
                        EpicCards.Remove(card);
                    }
                }
            }
            else if (_remainingAmountExodiaCards <= 5)
            {
                foreach (Card card in LegendaryCards)
                {
                    if (card.TryGetComponent<IAmExodia>(out _))
                    {
                        EpicCards.Add(card);
                        LegendaryCards.Remove(card);
                    }
                }
            }
        }

        #endregion
    }
}
