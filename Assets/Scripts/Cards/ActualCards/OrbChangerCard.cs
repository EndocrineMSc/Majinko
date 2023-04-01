using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards
{
    internal class OrbChangerCard : Card, IShiftOrbs
    {
        //will be set in the inspector of the respective Card
        [SerializeField] protected OrbType _orbType;
        [SerializeField] protected int _amountOrbs;
        public int AmountOrbs { get => _amountOrbs; set { _amountOrbs = value; } }

        protected override void CardEffect()
        {
            OrbManager.Instance.SwitchOrbs(_orbType, _amountOrbs);
        }
    }
}
