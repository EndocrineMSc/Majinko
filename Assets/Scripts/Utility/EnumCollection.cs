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
        CardHandling,
        Shooting,
        PlayerActions,
        EnemyTurn,
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

    //This enum needs to be in alphabetical order!
    //Will be used to reference orbs in OrbManager, which has a list of alphabetically sorted orb prefabs
    public enum OrbType
    {
        BaseManaOrb,        //gives basic Mana
        DarkBombOrb,        //explodes, and makes a gravitational pull towards the hit peggle? -> shot can stay in air longer?
        DarkManaOrb,        //gives dark Mana
        DefenseOrb,         //gives player a mana shield to block damage
        FireBombOrb,        //explodes, and triggers surrounding orbs
        FireManaOrb,        //gives fire Mana
        HealOrb,            //heals the player
        IceBombOrb,         //explodes, and freezes surrounding orbs -> can be hit multiple times without shattering
        IceManaOrb,         //gives ice Mana
        LightBombOrb,       //explodes, and blinds all enemies
        LightManaOrb,       //gives light Mana
        LightningBombOrb,   //explodes, and causes random chain lightning propagation to surrounding obs
        LightningManaOrb,   //gives lightning Mana
        ManaBlitzOrb,       //shoots a ManaBlitz at the first enemy
        MultiplierOrb,      //multiplies damage of next x hit orbs
        RefreshOrb,         //refreshes the board
        RottedOrb,          //does something negative, spawned by zombies
    }

    public enum PlayerAttackTarget
    {
        FirstEnemy,
        HighestHealthEnemy,
        LowestHeathEnemy,
        LastEnemy,
        RandomEnemy,
    }

    public enum Fade
    {
        In,
        Out,
    }

    public enum Track
    {
        Track_0001_LevelOne,
        GameTrackOne,

    }

    public enum SFX
    {
        SFX_0001_ButtonClick,
        SFX_0002_BasicPeggleHit,
        SFX_0003_ManaBlitz,
        SFX_0004_MouseOverCard
    }

    public enum StartDeck
    {
        Apprentice,
        Soldier,
        Druid,
        Alchemist
    }

    public enum CardType
    {
        Attack,
        Defense,
        Buff,
        Utility,
    }
}

