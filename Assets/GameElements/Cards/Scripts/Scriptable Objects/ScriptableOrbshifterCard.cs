using EnumCollection;
using Orbs;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/OrbshifterCard")]
    public class ScriptableOrbshifterCard : ScriptableCard
    {
        public override CardEffectType EffectType { get; } = CardEffectType.Orbshifter;

        [SerializeField] protected OrbData _orbData;
        [SerializeField] protected int _orbAmount;

        public OrbData OrbDataForSwitching
        {
            get { return _orbData;}
            private protected set { _orbData = value; }
        }

        public int OrbAmount
        {
            get { return _orbAmount; }
            private protected set { _orbAmount = value; }
        }
    }
}
