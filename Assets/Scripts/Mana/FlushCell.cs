using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vampeggle;

public class FlushCell : MonoBehaviour
{ 
    private FlushController _flushController;

    private void Awake()
    {
        _flushController = GetComponent<FlushController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Shot"))
        {
            StartCoroutine(_flushController.Flush());
        }
    }

}
