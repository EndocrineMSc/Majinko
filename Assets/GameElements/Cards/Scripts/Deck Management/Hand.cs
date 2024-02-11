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
    public class Hand : MonoBehaviour
    {
        #region Fields and Properties

        public static Hand Instance { get; private set; }

        [field: SerializeField] public Canvas CardCanvas { get; private set; } 
        [SerializeField] private Transform _cardSpawnTransform;
        
        public int DrawAmount { get; set; } = 5;
        private readonly int _maxDrawAmount = 10;

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
        private int _middlePositionIndex;
        private readonly float _cardHeight = 270;
        private readonly float _xOffset = 125;
        private readonly float _yOffset = 10;

        #endregion

        #region Functions

        private void Awake()
        {
            //Singleton Declaration
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _middlePositionIndex = Mathf.FloorToInt(_maxDrawAmount / 2f);

            CalculateCardPositionsAndAngles();
        }

        /// <summary>
        /// Sets the empty placeholder positions and their angles
        /// on the Canvas as Vectors and ints in lists
        /// </summary>
        private void CalculateCardPositionsAndAngles()
        {
            _evenCardPositions = CalculateEvenCardPositions();
            _oddCardPositions = CalculateOddCardPositions();
            _evenAngles = CalculateEvenAngles();
            _oddAngles = CalculateOddAngles();
        }

        public void DrawHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Deck.Instance.DrawCard();
                if (i > _maxDrawAmount - 1)
                    break;
            }
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0006_DrawHand);
            DealHand(true);
        }

        public void DealHand(bool isStartTurnDealing = false)
        {
            foreach (var cardObject in Deck.Instance.HandCards)
            {
                cardObject.GetComponent<RectTransform>().localPosition = _cardSpawnTransform.localPosition;
                cardObject.transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            }
            AlignCardsWrap(isStartTurnDealing);
        }

        public void AlignCardsWrap(bool isStartTurnDealing = false)
        {
            if (Deck.Instance.HandCards.Count > 0)
                StartCoroutine(AlignCards(isStartTurnDealing));       
        }

        private IEnumerator AlignCards(bool isStartTurnDealing = false)
        {
            var amountHandCards = Deck.Instance.HandCards.Count;
            var newCardPositions = amountHandCards % 2 == 0 ? _evenCardPositions : _oddCardPositions;
            var halfHandSize = Mathf.FloorToInt(amountHandCards / 2f);
            var index = _middlePositionIndex - halfHandSize;

            if (newCardPositions.Count != 0 && amountHandCards != 0 && !CardEvents.CardIsZoomed)
            {
                SetCardAngles();

                for (int i = 0; i < amountHandCards; i++)
                {
                    var cardObject = Deck.Instance.HandCards[i];
                    var currentCard = cardObject.GetComponent<Card>();
                    var rectTransform = cardObject.GetComponent<RectTransform>();

                    cardObject.SetActive(true);
                    currentCard.SetPositionInHand(newCardPositions[index]);
                    cardObject.transform.SetSiblingIndex(i);

                    if (isStartTurnDealing)
                    {
                        currentCard.IsBeingDealt = true;
                        DisableZoomComponents(currentCard);
                        yield return StartCoroutine(TweenCardSpawn(newCardPositions[index], rectTransform));
                        EnableZoomComponents(currentCard);
                    }
                    else
                    {
                        rectTransform.DOAnchorPos(newCardPositions[index], 0.5f).SetEase(Ease.OutCubic);
                    }
                    index++;
                }
            }

            if (isStartTurnDealing)
                for (int i = 0; i < amountHandCards; i++)
                    Deck.Instance.HandCards[i].GetComponent<Card>().IsBeingDealt = false;
        }

        private void SetCardAngles()
        {
            var amountHandCards = Deck.Instance.HandCards.Count;
            var evenAmountCards = amountHandCards % 2 == 0;
            var halfHandSize = Mathf.FloorToInt(amountHandCards / 2f);

            var cardAngles = evenAmountCards ? _evenAngles : _oddAngles;
            var angleIndex = evenAmountCards ? _middlePositionIndex - halfHandSize : _middlePositionIndex - (halfHandSize + 1);

            for (int i = 0; i < amountHandCards; i++)
            {
                var cardObject = Deck.Instance.HandCards[i];
                var zAngleOffset = cardAngles[angleIndex];
                var newAngle = new Vector3(0, 0, zAngleOffset);
                cardObject.GetComponent<RectTransform>().eulerAngles = newAngle;
                angleIndex++;
            }
        }

        private List<int> CalculateOddAngles()
        {
            var oddAngles = new List<int>();
            for (int i = 0; i < _maxDrawAmount; i++)
            {
                oddAngles.Add((_minimumOddAngle + (-i * _angleStepSize)));
            }
            return oddAngles;
        }

        private List<int> CalculateEvenAngles()
        {
            var evenAngles = new List<int>();

            //no center card -> no 0 angle, we need to skip at this index
            int doubleStepIndex = 5;
            for (int i = 0; i < _maxDrawAmount; i++)
            {
                var newAngle = i >= doubleStepIndex ? _minimumEvenAngle - ((i + 1) * _angleStepSize) : _minimumEvenAngle - (i * _angleStepSize);
                evenAngles.Add(newAngle);
            }
            return evenAngles;
        }

        private List<Vector2> CalculateEvenCardPositions()
        {
            var canvasHeight = CardCanvas.GetComponent<RectTransform>().rect.height;
            var yPosition = (-canvasHeight / 2f) + (_cardHeight / 3f);
            var offsetCounterHelper = 0;
            var offsetCounter = 1.5f;
            var evenCardPositions = new List<Vector2>();

            for (int i = 0; i < _maxDrawAmount; i++)
            {
                if (offsetCounterHelper >= 2)
                {
                    offsetCounter++;
                    offsetCounterHelper = 0;
                }

                var angleOffsetCounter = offsetCounter + 0.5f;
                var angleAdjustedYOffset = angleOffsetCounter * (_yOffset + ((angleOffsetCounter - 1) * (_yOffset / 2)));

                if (i == 0)
                {
                    var cardPosition = new Vector2(_xOffset / 2, yPosition - _yOffset);
                    evenCardPositions.Add(cardPosition);
                }
                else if (i == 1)
                {
                    var cardPosition = new Vector2(-_xOffset / 2, yPosition - _yOffset);
                    evenCardPositions.Add(cardPosition);
                }
                else if (i % 2 == 0)
                {
                    var cardPosition = new Vector2(offsetCounter * _xOffset, yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                    offsetCounterHelper += 1;
                }
                else
                {
                    var cardPosition = new Vector2(-offsetCounter * _xOffset, yPosition - angleAdjustedYOffset);
                    evenCardPositions.Add(cardPosition);
                    offsetCounterHelper += 1;
                }
            }
            List<Vector2> sortedCardPositions = evenCardPositions.OrderBy(vector => vector.x).ToList();
            return sortedCardPositions;
        }

        private List<Vector2> CalculateOddCardPositions()
        {
            var canvasHeight = CardCanvas.GetComponent<RectTransform>().rect.height;
            var yPosition = (-canvasHeight / 2f) + (_cardHeight / 3f);
            var offsetCounterHelper = 0;
            var offsetCounter = 1f;
            var oddCardPositions = new List<Vector2>();
           
            for (int i = 0; i < 10; i++)
            {
                if (offsetCounterHelper >= 2)
                {
                    offsetCounter++;
                    offsetCounterHelper = 0;
                }   
                
                var angleOffsetCounter = offsetCounter + 0.5f;
                var angleAdjustedYOffset = angleOffsetCounter * (_yOffset + ((angleOffsetCounter - 1) * (_yOffset / 2)));

                if (i == 0)
                {
                    var cardPosition = new Vector2(0, yPosition - _yOffset);
                    oddCardPositions.Add(cardPosition);
                }
                else if (i % 2 == 0)
                {
                    var cardPosition = new Vector2((offsetCounter * _xOffset), yPosition - angleAdjustedYOffset);
                    oddCardPositions.Add(cardPosition);
                    offsetCounterHelper++;
                }
                else
                {
                    var cardPosition = new Vector2((-offsetCounter * _xOffset), yPosition - angleAdjustedYOffset);
                    oddCardPositions.Add(cardPosition);
                    offsetCounterHelper++;
                }
            }
            List<Vector2> sortedCardPositions = oddCardPositions.OrderBy(vector => vector.x).ToList();
            return sortedCardPositions;
        }

        private IEnumerator TweenCardSpawn(Vector2 targetPosition, RectTransform cardTransform)
        {
            cardTransform.DOLocalMove(targetPosition, _moveDuration).SetEase(Ease.OutExpo);
            cardTransform.DOScale(_endScale, _moveDuration * 1.5f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds((_moveDuration * 0.5f));
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

        public void OnLevelVictory()
        {
            this.enabled = false;
        }

        public void OnCardPhaseStart()
        {
            DrawAmount += PlayerConditionTracker.FastHandStacks;
            PlayerConditionTracker.ResetFastHandsStacks();
            DrawHand(DrawAmount);
        }

        public void OnCardPhaseEnd()
        {
            Deck.Instance.DiscardHand();
            DrawAmount = 5;
        }

        private void DisableZoomComponents(Card card)
        {
            card.GetComponent<CardMovement>().enabled = false;
        }

        private void EnableZoomComponents(Card card)
        {
            card.GetComponent<CardMovement>().enabled = true;
        }
    }

    #endregion
}
