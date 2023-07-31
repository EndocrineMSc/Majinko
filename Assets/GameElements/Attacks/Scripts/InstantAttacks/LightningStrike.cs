namespace Attacks
{
    internal class LightningStrike : InstantAttack
    {
        public override string Bark { get; } = "Lightning Strike!";

        protected override void AdditionalEffectsOnImpact()
        {
            //none
        }
    }
}
