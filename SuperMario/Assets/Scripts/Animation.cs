using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Sprite[] sprite;
    public float frameRate = 1.0f / 6.0f;

    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), frameRate, frameRate);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void Animate()
    {
        frame++;
        if(frame >= sprite.Length)
        {
            frame = 0;
        }
        if(frame > 0 && frame < sprite.Length)
        {
            spriteRenderer.sprite = sprite[frame];
        }
    }

}
