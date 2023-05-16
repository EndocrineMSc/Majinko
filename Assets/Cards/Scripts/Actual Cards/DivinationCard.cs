using PeggleWars.Spheres;

namespace Cards
{
    internal class DivinationCard : Card
    {
        private SphereManager _sphereManager;

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _sphereManager = SphereManager.Instance;
        }

        protected override void CardEffect()
        {
            _sphereManager.NumberOfIndicators += 2;
            _sphereManager.MaxIndicatorCollisions++;
        }
    }
}
