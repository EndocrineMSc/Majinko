using PeggleWars.Shots;

namespace PeggleWars.Cards
{
    internal class DivinationCard : Card
    {
        private ShotManager _shotManager;

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = ShotManager.Instance;
        }

        protected override void CardEffect()
        {
            _shotManager.NumberOfIndicators += 2;
            _shotManager.MaxIndicatorCollisions++;
        }
    }
}
