using PeggleMana;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using Cards.DragDrop;
using Cards.ScriptableCards;
using Cards.Zoom;
using Cards.DeckManagement;
using Cards.DeckManagement.HandHandling;

namespace Cards
{
    [RequireComponent(typeof(CardDragDrop))]
    [RequireComponent(typeof(CardZoom))]
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

        private ManaPoolManager _manaPoolManager;
        private DeckManager _deckManager;
        private HandManager _hand;

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

        public virtual bool CardEndDragEffect(Vector3 startPosition)
        {
            bool enoughMana = CheckForMana();
            if (enoughMana)
            {
                SubtractManaCost();
                CardEffect();
                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Protected Virtual Functions

        protected virtual void Start()
        {
            _manaPoolManager = ManaPoolManager.Instance;
            _deckManager = DeckManager.Instance;
            _hand = HandManager.Instance;

            _cardName = _scriptableCard.CardName;
            _cardDescription = _scriptableCard.CardDescription;
            _manaCost = _scriptableCard.ManaCost;
            _manaType = _scriptableCard.ManaType;
            _cardType = _scriptableCard.CardType;
            _exhaustCard = _scriptableCard.IsExhaustCard;
            _cardImage = _scriptableCard.CardImage;
            _cardPrefab= _scriptableCard.CardPrefab;
        }

        protected virtual void SubtractManaCost()
        {
            _manaPoolManager.SpendMana(ManaType.BaseMana, _manaCost);
        }

        protected virtual void CardEffect()
        {
            //actual effects are implemented in child classes
        }

        protected virtual bool CheckForMana()
        {
            if (_manaPoolManager.BasicMana.Count >= _manaCost)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
