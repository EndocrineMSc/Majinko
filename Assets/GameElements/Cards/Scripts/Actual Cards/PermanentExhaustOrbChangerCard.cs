using Characters;
using Orbs;
using UnityEngine;

namespace Cards
{
    public class PermanentExhaustOrbChangerCard : OrbShifterCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbDataForSwitching);

            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 startPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
            OrbManager.Instance.SwitchOrbs(OrbDataForSwitching, startPosition);
            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}