using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/InstantCard")]
    internal class ScriptableCard : ScriptableObject
    {
        [SerializeField] internal string CardName;
        [SerializeField, TextArea] internal string CardDescription;
        [SerializeField] internal int BasicManaCost;
        [SerializeField] internal int FireManaCost;
        [SerializeField] internal int IceManaCost;
        [SerializeField] internal bool IsExhaustCard;
        [SerializeField] internal bool IsBuff;
        [SerializeField] internal CardType Type;
        [SerializeField] internal CardRarity Rarity;
        [SerializeField] internal CardElement Element;
        [SerializeField] internal Sprite Image;
        virtual internal CardEffectType EffectType { get; } = CardEffectType.Instant;
    }
}
