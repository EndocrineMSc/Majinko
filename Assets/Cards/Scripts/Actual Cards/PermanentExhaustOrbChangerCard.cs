using PeggleWars.Orbs;

namespace Cards
{
    internal class PermanentExhaustOrbChangerCard : OrbShifterCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)OrbType]);
            OrbManager.Instance.SwitchOrbs(OrbType, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}