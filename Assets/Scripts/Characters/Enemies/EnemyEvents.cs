using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace PeggleWars.Enemies
{
    internal class EnemyEvents : MonoBehaviour
    {
        #region Fields and Properties

        internal static EnemyEvents Instance { get; private set; }
        
        public UnityEvent EnemyDeathEvent;
        public UnityEvent EnemyMoveEndEvent;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion
    }
}
