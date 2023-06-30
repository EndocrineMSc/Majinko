using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    internal class StayButton : MonoBehaviour
    {
        private GameObject _devWarning;

        private void Awake()
        {
            _devWarning = transform.parent.gameObject;
            GetComponent<Button>().onClick.AddListener(DisableWarning);
        }

        private void DisableWarning()
        { 
            _devWarning.SetActive(false);
        }
    }
}
