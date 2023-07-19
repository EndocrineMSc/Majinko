using Utility;
using UnityEngine;
using UnityEngine.UI;

internal class CheatButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CheatButtonClick); 
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(CheatButtonClick);
    }

    void CheatButtonClick()
    {
        UtilityEvents.RaiseLevelVictory();
    }
}
