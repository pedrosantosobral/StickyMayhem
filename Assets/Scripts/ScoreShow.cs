using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomEventSystem;
using EZCameraShake;
public class ScoreShow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textHighScore;
    [SerializeField] private TextMeshProUGUI textHighScoreGameOver;
    [SerializeField] private VoidEvent stillAliveFeedback;

    [SerializeField] private int mainScore = 0;

    public Animator anim;

    public int score = 0;

    private void Start()
    {
        mainScore = PlayerPrefs.GetInt("highscore");
    }

    private void FixedUpdate()
    {

        if (text != null)
        {
            text.SetText(score.ToString());
        }

        if (textHighScore != null)
        {
            textHighScore.SetText(mainScore.ToString());
        }

        if (textHighScoreGameOver != null)
        {
            textHighScoreGameOver.SetText(score.ToString());
        }
    }
    public void Add1Score()
    {
        anim.SetTrigger("pop");
        score += 1;
        StartCoroutine("CameraShake");
        
    }

    public void Add10Score()
    {
        score += 10;
        stillAliveFeedback.Raise();
    }

    public void SaveMainScore()
    {
        PlayerPrefs.SetInt("highscore", mainScore);

        if(score > mainScore)
        {
            mainScore = score;
            //set new highscore and save it in player prefs
        }
    }

    public void ResetScore()
    {
        score = 0;
    }

    private IEnumerator CameraShake()
    {
        // Codigo aqui (antes de esperar)
        yield return new WaitForSeconds(0.185f);
        // Mais codigo aqui (depois de esperar)
        CameraShaker.Instance.ShakeOnce(3f, 3f, 0.2f, 0.2f);
    }
}
