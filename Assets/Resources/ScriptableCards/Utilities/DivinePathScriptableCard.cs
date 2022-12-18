using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.ScriptableCards
{
    public class DivinePathScriptableCard : ScriptableObject
    {
        public string CardName { get; } = "Divine Path";

        public string CardDescription { get; } = "Increases the range of the shot prediction.";

        public int ManaCost { get; } = 0;

        public ManaType ManaType { get; } = ManaType.BaseMana;

        public CardType CardType { get; } = CardType.Utility;      
    }
}
