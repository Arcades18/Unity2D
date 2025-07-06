using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon",menuName = "Pokemon/Create new Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private new string name;

    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private Sprite frontSprite; 
    [SerializeField] private Sprite backSprite;
    
    [SerializeField] private PokemonType type1;
    [SerializeField] private PokemonType type2;

    //Base State
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defence;
    [SerializeField] private int spAttack;
    [SerializeField] private int spDefence;
    [SerializeField] private int speed;

    [SerializeField] private List<LearnableMove> learnableMove;

    public string Name
    {
        get { return name; }
    }
    public string Description 
    {
        get { return description; }
    }
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }
    public Sprite BackSprite
    {
        get { return backSprite; }
    }
    public PokemonType Type1
    {
        get { return type1; }
    }
    public PokemonType Type2
    {
        get { return type2; }
    }
    public int MaxHp 
    {
        get { return maxHp; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defence
    {
        get { return defence; }
    }
    public int SpAttack
    {
        get { return spAttack; }
    }
    public int SpDefence
    {
        get { return spDefence; }
    }
    public int Speed
    {
        get { return speed; }
    }
    public List<LearnableMove> LearnableMove
    {
        get { return  learnableMove; }
    }
}
[System.Serializable]
public class LearnableMove
{
    [SerializeField] private MoveBase moveBase;
    [SerializeField] private int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }
    public int Level
    {
        get { return level; }
    }
}

  public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Electric,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy,
}
public enum Stat 
{
    Attack,
    Defense,
    SpAttack,
    SpDefence,
    Speed
}

public class TypeChart
{
    static float[][] chart =
    {
//           NOR  FIR  WAT  GRA  ELE  ICE  FIG  POI
/*NOR*/ new float[] {1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f},
/*FIR*/ new float[] {1f, 0.5f, 0.5f, 2f,  1f,  2f,  1f,  1f},
/*WAT*/ new float[] {1f,  2f, 0.5f, 0.5f, 2f,  1f,  1f,  1f},
/*GRA*/ new float[] {1f, 0.5f, 2f, 0.5f, 1f,  1f,  1f, 0.5f},
/*ELE*/ new float[] {1f,  1f,  2f, 0.5f, 0.5f, 1f,  1f,  1f},
/*ICE*/ new float[] {1f,  0.5f, 0.5f, 2f,  1f, 0.5f, 1f,  1f},
/*FIG*/ new float[] {2f,  1f,  1f, 1f,  1f,  2f,  1f, 0.5f},
/*POI*/ new float[] {1f,  1f,  1f, 2f,  1f,  1f,  1f, 0.5f},
};

    public static float GetEffectivenes(PokemonType attackType, PokemonType defenceType)
    {
        if (attackType == PokemonType.None || defenceType == PokemonType.None)
        {
            return 1;
        }
        int row = (int)attackType - 1;
        int column = (int)defenceType - 1;
        return chart[row][column];

    }
}


