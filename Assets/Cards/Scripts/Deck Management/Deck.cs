using PeggleWars.Utilities;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditorInternal;

namespace Cards
{
    [RequireComponent(typeof(DisplayDeck))]
    internal class Deck : MonoBehaviour
    {
        #region Fields and Properties

        internal static Deck Instance { get; private set; }

        internal List<Card> DiscardPile { get; set; } = new();
        internal List<Card> LocalDeck { get; set; } = new();
        internal List<Card> ExhaustPile { get; set; } = new();

        private Hand _hand;
        private bool _isDeckBuilt = false;

        //Tweening
        internal Vector3 DiscardPosition { get; private set; }
        internal Vector3 ExhaustPosition { get; private set; }
        [SerializeField] private GameObject DiscardPileObject;
        [SerializeField] private GameObject ExhaustPileObject;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _hand = Hand.Instance;
            DiscardPosition = Hand.Instance.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition;
            ExhaustPosition = Hand.Instance.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().localPosition;
            WinLoseConditionManager.Instance.LevelVictory?.AddListener(OnLevelVictory);
            BuildLevelDeck();
        }

        private void Update()
        {
            if (!_isDeckBuilt)
            {
                _isDeckBuilt = true;
                BuildLevelDeck();
            }
        }

        private void OnDisable()
        {
            WinLoseConditionManager.Instance.LevelVictory?.RemoveListener(OnLevelVictory);
        }

        private void BuildLevelDeck()
        {
            BuildDeckFromGlobalDeck(GlobalDeckManager.Instance.GlobalDeck);
            ShuffleDeck();
        }

        private void BuildDeckFromGlobalDeck(List<Card> globalDeck)
        {
            foreach (Card card in globalDeck)
            {
                LocalDeck.Add(card);
            }
        }

        private void OnLevelVictory()
        {
            this.enabled = false;
        }

        internal Card DrawCard()
        {
            if (LocalDeck.Count == 0 && DiscardPile.Count != 0)
            {
                LocalDeck.AddRange(DiscardPile);
                DiscardPile.Clear();
                ShuffleDeck();
            }

            if (LocalDeck.Count > 0)
            {
                Card card = LocalDeck[0];
                LocalDeck.RemoveAt(0);
                return card;
            }
            else { return null; }
        }

        internal void DiscardCard(Card card)
        {            
            DiscardPile.Add(card);
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
            for (int i = LocalDeck.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Card temp = LocalDeck[i];
                LocalDeck[i] = LocalDeck[j];
                LocalDeck[j] = temp;
            }
        }

        #endregion
    }
}
