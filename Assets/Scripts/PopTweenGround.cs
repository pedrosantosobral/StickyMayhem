using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTweenGround : MonoBehaviour
{
    [SerializeField] private LeanTweenType IN_easeType;
    [SerializeField] private LeanTweenType OUT_easeType;

    [SerializeField] private float betweenAnimationsDelay;
    [SerializeField] private float IN_animSpeed;
    [SerializeField] private float OUT_animSpeed;
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        InAnim();
    }

    public void InAnim()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), IN_animSpeed).setEase(IN_easeType);
    }
    private void OutAnim()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), OUT_animSpeed).setEase(OUT_easeType).setDelay(betweenAnimationsDelay);
    }

}
