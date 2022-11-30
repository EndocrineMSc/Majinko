using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars;
using EnumCollection;

public class ShotManager : MonoBehaviour
{

    #region Fields

    [SerializeField] private BasicShot _basicShot;
    private BasicShot _currentBall;
    private bool _ballActive;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == State.PlayerShooting)
        {
            if (!_ballActive)
            {
                _currentBall = Instantiate(_basicShot, new Vector3(1,3), Quaternion.identity);
                _ballActive = true;
            }

            if (_currentBall.DestroyBall)
            {
                Destroy(_currentBall.gameObject);
                _ballActive = false;
                StartCoroutine(GameManager.Instance.SwitchState(State.PlayerTurn));
            }
        }
    }
}
