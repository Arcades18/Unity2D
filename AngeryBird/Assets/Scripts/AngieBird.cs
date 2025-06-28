using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angry : MonoBehaviour
{

    [SerializeField] private AudioClip hittingClip;
    private AudioSource AudioSource;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool hasBeenLaunch;
    private bool shouldFaceVelocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        AudioSource = GetComponent<AudioSource>();

        rb.isKinematic = true;
        circleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (hasBeenLaunch && shouldFaceVelocity)
        {
            transform.right = rb.velocity;
        }
    }

    public void LauchBird(Vector2 direction,float force)
    {
        rb.isKinematic = false;
        circleCollider.enabled = true;

        rb.AddForce(direction * force, ForceMode2D.Impulse);
        hasBeenLaunch = true;
        shouldFaceVelocity = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shouldFaceVelocity = false;
        SoundManager.instance.PlayClip(hittingClip, AudioSource);
        Destroy(this);
    }
}
