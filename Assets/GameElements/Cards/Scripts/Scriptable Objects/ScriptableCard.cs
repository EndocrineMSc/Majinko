using EnumCollection;
using UnityEngine;
using Utility;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/InstantCard")]
    public class ScriptableCard : ScriptableObject
    {
        [SerializeField] protected string _cardName;
        [SerializeField] protected string _cardDescription;
        [SerializeField] protected int _basicManaCost;
        [SerializeField] protected int _fireManaCost;
        [SerializeField] protected int _iceManaCost;
        [SerializeField] protected bool _isExhaustCard;
        [SerializeField] protected bool _isBuff;
        [SerializeField] protected CardType _cardType;
        [SerializeField] protected CardRarity _cardRarity;
        [SerializeField] protected CardElement _cardElement;
        [SerializeField] protected Sprite _cardImage;
        [SerializeField] protected CardBuff _cardBuff;
        [SerializeField] protected EffectValueCollection _effectValues;

        [SerializeField] protected string _modifiedDescription;

        public string CardName
        {
            get { return _cardName; }
            set { _cardName = value; }
        }

        //it is intentional that getter and setter refer to different fields see method below
        public string CardDescription
        {
            get { return _modifiedDescription; }
            private protected set { _cardDescription = value; }
        }

        public int BasicManaCost
        {
            get { return _basicManaCost; }
            set { _basicManaCost = value ;}
        }

        public int FireManaCost
        {
            get { return _fireManaCost; }
            set { _fireManaCost = value; }
        }

        public int IceManaCost
        {
            get { return _iceManaCost; }
            set { _iceManaCost = value; }
        }

        public bool IsExhaustCard
        {
            get { return _isExhaustCard; }
            set { _isExhaustCard = value; }
        }

        public bool IsBuff
        {
            get { return _isBuff; }
            set { _isBuff = value; }
        }

        public CardType Type
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        public CardRarity Rarity
        {
            get { return _cardRarity; }
            set { _cardRarity = value; }
        }

        public CardElement Element
        {
            get { return _cardElement; }
            set { _cardElement = value; }
        }

        public Sprite Image
        {
            get { return _cardImage; }
            set { _cardImage = value; }
        }

        public CardBuff Buff
        {
            get { return _cardBuff; }
            set { _cardBuff = value; }
        }

        virtual public CardEffectType EffectType { get; } = CardEffectType.Instant;


        /*
         * The following method will replace placeholder strings in the card descriptions with the actual
         * values of the cards. Please use the exact placeholders in the description texts, denominated
         * by setting them in "{}"
         */

        protected void OnValidate()
        {
            if (_cardDescription != null)
                ModifyDescription();
        }

        protected virtual void ModifyDescription()
        {
            _modifiedDescription = _cardDescription;

            _modifiedDescription = _modifiedDescription.Replace("\\n", "\n");
            _modifiedDescription = _modifiedDescription.Replace("{BasicMana}", BasicManaCost.ToString());
            _modifiedDescription = _modifiedDescription.Replace("{FireMana}", FireManaCost.ToString());
            _modifiedDescription = _modifiedDescription.Replace("{IceMana}", IceManaCost.ToString());

            if (_effectValues != null)
            {
                _modifiedDescription = _modifiedDescription.Replace("{Damage}", _effectValues.Damage.ToString());
                _modifiedDescription = _modifiedDescription.Replace("{Shield}", _effectValues.ShieldStacks.ToString());
                _modifiedDescription = _modifiedDescription.Replace("{Burning}", _effectValues.BurningStacks.ToString());
                _modifiedDescription = _modifiedDescription.Replace("{Freezing}", _effectValues.FreezingStacks.ToString());
                _modifiedDescription = _modifiedDescription.Replace("{FrozenPercent}", _effectValues.PercentToFreeze.ToString());
            }
        }
    }
}
