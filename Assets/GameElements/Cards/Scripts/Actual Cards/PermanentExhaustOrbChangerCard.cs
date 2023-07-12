using Orbs;

namespace Cards
{
    internal class PermanentExhaustOrbChangerCard : OrbShifterCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddLevelLoadOrb(OrbType);
            OrbManager.Instance.SwitchOrbs(OrbType, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)CardType]);
        }
    }
}