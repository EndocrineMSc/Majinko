using System.Collections;
using UnityEngine;
using Orbs;
using PeggleWars.ScrollDisplay;
using Utility.TurnManagement;
using PeggleWars.Spheres;

namespace Spheres
{
    internal class CloneSphere : Sphere
    {
        #region Fields and Properties

        internal bool CannotSpawnMoreSpheres { get; set; }
        internal int ExtraSpheres { get; set; } = 2;
        internal int BallsInScene { get; set; } = 3;
        
        private bool _hasHitPortal;

        #endregion

        #region Functions

        protected override void OnEnable()
        {
            base.OnEnable();
            SphereEvents.OnSphereDestruction += OnSphereDestruction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SphereEvents.OnSphereDestruction -= OnSphereDestruction;
        }

        protected void OnSphereDestruction()
        {
            BallsInScene--;

            if (_hasHitPortal)
            {
                if (BallsInScene <= 0)
                {
                    PhaseManager.Instance.StartPlayerAttackPhase();
                }
                Destroy(gameObject);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Contains(PORTAL_PARAM) && !_hasHitPortal)
            {
                _hasHitPortal = true;
                SphereEvents.RaiseSphereDestruction();
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (collision.gameObject.TryGetComponent<Orb>(out _) && !CannotSpawnMoreSpheres)
            {
                SplitSphere(ExtraSpheres, collision.transform);
            }     
        }

        private void SplitSphere(int amountOfExtraSpheres, Transform orbTransform)
        {
            float orbX = orbTransform.position.x + 1;
            float calculateAngleY = orbTransform.position.y + 1;
            CannotSpawnMoreSpheres = true;

            for (int i = 1; i < amountOfExtraSpheres + 1; i++) 
            {
                Vector2 sphereDestination = new(orbX, calculateAngleY * i);
                Vector2 orbPosition = orbTransform.position;
                Vector2 direction = sphereDestination - orbPosition;
                direction = direction.normalized;         

                CloneSphere cloneSphere = Instantiate(this, new Vector2(orbTransform.position.x, orbTransform.position.y), Quaternion.identity);
                cloneSphere.CannotSpawnMoreSpheres = true;
                cloneSphere.BallsInScene = BallsInScene;
                cloneSphere.SetShotAsShotAlready();
                StartCoroutine(DisableAndResetCollider(cloneSphere));

                Rigidbody2D rigidbody = cloneSphere.GetComponent<Rigidbody2D>();
                rigidbody.gravityScale = _gravity;
                rigidbody.velocity = direction * _shotSpeed;               
            }
        }

        private IEnumerator DisableAndResetCollider(CloneSphere tempShot)
        {
            Collider2D collider = tempShot.GetComponent<Collider2D>();
            collider.enabled = false;

            yield return new WaitForSeconds(0.1f);
            collider.enabled = true;
        }

        internal override void SphereStackEffect()
        {
            ExtraSpheres++;
            BallsInScene++;
        }

        protected override void OnShootAdditions()
        {
            //ToDo Sound and other polish
        }

        public override void SetDisplayDescription()
        {
            IDisplayOnScroll displayOnScroll = GetComponent<IDisplayOnScroll>();
            displayOnScroll.DisplayDescription = "This sphere will multiply on first contact with an orb. " +
                "For each additional Multi Sphere card played, an additional sphere will be created on contact.";
        }
        #endregion

    }
}
