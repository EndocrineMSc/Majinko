using UnityEngine;
using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards.Orbchangers
{
    /// <summary>
    /// These kind of cards will change one or more orbs on the fields into a new kind of orb.
    /// The new Orb is signified by the enum OrbType
    /// </summary>
    public class OrbChangerCard : Card
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
