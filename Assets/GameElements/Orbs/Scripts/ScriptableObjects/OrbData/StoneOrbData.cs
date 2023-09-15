using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/StoneOrbData")]
    public class StoneOrbData : OrbData
    {
        [SerializeField] private ParticleSystem _crumbleSystem;
        private readonly string CRUMBLE_TRIGGER = "Crumble";

        public override void OrbEffect()
        {
            if (_parentOrb != null)
            {
                var animator = _parentOrb.GetComponent<Animator>();
                animator.SetTrigger(CRUMBLE_TRIGGER);
                //ToDo: Figure out particle system for OrbData, should play on collision 
            }    
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Stone Orb</b><size=20%>\n\n<size=100%>This orb has <b>Stalwart-1</b>. " +
                "It will endure an additional hit before popping.\n" +
                "Otherwise, it has no effects - it's a rock...";
        }
    }
}
