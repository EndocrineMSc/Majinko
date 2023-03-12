using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.Orbs
{
    internal class OrbEvents : MonoBehaviour
    {
        #region Fields

        internal static OrbEvents Instance { get; private set; }

        public UnityEvent OrbEffectEnd;

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
