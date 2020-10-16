using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomEventSystem;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private AudioClip pop;
    [SerializeField] private GameObject inGamePannel;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private VoidEvent changeGameOverText;


    public int addsInterval;

    public UnityAdds AddsRef;

    public float timeToGameOverScreen;

    private int addsIntervalCounter;

    private void Start()
    {
        addsIntervalCounter = addsInterval;
    }
    public void EnableInGamePannel()
    {
        inGamePannel.SetActive(true);
        DisablePauseMenu();
        DisableMenu();
        DisableGameOverMenu();
    }

    public void EnableMenu()
    {
        AudioManager.Instance.PlaySFX(pop);
        menu.SetActive(true);
        DisablePauseMenu();
        DisableInGamePannel();
        DisableGameOverMenu();
    }
    public void EnablePauseMenu()
    {
        AudioManager.Instance.PlaySFX(pop);
        pauseMenu.SetActive(true);
        DisableInGamePannel();
        DisableMenu();
        DisableGameOverMenu();
    }

    public void EnableGameOverMenu()
    {
        Invoke("GameOverMenu", timeToGameOverScreen);
    }

    public void DisableGameOverMenu()
    {
        gameOverMenu.SetActive(false);
    }

    public void DisableInGamePannel()
    {
        inGamePannel.SetActive(false);

    }
    public void DisableMenu()
    {
        menu.SetActive(false);
    }
    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
        DisableInGamePannel();
        DisableMenu();
        DisablePauseMenu();
        changeGameOverText.Raise();

        //adds part
        addsIntervalCounter -= 1;
        if(addsIntervalCounter <= 0)
        {
            AddsRef.DisplayInterstitialAD();
            addsIntervalCounter = addsInterval;
        }

        
       


    }

    public void PlayPop()
    {
        AudioManager.Instance.PlaySFX(pop,0.7f);
    }

    


}
