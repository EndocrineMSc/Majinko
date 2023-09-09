using Orbs;
using Characters;
using UnityEngine;

namespace Cards
{
    internal class RefreshOrbCard : OrbShifterCard
    {
        protected override void CardEffect()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 startPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbDataForSwitching);
            OrbManager.Instance.SwitchOrbs(OrbDataForSwitching, startPosition);
            Player.Instance.GetComponentInChildren<Animator>().SetTrigger("Attack");

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}