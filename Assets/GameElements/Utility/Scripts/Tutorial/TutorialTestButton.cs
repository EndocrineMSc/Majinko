using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class TutorialTestButton : MonoBehaviour
    {
        private int _index = 0;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(EventRaiseWrap);
        }

        private void EventRaiseWrap()
        {
            _index++;
            TutorialEvents.RaiseCurrentActionCompleted(_index);
        }
    }
}
