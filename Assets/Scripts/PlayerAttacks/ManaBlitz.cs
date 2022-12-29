using EnumCollection;
using PeggleWars.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeggleAttacks.Player.ManaBlitz
{
    public class ManaBlitz : PlayerAttack
    {
        //ToDo: Do special stuff in here
        private void Awake()
        {
            AudioManager.Instance.PlaySoundEffectWithoutLimit(SFX.SFX_0003_ManaBlitz);
        }
    }
}
