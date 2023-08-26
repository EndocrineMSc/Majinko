using Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    internal class BuffCard : Card
    {
        [SerializeField] CardBuff _buff;

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
        ShieldBeetle,
        BubbleWand,
        WardingRune,
        OrbInlayedGauntlets,
    }
}
