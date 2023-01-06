using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using PeggleWars.Shots;

namespace Cards
{
    public class DivinationCard : Card
    {
        private ShotManager _shotManager;

        protected override void Start()
        {
            base.Start();
            _shotManager = ShotManager.Instance;
        }

        protected override void CardEffect()
        {
            base.CardEffect();
            _shotManager.NumberOfIndicators += 2;
            _shotManager.MaxIndicatorCollisions++;
        }
    }
}
