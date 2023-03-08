using System;
using UnityEngine;


namespace PeggleWars.TurnManagement
{
    public class TurnManager : MonoBehaviour
    {
        public event Action StartCardTurn;
        public event Action EndCardTurn;
        public event Action StartEnemyTurn;
        public event Action EndEnemyTurn;

        public static TurnManager Instance { get; private set; }

        public void RaiseStartCardTurn()
        {
            StartCardTurn?.Invoke();
        }

        public void RaiseEndCardTurn()
        {
            EndCardTurn?.Invoke();
        }

        public void RaiseStartEnemyTurn()
        {
            StartEnemyTurn?.Invoke();
        }

        public void RaiseEndEnemyTurn()
        {
            EndEnemyTurn?.Invoke();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
