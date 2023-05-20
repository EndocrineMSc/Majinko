using EnumCollection;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/OrbshifterCard")]
    internal class ScriptableOrbshifterCard : ScriptableCard
    {
        internal override CardEffectType EffectType { get; } = CardEffectType.Orbshifter;
        [SerializeField] internal OrbType OrbType;
        [SerializeField] internal int OrbAmount;
    }
}
