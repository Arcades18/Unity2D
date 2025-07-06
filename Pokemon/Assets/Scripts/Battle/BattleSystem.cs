using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public enum BattleState
{
    start,
    ActionSelection,
    MoveSelection,
    PerformMove,
    Busy,
    PartyScreen,
    BattleOver,
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;

    private BattleState state;
    private int currentAction;
    private int currentMove;
    private int currentMember;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    public event Action<bool> OnBattleOver;

    public void StartBattle(PokemonParty playerParty , Pokemon wildpokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildpokemon;
        StartCoroutine(SetUpBattle());
    }
    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelector();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if(state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }
    public IEnumerator SetUpBattle()
    {
        playerUnit.SetUp(playerParty.GetHeathlyPokemon());
        enemyUnit.SetUp(wildPokemon);

        partyScreen.Init();

        dialogBox.SetMoveName(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A Wild {enemyUnit.Pokemon.Base.Name} appeared");

        ActionSelection();
    }

   private void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        OnBattleOver(won);
    }


    #region Player Move Selector
    private void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);

    }
    private void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove -= 2;
        }
        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);
        dialogBox.UpdateMoveSelector(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }
    private IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return RunMove(playerUnit,enemyUnit,move);
        
        if(state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }
    private IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return RunMove(enemyUnit,playerUnit,move);
        
        if(state == BattleState.PerformMove)
        {
            ActionSelection();
        }
    }
    private IEnumerator RunMove(BattleUnit soucerUnit,BattleUnit targetUnit,Move move)
    {
        move.Pp--;
        yield return dialogBox.TypeDialog($"{soucerUnit.Pokemon.Base.Name} Used {move.Base.Name}");

        soucerUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayHitAnimation();

        if(move.Base.Category == MoveCategory.Status)
        {
            var effect = move.Base.Effects;
            if(effect.Boosts != null)
            {
                if(move.Base.Target == MoveTarget.Self)
                {
                    soucerUnit.Pokemon.ApplyBoost(effect.Boosts);
                }
                else
                {
                    targetUnit.Pokemon.ApplyBoost(effect.Boosts);
                }
            }
        }
        else
        {
            var damageDetail = targetUnit.Pokemon.TakeDamage(move, soucerUnit.Pokemon);
            yield return targetUnit.Hud.UpdateHp();
            yield return ShowDamageDetail(damageDetail);

        }


        if (targetUnit.Pokemon.Hp <=0)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} is Fainted");
            targetUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
    }

    private void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if(faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHeathlyPokemon();
            if (nextPokemon != null)
            {
                OpenPlayerParty();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }
    private IEnumerator ShowDamageDetail(DamageDetail damageDetail)
    {
        if(damageDetail.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A Critical Hit!");
        }
        if(damageDetail.Type > 1f)
        {
            yield return dialogBox.TypeDialog("It's Super Effective");
        }
        else if (damageDetail.Type < 1f)
        {
            yield return dialogBox.TypeDialog("It's Not Very Effective");
        }
    }
    #endregion
    #region PLaye Aciton Selector
    private void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an Action");
        dialogBox.EnableActionSelector(true);
    }
    private void HandleActionSelector()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;
        }
        else if( Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }
        currentAction = Mathf.Clamp(currentAction,0, 3);
            dialogBox.UpdateActionSelector(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                //Bag
            }
            else if (currentAction == 2)
            {
                //Pokemon
                OpenPlayerParty();
            }
            else if (currentAction == 3)
            {
                //Run
            }
        }
    }
    #endregion

    #region
    private void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember -= 2;
        }
        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);
        var selectedMember = playerParty.Pokemons[currentMember];
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(selectedMember.Hp <= 0)
            {
                partyScreen.SetMessageText("You Can't Send Out a Fainted Pokemon");
                return;
            }
            if(selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You Can't switch with same Pokemon");
            }
            else
            {
                partyScreen.gameObject.SetActive(false);
                state = BattleState.Busy;
                StartCoroutine(switchPokemon(selectedMember));
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }
    private IEnumerator switchPokemon(Pokemon newPokemon)
    {
        if(playerUnit.Pokemon.Hp > 0)
        {
            yield return dialogBox.TypeDialog($"Come back!!{playerUnit.Pokemon.Base.Name}");
            playerUnit.PlayComeBackAnimation();
            yield return new WaitForSeconds(2f);

        }

        playerUnit.SetUp(newPokemon);

        dialogBox.SetMoveName(newPokemon.Moves);
        yield return dialogBox.TypeDialog($"Go! {newPokemon.Base.Name}");
        StartCoroutine(EnemyMove());


    }
    private void OpenPlayerParty()
    {
        state  = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }
    #endregion
}
