using PeggleWars.Spheres;

namespace PeggleWars.Cards
{
    internal class DivinationCard : Card
    {
        private SphereManager _shotManager;

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = SphereManager.Instance;
        }

        protected override void CardEffect()
        {
            _shotManager.NumberOfIndicators += 2;
            _shotManager.MaxIndicatorCollisions++;
        }
    }
}
