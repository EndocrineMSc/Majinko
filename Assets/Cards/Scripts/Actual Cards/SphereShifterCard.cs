using UnityEngine;
using EnumCollection;
using PeggleWars.Spheres;

namespace Cards
{
    internal class SphereShifterCard : Card
    {
        #region Fields and Properties

        internal SphereType SphereType { get; private set; }
        private SphereManager _shotManager;
        private Sphere _sphere;

        #endregion

        #region Functions

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = SphereManager.Instance;
        }

        protected override void SetCardFields()
        {
            base.SetCardFields();
            ScriptableSphereshifterCard sphereShifter = (ScriptableSphereshifterCard)_scriptableCard;
            SphereType = sphereShifter.SphereType;
            _sphere = _shotManager.AllShots[(int)SphereType];
        }

        protected override void CardEffect()
        {
            Sphere _shotInScene = _shotManager.ShotToBeSpawned;

            if (_shotInScene == _sphere) //maybe won't work as intended, check first for bugfixes
            {          
                ShotEvents.Instance.ShotStackedEvent?.Invoke();
            }
            else
            {
                _shotManager.SetShotToBeSpawned(_sphere);
            }
        }

        #endregion
    }
}
