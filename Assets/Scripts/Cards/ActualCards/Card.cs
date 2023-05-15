using UnityEngine;
using PeggleWars.Orbs;
using PeggleWars.ManaManagement;
using EnumCollection;
using System.Collections;
using DG.Tweening;

namespace Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    [RequireComponent(typeof(CardZoom))]
    [RequireComponent(typeof(CardZoomEventMovement))]
    [RequireComponent(typeof(CardUIDisplayer))]
    internal abstract class Card : MonoBehaviour
    {
        #region Fields and Properties

        [Header("Card Data")]
        [SerializeField] protected string _cardName;
        [SerializeField, TextArea] protected string _cardDescription;
        [SerializeField] protected int _basicManaCost;
        [SerializeField] protected int _fireManaCost;
        [SerializeField] protected int _iceManaCost;
        [SerializeField] protected CardType _cardType;
        [SerializeField] protected bool _exhaustCard;
        [SerializeField] protected CardRarity _cardRarity;
        [SerializeField] protected CardElement _cardElement;
        [SerializeField] protected CardEffectType _cardEffectType;
        protected Sprite _cardImage;

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

        //Properties
        internal string CardName { get => _cardName;}
        internal string CardDescription { get => _cardDescription;}
        internal int BasicManaCost { get => _basicManaCost;}
        internal int FireManaCost { get => _fireManaCost;}
        internal int IceManaCost { get => _iceManaCost;}
        internal CardType CardType { get => _cardType;}
        internal bool ExhaustCard { get => _exhaustCard;}
        internal Sprite CardImage { get => _cardImage; }
        internal CardRarity Rarity { get => _cardRarity;}
        internal CardElement Element { get => _cardElement;}
        internal CardEffectType EffectType { get => _cardEffectType;}
        internal bool IsBeingDealt { get; set; } = true;

        #endregion

        #region Functions

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
        } 
        
        protected virtual void CalculateManaAmounts()
        {
            int modifier = _manaPool.ManaCostMultiplier;

            _adjustedBasicManaAmount = _basicManaCost * modifier;
            _adjustedFireManaAmount = _fireManaCost * modifier;
            _adjustedIceManaAmount = _iceManaCost * modifier;
        }

        internal virtual bool CardEndDragEffect()
        {
            if (CheckIfEnoughMana())
            {
                CardEffect();
                _manaPool.SpendMana(_adjustedBasicManaAmount, _adjustedFireManaAmount, _adjustedIceManaAmount);
                _orbManager.CheckForRefreshOrbs(); //Checks if RefreshOrb was overwritten and makes a new one if so
                HandleDiscard();
                _hand.InstantiatedCards.Remove(this); //list of instantiated cards in hand
                StartCoroutine(DestroyCardAfterAnimation());
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
            if (_exhaustCard)             
                _deck.ExhaustCard(GlobalCardManager.Instance.AllCards[(int)_cardType]);
            else
                _deck.DiscardCard(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }

        protected IEnumerator DestroyCardAfterAnimation()
        {
            Vector3 targetPosition = (_exhaustCard) ? Deck.Instance.ExhaustPosition : Deck.Instance.DiscardPosition;

            GetComponent<RectTransform>().DOLocalMove(targetPosition, _tweenDiscardDuration).SetEase(Ease.OutExpo);
            GetComponent<RectTransform>().DOScale(_tweenEndScale, _tweenDiscardDuration).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(_tweenDiscardDuration * 5/10);

            CardEvents.Instance.CardDestructionEvent?.Invoke();

            if(_exhaustCard)
                Deck.Instance.StartExhaustPileAnimation();
            else
                Deck.Instance.StartDiscardPileAnimation();

            yield return new WaitForSeconds(_tweenDiscardDuration * 5 / 10);
            Destroy(gameObject);
        }

        #endregion
    }
}
