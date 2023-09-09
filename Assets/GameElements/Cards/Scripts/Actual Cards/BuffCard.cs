using Characters;

namespace Cards
{
    public class BuffCard : Card
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

    public enum CardBuff
    {
        None,
        ShieldBeetle,
        BubbleWand,
        WardingRune,
        OrbInlayedGauntlets,
    }
}
