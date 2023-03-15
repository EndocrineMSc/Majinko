using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Cards;
using EnumCollection;

internal class GlobalCardManager : MonoBehaviour
{
    internal Dictionary<CardRarity, float> CardRarityModifier { get; private set; } = new();

    private void SetRarityModifiers()
    {
        CardRarityModifier.Add(CardRarity.Uncommon, 1f);
        CardRarityModifier.Add(CardRarity.Common, 0.75f);
        CardRarityModifier.Add(CardRarity.Rare, 0.5f);
        CardRarityModifier.Add(CardRarity.Epic, 0.25f);
    }

    private void Start()
    {
        
    }
}
