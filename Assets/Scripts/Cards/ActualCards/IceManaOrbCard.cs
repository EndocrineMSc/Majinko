using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards
{
    internal class IceManaOrbCard : Card
    {
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.IceManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.IceManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(_globalDeckManager.AllCards[(int)_cardType]);
        }
    }
}
