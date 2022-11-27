using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using PeggleWars;

public class FlushController : MonoBehaviour
{

    //May I present to you: How to kill your performance easily

    #region Fields

    private GameObject _cork;
    private List<GameObject> _activeLiquidBlood;
    private List<GameObject> _corkFinder;

    #endregion

    #region Private Functions

    private void Awake()
    {
        _corkFinder = new List<GameObject>();

        foreach (GameObject _gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!EditorUtility.IsPersistent(_gameObject.transform.root.gameObject) && !(_gameObject.hideFlags == HideFlags.NotEditable || _gameObject.hideFlags == HideFlags.HideAndDontSave))
            {
                _corkFinder.Add(_gameObject);
            }
        }

        foreach (GameObject _gameObject in _corkFinder)
        {
            if (_gameObject.name == "Cork")
            {
                _cork = _gameObject;
            }
        }
    }

    public IEnumerator Flush()
    {
        _cork.SetActive(false);

        yield return new WaitForSeconds(5f);

        _cork.SetActive(true);
    }
    #endregion

}
