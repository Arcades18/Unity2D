using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public float speedMultipler = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startPosition { get; private set; }
    public new Rigidbody2D rigidbody { get; private set; }

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startPosition = transform.position;
    }
    private void Start()
    {
        Resetstate();
    }
    public void Resetstate()
    {
        this.speedMultipler = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startPosition;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }
    private void Update()
    {
        if(this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMultipler * Time.fixedDeltaTime;
        this.rigidbody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction,bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            this.nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position,Vector2.one * 0.75f,0.0f,direction,1.5f,obstacleLayer);
        return hit.collider != null;
    }

}
