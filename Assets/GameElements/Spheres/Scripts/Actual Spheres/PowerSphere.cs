using Utility;
using System.Collections;
using UnityEngine;
using Utility.TurnManagement;

namespace Spheres
{
    internal class PowerSphere : Sphere
    {
        protected int _allowedPortalCollisions = 0;
        protected bool _isWithinPortal;

        protected override void Awake()
        {
            base.Awake();
            _gravity = 0.01f;
            _shotSpeed *= 2f;
        }

        internal override void SphereStackEffect()
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
                    PhaseManager.Instance.StartPlayerAttackPhase();
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

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "A powerful magic sphere that is almost unaffected by gravity. It won't lose speed until hitting the portal. " +
                "Will ignore the Portal for each additional time a Power Sphere card is played.";
        }
    }
}
