using Audio;
using PeggleWars;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Utility.TurnManagement
{
    internal class PhaseManager : MonoBehaviour
    {
        #region Fields

        internal static PhaseManager Instance { get; private set; }

        internal Phase CurrentPhase { get; private set; }
        
        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        internal void StartCardPhase()
        {
            CurrentPhase = Phase.CardPhase;
            LevelPhaseEvents.RaiseStartCardPhase();
        }

        internal void StartShootingPhase()
        {
            CurrentPhase = Phase.Shooting;
            LevelPhaseEvents.RaiseStartShootingPhase();
        }

        internal void StartPlayerAttackPhase()
        {
            CurrentPhase = Phase.PlayerActions;
            LevelPhaseEvents.RaiseStartPlayerAttackPhase();
        }

        internal void StartEnemyPhase()
        {
            CurrentPhase = Phase.EnemyTurn;
            LevelPhaseEvents.RaiseStartEnemyPhase();
        }

        internal void EndEnemyPhase()
        {
            CurrentPhase = Phase.EnemyTurn;
            LevelPhaseEvents.RaiseEndEnemyPhase();
        }

        public void EndCardTurnButton()
        {
            AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);

            if (CurrentPhase == Phase.CardPhase)
            {
                LevelPhaseEvents.RaiseEndCardPhase();
                StartShootingPhase();
            }
        }

        #endregion
    }

    internal enum Phase
    {
        CardPhase,
        Shooting,
        PlayerActions,
        EnemyTurn,
        CardShop,
    }
}
