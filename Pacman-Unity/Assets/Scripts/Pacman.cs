using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firepoint;
    public int maxBullets = 5;
    public int currentBullets = 5;
    public Movement movement { get; private set; }
    public AudioClip ghostDieSound;
    public AudioSource AudioSource { get; private set; }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            SoundManager.instance.PlayClip(ghostDieSound, AudioSource);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.movement.SetDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.movement.SetDirection(Vector2.left);
        }

        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Space) && currentBullets > 0)
        {
            Shoot();
        }
    }
    public void ResetState()
    {
        this.movement.Resetstate();
        this.gameObject.SetActive(true);
        currentBullets = 5;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firepoint.transform.position, firepoint.rotation);
        currentBullets --;
    }


}
