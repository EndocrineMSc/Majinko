using Utility;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Cards
{
    [RequireComponent(typeof(DisplayDeck))]
    public class Deck : MonoBehaviour
    {
        #region Fields and Properties

        public static Deck Instance { get; private set; }

        public List<GameObject> LocalDeck = new();
        public List<GameObject> DeckPile = new();
        public List<GameObject> DiscardPile = new();
        public List<GameObject> ExhaustPile = new();
        public List<GameObject> HandCards = new();

        private RectTransform _cardCanvasTransform;

        //Tweening
        public Vector3 DiscardPosition { get; private set; }
        public Vector3 ExhaustPosition { get; private set; }
        [SerializeField] private GameObject _discardPileObject;
        [SerializeField] private GameObject _exhaustPileObject;
        private float _tweenDiscardDuration = 0.5f;
        private Vector3 _tweenEndScale = new(0.05f, 0.05f, 0.05f);
        private RectTransform _rectTransform;
        private bool _discardPileIsTweening;
        private bool _exhaustPileIsTweening;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

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
            _cardCanvasTransform = Hand.Instance.CardCanvas.GetComponent<RectTransform>();
            DiscardPosition = Camera.main.WorldToScreenPoint(_discardPileObject.transform.position);
            ExhaustPosition = Camera.main.WorldToScreenPoint(_exhaustPileObject.transform.position);
            BuildLevelDeck();
        }

        private void BuildLevelDeck()
        {
            InstantiateDeckCards();
            ShuffleDeck();
        }

        private void InstantiateDeckCards()
        {
            foreach (Card card in GlobalDeckManager.Instance.GlobalDeck)
            {
                if (card != null)
                {
                    var cardObject = Instantiate(card, _cardCanvasTransform).gameObject;
                    LocalDeck.Add(cardObject);
                    DeckPile.Add(cardObject);
                    cardObject.SetActive(false);
                }
            }
        }

        private void OnLevelVictory()
        {
            foreach (var cardObject in LocalDeck)
                Destroy(cardObject);
        }

        public void DrawCard()
        {
            if (DeckPile.Count == 0 && DiscardPile.Count != 0)
            {
                DeckPile.AddRange(DiscardPile);
                DiscardPile.Clear();
                ShuffleDeck();
            }

            if (DeckPile.Count > 0)
            {
                var cardObject = DeckPile[0];
                DeckPile.Remove(cardObject);
                HandCards.Add(cardObject);
            }
        }

        public void DiscardHand()
        {
            while (HandCards.Count > 0)
            {
                DiscardCard(HandCards[0]);          
            }
        }

        public void DiscardCard(GameObject cardObject)
        {            
            DiscardPile.Add(cardObject);
            HandCards.Remove(cardObject);
            StartCoroutine(DisableCardAfterAnimation(true, cardObject));
        }

        public void ExhaustCard(GameObject cardObject)
        {
            ExhaustPile.Add(cardObject);
            HandCards.Remove(cardObject);
            StartCoroutine(DisableCardAfterAnimation(false, cardObject));
        }

        public IEnumerator StartDiscardPileAnimation()
        {
            if (!_discardPileIsTweening)
            {
                _discardPileIsTweening = true;
                Vector3 startScale = _discardPileObject.transform.localScale;
                _discardPileObject.transform.DOPunchScale(startScale * 0.25f, 0.1f,1,0.1f);
                yield return new WaitForSeconds(0.1f);
                _discardPileIsTweening = false;
            }
        }

        public IEnumerator StartExhaustPileAnimation()
        {
            if (!_exhaustPileIsTweening)
            {
                _exhaustPileIsTweening = true;
                Vector3 startScale = _exhaustPileObject.transform.localScale;
                _exhaustPileObject.transform.DOPunchScale(startScale * 0.25f, 0.1f, 1, 0.1f);
                yield return new WaitForSeconds(0.1f);
                _exhaustPileIsTweening = false;
            }
        }

        //Shuffles the deck using the Fisher-Yates shuffle algortihm
        public void ShuffleDeck()
        {
            for (int i = DeckPile.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var temp = DeckPile[i];
                DeckPile[i] = DeckPile[j];
                DeckPile[j] = temp;
            }
        }

        protected IEnumerator DisableCardAfterAnimation(bool targetIsDiscardPile, GameObject cardObject)
        {
            Vector3 targetPosition = (targetIsDiscardPile) ? DiscardPosition : ExhaustPosition;
            
            var rectTransform = cardObject.GetComponent<RectTransform>();
            rectTransform.DOMove(targetPosition, _tweenDiscardDuration).SetEase(Ease.OutExpo);
            rectTransform.DOScale(_tweenEndScale, _tweenDiscardDuration).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(_tweenDiscardDuration / 2);

            if (targetIsDiscardPile)
                StartCoroutine(StartDiscardPileAnimation());
            else
                StartExhaustPileAnimation();

            yield return new WaitForSeconds(_tweenDiscardDuration / 2);
            cardObject.SetActive(false);
            CardEvents.RaiseCardDisabled();
        }

        #endregion
    }
}
