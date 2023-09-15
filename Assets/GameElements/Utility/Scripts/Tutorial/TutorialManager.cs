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

        private float _startTimer;
        private bool _startStopDone;

        #endregion

        #region Functions

        private void Awake()
        {
            _startTimer = LoadHelper.LoadDuration * 1.01f;
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

        private void Update()
        {
            if (_startTimer > 0f && !_startStopDone)
            {
                _startTimer -= Time.deltaTime;
            }
            else if (!_startStopDone)
            {
                _startStopDone = true;
                Time.timeScale = 0f;
            }
        }

        #endregion
    }
}
