using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate_Sprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public int animationFrame { get; private set; }
    public Sprite[] sprite;
    public float animationTime = 0.25f;
    public bool loop = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if(!this.spriteRenderer.enabled)
        {  
            return; 
        }
        this.animationFrame++;
        if(this.animationFrame >= this.sprite.Length && loop)
        {
            this.animationFrame = 0;
        }
        if(this.animationFrame >=0 && this.animationFrame <= this.sprite.Length)
        {
            this.spriteRenderer.sprite = this.sprite[animationFrame];
        }
    }

    public void RestartAnimation()
    {
        animationFrame = -1;
        Advance();
    }
}
