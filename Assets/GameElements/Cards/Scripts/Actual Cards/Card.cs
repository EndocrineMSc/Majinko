using UnityEngine;
using Orbs;
using ManaManagement;
using EnumCollection;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    [RequireComponent(typeof(CardZoom))]
    [RequireComponent(typeof(CardUIDisplayer))]
    internal abstract class Card : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] internal ScriptableCard ScriptableCard;

        //Adjusted mana amounts
        protected int _adjustedBasicManaAmount;
        protected int _adjustedFireManaAmount;
        protected int _adjustedIceManaAmount;

        //Necessary level references
        protected ManaPool _manaPool;
        protected Hand _hand;
        protected OrbManager _orbManager;
        protected Deck _deck;
        protected GlobalDeckManager _globalDeckManager;

        //Animations
        protected float _tweenDiscardDuration = 0.5f;
        protected Vector3 _tweenEndScale = new(0.05f, 0.05f, 0.05f);
        protected RectTransform _rectTransform;
        internal Vector2 PositionInHand = new();

        //Properties
        internal string CardName { get; private protected set; }
        internal string CardDescription { get; private protected set; }
        internal int BasicManaCost { get; private protected set; }
        internal int FireManaCost { get; private protected set; }
        internal int IceManaCost { get; private protected set; }
        internal CardType CardType { get; private protected set; }
        internal bool IsExhaustCard { get; private protected set; }
        internal Sprite CardImage { get; private protected set; }
        internal CardRarity Rarity { get; private protected set; }
        internal CardElement Element { get; private protected set; }
        internal CardEffectType EffectType { get; private protected set; }
        internal bool IsBuff { get; private protected set; }
        internal bool IsBeingDealt { get; set; } = true;

        #endregion

        #region Functions

        protected void Awake()
        {
            SetCardFields();           
        }

        protected void OnValidate()
        {
            SetCardFields();
        }

        protected void Start()
        {
            SetReferencesToLevelComponents();
            CalculateManaAmounts();
        }

        protected virtual void SetReferencesToLevelComponents()
        {
            _manaPool = ManaPool.Instance;
            _hand = Hand.Instance;
            _orbManager = OrbManager.Instance;
            _deck = Deck.Instance;
            _globalDeckManager = GlobalDeckManager.Instance;
            _rectTransform = GetComponent<RectTransform>();
        }
        
        protected virtual void SetCardFields()
        {
            if (ScriptableCard != null)
            {
                CardName = ScriptableCard.CardName;
                CardDescription = ScriptableCard.CardDescription;
                BasicManaCost = ScriptableCard.BasicManaCost;
                FireManaCost = ScriptableCard.FireManaCost;
                IceManaCost = ScriptableCard.IceManaCost;
                CardType = ScriptableCard.Type;
                IsExhaustCard = ScriptableCard.IsExhaustCard;
                CardImage = ScriptableCard.Image;
                Rarity = ScriptableCard.Rarity;
                Element = ScriptableCard.Element;
                EffectType = ScriptableCard.EffectType;
            }
        }
        
        protected virtual void CalculateManaAmounts()
        {
            if (ManaPool.Instance != null)
            {
                int modifier = _manaPool.ManaCostMultiplier;

                _adjustedBasicManaAmount = BasicManaCost * modifier;
                _adjustedFireManaAmount = FireManaCost * modifier;
                _adjustedIceManaAmount = IceManaCost * modifier;
            }
        }

        internal virtual bool CardEndDragEffect()
        {
            if (CheckIfEnoughMana())
            {
                CardEffect();
                _manaPool.SpendMana(_adjustedBasicManaAmount, _adjustedFireManaAmount, _adjustedIceManaAmount);
                _orbManager.CheckForRefreshOrbs(); //Checks if RefreshOrb was overwritten and makes a new one if so
                HandleDiscard();
                _hand.AlignCardsWrap();
                GetComponent<CardZoom>().enabled = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool CheckIfEnoughMana()
        {
            return _manaPool.BasicMana.Count >= _adjustedBasicManaAmount
                   && _manaPool.FireMana.Count >= _adjustedFireManaAmount
                   && _manaPool.IceMana.Count >= _adjustedIceManaAmount;
        }

        protected abstract void CardEffect();

        protected virtual void HandleDiscard()
        {
            if (IsExhaustCard)             
                _deck.ExhaustCard(this.gameObject);
            else
                _deck.DiscardCard(this.gameObject);
        }

        protected void OnDestroy()
        {
            transform.DOKill();
        }

        internal void SetPositionInHand(Vector2 positionInHand)
        {
            PositionInHand = positionInHand;
        }

        #endregion
    }
}
