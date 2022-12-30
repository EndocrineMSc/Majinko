using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PeggleOrbs;
using EnumCollection;
using PeggleWars.Audio;

namespace Enemies.Zombies
{
    public class Zombie : Enemy
    {

        #region Public Functions

        protected override void HandleDeath()
        {
            base.HandleDeath();
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX.SFX_0011_ZombieDeath);
            OrbManager.Instance.SwitchOrbs(OrbType.RottedOrb, 2);
        }

        protected override void PlaySpawnSound()
        {
            AudioManager.Instance.PlaySoundEffect(SFX.SFX_0009_ZombieSpawn);
        }

        #endregion
    }
}
