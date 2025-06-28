using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimationSpriteRenderer start;
    public AnimationSpriteRenderer middle;
    public AnimationSpriteRenderer end;

    public void SetActiveRender(AnimationSpriteRenderer Renderer)
    {
        start.enabled = Renderer == start;
        middle.enabled = Renderer == middle;
        end.enabled = Renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y , direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float Second)
    {
        Destroy(gameObject, Second);
    }
}
