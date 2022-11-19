using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vampeggle;
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
        if (GameManager.Instance.GameState == State.Shooting)
        {
            if (!_ballActive)
            {
                _currentBall = Instantiate(_basicShot, new Vector3(0, 9), Quaternion.identity);
                _ballActive = true;
            }

            if (_currentBall.BallHasStopped)
            {
                Destroy(_currentBall.gameObject);
                _ballActive = false;
                StartCoroutine(GameManager.Instance.SwitchState(State.PlayerTurn));
            }
        }
    }
}
