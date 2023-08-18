using UnityEngine;
using EnumCollection;
using Orbs;
using Utility;
using Characters;

namespace Cards
{
    internal class OrbShifterCard : Card
    {
        internal int OrbAmount { get; private set; }
        internal OrbType OrbType { get; private set; }

        protected EffectValueCollection EffectValues { get; private set; }

        protected override void CardEffect()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 startPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
            StartCoroutine(OrbManager.Instance.SwitchOrbs(OrbType, startPosition, OrbAmount));
            Player.Instance.GetComponentInChildren<Animator>().SetTrigger("Attack");
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
