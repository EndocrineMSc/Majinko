using Characters.Enemies;
using Orbs;
using UnityEngine;

namespace Characters
{
    public class PlayerBuffDebuffEffectHandler : MonoBehaviour
    {
        #region Fields and Properties

        public static PlayerBuffDebuffEffectHandler Instance { get; private set; }

        private int AmountOrbsHit = 0;
        private Vector3 _orbStartPosition;

        [SerializeField] private OrbData _manaBlitzData;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            OrbEvents.OnOrbHit += OrbWasHit;
            PlayerEvents.OnGainedShield += WardingRuneEffect;
        }

        private void OnDisable()
        {
            OrbEvents.OnOrbHit -= OrbWasHit;
            PlayerEvents.OnGainedShield -= WardingRuneEffect;
        }

        private void Start()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            _orbStartPosition = new(playerPosition.x + 2, playerPosition.y, playerPosition.z);
        }

        private void OrbWasHit(GameObject orb)
        {
            AmountOrbsHit++;
            CheckForBubbleWandDamage();
            GenerateGauntletBlock();
        }

        private void CheckForBubbleWandDamage()
        {
            if (AmountOrbsHit >= 20)
            {
                if (PlayerConditionTracker.HasBubbleWand && EnemyManager.Instance.EnemiesInScene.Count > 0)
                    EnemyManager.Instance.EnemiesInScene[0].TakeDamage(10, false);

                AmountOrbsHit = 0;
            }
        }

        private void GenerateGauntletBlock()
        {
            if (PlayerConditionTracker.HasOrbInlayedGauntlets)
                Player.Instance.GainShield(1, false);
        }

        private void WardingRuneEffect()
        {
            if (PlayerConditionTracker.HasWardingRune && OrbManager.Instance != null)
                OrbManager.Instance.SwitchOrbs(_manaBlitzData, _orbStartPosition);
            else if (OrbManager.Instance == null)
                Debug.Log("Couldn't switch in Mana Blitz Orb, Warding Rune Effect thinks OrbManager is null.");
        }



        #endregion
    }
}
