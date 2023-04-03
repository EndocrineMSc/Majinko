using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace PeggleWars.Cards
{
    internal class CardEvents : MonoBehaviour
    {
        #region Fields and Properties

        internal static CardEvents Instance { get; private set; }

        public UnityEvent CardDestructionEvent;

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
