using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.Spheres
{
    internal class ShotEvents : MonoBehaviour
    {
        #region Fields

        internal static ShotEvents Instance { get; private set; }

        public UnityEvent ShotStackedEvent;
        public UnityEvent ShotDestructionEvent;

        #endregion

        #region Functions

        private void Awake()
        {
            if(Instance != null && Instance != this)
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
