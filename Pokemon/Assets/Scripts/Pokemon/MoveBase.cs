using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move",menuName = "Pokemon/Create new Move")]
public class MoveBase :ScriptableObject
{
    [SerializeField] private new string name;

    [TextArea]
    [SerializeField] private string description;

    [SerializeField] PokemonType type;
    [SerializeField] private int power;
    [SerializeField] private int arruracy;
    [SerializeField] private int pp;
    [SerializeField] private MoveCategory category;
    [SerializeField] private MoveEffect effects;
    [SerializeField] private MoveTarget target;
    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public PokemonType Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Arruracy
    {
        get { return arruracy; }
    }
    public int Pp
    {
        get { return pp; }
    }
    public MoveCategory Category
    {
        get { return category; }
    }
    public MoveEffect Effects
    {
        get { return  effects; }
    }
    public MoveTarget Target
    {
        get { return target; }
    }
}
[System.Serializable]
public class MoveEffect
{
    [SerializeField] List<StatBoost> boosts;

    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }
}

[System.Serializable]
public class StatBoost 
{
    public Stat stat;
    public int boost;
}


public enum MoveCategory
{
    Physical ,
    Special,
    Status
}
public enum MoveTarget 
{
    Foe,
    Self
}

