using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public float respawnTime = 3.0f;
    public float playerInvincibleTime = 3.0f;
    public int lives = 3;
    private int score = 0;
    public ParticleSystem Explosion;

    public Text scoreText;
    public Text livesText;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void AsteroidsDestroy(Asteroids asteroid)
    {
        this.Explosion.transform.position = asteroid.transform.position;
        this.Explosion.Play();

        if(asteroid.size < 0.75f)
        {
            score += 100;
        }
        else if(asteroid.size < 1.2f)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }
        this.scoreText.text = score.ToString();
    }
    public void playerDies()
    {
        lives--;
        this.livesText.text = lives.ToString();
        
        this.Explosion.transform.position = this.player.transform.position;
        this.Explosion.Play();

        if(this.lives <= 0)
        {
            GameOver();    
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnorColllision");

        player.gameObject.SetActive(true);
        Invoke(nameof(turnOnCollision), playerInvincibleTime);

    }

    private void turnOnCollision()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void GameOver()
    {
        this.lives = 3;
        this.livesText.text = lives.ToString();
        this.score = 0;
        this.scoreText.text = score.ToString();

        Invoke(nameof(Respawn), this.respawnTime);
    }
}
