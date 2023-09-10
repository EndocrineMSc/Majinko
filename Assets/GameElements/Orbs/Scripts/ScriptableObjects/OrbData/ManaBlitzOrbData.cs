using Characters;
using UnityEngine;

namespace Orbs
{
    [CreateAssetMenu(menuName = "Orbs/OrbData/ManaBlitzOrbData")]
    public class ManaBlitzOrbData : OrbData
    {
        public override void OrbEffect()
        {
            if (_orbAttackPrefab != null && Player.Instance != null)
                _orbAttackPrefab.ShootAttack(Player.Instance.transform.position);
            else
                Debug.Log("OrbData: Mana Blitz couldn't be shot, either attack prefab or player is null");
        }

        protected override void SetDescription()
        {
            OrbDescription = "<size=120%><b>Mana Blitz Orb</b><size=20%>\n\n<size=100%>When hit, will cast " +
                "a standard <b>Mana Blitz</b> at the first enemy, dealing <b>" + _damage + "Damage</b>.\n" +
                "One of the earliest spells taught to young witches.";
        }
    }
}
