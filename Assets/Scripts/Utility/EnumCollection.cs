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

    public enum GameState
    {
        MainMenu,
        CardHandling,
        Shooting,
        PlayerActions,
        EnemyTurn,
        LevelWon,
        NewLevel,
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
        BasicMana,
        FireMana,
        IceMana,
    }

    //This enum needs to be in alphabetical order!
    //Will be used to reference orbs in OrbManager, which has a list of alphabetically sorted orb prefabs
    public enum OrbType
    {
        BaseManaOrb,        //gives basic Mana
        DarkBombOrb,        //explodes, and makes a gravitational pull towards the hit peggle? -> shot can stay in air longer?
        DarkManaOrb,        //gives dark Mana
        DefenseOrb,         //gives player a mana shield to block damage
        FastHandsOrb,       //draw an extra card on turn start
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
        ManaShieldOrb,      //shields player from incoming damage
        MultiplierOrb,      //multiplies damage of next x hit orbs
        RefreshOrb,         //refreshes the board
        RottedOrb,          //spawns rotted mana (reduces player attack power when used)
    }

    public enum PlayerAttackTarget
    {
        FirstEnemy,
        HighestHealthEnemy,
        LowestHealthEnemy,
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
        _0001_LevelOne,
        GameTrackOne,
    }

    public enum SFX
    {
        _0001_ButtonClick,
        _0002_BasicPeggleHit,
        _0003_ManaBlitz,
        _0004_MouseOverCard,
        _0005_EndTurnClick,
        _0006_CardDrag,
        _0007_CardDragReturn,
        _0008_ManaBlitzSpawn,
        _0009_ZombieSpawn,
        _0010_BluntSpellImpact,
        _0011_ZombieDeath,
        _0012_DrawHand,
    }

    public enum StartDeck
    {
        Apprentice,
        Soldier,
        Druid,
        Alchemist
    }

    public enum ShotType
    {
        BasicShot,
        MultiShot,
        PowerShot,
    }
}

