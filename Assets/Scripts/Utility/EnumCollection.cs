using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumCollection
{
    public enum SpawnNumber
    {
        One,
        Two,
        Three,
        Four,
    }

    public enum State
    {
        MainMenu,
        PlayerShooting,
        PlayerTurn,
        MonsterTurn,
        GameOver,
        Quit,
    }

    public enum EnemyAttackType
    {
        Melee,
        Distance,
    }

    public enum EnemyType
    {
        CloakedZombie,
    }

    public enum ManaType
    {
        BaseMana,
        FireMana,
        IceMana,
        LightningMana,
        DarkMana,
        LightMana,
    }

    public enum OrbType
    {
        BaseOrb,            //gives basic Mana
        FireOrb,            //gives fire Mana
        IceOrb,             //gives ice Mana
        LightningOrb,       //gives lightning Mana
        DarkOrb,            //gives dark Mana
        LightOrb,           //gives light Mana
        MultiplierOrb,      //multiplies damage of next x hit orbs
        HealOrb,            //heals the player
        DefenseOrb,         //gives player a mana shield to block damage
        RefreshOrb,         //refreshes the board
        FireBombOrb,        //explodes, and triggers surrounding orbs
        IceBombOrb,         //explodes, and freezes surrounding orbs -> can be hit multiple times without shattering
        LightningBombOrb,   //explodes, and causes random chain lightning propagation to surrounding obs
        DarkBombOrb,        //explodes, and makes a gravitational pull towards the hit peggle? -> shot can stay in air longer?
        LightBombOrb,       //explodes, and blinds all enemies
    }

    public enum PlayerAttackTarget
    {
        FirstEnemy,
        HighestHealthEnemy,
        LowestHeathEnemy,
        LastEnemy,
        RandomEnemy,
    }
}

