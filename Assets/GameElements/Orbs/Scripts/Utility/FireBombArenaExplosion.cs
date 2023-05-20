using PeggleWars.Spheres;
using System.Collections;
using UnityEngine;


namespace PeggleWars.Orbs
{
    internal class FireBombArenaExplosion : MonoBehaviour, IAmSphere
    {
        private Collider2D _collider;
        private float _colliderMaxRadius;
        private readonly float _expansionSpeed = 10f;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _colliderMaxRadius = _collider.bounds.extents.x * 8;
            _collider.enabled = false;
        }

        private void OnEnable()
        {
            StartCoroutine(ExpandCollider());
        }

        private IEnumerator ExpandCollider()
        {
            _collider.enabled = true;
            float currentRadius = _collider.bounds.extents.x;

            while (currentRadius < _colliderMaxRadius)
            {
                currentRadius = Mathf.Lerp(currentRadius, _colliderMaxRadius, Time.deltaTime * _expansionSpeed);
                _collider.bounds.Expand(currentRadius);
                yield return null;
            }

            _collider.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
