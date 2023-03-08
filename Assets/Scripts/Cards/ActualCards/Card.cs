using UnityEngine;
using EnumCollection;
using PeggleWars.Cards.DragDrop;
using Cards.Zoom;
using PeggleWars.Cards.DeckManagement.HandHandling;
using PeggleWars.Orbs;
using PeggleWars.ManaManagement;
using PeggleWars.Cards.DeckManagement;
using PeggleWars.Cards.DeckManagement.Global;

namespace PeggleWars.Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    [RequireComponent(typeof(CardZoom))]
    [RequireComponent(typeof(CardZoomEventMovement))]
    [RequireComponent(typeof(CardTextUI))]
    public abstract class Card : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] protected string _cardName;
        [SerializeField] protected string _cardDescription;
        [SerializeField] protected int _basicManaCost;
        [SerializeField] protected int _fireManaCost;
        [SerializeField] protected int _iceManaCost;
        [SerializeField] protected CardType _cardType;
        [SerializeField] protected bool _exhaustCard;
        protected Sprite _cardImage;

        protected int _adjustedBasicManaAmount;
        protected int _adjustedFireManaAmount;
        protected int _adjustedIceManaAmount;

        //Necessary level references
        protected ManaPool _manaPool;
        protected Hand _hand;
        protected OrbManager _orbManager;
        protected Deck _deck;
        protected GlobalDeckManager _globalDeckManager;

        public string CardName { get => _cardName;}
        public string CardDescription { get => _cardDescription;}
        public int BasicManaCost { get => _basicManaCost;}
        public int FireManaCost { get => _fireManaCost;}
        public int IceManaCost { get => _iceManaCost;}
        public CardType CardType { get => _cardType;}
        public bool ExhaustCard { get => _exhaustCard;}
        public Sprite CardImage { get => _cardImage; }

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

        public virtual bool CardEndDragEffect()
        {
            if (CheckIfEnoughMana())
            {
                CardEffect();

                _manaPool.SpendMana(_adjustedBasicManaAmount, _adjustedFireManaAmount, _adjustedIceManaAmount);
                _orbManager.CheckForRefreshOrbs(); //Checks if RefreshOrb was overwritten and makes a new one if so
                _hand.InstantiatedCards.Remove(this); //list of instantiated cards in hand
                _hand.AlignCards();
                HandleDiscard();

                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool CheckIfEnoughMana()
        {
            if (_manaPool.BasicMana.Count >= _adjustedBasicManaAmount
                && _manaPool.FireMana.Count >= _adjustedFireManaAmount
                && _manaPool.IceMana.Count >= _adjustedIceManaAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract void CardEffect();

        protected virtual void HandleDiscard()
        {
            if (_exhaustCard)
            {              
                _deck.ExhaustCard(_globalDeckManager.AllCards[(int)_cardType]);
            }
            else
            {
                _deck.DiscardCard(_globalDeckManager.AllCards[(int)_cardType]);
            }
        }

        #endregion
    }
}
