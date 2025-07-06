using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Camera _camera;
    private Collider2D _collider;
    
    private float inputAxis;
    private Vector2 velocity;

    public float moveSpeed = 8.0f;
    public float maxJumpHeight = 5.0f;
    public float maxJumpTime = 1.0f;

    public float jumpForce => (2 * maxJumpHeight) / (maxJumpTime / 2);
    public float gravity => (-2 * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2), 2);
    public bool grounded {  get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0.0f && velocity.x < 0.0f) || (inputAxis < 0.0f && velocity.x > 0.0f);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _camera = Camera.main;
    }
    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        _collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }
    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        _collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();
        }
        ApplyGravity();
    }
    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0);
        jumping = velocity.y > 0;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }
    private void ApplyGravity()
    {
        bool falling = velocity.y < 0 || !Input.GetButton("Jump");
        float multipler = falling ? 2f : 1f;

        velocity.y += gravity * multipler * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2);

    }
    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if(rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0.0f;
        }
        if (velocity.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(velocity.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = _camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rigthEdge = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rigthEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2;
                jumping = true;
            }
        }
        if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUP"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0.0f;
            }
        }
    }
}
