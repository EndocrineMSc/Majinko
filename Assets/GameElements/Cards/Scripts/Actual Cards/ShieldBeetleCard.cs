using Cards;
using Characters;
using UnityEngine;

internal class ShieldBeetleCard : Card
{
    protected override void CardEffect()
    {
        PlayerConditionTracker.ActivateShieldBeetle();
    }
}
