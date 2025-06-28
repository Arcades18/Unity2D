using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float damageThresold = 0.2f;
    [SerializeField] private GameObject enemieDeathPartical;
    [SerializeField] private AudioClip EnemieDeathSound;

    private float current;

    private void Awake()
    {
        current = maxHealth;
    }

    public void damageEnemie(float damageAmount)
    {
        current -= damageAmount;
        if(current <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.RemoveEneime(this);
        Instantiate(enemieDeathPartical , transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(EnemieDeathSound, transform.position);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if(impactVelocity > damageThresold)
        {
            damageEnemie(impactVelocity);
        }
    }
}
