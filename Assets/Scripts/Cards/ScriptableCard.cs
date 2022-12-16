using EnumCollection;
using System.Collections;
using System.Collections.Generic;
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
    }
}
