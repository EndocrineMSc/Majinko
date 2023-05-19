using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu]
    internal class ScriptableSphereshifterCard : ScriptableCard
    {
        internal new CardEffectType EffectType = CardEffectType.Sphereshifter;
        [SerializeField] internal ShotType ShotType;
    }
}
