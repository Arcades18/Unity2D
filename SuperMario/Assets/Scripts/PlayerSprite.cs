using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer {  get; private set; }
    private PlayerMovement movement;

    public Sprite idleSprite;
    public Sprite jumpSprite;
    public Sprite slideSprite;
    public Animation runSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }
    private void LateUpdate()
    {
        runSprite.enabled = movement.running;
        if (movement.jumping)
        {
            spriteRenderer.sprite = this.jumpSprite;
        }
        else if (movement.sliding)
        {
            spriteRenderer.sprite = this.slideSprite;
        }
        else if(!movement.running)
        {
            spriteRenderer.sprite = this.idleSprite;
        }
    }


}
