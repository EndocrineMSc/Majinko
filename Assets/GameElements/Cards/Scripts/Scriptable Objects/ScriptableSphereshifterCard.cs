using EnumCollection;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/SphereshifterCard")]
    public class ScriptableSphereshifterCard : ScriptableCard
    {
        public override CardEffectType EffectType { get; } = CardEffectType.Sphereshifter;
        [SerializeField] internal SphereType SphereType;
    }
}
