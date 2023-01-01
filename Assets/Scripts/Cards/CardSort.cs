using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardSort : MonoBehaviour
{

    #region Private Functions

    private void Awake()
    {
        Canvas _cardCanvas = GetComponentInParent<Canvas>();
        RectTransform[] _cardsInHand = _cardCanvas.GetComponentsInChildren<RectTransform>();
        int presentCards = _cardsInHand.Length;
        transform.position = new Vector3(550 + (presentCards * 10), 50, 0);
    }

    #endregion
}
