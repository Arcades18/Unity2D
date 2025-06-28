using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int maxNumberOfShot = 3;
    public float waitForSecondsAfterLastShot = 3f;
    private int usedNumberOfshot;
    public static GameManager instance;
    private Iconhandler iconhandler;

    [SerializeField] private GameObject restartGame;
    [SerializeField] private Image NextLevel;
    [SerializeField] private SlingShotHandler SlingShotHandler;

    private List<Enemie> _Enemies = new List<Enemie>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        iconhandler = FindObjectOfType<Iconhandler>();

        Enemie[] enemies = FindObjectsOfType<Enemie>();
        for(int i = 0; i < enemies.Length; i++)
        {
            _Enemies.Add(enemies[i]);
        }
    }
    public void usedShot()
    {
        usedNumberOfshot++;
        iconhandler.UsedShot(usedNumberOfshot);

        checkForLastShot();
    }

    public bool hasEnoughShot()
    {
        if(usedNumberOfshot < maxNumberOfShot)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void checkForLastShot()
    {
        if( usedNumberOfshot == maxNumberOfShot )
        {
            StartCoroutine(waitAfterLastShot());
        }
    }

    private IEnumerator waitAfterLastShot()
    {
        yield return new WaitForSeconds(waitForSecondsAfterLastShot);
        if(_Enemies.Count == 0)
        {
            WinGame();
        }
        else
        {
            Restartscene();
        }
    }

    public void RemoveEneime(Enemie enemie)
    {
        _Enemies.Remove(enemie);
        checkForAllEnemieDead();
    }

    private void checkForAllEnemieDead()
    {
        if(_Enemies.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose Function

    private void WinGame()
    {

        restartGame.SetActive(true);
        SlingShotHandler.enabled = false;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = SceneManager.sceneCountInBuildSettings;
        if( currentSceneIndex < nextLevel)
        {
            NextLevel.enabled = true;
        }
    }

    public void Restartscene()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevels()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
