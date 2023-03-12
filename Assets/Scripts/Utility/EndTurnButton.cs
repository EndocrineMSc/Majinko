using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PeggleWars.Utilities
{
    internal class EndTurnButton : MonoBehaviour
    {
        private Button _endTurnButton;

        private void Awake()
        { 
            _endTurnButton = GetComponent<Button>();
        }

        private void Start()
        {
            _endTurnButton.onClick.AddListener(GameManager.Instance.EndCardTurnButton);
        }
    }
}
