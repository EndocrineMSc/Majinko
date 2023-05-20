using Orbs;
using EnumCollection;

namespace Cards
{
    internal class RefreshOrbCard : OrbShifterCard
    {
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.RefreshOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}