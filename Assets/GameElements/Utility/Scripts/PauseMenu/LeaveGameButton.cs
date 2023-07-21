using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class LeaveGameButton : MonoBehaviour
    {
        private GameObject _goToMenuWarning;

        private void Awake()
        {
            _goToMenuWarning = transform.GetChild(0).gameObject;
            GetComponent<Button>().onClick.AddListener(ActivateWarning);
        }

        private void ActivateWarning()
        {
            _goToMenuWarning.SetActive(true);
        }
    }
}