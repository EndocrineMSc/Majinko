using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace PeggleWars.Shots
{
    public class PowerShot : Shot
    {
        protected int _allowedPortalCollisions = 0;
        protected bool _isWithinPortal;

        protected override void Awake()
        {
            base.Awake();
            _gravity = 0.01f;
            _shotSpeed *= 3f;
        }

        public override void ShotStackEffect()
        {
            _allowedPortalCollisions++;
        }

        protected override void OnShootAdditions()
        {
            //ToDo: Implement sound and stuff later
            Debug.Log(_rigidbody.velocity);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains(PORTAL_PARAM))
            {
                if (_allowedPortalCollisions <= 0) 
                {
                    _destroyBall = true;
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
