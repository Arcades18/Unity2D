using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    
    public Rigidbody2D player {get; private set;}
    private Vector2 direction = Vector2.down;
    public float Speed = 5f;

    public KeyCode InputUp = KeyCode.W;
    public KeyCode InputDown = KeyCode.S;
    public KeyCode InputLeft = KeyCode.A;
    public KeyCode InputRight = KeyCode.D;

    public AnimationSpriteRenderer spriteRendererUp;
    public AnimationSpriteRenderer spriteRendererDown;
    public AnimationSpriteRenderer spriteRendererLeft;
    public AnimationSpriteRenderer spriteRendererRight;
    public AnimationSpriteRenderer spriteRendererDeath;
    private AnimationSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
     
        if(Input.GetKey(InputUp) )
        {
            SetDirection(Vector2.up , spriteRendererUp);
        }
        else if(Input.GetKey(InputDown))
        {
            SetDirection(Vector2.down , spriteRendererDown);
        }
        else if(Input.GetKey(InputLeft))
        {
            SetDirection(Vector2.left , spriteRendererLeft);
        }
        else if(Input.GetKey(InputRight))
        {
            SetDirection(Vector2.right , spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSpriteRenderer);
        }
   
    }

    private void FixedUpdate()
    {
        Vector2 position = player.position;
        Vector2 translate = direction * Speed * Time.fixedDeltaTime;

        player.MovePosition(position +  translate);
    }

    private void SetDirection(Vector2 newDirection , AnimationSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer== LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();    
        }
    }

    public void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeath) , 1.25f);
    }
    public void OnDeath()
    {
        gameObject.SetActive(false);
        FindAnyObjectByType<GameManager>().CheckWinState();
    }
}
