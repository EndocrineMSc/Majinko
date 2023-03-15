using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards
{
    internal class OrbChangerCard : Card
    {
        //will be set in the inspector of the respective Card
        [SerializeField] protected OrbType _orbType;
        [SerializeField] protected int _amountOrbs;

        protected override void CardEffect()
        {
            OrbManager.Instance.SwitchOrbs(_orbType, _amountOrbs);
        }
    }
}
