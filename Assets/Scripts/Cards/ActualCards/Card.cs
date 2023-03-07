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
    /// <summary>
    /// Parent class to all Cards used in the game. This class and its children handle what a card is and the effects of cards when drawn, played, discarded, etc.
    /// A GameObject that has this script (or a child inheriting this script) attached to it also immediatly is assigned the CardDragDrop and CardZoom components. 
    /// </summary>

    [RequireComponent(typeof(CardDragDrop))]
    [RequireComponent(typeof(CardZoom))]
    [RequireComponent(typeof(CardZoomEventMovement))]
    public abstract class Card : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] protected string _cardName;
        [SerializeField] protected string _cardDescription;
        [SerializeField] protected int _manaCost;
        [SerializeField] protected ManaType _manaType;
        [SerializeField] protected CardType _cardType;
        [SerializeField] protected bool _exhaustCard;
        protected Sprite _cardImage;

        //Necessary level references
        protected ManaPool _manaPool;
        protected Hand _hand;
        protected OrbManager _orbManager;
        protected Deck _deck;
        protected GlobalDeckManager _globalDeckManager;

        [SerializeField] protected GameObject _cardPrefab;

        public GameObject CardPrefab { get => _cardPrefab; set => _cardPrefab = value; }

        public string CardName { get => _cardName;}
        public string CardDescription { get => _cardDescription;}
        public int ManaCost { get => _manaCost;}
        public CardType CardType { get => _cardType;}
        public bool ExhaustCard { get => _exhaustCard;}
        public Sprite CardImage { get => _cardImage; }

        #endregion

        #region Functions
        protected void Start()
        {
            SetReferencesToLevelComponents();      
        }

        public virtual bool CardEndDragEffect()
        {
            bool enoughMana = CheckIfEnoughMana();

            if (enoughMana)
            {
                CardEffect();

                _manaPool.SpendMana(_manaType, _manaCost);
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


        protected virtual void SetReferencesToLevelComponents()
        {
            _manaPool = ManaPool.Instance;
            _hand = Hand.Instance;
            _orbManager = OrbManager.Instance;
            _deck = Deck.Instance;
            _globalDeckManager = GlobalDeckManager.Instance;
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

        protected virtual bool CheckIfEnoughMana()
        {
            bool enoughMana = _manaPool.CheckForManaAmount(_manaType, _manaCost);
            return enoughMana;
        }

        #endregion
    }
}
