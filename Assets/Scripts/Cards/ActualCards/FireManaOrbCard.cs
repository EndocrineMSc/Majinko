using PeggleWars.Orbs;
using EnumCollection;

namespace Cards
{
    internal class FireManaOrbCard : Card, IShiftOrbs
    {
        public int AmountOrbs { get; set; } = 1;

        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType.FireManaOrb]);
            OrbManager.Instance.SwitchOrbs(OrbType.FireManaOrb, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}