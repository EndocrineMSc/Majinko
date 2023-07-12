using Orbs;
using EnumCollection;

namespace Cards
{
    internal class RefreshOrbCard : OrbShifterCard
    {
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbType.RefreshOrb);
            OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}