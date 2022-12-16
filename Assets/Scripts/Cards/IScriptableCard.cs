using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace Cards.ScriptableCards
{
    public interface IScriptableCard
    {
        string CardName { get; }
        string CardDescription { get; }
        int ManaCost { get; }
        ManaType ManaType { get; }
        CardType CardType { get; }
    }
}
