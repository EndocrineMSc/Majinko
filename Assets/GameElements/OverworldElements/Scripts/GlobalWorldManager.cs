using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    internal class GlobalWorldManager : MonoBehaviour
    {
        #region Fields and Properties

        internal static GlobalWorldManager Instance { get; private set; }
        internal int WorldIndex { get; private set; } = 1;

        #endregion

        #region Functions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        internal void ChangeWorldIndex()
        {
            WorldIndex += 1;
        }

        #endregion
    }
}