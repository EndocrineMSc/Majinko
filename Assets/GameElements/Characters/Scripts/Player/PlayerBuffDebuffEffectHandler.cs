using Characters.Enemies;
using Orbs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace Characters
{
    internal class PlayerBuffDebuffEffectHandler : MonoBehaviour
    {
        #region Fields and Properties

        internal static PlayerBuffDebuffEffectHandler Instance { get; private set; }

        private int AmountOrbsHit = 0;
        private Vector3 _orbStartPosition;



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

        private void OrbWasHit()
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
            if (PlayerConditionTracker.HasWardingRune)
                StartCoroutine(OrbManager.Instance.SwitchOrbs(OrbType.ManaBlitzOrb, _orbStartPosition));
        }



        #endregion
    }
}
