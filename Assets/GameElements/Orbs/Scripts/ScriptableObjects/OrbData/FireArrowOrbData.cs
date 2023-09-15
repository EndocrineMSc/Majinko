using UnityEngine;
using Characters;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/FireArrowOrbData")]
    public class FireArrowOrbData : OrbData
    {
        public override void OrbEffect()
        {
            if (_orbAttackPrefab != null && Player.Instance != null)
                _orbAttackPrefab.ShootAttack(Player.Instance.transform.position);
            else
                Debug.Log("OrbData: Fire Arrow couldn't be shot, either attack prefab or player is null");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Fire Arrow Orb</b><size=20%>\n\n<size=100%>Hitting this orb " +
                "will cast a <b>Fire Arrow</b> at the first enemy, dealing <b>" + _damage + " Damage</b> and applying " +
                "<b>" + _burningStacks + " Burning</b> to the target.";
        }
    }
}
