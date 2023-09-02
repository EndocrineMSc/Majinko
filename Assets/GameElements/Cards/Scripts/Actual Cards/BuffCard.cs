using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    internal class BuffCard : Card
    {
        CardBuff _buff;

        protected override void SetCardFields()
        {
            base.SetCardFields();
            if (ScriptableCard != null)
                _buff = ScriptableCard.Buff;
        }

        protected override void CardEffect()
        {
            switch (_buff)
            {
                case CardBuff.ShieldBeetle:
                    PlayerConditionTracker.ActivateShieldBeetle();
                    break;
                case CardBuff.BubbleWand:
                    PlayerConditionTracker.ActivateBubbleWand();
                    break;
                case CardBuff.WardingRune:
                    PlayerConditionTracker.ActivateWardingRune();
                    break;
                case CardBuff.OrbInlayedGauntlets:
                    PlayerConditionTracker.ActivateOrbInlayedGauntlets();
                    break;
            }
        }
    }

    internal enum CardBuff
    {
        None,
        ShieldBeetle,
        BubbleWand,
        WardingRune,
        OrbInlayedGauntlets,
    }
}
