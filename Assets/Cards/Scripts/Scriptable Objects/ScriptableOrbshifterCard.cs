using EnumCollection;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu]
    internal class ScriptableOrbshifterCard : ScriptableCard
    {
        internal new CardEffectType EffectType = CardEffectType.Orbshifter;
        [SerializeField] internal OrbType OrbType;
        [SerializeField] internal int OrbAmount;
    }
}
