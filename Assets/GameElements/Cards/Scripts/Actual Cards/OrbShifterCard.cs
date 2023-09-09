using UnityEngine;
using Orbs;
using Utility;
using Characters;

namespace Cards
{
    public class OrbShifterCard : Card
    {
        public int OrbAmount { get; private set; }
        public OrbData OrbDataForSwitching { get; private set; }
        protected EffectValueCollection EffectValues { get; private set; }

        protected override void CardEffect()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 startPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
            OrbManager.Instance.SwitchOrbs(OrbDataForSwitching, startPosition, OrbAmount);
            Player.Instance.GetComponentInChildren<Animator>().SetTrigger("Attack");
        }

        protected override void SetCardFields()
        {
            base.SetCardFields();
            if (ScriptableCard != null)
            {
                ScriptableOrbshifterCard scriptableOrbshifterCard = (ScriptableOrbshifterCard)ScriptableCard;
                OrbDataForSwitching = scriptableOrbshifterCard.OrbDataForSwitching;
                OrbAmount = scriptableOrbshifterCard.OrbAmount;
            }
        }
    }
}
