using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public Sprite[] sprite;
    public float lifeTime = 30.0f;
    public float speed = 50.0f;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprite[Random.Range(0, sprite.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * size;
        this._rigidbody.mass = this.size;
    }

    public void SetTrajectory( Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet") 
        {
            if((this.size * 0.5) >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            GameManager.instance.AsteroidsDestroy(this);
            Destroy(this.gameObject);
        }

    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroids half = Instantiate(this,position,this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
