using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanBulletManager : MonoBehaviour
{
    public float speed = 5f;
    public Ghost ghost;


    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            ghost = other.GetComponent<Ghost>();
            if(ghost != null)
            {
                this.ghost.frightened.eaten();
            }
        }
    }
}
