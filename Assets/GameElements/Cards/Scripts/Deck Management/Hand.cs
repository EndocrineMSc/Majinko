using System.Collections.Generic;
using UnityEngine;
using Utility.TurnManagement;
using Audio;
using System.Collections;
using Utility;
using System.Linq;
using DG.Tweening;
using Characters;

namespace Cards
{
    internal class Hand : MonoBehaviour
    {
        #region Fields and Properties

        internal static Hand Instance { get; private set; }
        internal int DrawAmount { get; set; } = 5;
        private readonly int _maxDrawAmount = 10;

        private Transform _cardSpawnTransform;
        private Deck _deck;
        internal Canvas CardCanvas { get; private set; } //The Card Canvas will be a child of the Hand and contain the UI of instantiated Cards

        //Tweening
        private readonly float _moveDuration = 0.35f;
        private readonly float _startScale = 0.25f;
        private readonly float _endScale = 0.75f;

        //Alignment Handling
        private List<Vector2> _evenCardPositions;
        private List<Vector2> _oddCardPositions;
        private List<int> _evenAngles;
        private List<int> _oddAngles;
        private readonly int _angleStepSize = 5;
        private readonly int _minimumEvenAngle = 25;
        private readonly int _minimumOddAngle = 20;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            CardCanvas = transform.GetChild(0).GetComponent<Canvas>();
            _cardSpawnTransform = transform.GetChild(0).GetChild(0);

            _evenCardPositions = SetEvenCardPositions();
            _oddCardPositions = SetOddCardPositions();
            _evenAngles = SetEvenAngles();
            _oddAngles = SetOddAngles();
        }

        private void OnEnable()
        {
            LevelPhaseEvents.OnStartCardPhase += OnCardPhaseStart;
            LevelPhaseEvents.OnEndCardPhase += OnCardPhaseEnd;
            UtilityEvents.OnLevelVictory += OnLevelVictory;
        }

        private void OnDisable()
        {
            LevelPhaseEvents.OnStartCardPhase -= OnCardPhaseStart;
            LevelPhaseEvents.OnEndCardPhase -= OnCardPhaseEnd;
            UtilityEvents.OnLevelVictory -= OnLevelVictory;
        }

        private void Start()
        {
            _deck = Deck.Instance;
        }

        internal void OnLevelVictory()
        {
            this.enabled = false;
        }

        internal void OnCardPhaseStart()
        {
            DrawAmount += PlayerConditionTracker.FastHandStacks;
            PlayerConditionTracker.ResetFastHandsStacks();
            DrawHand(DrawAmount);
        }

        internal void OnCardPhaseEnd()
        {
            _deck.DiscardHand();
            DrawAmount = 5;
        }

        internal void DrawHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _deck.DrawCard();
                if (i > _maxDrawAmount - 1)
                    break;
            }
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0006_DrawHand);
            DealHand(true);
        }

        //makes a new set of displayed instantiated cards for each card in the _handCards list
        internal void DealHand(bool isStartTurnDealing = false)
        {
            foreach (var cardObject in _deck.HandCards)
            {
                cardObject.GetComponent<RectTransform>().localPosition = _cardSpawnTransform.localPosition;
                cardObject.transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            }
            AlignCardsWrap(isStartTurnDealing);
        }

        internal void AlignCardsWrap(bool isStartTurnDealing = false)
        {
            if (_deck.HandCards.Count > 0)
                StartCoroutine(AlignCards(isStartTurnDealing));       
        }

        private IEnumerator AlignCards(bool isStartTurnDealing = false)
        {
            var newCardPositions = _deck.HandCards.Count % 2 == 0 ? _evenCardPositions : _oddCardPositions;
            var index = 5 - Mathf.FloorToInt((float)_deck.HandCards.Count / 2);

            if (newCardPositions.Count != 0 && _deck.HandCards.Count != 0 && !CardEvents.CardIsZoomed)
            {
                SetCardAngles();

                for (int i = 0; i < _deck.HandCards.Count; i++)
                {
                    var cardObject = _deck.HandCards[i];
                    var currentCard = cardObject.GetComponent<Card>();
                    cardObject.SetActive(true);
                    currentCard.SetPositionInHand(newCardPositions[index]);
                    RectTransform rectTransform = cardObject.GetComponent<RectTransform>();
                    cardObject.transform.SetSiblingIndex(i);

                    if (isStartTurnDealing)
                    {
                        currentCard.IsBeingDealt = true;
                        DisableZoomComponents(currentCard);
                        yield return StartCoroutine(TweenCardSpawn(newCardPositions[index], rectTransform));
                        currentCard.IsBeingDealt = false;
                        EnableZoomComponents(currentCard);
                    }
                    else
                    {
                        rectTransform.DOAnchorPos(newCardPositions[index], 0.5f).SetEase(Ease.OutCubic);
                    }
                    index++;
                }            
            }
        }

        private void SetCardAngles()
        {
            var amountHandCards = _deck.HandCards.Count;
            var cardAngles = amountHandCards % 2 == 0 ? _evenAngles : _oddAngles;
            var index = amountHandCards % 2 == 0 ? 
                (5 - Mathf.FloorToInt((float)amountHandCards / 2)) 
                : (4 - Mathf.FloorToInt((float)amountHandCards / 2));

            for (int i = 0; i < amountHandCards; i++)
            {
                var cardObject = _deck.HandCards[i];
                int zAngleOffset = cardAngles[index];
                Vector3 newAngle = new(0, 0, zAngleOffset);
                cardObject.GetComponent<RectTransform>().eulerAngles = newAngle;
                index++;
            }
        }

        private List<int> SetOddAngles()
        {
            var oddAngles = new List<int>();
            for (int i = 0; i < 10; i++)
                oddAngles.Add((_minimumOddAngle + (-i * _angleStepSize)));
            return oddAngles;
        }

        private List<int> SetEvenAngles()
        {
            var evenAngles = new List<int>();
            //no center card -> no 0 angle, we need to skip at this index
            int doubleStepIndex = 5;
            for (int i = 0; i < _maxDrawAmount; i++)
            {
                if (i >= doubleStepIndex)
                    evenAngles.Add(_minimumEvenAngle - ((i + 1) * _angleStepSize));
                else
                    evenAngles.Add(_minimumEvenAngle - (i * _angleStepSize));
            }
            return evenAngles;
        }

        private IEnumerator TweenCardSpawn(Vector2 targetPosition, RectTransform cardTransform)
        {
            cardTransform.DOLocalMove(targetPosition, _moveDuration).SetEase(Ease.OutExpo);
            cardTransform.DOScale(_endScale, _moveDuration * 1.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds((_moveDuration * 0.5f));
        }

        private void DisableZoomComponents(Card card)
        {
            card.GetComponent<CardZoom>().enabled = false;
            card.GetComponent<CardDragDrop>().enabled = false;
        }

        private void EnableZoomComponents(Card card)
        {
            card.GetComponent<CardZoom>().enabled = true;
            card.GetComponent<CardDragDrop>().enabled = true;
        }

        private List<Vector2> SetEvenCardPositions()
        {
            float canvasHeight = CardCanvas.GetComponent<RectTransform>().rect.height;
            float cardHeight = 270;
            float xOffset = 125;
            float yOffset = 10;
            float yPosition = ((-canvasHeight / 2) + (cardHeight / 3));
            int offsetCounterHelper = 0;
            float offsetCounter = 1.5f;
            List<Vector2> evenCardPositions = new();

            for (int i = 0; i < _maxDrawAmount; i++)
            {
                if (offsetCounterHelper >= 2)
                {
                    offsetCounter++;
                    offsetCounterHelper = 0;
                }

                if (i == 0)
                {
                    var angleOffsetCounter = offsetCounter - 0.5f;
                    var angleAdjustedYOffset = angleOffsetCounter * (yOffset + ((angleOffsetCounter - 1) * (yOffset / 2)));
                    Vector2 cardPosition = new((xOffset / 2), yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                }
                else if (i == 1)
                {
                    var angleOffsetCounter = offsetCounter - 0.5f;
                    var angleAdjustedYOffset = angleOffsetCounter * (yOffset + ((angleOffsetCounter - 1) * (yOffset / 2)));
                    Vector2 cardPosition = new((-xOffset / 2), yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                }
                else if (i % 2 == 0)
                {
                    var angleOffsetCounter = offsetCounter + 0.5f;
                    var angleAdjustedYOffset = angleOffsetCounter * (yOffset + ((angleOffsetCounter - 1) * (yOffset / 2)));
                    Vector2 cardPosition = new((offsetCounter * xOffset), yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                    offsetCounterHelper += 1;
                }
                else
                {
                    float angleOffsetCounter = offsetCounter + 0.5f;
                    float angleAdjustedYOffset = angleOffsetCounter * (yOffset + ((angleOffsetCounter - 1) * (yOffset / 2)));
                    Vector2 cardPosition = new((-offsetCounter * xOffset), yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                    offsetCounterHelper += 1;
                }
            }
            List<Vector2> sortedCardPositions = evenCardPositions.OrderBy(vector => vector.x).ToList<Vector2>();
            return sortedCardPositions;
        }

        private List<Vector2> SetOddCardPositions()
        {
            float canvasHeight = CardCanvas.GetComponent<RectTransform>().rect.height;
            float cardHeight = 270;
            float xOffset = 125;
            float yOffset = 10;
            float yPosition = ((-canvasHeight / 2) + (cardHeight / 3));
            int offsetCounterHelper = 0;
            float offsetCounter = 1;
            List<Vector2> oddCardPositions = new();
           
            for (int i = 0; i < 10; i++)
            {
                if (offsetCounterHelper >= 2)
                {
                    offsetCounter++;
                    offsetCounterHelper = 0;
                }   
                
                var angleOffsetCounter = offsetCounter + 0.5f;
                var angleAdjustedYOffset = angleOffsetCounter * (yOffset + ((angleOffsetCounter - 1) * (yOffset / 2)));

                if (i == 0)
                {
                    Vector2 cardPosition = new(0, yPosition - yOffset);
                    oddCardPositions.Add(cardPosition);
                }
                else if (i % 2 == 0)
                {
                    Vector2 cardPosition = new((offsetCounter * xOffset), yPosition - angleAdjustedYOffset);
                    oddCardPositions.Add(cardPosition);
                    offsetCounterHelper++;
                }
                else
                {
                    Vector2 cardPosition = new((-offsetCounter * xOffset), yPosition - angleAdjustedYOffset);
                    oddCardPositions.Add(cardPosition);
                    offsetCounterHelper++;
                }
            }

            List<Vector2> sortedCardPositions = oddCardPositions.OrderBy(vector => vector.x).ToList<Vector2>();
            return sortedCardPositions;
        }
    }

    #endregion
}
