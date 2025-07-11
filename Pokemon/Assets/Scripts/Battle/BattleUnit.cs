using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] private bool isPlayerUnit;
    [SerializeField] private BattleHud hud;

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }
    public BattleHud Hud
    {
        get { return hud; }
    }

    private Image image;
    private Vector3 originalPosition;
    private Color originalColor;

    public Pokemon Pokemon { get; set; }

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPosition = this.transform.localPosition;
        originalColor = image.color;
    }
    public void SetUp(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }

        hud.SetData(pokemon);
        image.color = originalColor;
        PlayEnterAnimation();
    }
    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPosition.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPosition.y);
        }
        image.transform.DOLocalMoveX(originalPosition.x, 1.0f);
    }
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, 0.25f));
    }
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f,0.5f));
    }
    public void PlayComeBackAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosition.x - 250f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
