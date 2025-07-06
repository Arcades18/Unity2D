using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }
    private void NewGame()
    {
        AddCoin(0);
        AddLive(3);
        LoadLevel(1, 1);
    }
    private void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }
    public void nextLevel()
    {
        LoadLevel(world, stage + 1);
    }
    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }
    public void ResetLevel()
    {
        lives--;

        if(lives < 0)
        {
            LoadLevel(world,stage);
        }
        else
        {
            Gameover();
        }
    }
    private void Gameover()
    {
        NewGame();
    }
    public void AddCoin(int coin)
    {
       this.coins = coin;

        if(coins == 100)
        {
            AddLive(this.lives + 1);
            coins = 0;
        }
    }
    public void AddLive(int live)
    {
        this.lives = live;
    }
}
