using UnityEngine;
using UnityEngine.Events;

namespace PeggleWars.ScrollDisplay
{
    internal class ScrollEvents : MonoBehaviour
    {
        #region Fields

        internal static ScrollEvents Instance;

        public UnityEvent<GameObject> ScrollDisplayEvent;
        public UnityEvent StopDisplayingEvent;

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
