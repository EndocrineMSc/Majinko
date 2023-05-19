using UnityEngine;
using UnityEngine.UI;

namespace Utility.TurnManagement
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
            _endTurnButton.onClick.AddListener(PhaseManager.Instance.EndCardTurnButton);
        }
    }
}
