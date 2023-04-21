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
        #region 0 - 100 UI-SFX

        _0001_ButtonClick,
        _0002_MouseOverCard,
        _0003_EndTurnClick,
        _0004_CardDrag,
        _0005_CardDragReturn,
        _0006_DrawHand,
        _0007_ShopCard_Picked,
        _0008_Mouse_Over_Button,

        #endregion

        #region 101 - 500 Attack-SFX

        _0101_ManaBlitz_Shot,
        _0102_ManaBlitz_Spawn,
        _0103_Blunt_Spell_Impact,
        _0104_Player_Takes_Damage,

        #endregion

        #region 501 - 750 Monster-SFX

        _0501_Zombie_Spawn,
        _0502_Zombie_Death,
        _0503_Wraith_Spawn,
        _0504_Wraith_Death,

        #endregion

        #region 751 - 1000 Arena-SFX

        _0751_Orb_Impact_01,
        _0752_Orb_Impact_02,
        _0753_Orb_Impact_03,
        _0754_Orb_Impact_04,
        _0755_Orb_Impact_05,
        _0756_Orb_Impact_06,
        _0757_Orb_Impact_07,
        _0758_Orb_Impact_08,
        _0759_Orb_Impact_09,
        _0760_Sphere_In_Portal,
        _0761_Sphere_Shot_01,
        _0762_Sphere_On_Wood_01,
        _0763_Sphere_On_Wood_02,
        _0764_Sphere_On_Wood_03,
        _0765_Sphere_On_Wood_04,
        _0766_Sphere_On_Wood_05,
        _0767_Sphere_On_Wood_06,
        _0768_Sphere_On_Wood_07,
        _0769_Sphere_On_Wood_08,
        _0770_Orb_Spawn_Whoosh,

        #endregion
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

