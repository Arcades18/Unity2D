using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }
    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicalCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true;
        physicalCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0.0f;
        float duration = 0.5f;

        Vector3 startingPosition = transform.localPosition;
        Vector3 endingPosition = startingPosition + Vector3.up;

        while(elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(startingPosition, endingPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = endingPosition;

        rigidbody.isKinematic = false;
        physicalCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}
