using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] private List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get
        {
            return pokemons;
        }
    }

    private void Start()
    {
        foreach(var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }
    public Pokemon GetHeathlyPokemon()
    {
       return pokemons.Where(x => x.Hp > 0).FirstOrDefault();

    }
}
