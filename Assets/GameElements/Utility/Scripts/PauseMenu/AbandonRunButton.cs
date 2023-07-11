using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class AbandonRunButton : MonoBehaviour
    {
        private GameObject _warning;

        private void Awake()
        {
            _warning = transform.GetChild(0).gameObject;
            GetComponent<Button>().onClick.AddListener(ActivateWarning);
        }

        private void ActivateWarning()
        {
            _warning.SetActive(true);
        }
    }
}
