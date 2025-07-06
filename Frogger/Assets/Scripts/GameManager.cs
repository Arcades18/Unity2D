using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Home[] homes;
    private Frogger frogger;
    private int score;
    private int lives;
    private int time;
    public GameObject gameOverMenu;
    public Text scoreText;
    public Text livesText;
    public Text timeText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }

    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
    }
    private void NewLevel()
    {
        for(int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        NewRound();
    }
    private void NewRound()
    {
        Respawn();
    }

    private void Respawn()
    {
        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        time = duration; 
        timeText.text = time.ToString();

        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            timeText.text = time.ToString();
        }
        frogger.Death();
    }

    public void HomeOccupied()
    {
        frogger.gameObject.SetActive(false);

        int bonusPoints = time * 20;
        SetScore(score + 50 + bonusPoints);

        if (Cleared())
        {
            SetScore(score + 1000);
            Invoke(nameof(NewLevel),1.0f);
        }
        else
        {
            Invoke(nameof(NewRound), 1.0f);
        }
    }
    private bool Cleared()
    {
        for(int i = 0;i < homes.Length; i++)
        {
            if (!homes[i].enabled)
            {
                return false;
            }
        }
        return true;
    }

    public void Died()
    {
        SetLives(lives - 1);
        if(lives > 0)
        {
            Invoke(nameof(Respawn), 1.0f);
        }
        else
        {
            Invoke(nameof(Gameover), 1.0f);
        }
    }
    private void Gameover()
    {
        frogger.gameObject.SetActive(false );
        gameOverMenu.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(CheckForInput());
    }
    private IEnumerator CheckForInput() 
    {
        bool playAgain = false;
        while (!playAgain) 
        {
            if(Input.GetKeyDown(KeyCode.Return)) 
            {
                playAgain = true;
            }
            yield return null;
        }
        NewGame();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}
