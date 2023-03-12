using System;
using UnityEngine;
using UnityEngine.Events;


namespace PeggleWars.TurnManagement
{
    internal class TurnManager : MonoBehaviour
    {
        #region Fields

        public UnityEvent StartCardTurn;
        public UnityEvent EndCardTurn;
        public UnityEvent StartEnemyTurn;
        public UnityEvent EndEnemyTurn;
        public UnityEvent StartPlayerAttackTurn;

        internal static TurnManager Instance { get; private set; }

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        #endregion
    }
}
