using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utility
{
    internal class TutorialManager : MonoBehaviour
    {
        #region Fields and Properties

        internal int TotalTutorialActions { get; private set; } = 3;
        internal int CurrentActionIndex { get; private set; } = 0;

        #endregion

        #region Functions

        private void Awake()
        {
            Time.timeScale = 0f;
        }

        private void OnEnable()
        {
            TutorialEvents.OnCurrentActionCompleted += OnCurrentActionCompleted;
        }

        private void OnDisable()
        {
            TutorialEvents.OnCurrentActionCompleted -= OnCurrentActionCompleted;
        }

        private void OnCurrentActionCompleted(int index)
        {
            if (index < TotalTutorialActions)
                CurrentActionIndex = index;
            else
                TutorialEvents.RaiseTutorialFinished();
        }

        #endregion
    }
}
