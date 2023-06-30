using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class LeaveGameButton : MonoBehaviour
    {
        private GameObject _devVersionWarning;

        private void Awake()
        {
            _devVersionWarning = transform.GetChild(0).gameObject;
            GetComponent<Button>().onClick.AddListener(ActivateWarning);
        }

        private void ActivateWarning()
        {
            _devVersionWarning.SetActive(true);
        }
    }
}