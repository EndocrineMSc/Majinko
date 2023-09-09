using Characters;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/IcicleOrbData")]
    public class IcicleOrbData : OrbData
    {
        public override void OrbEffect()
        {
            if (_orbAttackPrefab != null && Player.Instance != null)
                _orbAttackPrefab.ShootAttack(Player.Instance.transform.position);
            else
                Debug.Log("OrbData: Icicle couldn't be shot, either attack prefab or player is null");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Icicle Orb</b><size=20%>\n\n<size=100%>Hitting this orb " +
                "will cast a <b>Icicle</b> at the first enemy, dealing <b>" + _damage + " Damage</b> and applying " +
                "<b>" + _freezingStacks + " Freezing</b> to the target. Has a <b>" + _percentToFreeze + "% Chance</b>" +
                "to also apply <b>Frozen</b>.";
        }
    }
}
