using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private HpBar hpBar;
    private Pokemon _pokemon;
    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHp((float)pokemon.Hp / pokemon.MaxHp);
    }
    public IEnumerator UpdateHp()
    {
       yield return hpBar.SetHpSmooth((float)_pokemon.Hp / _pokemon.MaxHp);
    }
}
