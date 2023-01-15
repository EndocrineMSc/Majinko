using PeggleWars.Shots;

namespace PeggleWars.Cards.InstantEffects
{
    /// <summary>
    /// Child class to cards, has an instant effect on the game.
    /// Increases the range for shot prediction for the player.
    /// </summary>
    public class DivinationCard : Card
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
