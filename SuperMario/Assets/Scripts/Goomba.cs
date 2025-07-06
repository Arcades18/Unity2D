using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flateSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starPower)
            {
                Hit();
            }
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                flatten();
            }
            else
            {
                player.Hit();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }
    private void flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovemnet>().enabled = false;
        GetComponent<Animation>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flateSprite;
        Destroy(gameObject, 0.5f);
    }
    private void Hit()
    {
        GetComponent<Animation>().enabled = false;
        GetComponent<DeathAnimtion>().enabled = true;
        Destroy(gameObject, 3.0f);
    }
}
