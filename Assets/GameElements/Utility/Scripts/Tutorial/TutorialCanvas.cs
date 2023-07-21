using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    internal class TutorialCanvas : MonoBehaviour
    {
        private void OnEnable()
        {
            TutorialEvents.OnTutorialFinished += OnTutorialFinished;
        }

        private void OnDisable()
        {
            TutorialEvents.OnTutorialFinished -= OnTutorialFinished;
        }

        private void OnTutorialFinished()
        {
            GetComponent<Canvas>().enabled = false;
        }
    }
}
