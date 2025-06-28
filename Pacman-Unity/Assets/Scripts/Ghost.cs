using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int points = 200;
    public Ghost_Behaviour initialBehavior;
    public Transform target;
    public AudioClip pacmanDeathClip;
    public AudioSource AudioSource {  get; private set; }
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostFrightened frightened { get; private set; }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        chase = GetComponent<GhostChase>();
        scatter = GetComponent<GhostScatter>();
        frightened = GetComponent<GhostFrightened>();
        AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.Resetstate();
        this.chase.Disable();
        this.frightened.Disable();
        this.scatter.Enable();

        if(this.home != this.initialBehavior)
        {
            this.home.Disable();
        }
        if(this.initialBehavior != null)
        {
            this.initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().ghostEaten(this);
            }
            else
            {
                if (!AudioSource.gameObject.activeInHierarchy)
                {
                    AudioSource.gameObject.SetActive(true);
                }
                SoundManager.instance.PlayClip(pacmanDeathClip, AudioSource);
                FindObjectOfType<GameManager>().pacmanEaten();
            }
        }
    }
}
