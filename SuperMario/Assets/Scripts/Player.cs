using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSprite smallRenderer;
    public PlayerSprite bigRenderer;
    private PlayerSprite activeRenderer;

    private DeathAnimtion deathAnimation;
    private CapsuleCollider2D playerCollider;
    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starPower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimtion>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }
    public void Hit()
    {
        if(!dead && !starPower)
        if (big)
        {
            Shrink();
        }
        else
        {
            Death();
        }
    }
    public void grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        playerCollider.size = new Vector2(1f, 2f);
        playerCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }
    private void Shrink()
    {
        bigRenderer.enabled = false;
        smallRenderer.enabled = true;
        activeRenderer = smallRenderer;

        playerCollider.size = new Vector2(1f, 1f);
        playerCollider.offset = Vector2.zero;

        StartCoroutine(ScaleAnimation());
    }
    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.instance.ResetLevel(3f);
    }
    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }
            yield return null;
        }
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;

    }
    public void StarPower(float duration = 10.0f)
    {
        StartCoroutine(StarpowerAnimation(duration));
    }
    private IEnumerator StarpowerAnimation(float duration)
    {
        starPower = true;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if(Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
        }
        activeRenderer.spriteRenderer.color = Color.white;
        starPower = false;
    }
}
