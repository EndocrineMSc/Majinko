using UnityEngine;
using UnityEngine.Events;

namespace Cards
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
