using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PeggleWars.Shots;

namespace PeggleWars.Cards.ShotChangers
{
    public class ShotChangerCard : Card
    {
        #region Fields and Properties

        [SerializeField] private ShotType _shotType;
        private ShotManager _shotManager;
        private Shot _shot;

        #endregion

        #region Functions

        protected override void SetReferencesToLevelComponents()
        {
            base.SetReferencesToLevelComponents();
            _shotManager = ShotManager.Instance;
            _shot = _shotManager.AllShots[(int)_shotType];
        }

        protected override void CardEffect()
        {
            Shot _shotInScene = _shotManager.ShotToBeSpawned;

            if (_shotInScene == _shot) //maybe won't work as intended, check first for bugfixes
            {          
                _shotManager.OnShotStacked?.Invoke();
            }
            else
            {
                _shotManager.SetShotToBeSpawned(_shot);
            }
        }
        #endregion
    }
}
