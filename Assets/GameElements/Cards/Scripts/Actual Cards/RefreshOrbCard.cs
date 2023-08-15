using Orbs;
using EnumCollection;
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
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbType.RefreshOrb);
            StartCoroutine(OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb, startPosition));
            Player.Instance.GetComponentInChildren<Animator>().SetTrigger("Attack");

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}