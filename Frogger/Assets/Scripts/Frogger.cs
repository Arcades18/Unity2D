using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;

    private Vector3 startposition;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startposition = transform.position;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            Move(Vector3.up);
        }
        else if(Input.GetKeyDown(KeyCode.S) ||  Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            Move(Vector3.down);
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            Move(Vector3.right);
        }
        else if( Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
            Move(Vector3.left);
        }
    }
    private void Move(Vector3 direction)
    {
        Vector3 destination = this.transform.position + direction;

       Collider2D boundary = Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("boundary"));
       Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("platform"));
       Collider2D obstacal = Physics2D.OverlapBox(destination, Vector2.zero, 0.0f, LayerMask.GetMask("obstacal"));


        if (boundary != null)
        {
            return;
        }
        if(platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }
        if(obstacal != null && platform == null)
        {
            transform.position = destination;
            Death();
        }
        else
        {
            StartCoroutine(Leap(destination));
        }
    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = this.transform.position;

        float elapsed = 0.0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while(elapsed < duration)
        {
            float t = elapsed / duration;
            this.transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.position = destination;
        spriteRenderer.sprite = idleSprite;
    }
    public void Death()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = deadSprite;
        enabled = false;
        GameManager.instance.Died();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.parent == null && enabled && other.gameObject.layer == LayerMask.NameToLayer("obstacal"))
        {
            Death();
        }
    }

    public void Respawn()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        transform.position = startposition;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;
    }

}
