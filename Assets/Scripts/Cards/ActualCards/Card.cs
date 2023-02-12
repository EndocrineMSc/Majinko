using UnityEngine;
using EnumCollection;
using PeggleWars.Cards.DragDrop;
using Cards.ScriptableCards;
using Cards.Zoom;
using PeggleWars.Cards.DeckManagement.HandHandling;
using PeggleWars.Orbs;
using PeggleWars.ManaManagement;

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

        //Fields set by Scriptable Object
        private string _cardName;
        private string _cardDescription;
        private int _manaCost;
        private ManaType _manaType;
        private CardType _cardType;
        private bool _exhaustCard;
        private Sprite _cardImage;

        //Necessary level references
        private ManaPool _manaPool;
        private Hand _hand;
        private OrbManager _orbManager;

        [SerializeField] protected ScriptableCard _scriptableCard;
        [SerializeField] protected GameObject _cardPrefab;

        public GameObject CardPrefab { get => _cardPrefab; set => _cardPrefab = value; }


        public string CardName { get => _cardName;}
        public string CardDescription { get => _cardDescription;}
        public int ManaCost { get => _manaCost;}
        public CardType CardType { get => _cardType;}
        public bool ExhaustCard { get => _exhaustCard;}
        public Sprite CardImage { get => _cardImage; }

        #endregion

        #region Public Virtual Functions

        /// <summary>
        /// Handles what happens when the card is dragged and let go.
        /// Is called by the CardDragDrop class.
        /// Can be overriden by child classes, if additional effects are necessary.
        /// </summary>
        /// <returns>Returns whether there was enough mana to play the card or not (boolean)</returns>
        public virtual bool CardEndDragEffect()
        {
            bool enoughMana = CheckIfEnoughMana();

            if (enoughMana)
            {
                CardEffect();

                _manaPool.SpendMana(_manaType, _manaCost);
                _orbManager.CheckForRefreshOrbs(); //Checks if RefreshOrb was overwritten and makes a new one if so
                _hand.InstantiatedCards.Remove(gameObject); //list of instantiated cards in hand
                _hand.AlignCards();

                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Private Functions

        private void Start()
        {
            SetReferencesToLevelComponents();
            SetCardValuesAndTexts();           
        }

        #endregion

        #region Protected Virtual Functions

        protected virtual void SetReferencesToLevelComponents()
        {
            _manaPool = ManaPool.Instance;
            _hand = Hand.Instance;
            _orbManager = OrbManager.Instance;
        }

        protected virtual void SetCardValuesAndTexts()
        {
            _cardName = _scriptableCard.CardName;
            _cardDescription = _scriptableCard.CardDescription;
            _manaCost = _scriptableCard.ManaCost;
            _manaType = _scriptableCard.ManaType;
            _cardType = _scriptableCard.CardType;
            _exhaustCard = _scriptableCard.IsExhaustCard;
            _cardImage = _scriptableCard.CardImage;
            _cardPrefab = _scriptableCard.CardPrefab;
        }

        protected abstract void CardEffect();      

        protected virtual bool CheckIfEnoughMana()
        {
            bool enoughMana = _manaPool.CheckForManaAmount(_manaType, _manaCost);
            return enoughMana;
        }

        #endregion
    }
}
