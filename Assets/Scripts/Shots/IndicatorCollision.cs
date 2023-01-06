using PeggleWars.Shots;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PeggleWar.Shots.Indicators
{
    public class IndicatorCollision : MonoBehaviour
    {
        private ShotManager _shotManager;
        private int _currentNumberOfCollisions;

        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(16, 17);
            Physics2D.IgnoreLayerCollision(16, 16);
        }

        private void Start()
        {
            _shotManager = ShotManager.Instance;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Orb"))
            {
                _currentNumberOfCollisions++;
            }

            if (_currentNumberOfCollisions >= _shotManager.MaxIndicatorCollisions)
            {
                Destroy(gameObject);
            }
        }
    }
}
