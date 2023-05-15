using PeggleWars.Utilities;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cards
{
    [RequireComponent(typeof(DisplayDeck))]
    internal class Deck : MonoBehaviour
    {
        #region Fields and Properties

        internal static Deck Instance { get; private set; }

        [SerializeField] private List<Card> _localDeck = new();
        [SerializeField] private List<Card> _discardPile = new();

        internal List<Card> DiscardPile { get => _discardPile; set => _discardPile = value; }
        internal List<Card> LocalDeck { get => _localDeck; set => _localDeck = value; }
        internal List<Card> ExhaustPile { get; set; } = new();

        private Hand _hand;
        private bool _deckIsBuilt;

        //For tweening
        internal Vector3 DiscardPosition { get; private set; }
        internal Vector3 ExhaustPosition { get; private set; }
        [SerializeField] private GameObject DiscardPileObject;
        [SerializeField] private GameObject ExhaustPileObject;

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
            }
        }

        private void Start()
        {
            _hand = Hand.Instance;
            DiscardPosition = Hand.Instance.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition;
            ExhaustPosition = Hand.Instance.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().localPosition;
            WinLoseConditionManager.Instance.LevelVictory?.AddListener(OnLevelVictory);
        }

        private void Update()
        {
            if(!_deckIsBuilt)
            {
                BuildLevelDeck();
            }
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        private void BuildLevelDeck()
        {
            _deckIsBuilt = true;
            BuildDeckFromGlobalDeck(GlobalDeckManager.Instance.GlobalDeck);
            ShuffleDeck();
        }

        private void BuildDeckFromGlobalDeck(List<Card> globalDeck)
        {
            foreach (Card card in globalDeck)
            {
                _localDeck.Add(card);
            }
        }

        private void OnLevelVictory()
        {
            this.enabled = false;
        }

        internal Card DrawCard()
        {
            if (_localDeck.Count == 0 && _discardPile.Count != 0)
            {
                _localDeck.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }

            if (_localDeck.Count > 0)
            {
                Card card = _localDeck[0];
                _localDeck.RemoveAt(0);
                return card;
            }
            else { return null; }
        }

        internal void DiscardCard(Card card)
        {            
            _discardPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        internal void ExhaustCard(Card card)
        {
            ExhaustPile.Add(card);
            _hand.HandCards.Remove(card);
        }

        internal void StartDiscardPileAnimation()
        {
            Vector3 startScale = DiscardPileObject.transform.localScale;
            DiscardPileObject.transform.DOPunchScale(startScale * 0.25f, 0.1f,1,0.1f);
        }

        internal void StartExhaustPileAnimation()
        {
            Vector3 startScale = ExhaustPileObject.transform.localScale;
            ExhaustPileObject.transform.DOPunchScale(startScale * 0.25f, 0.1f, 1, 0.1f);
        }

        //Shuffles the deck using the Fisher-Yates shuffle algortihm
        internal void ShuffleDeck()
        {
            for (int i = _localDeck.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Card temp = _localDeck[i];
                _localDeck[i] = _localDeck[j];
                _localDeck[j] = temp;
            }
        }

        #endregion
    }
}
