using EnumCollection;
using UnityEngine;

namespace Cards.ScriptableCards
{
    public class ScriptableCard : ScriptableObject
    {
        [SerializeField] private string _cardName;
        public string CardName { get => _cardName; }

        [SerializeField] private string _cardDescription;
        public string CardDescription { get => _cardDescription; }

        [SerializeField] private int _manaCost;
        public int ManaCost { get => _manaCost; }

        [SerializeField] private ManaType _manaType;
        public ManaType ManaType { get => _manaType; }

        [SerializeField] private CardType _cardType;
        public CardType CardType { get  => _cardType; }

        [SerializeField] private bool _exhaustCard;
        public bool IsExhaustCard { get => _exhaustCard; }

        [SerializeField] private Sprite _cardImage;
        public Sprite CardImage { get => _cardImage; }

        [SerializeField] private GameObject _cardPrefab;
        public GameObject CardPrefab { get => _cardPrefab; }
    }
}
