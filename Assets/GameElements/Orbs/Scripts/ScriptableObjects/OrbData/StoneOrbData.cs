using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/StoneOrbData")]
    public class StoneOrbData : OrbData
    {
        [SerializeField] private ParticleSystem _crumbleSystem;
        private readonly string CRUMBLE_TRIGGER = "Crumble";

        public override void InitializeOrbData(Orb orb)
        {
            base.InitializeOrbData(orb);
            OrbEvents.OnOrbHit += OnOrbHit;
        }

        private void OnOrbHit(GameObject orb)
        {
            if (orb.GetComponent<Orb>().Data == this)
            {
                var animator = orb.GetComponent<Animator>();
                animator.SetTrigger(CRUMBLE_TRIGGER);
            }
        }

        public override void OrbEffect()
        {
            //ToDo: Figure out particle system for OrbData, should play on collision   
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Stone Orb</b><size=20%>\n\n<size=100%>This orb has <b>Stalwart-1</b>. " +
                "It will endure an additional hit before popping.\n" +
                "Otherwise, it has no effects - it's a rock...";
        }

        private void OnDisable()
        {
            OrbEvents.OnOrbHit -= OnOrbHit;
        }
    }
}
