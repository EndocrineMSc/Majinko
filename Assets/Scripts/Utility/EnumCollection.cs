using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumCollection
{
    internal enum SpawnNumber
    {
        One,
        Two,
        Three,
        Four,
    }

    internal enum GameState
    {
        MainMenu,
        LevelSetup,
        CardHandling,
        Shooting,
        PlayerActions,
        EnemyTurn,
        LevelWon,
        NewLevel,
        GameOver,
        Quit,
    }

    internal enum EnemyAttackType
    {
        Melee,
        Ranged,
    }

    internal enum EnemyType
    {
        CloakedZombie,
        WraithCaster,
    }

    internal enum ManaType
    {
        BasicMana,
        FireMana,
        IceMana,
        RottedMana,
    }

    //This enum needs to be in alphabetical order!
    //Will be used to reference orbs in OrbManager, which has a list of alphabetically sorted orb prefabs
    internal enum OrbType
    {
        BaseManaOrb,        //gives basic Mana
        DarkBombOrb,        //explodes, and makes a gravitational pull towards the hit peggle? -> shot can stay in air longer?
        DarkManaOrb,        //gives dark Mana
        DefenseOrb,         //gives player a mana shield to block damage
        FastHandsOrb,       //draw an extra card on turn start
        FireArrowOrb,       //fire version of the manablitz, sets enemy on fires
        FireBombOrb,        //explodes, and triggers surrounding orbs
        FireManaOrb,        //gives fire Mana
        ForbiddenOrbA,      //
        ForbiddenOrbD,      //
        ForbiddenOrbE,      //
        ForbiddenOrbI,      //
        ForbiddenOrbO,      //
        ForbiddenOrbX,      //
        HailStormOrb,       //will hit all enemies in scene, adding freezing stack with a chance to apply frozen
        HealOrb,            //heals the player
        IceManaOrb,         //gives ice Mana
        IcicleOrb,          //shoots an icicle at the first enemy, with a low chance to freeze it on impact
        IntangibleEnemyOrb, //will make all enemies with the ability to do so intangible for a turn
        LightningStrikeOrb, //Attack, strikes last enemy
        ManaBlitzOrb,       //shoots a ManaBlitz at the first enemy
        ManaShieldOrb,      //shields player from incoming damage
        MultiplierOrb,      //multiplies damage of next x hit orbs
        RefreshOrb,         //refreshes the board
        RottedOrb,          //spawns rotted mana (reduces player attack power when used)
    }

    internal enum PlayerAttackTarget
    {
        FirstEnemy,
        HighestHealthEnemy,
        LowestHealthEnemy,
        LastEnemy,
        RandomEnemy,
    }

    internal enum Fade
    {
        In,
        Out,
    }

    internal enum Track
    {
        _0001_LevelOne,
        GameTrackOne,
    }

    internal enum SFX
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

    internal enum StartDeck
    {
        Apprentice,
        Soldier,
        Druid,
        Alchemist
    }

    internal enum ShotType
    {
        BasicShot,
        MultiShot,
        PowerShot,
    }

    internal enum CardRarity
    {
        Basic,
        Common,
        Rare,
        Epic,
        Legendary,
    }

    internal enum CardElement
    {
        None,
        Fire,
        Ice,
        Lightning,
    }

    internal enum CardEffectType
    {
        Instant,
        Orbshifter,
        Sphereshifter,        
    }
}

