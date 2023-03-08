using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleWars.Orbs.OrbActions;

namespace PeggleWars.Orbs
{
    public interface IAmActionOrb
    {
        protected void AddOrbActionToPlayerTurn(Orb orb)
        {
            OrbActionManager.Instance.AddOrbToActionList(orb);
        }
    }
}
