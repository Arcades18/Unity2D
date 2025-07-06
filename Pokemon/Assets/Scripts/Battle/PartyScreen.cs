using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] private Text messageText;

    private PartyMemberUi[] memberSlots;
    private List<Pokemon> pokemons;

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUi>();
    }
    public void SetPartyData(List<Pokemon> pokemon)
    {
        this.pokemons = pokemon;
        for (int i = 0; i < memberSlots.Length; i++)
        {
            if(i < pokemon.Count)
            {
                memberSlots[i].SetData(pokemon[i]);
            }
            else
            {
                memberSlots[i].gameObject.SetActive(false);
            }
        }
        messageText.text = "Choose A Pokemon";
    }
    public void UpdateMemberSelection(int selectedMember)
    {
        for(int i = 0;i < pokemons.Count;i++)
        {
            if(i == selectedMember)
            {
                memberSlots[i].SetSelected(true);
            }
            else
            {
                memberSlots[i].SetSelected(false);
            }
        }
    }
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }

}
