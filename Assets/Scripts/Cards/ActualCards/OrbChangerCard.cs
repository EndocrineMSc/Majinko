using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards
{
    internal class OrbChangerCard : Card
    {
        //will be set in the inspector of the respective Card
        [SerializeField] private OrbType _orbType;
        [SerializeField] private int _amountOrbs;

        protected override void CardEffect()
        {
            OrbManager.Instance.SwitchOrbs(_orbType, _amountOrbs);
        }
    }
}
