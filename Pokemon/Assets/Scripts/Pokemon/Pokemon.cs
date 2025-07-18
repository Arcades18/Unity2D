using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base;
    [SerializeField] private int _level;
    public PokemonBase Base 
    {
        get
        {
            return _base;
        }
    }
    public int Level 
    {
        get
        {
            return _level;
        }
    }

    public int Hp { get; set; }
    public List<Move> Moves;
    public Dictionary<Stat, int> Stats {get; private set;}
    public Dictionary<Stat, int> StatBoost {get; private set;}
    public Queue<string> StatusChanges {get; private set;} = new Queue<string>();
    public void Init()
    {


        Moves = new List<Move>();
        foreach(var move in Base.LearnableMove)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if(Moves.Count >= 4)
            {
                break;
            }
        }
        CalculateStats();
        Hp = MaxHp;

        StatBoost = new Dictionary<Stat, int>()
        {
            {Stat.Attack,0},
            {Stat.Defense,0},
            {Stat.SpAttack,0},
            {Stat.SpDefence,0},
            {Stat.Speed,0},
        };
    }
    private void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefence, Mathf.FloorToInt((Base.SpDefence * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10;
    }
    private int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        int boost = StatBoost[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if(boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal /  boostValues[-boost]);
        }

            return statVal;
    }


    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach(var  statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoost[stat] = Mathf.Clamp(StatBoost[stat] + boost,-6,6);

            if(boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoost[stat]}");
        }
    }
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }
    public int Defence
    {
        get { return GetStat(Stat.Defense); }
    }
    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }
    public int SpDefence
    {
        get { return GetStat(Stat.SpDefence); }
    }
    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }
    public int MaxHp { get; private set; }

    public DamageDetail TakeDamage(Move move,Pokemon attacker)
    {
        float critical = 1f;
        if(Random.value * 100f <= 6.25f)
        {
            critical = 2f;
        }

        float type = TypeChart.GetEffectivenes(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectivenes(move.Base.Type, this.Base.Type2);

        float attack = move.Base.Category == MoveCategory.Special ? attacker.SpAttack : attacker.Attack;
        float defence = move.Base.Category == MoveCategory.Special ? SpDefence : Defence;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defence) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        var damageDetail = new DamageDetail()
        {
            Type = type,
            Critical = critical,
            Fainted = false

        };
        Hp -= damage;
        if(Hp <= 0)
        {
            Hp = 0;
           damageDetail.Fainted = true;
        }
        return damageDetail;
    }

    public Move GetRandomMove()
    {
        int randomIndex = Random.Range(0, Moves.Count);
        return Moves[randomIndex];
    }
}
public class DamageDetail 
{
    public bool Fainted {  get; set; }
    public float Critical{  get; set; }
    public float Type{  get; set; }

}
