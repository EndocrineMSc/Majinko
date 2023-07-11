using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;
using Utility;

namespace Cards
{
    internal class GlobalCardManager : MonoBehaviour, IResetOnQuit
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

        private readonly string COMMON_CARDS_PATH = "CommonCards";
        private readonly string RARE_CARDS_PATH = "RareCards";
        private readonly string EPIC_CARDS_PATH = "EpicCards";
        private readonly string LEGENDARY_CARDS_PATH = "LegendaryCards";
        private readonly string CARDRARITY_PATH = "RarityDictionary";

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
                InitializeManager();
        }

        private void OnEnable()
        {
            UtilityEvents.OnGameReset += OnGameReset;
        }

        private void OnDisable()
        {
            UtilityEvents.OnGameReset -= OnGameReset;
        }

        private void Start()
        {
            if (ES3.KeyExists(CARDRARITY_PATH))
                GlobalCardManager.Instance.CardRarityThreshold = ES3.Load<Dictionary<CardRarity, float>>(CARDRARITY_PATH);

            if (ES3.KeyExists(COMMON_CARDS_PATH))
            {
                GlobalCardManager.Instance.CommonCards = ES3.Load<List<Card>>(COMMON_CARDS_PATH);
                GlobalCardManager.Instance.RareCards = ES3.Load<List<Card>>(RARE_CARDS_PATH);
                GlobalCardManager.Instance.EpicCards = ES3.Load<List<Card>>(EPIC_CARDS_PATH);
                GlobalCardManager.Instance.LegendaryCards = ES3.Load<List<Card>>(LEGENDARY_CARDS_PATH);
            }
        }

        private void OnApplicationQuit()
        {
            ES3.Save(CARDRARITY_PATH, CardRarityThreshold);
            ES3.Save(COMMON_CARDS_PATH, CommonCards);
            ES3.Save(RARE_CARDS_PATH, RareCards);
            ES3.Save(EPIC_CARDS_PATH, EpicCards);
            ES3.Save(LEGENDARY_CARDS_PATH, LegendaryCards);
        }

        private void InitializeManager()
        {
            AllCards = _allCardsCollection.AllCards.ToList();
            AllCards = AllCards.OrderBy(card => card.ScriptableCard.CardName).ToList();
            
            if (CardRarityThreshold.Count == 0)
                SetRarityThresholds();
            
            BuildRarityLists();
            _isFirstInit = false;
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
            CommonCards = new();
            RareCards = new();
            EpicCards = new();
            LegendaryCards = new();

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
                        CommonCards.Add(AllCards[(int)card.CardType]);
                    else
                        newRareCards.Add(card);

                    RareCards = newRareCards;
                }
            }
            else if (_remainingAmountExodiaCards <= 3)
            {
                List<Card> newEpicCards = new();
                foreach (Card card in EpicCards)
                {
                    if (card.name.Contains(EXODIA_CHECK))
                        RareCards.Add(AllCards[(int)card.CardType]);
                    else
                        newEpicCards.Add(card);
                }
                EpicCards = newEpicCards;
            }
            else if (_remainingAmountExodiaCards <= 5)
            {
                List<Card> newLegendaryList = new();
                foreach (Card card in LegendaryCards)
                {
                    if (card.name.Contains(EXODIA_CHECK))
                        EpicCards.Add(AllCards[(int)card.CardType]);
                    else
                        newLegendaryList.Add(card);
                }
                LegendaryCards = newLegendaryList;
            }
        }

        public void OnGameReset()
        {
            _isFirstInit = true;

            ES3.DeleteKey(CARDRARITY_PATH);
            ES3.DeleteKey(COMMON_CARDS_PATH);
            ES3.DeleteKey(RARE_CARDS_PATH);
            ES3.DeleteKey(EPIC_CARDS_PATH);
            ES3.DeleteKey(LEGENDARY_CARDS_PATH);

            InitializeManager();
        }
        #endregion
    }
}
