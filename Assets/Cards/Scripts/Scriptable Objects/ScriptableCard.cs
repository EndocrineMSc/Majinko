using EnumCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu]
    internal class ScriptableCard : ScriptableObject
    {
        [SerializeField] internal string CardName;
        [SerializeField, TextArea] internal string CardDescription;
        [SerializeField] internal int BasicManaCost;
        [SerializeField] internal int FireManaCost;
        [SerializeField] internal int IceManaCost;
        [SerializeField] internal bool IsExhaustCard;
        [SerializeField] internal int Damage;
        [SerializeField] internal int Shield;
        [SerializeField] internal int BurningStacks;
        [SerializeField] internal int IceStacks;
        [SerializeField] internal CardType Type;
        [SerializeField] internal CardRarity Rarity;
        [SerializeField] internal CardElement Element;
        internal CardEffectType EffectType = CardEffectType.Instant;
    }
}
