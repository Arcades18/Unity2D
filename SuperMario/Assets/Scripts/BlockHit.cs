using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public int maxHit = -1;
    public Sprite emptyBlock;
    public GameObject blockCoin;

    private bool animating; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( maxHit != 0 && !animating && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }   
    }
    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        maxHit--;
        if(maxHit == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }
        if (blockCoin != null)
        {
            Instantiate(blockCoin, this.transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());
    }
    private IEnumerator Animate()
    {
        animating = true;

        Vector3 startingPosiiton = transform.localPosition;
        Vector3 animationPosition = startingPosiiton + Vector3.up * 0.5f;

        yield return Move(startingPosiiton, animationPosition);
        yield return Move(animationPosition, startingPosiiton);

        animating = false;
    }
    private IEnumerator Move(Vector3 starting, Vector3 ending)
    {
        float elapsed = 0.0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(starting,ending, t);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = ending;
    }
}
