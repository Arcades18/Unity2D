using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : Ghost_Behaviour
{
    public SpriteRenderer body;
    public SpriteRenderer Eyes;
    public SpriteRenderer Blue;
    public SpriteRenderer White;

    public bool Eaten { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.Eyes.enabled = false;
        this.Blue.enabled = true;
        this.White.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }
    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.Eyes.enabled = true;
        this.Blue.enabled = false;
        this.White.enabled = false;
    }

    private void Flash()
    {
        if (!this.Eaten)
        {
            this.Blue.enabled = false;
            this.White.enabled = true;
            this.White.GetComponent<Animate_Sprite>().RestartAnimation();
        }
    }

    public void eaten()
    {
        this.Eaten = true;

        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.Eyes.enabled = true;
        this.Blue.enabled = false;
        this.White.enabled = false;
    }
    private void OnEnable()
    {
        this.ghost.movement.speedMultipler = 0.6f;
        this.Eaten = false;
    }
    private void OnDisable()
    {
        this.ghost.movement.speedMultipler = 1.0f;
        this.Eaten = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                eaten();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();
        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }
}
