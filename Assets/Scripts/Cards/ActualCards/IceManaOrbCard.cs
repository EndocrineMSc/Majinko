using EnumCollection;
using PeggleWars.Orbs;

namespace PeggleWars.Cards
{
    internal class IceManaOrbCard : Card, IShiftOrbs
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.IceManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.IceManaOrb);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}
