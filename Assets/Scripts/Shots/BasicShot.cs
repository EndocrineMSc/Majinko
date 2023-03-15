
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Shots
{
    internal class BasicShot : Shot
    {
        internal override void ShotStackEffect()
        {
            //do nothing
        }

        protected override void OnShootAdditions()
        {
            //Sound Effects go here
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A basic sphere of magic to gather mana and trigger spell orbs.";
        }
    }
}
