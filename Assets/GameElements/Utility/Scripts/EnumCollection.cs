namespace EnumCollection
{
    public enum EnemyType
    {
        Boss_ViciousBoar,
        LeafMage,
        PineMouse,
        Shroombie,
        StoneGolem,
    }

    public enum ManaType
    {
        BasicMana,
        FireMana,
        IceMana,
        RottedMana,
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
        MultiplierOrb,      //multiplies damage of next x hit orb
        PineConeOrb,        //Spawnes a pine cone enemy
        RefreshOrb,         //refreshes the board
        RottedOrb,          //spawns rotted mana (reduces player attack power when used)
        StoneOrb,           //blocks orb position destroyed after mutliple hits
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

    public enum StartDeck
    {
        Apprentice,
        Soldier,
        Druid,
        Alchemist
    }

    public enum SphereType
    {
        BasicSphere,
        CloneSphere,
        PowerSphere,
    }

    public enum CardRarity
    {
        Basic,
        Common,
        Rare,
        Epic,
        Legendary,
    }

    public enum CardElement
    {
        None,
        Fire,
        Ice,
        Lightning,
    }

    public enum CardEffectType
    {
        Instant,
        Orbshifter,
        Sphereshifter,        
    }
}

