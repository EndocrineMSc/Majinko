using PeggleWars.Orbs;

namespace Cards
{
    internal class PermanentExhaustOrbChangerCard : OrbChangerCard
    { 
        protected override void CardEffect()
        {
            GlobalOrbManager.Instance.AddGlobalOrb(GlobalOrbManager.Instance.AllOrbsList[(int)_orbType]);
            OrbManager.Instance.SwitchOrbs(_orbType, transform.position);

            _globalDeckManager.RemoveCardFromGlobalDeck(GlobalCardManager.Instance.AllCards[(int)_cardType]);
        }
    }
}