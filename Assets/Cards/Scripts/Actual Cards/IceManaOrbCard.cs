using EnumCollection;
using PeggleWars.Orbs;

namespace Cards
{
    internal class IceManaOrbCard : Card
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.IceManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.IceManaOrb, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}