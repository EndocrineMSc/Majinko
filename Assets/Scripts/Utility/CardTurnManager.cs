using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWars.TurnManagement
{
    public class CardTurnManager : MonoBehaviour
    {
        public event Action EndCardTurn;
        public event Action StartCardTurn;

        public static CardTurnManager Instance { get; private set; }

        public void RaiseEndCardTurn()
        {
            EndCardTurn?.Invoke();
        }

        public void RaiseStartCardTurn()
        {
            StartCardTurn?.Invoke();
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
