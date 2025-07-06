using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Ball ball;
    private int playerScore;
    private int computerScore;
    public Text PlayerText;
    public Text ComputerText;


    public void playerscore()
    {
        playerScore++;
        PlayerText.text = playerScore.ToString(); 
        this.ball.ResetPosition();
    }
    public void computerscore()
    {
        computerScore++;
        ComputerText.text = computerScore.ToString();
        this.ball.ResetPosition();
    }
    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
