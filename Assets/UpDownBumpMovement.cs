using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleWars.Orbs
{
    internal class UpDownBumpMovement : MonoBehaviour
    {
        private float _moveSpeed = 0.003f;
        private float _moveSeconds = 0.5f;
        private float _currentTime = 0;
        private bool _isMovingUp = true;
        private bool _awakeOffsetDone;

        private void Start()
        {
            StartCoroutine(RandomTimeOffset());
        }

        private void Update()
        {
            if (_awakeOffsetDone)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime >= _moveSeconds)
                {
                    _currentTime = 0;
                    if (_isMovingUp)
                    {
                        _isMovingUp = false;
                    }
                    else
                    {
                        _isMovingUp = true;
                    }
                }

                if (_isMovingUp)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + _moveSpeed);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - _moveSpeed);
                }
            }
        }

        private IEnumerator RandomTimeOffset()
        {
            System.Random random = new();
            float randomTime = (float)random.NextDouble() * 2;

            yield return new WaitForSeconds(randomTime);
            _awakeOffsetDone = true;
        }
    }
}
