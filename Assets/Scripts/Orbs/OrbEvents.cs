using PeggleWars.ManaManagement;
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
        public ManaSpawnEvent ManaSpawnTrigger;

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
            ManaSpawnTrigger ??= new ManaSpawnEvent();
        }

        #endregion
    }
}
