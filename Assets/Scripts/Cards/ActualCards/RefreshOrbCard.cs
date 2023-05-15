using PeggleWars.Orbs;
using EnumCollection;

namespace Cards
{
    internal class RefreshOrbCard : Card
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.RefreshOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.RefreshOrb, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}