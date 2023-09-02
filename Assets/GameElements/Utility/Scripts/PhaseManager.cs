using Audio;
using PeggleWars;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Utility.TurnManagement
{
    public class PhaseManager : MonoBehaviour
    {
        #region Fields

        public static PhaseManager Instance { get; private set; }
        public Phase CurrentPhase { get; private set; }

        
        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void StartCardPhase()
        {
            CurrentPhase = Phase.CardPhase;
            LevelPhaseEvents.RaiseStartCardPhase();
        }

        public void StartShootingPhase()
        {
            CurrentPhase = Phase.Shooting;
            LevelPhaseEvents.RaiseStartShootingPhase();
        }

        public void StartPlayerAttackPhase()
        {
            CurrentPhase = Phase.PlayerActions;
            LevelPhaseEvents.RaiseStartPlayerAttackPhase();
        }

        public void StartEnemyPhase()
        {
            CurrentPhase = Phase.EnemyTurn;
            LevelPhaseEvents.RaiseStartEnemyPhase();
        }

        public void EndEnemyPhase()
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

    public enum Phase
    {
        CardPhase,
        Shooting,
        PlayerActions,
        EnemyTurn,
        CardShop,
    }
}
