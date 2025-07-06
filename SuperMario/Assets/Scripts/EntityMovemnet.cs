using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovemnet : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector2 direction = Vector2.left;
    private Rigidbody2D _rigidbody;
    private Vector2 velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }
    private void OnBecameVisible()
    {
        enabled = true;
    }
    private void OnBecameInvisible()
    {
        enabled = false;
    }
    private void OnEnable()
    {
        _rigidbody.WakeUp();
    }
    private void OnDisable()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.Sleep();
    }
    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);

        if (_rigidbody.Raycast(direction))
        {
            direction = -direction;
        }
        if (_rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0);
        }
    }
}
