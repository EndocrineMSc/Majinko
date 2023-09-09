using Characters.Enemies;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/IntangibleEnemyOrbData")]
    public class IntangibleEnemyOrbData : OrbData
    {
        public override void OrbEffect()
        {
            EnemyEvents.RaiseIntangibleTriggered();
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Intangible Orb</b><size=20%>\n\n<size=100%>When hit, this orb will apply " +
                "<b>1 Intangible</b> to every enemy that has the ability to turn intagible. " +
                "This prevents them from being hit by most attacks.";
        }
    }
}
