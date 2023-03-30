using UnityEngine;
using EnumCollection;
using PeggleWars.Spheres;

namespace PeggleWars.Cards
{
    internal class ShotChangerCard : Card
    {
        #region Fields and Properties

        [SerializeField] private ShotType _shotType;
        private SphereManager _shotManager;
        private Sphere _shot;

        #endregion

        #region Functions

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = SphereManager.Instance;
            _shot = _shotManager.AllShots[(int)_shotType];
        }

        protected override void CardEffect()
        {
            Sphere _shotInScene = _shotManager.ShotToBeSpawned;

            if (_shotInScene == _shot) //maybe won't work as intended, check first for bugfixes
            {          
                ShotEvents.Instance.ShotStackedEvent?.Invoke();
            }
            else
            {
                _shotManager.SetShotToBeSpawned(_shot);
            }
        }

        #endregion
    }
}
