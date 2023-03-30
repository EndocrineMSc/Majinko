
using PeggleWars.ScrollDisplay;

namespace PeggleWars.Spheres
{
    internal class BasicSphere : Sphere
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
