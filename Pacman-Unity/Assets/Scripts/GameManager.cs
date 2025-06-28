using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Image[] icon;
    public Color iconColor;
    public Color usedColor;
    public Ghost[] ghost;
    public Pacman pacman;
    public Transform pellets;
    public Text scoreText;
    
    public int ghostMultipler { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    private int maxNoOfLive = 3;
    private int currentNoOfLive;

    
    private void Start()
    {
        NewGame();
    }
    private void Update()
    {
        if(Input.anyKeyDown && this.lives <= 0)
        {
            NewGame();
        }
    }
    #region NewGame/GameOver Functions
    private void GameOver()
    {
        for (int i = 0; i < ghost.Length; i++)
        {
            this.ghost[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);
        this.gameOverScreen.SetActive(true);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(maxNoOfLive);
        NewRound();
        for(int i = 0;i < icon.Length; i++)
        {
            icon[i].color = usedColor;
        }
        this.gameOverScreen.SetActive(false);
    }

    private void NewRound()
    {
        foreach(Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }

    public void ResetState()
    {
        ResetGhostMultipler();
        for(int i = 0; i < ghost.Length;i++)
        {
            this.ghost[i].ResetState();
        }
        this.pacman.ResetState();

    }
    
    private void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = score.ToString();
    }
    private void SetLives(int lives)
    {
        this.lives = lives;
    }
    private void ResetGhostMultipler()
    {
        this.ghostMultipler = 1;
    }
    #endregion

    #region Eaten Functions

    public void pacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);
        SetLives(this.lives - 1);
        Die();
        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    public void ghostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultipler;
        SetScore(this.score + points);
        ghostMultipler++;
    }

    public void pelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

        if (!HasRemainingPellet())
        {
            this.pacman.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void powerPelletEaten(PowerPellet powerPellet)
    {
        for (int i = 0; i < this.ghost.Length; i++)
        {
            this.ghost[i].frightened.Enable(powerPellet.duration);
        }
        pelletEaten(powerPellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultipler), powerPellet.duration);

    }

    public bool HasRemainingPellet()
    {
        foreach(Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }   
        }
        return false;
    }
    #endregion

    private void Die()
    {
        currentNoOfLive++;
        IconHandler(currentNoOfLive);

    }

    private void IconHandler(int NoOfDeath)
    {
        for(int i = 0;i < icon.Length; i++)
        {
            if(NoOfDeath == i + 1)
            {
                icon[i].color = iconColor;
                return;
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void returnToStartMenu()
    {
        SceneManager.LoadScene(0);
    }

   
}
