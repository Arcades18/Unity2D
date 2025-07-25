using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float speed = 500.0f;
    public float maxLifeTime = 10.0f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ShootBullet(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
        Destroy(this.gameObject, maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
