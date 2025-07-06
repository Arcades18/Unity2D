using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimtion : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite deathSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        if (deathSprite != null)
        {
            spriteRenderer.sprite = deathSprite;
        }
    }
    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        
        foreach(Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        GetComponent<Rigidbody2D>().isKinematic = true;

        PlayerMovement playermovement = GetComponent<PlayerMovement>();
        EntityMovemnet entityMovemnet = GetComponent<EntityMovemnet>();
        PlayerSprite playersprite = GetComponentInChildren<PlayerSprite>();
        Animation animation = GetComponentInChildren<Animation>();
        if(playersprite != null)
        {
            playersprite.runSprite.enabled = false;
        }
        if (animation != null)
        {
            animation.enabled = false;
        }
        if (playermovement != null )
        {
            playermovement.enabled = false;
        }
        if( entityMovemnet != null )
        {
            entityMovemnet.enabled = false;
        }

    }
    private IEnumerator Animate()
    {
        float elapsed = 0.0f;
        float duration = 3f;

        float jumpVelocity = 10.0f;
        float gravity = -36.0f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while(elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
