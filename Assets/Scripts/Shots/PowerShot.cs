using System.Collections;
using UnityEngine;

namespace PeggleWars.Shots
{
    internal class PowerShot : Shot
    {
        protected int _allowedPortalCollisions = 0;
        protected bool _isWithinPortal;

        protected override void Awake()
        {
            base.Awake();
            _gravity = 0.01f;
            _shotSpeed *= 3f;
        }

        internal override void ShotStackEffect()
        {
            _allowedPortalCollisions++;
        }

        protected override void OnShootAdditions()
        {
            //ToDo: Implement sound and stuff later
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains(PORTAL_PARAM))
            {
                if (_allowedPortalCollisions <= 0) 
                {
                    StartCoroutine(GameManager.Instance.SwitchState(EnumCollection.GameState.PlayerActions));
                    Destroy(gameObject);
                }
                else
                {
                    if (!_isWithinPortal)
                    {
                        StartCoroutine(PortalCollisionDecentCount());
                    }
                }
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            _rigidbody.velocity = _rigidbody.velocity.normalized * _shotSpeed;
        }

        protected IEnumerator PortalCollisionDecentCount()
        {
            _isWithinPortal = true;
            yield return new WaitForSeconds(0.2f);
            _allowedPortalCollisions--;
            _isWithinPortal = false;
        }
    }
}
