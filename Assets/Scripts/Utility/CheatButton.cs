using PeggleWars.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheatButton : MonoBehaviour
{
    // Start is called before the first frame update
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
        WinLoseConditionManager.Instance.LevelVictory?.Invoke();
    }
}
