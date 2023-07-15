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
            if (ScriptableCard != null)
            {
                ScriptableSphereshifterCard sphereShifter = (ScriptableSphereshifterCard)ScriptableCard;
                SphereType = sphereShifter.SphereType;
            }
        }

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            if (SphereManager.Instance != null)
            {
                _shotManager = SphereManager.Instance;
                _sphere = _shotManager.AllSpheres[(int)SphereType];
            }
        }

        protected override void CardEffect()
        {
            Sphere _shotInScene = _shotManager.SphereToBeSpawned;

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
