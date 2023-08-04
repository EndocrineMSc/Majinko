using UnityEngine;
using Orbs;

namespace Spheres
{
    /// <summary>
    /// This class mainly tracks how many collisions with a orb a SphereIndicator already had.
    /// </summary>
    internal class SphereIndicator : MonoBehaviour
    {
        private SphereManager _sphereManager;
        private int _currentNumberOfCollisions;

        //ignore collisions with shots and shotindicators
        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(16, 17);
            Physics2D.IgnoreLayerCollision(16, 16);
        }

        private void Start()
        {
            _sphereManager = SphereManager.Instance;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Orb>(out _))
            {
                _currentNumberOfCollisions++;
            }

            if (_currentNumberOfCollisions >= _sphereManager.MaxIndicatorCollisions)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains("Portal"))
                Destroy(gameObject);

            //ToDo: Disable for stacked power sphere
        }
    }
}
