using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;
    public AudioClip pelletEating;
    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().pelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            SoundManager.instance.PlayClip(pelletEating, audioSource);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
