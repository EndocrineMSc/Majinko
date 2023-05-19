using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu]
    internal class ScriptableSphereshifterCard : ScriptableCard
    {
        internal override CardEffectType EffectType { get; } = CardEffectType.Sphereshifter;
        [SerializeField] internal SphereType SphereType;
    }
}
