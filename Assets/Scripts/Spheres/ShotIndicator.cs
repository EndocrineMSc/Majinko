using UnityEngine;


namespace PeggleWars.Spheres.Indicators
{
    /// <summary>
    /// This class mainly tracks how many collisions with a orb a ShotIndicator already had.
    /// </summary>
    public class ShotIndicator : MonoBehaviour
    {
        private SphereManager _shotManager;
        private int _currentNumberOfCollisions;

        //ignore collisions with shots and shotindicators
        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(16, 17);
            Physics2D.IgnoreLayerCollision(16, 16);
        }

        private void Start()
        {
            _shotManager = SphereManager.Instance;
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
