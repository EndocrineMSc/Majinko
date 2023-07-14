using UnityEngine;
using EnumCollection;
using Orbs;
using Utility;

namespace Cards
{
    internal class OrbShifterCard : Card
    {
        internal int OrbAmount { get; private set; }
        internal OrbType OrbType { get; private set; }

        protected EffectValueCollection EffectValues { get; private set; }

        protected override void CardEffect()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(GetComponent<RectTransform>().transform.position);            
            OrbManager.Instance.SwitchOrbs(OrbType, worldPosition, OrbAmount);
        }

        protected override void SetCardFields()
        {
            base.SetCardFields();
            if (ScriptableCard != null)
            {
                ScriptableOrbshifterCard scriptableOrbshifterCard = (ScriptableOrbshifterCard)ScriptableCard;
                OrbType = scriptableOrbshifterCard.OrbType;
                OrbAmount = scriptableOrbshifterCard.OrbAmount;
            }
        }
    }
}
