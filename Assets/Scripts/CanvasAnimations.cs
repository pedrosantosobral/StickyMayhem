using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimations : MonoBehaviour
{
    [SerializeField] private LeanTweenType animIN;
    [SerializeField] private LeanTweenType animOUT;

    [SerializeField] private float animSpeed;
    [SerializeField] private float howToPlayAnimSpeed;

    [SerializeField] private GameObject stickyString;
    [SerializeField] private GameObject mayhemString;
    [SerializeField] private CanvasGroup blackPannel;
    [SerializeField] private GameObject howToPannel;



    private void Start()
    {
        LeanTween.alphaCanvas(blackPannel, 0f, 0.4f).setOnComplete(AnimateMainScreenUI);
        LeanTween.scale(howToPannel, new Vector3(0, 0, 0), 0f);
    }

    private void AnimateMainScreenUI()
    {
        LeanTween.scale(stickyString, new Vector3(1, 1, 1), animSpeed).setEase(animIN);
        LeanTween.scale(mayhemString, new Vector3(1, 1, 1), animSpeed).setEase(animIN);
    }

    private void FadeOUT()
    {
        LeanTween.alphaCanvas(blackPannel, 0f, 0.4f);
    }
    private void FadeIN()
    {
        LeanTween.alphaCanvas(blackPannel, 1f, 0.4f);
    }

    public void OpenHowTO()
    {
      LeanTween.scale(howToPannel, new Vector3(1, 1, 1), howToPlayAnimSpeed).setEase(animIN);
        Debug.Log("adadada");
        
    }

    public void CloseHowTo()
    {
        LeanTween.scale(howToPannel, new Vector3(0, 0, 0), howToPlayAnimSpeed).setEase(animOUT);
        Debug.Log("Isclosign");
    }

    public void CancelTweens()
    {
        LeanTween.cancel(gameObject);
    }







}
