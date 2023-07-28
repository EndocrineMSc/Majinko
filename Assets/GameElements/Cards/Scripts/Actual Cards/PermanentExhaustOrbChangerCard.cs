using Characters;
using Orbs;
using UnityEngine;

namespace Cards
{
    internal class PermanentExhaustOrbChangerCard : OrbShifterCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbType);

            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 startPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
            OrbManager.Instance.SwitchOrbs(OrbType, startPosition);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}