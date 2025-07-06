using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] AnimationSprites;
    public float animationTime = 1.0f;
    private SpriteRenderer spriterenderer;
    private int _animationFrame;
    public System.Action killed; 

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(animateSprite), this.animationTime, this.animationTime);
    }

    private void animateSprite()
    {
        _animationFrame++;
        if(_animationFrame >= this.AnimationSprites.Length)
        {
            _animationFrame = 0;
        }
        spriterenderer.sprite = this.AnimationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }

}