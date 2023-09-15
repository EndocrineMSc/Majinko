using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/HailStormOrbData")]
    public class HailStormOrbData : OrbData
    {
        private readonly string AOE_TARGET_TAG = "AOE_Target";

        public override void OrbEffect()
        {
            var aoeTargetObject = GameObject.FindGameObjectWithTag(AOE_TARGET_TAG);
            if (aoeTargetObject != null)
                _orbAttackPrefab.ShootAttack(aoeTargetObject.transform.position + new Vector3(0, 1, 0));
            else
                Debug.Log("OrbData: Hail Storm couldn't be shot, orb thinks aoe target is null.");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Hail Storm Orb</b><size=20%>\n\n<size=100%>Will cast a <b>Hail Storm</b>, " +
                "hitting all enemies for <b>" + _damage + " Damage</b> and applying <b>" + _freezingStacks + " Freezing</b>" +
                " to each enemy. Has a <b>" + _percentToFreeze + "% Chance</b> to also apply <b>Frozen</b>.";
        }
    }
}
