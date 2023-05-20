using EnumCollection;
using PeggleWars.Spheres;
using Spheres;

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
        protected override void SetCardFields()
        {
            base.SetCardFields();
            ScriptableSphereshifterCard sphereShifter = (ScriptableSphereshifterCard)ScriptableCard;
            SphereType = sphereShifter.SphereType;
        }

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = SphereManager.Instance;
            _sphere = _shotManager.AllShots[(int)SphereType];
        }

        protected override void CardEffect()
        {
            Sphere _shotInScene = _shotManager.ShotToBeSpawned;

            if (_shotInScene == _sphere) //maybe won't work as intended, check first for bugfixes
            {
                SphereEvents.RaiseSphereStacked();
            }
            else
            {
                _shotManager.SetSphereToBeSpawned(_sphere);
            }
        }

        #endregion
    }
}
