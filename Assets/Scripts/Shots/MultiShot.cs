using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs;

namespace PeggleWars.Shots
{
    public class MultiShot : Shot
    {
        #region Fields

        private bool _cannotSpawnMoreShots;

        public bool CannotSpawnMoreShots
        {
            get { return _cannotSpawnMoreShots; }
            set { _cannotSpawnMoreShots = value; }
        }


        protected int _amountOfExtraBalls = 2;

        public int ExtraBalls
        {
            get { return _amountOfExtraBalls; }
            set { _amountOfExtraBalls = value; }
        }

        #endregion

        #region Functions

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (collision.gameObject.TryGetComponent(out Orb orb) && !_cannotSpawnMoreShots)
            {
                SplitBall(_amountOfExtraBalls, collision.transform);
            }     
        }

        private void SplitBall(int amountOfExtraOrbs, Transform peggleTransform)
        {
            float peggleX = peggleTransform.position.x + 1;
            float calculateAngleY = peggleTransform.position.y + 1;
            _cannotSpawnMoreShots = true;

            for (int i = 1; i < amountOfExtraOrbs + 1; i++) 
            {
                Vector2 shotDestination = new(peggleX, calculateAngleY * i);
                Vector2 pegglePosition = peggleTransform.position;
                Vector2 direction = shotDestination - pegglePosition;
                direction = direction.normalized;         

                MultiShot tempShot = Instantiate(this, new Vector2(peggleTransform.position.x, peggleTransform.position.y), Quaternion.identity);
                tempShot.CannotSpawnMoreShots = true;
                tempShot.SetShotAsShotAlready();
                StartCoroutine(DisableAndResetCollider(tempShot));

                Rigidbody2D rigidbody = tempShot.GetComponent<Rigidbody2D>();
                rigidbody.gravityScale = _gravity;
                rigidbody.velocity = direction * _shotSpeed;               
            }
        }

        private IEnumerator DisableAndResetCollider(MultiShot tempShot)
        {
            Collider2D collider = tempShot.GetComponent<Collider2D>();
            collider.enabled = false;

            yield return new WaitForSeconds(0.1f);
            collider.enabled = true;
        }

        protected override void OnShootAdditions()
        {
           //not implemented yet
        }

        public override void ShotStackEffect()
        {
            _amountOfExtraBalls++;
        }

        #endregion

    }
}
