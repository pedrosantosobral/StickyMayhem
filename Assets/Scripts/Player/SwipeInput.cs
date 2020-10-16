using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    #region Instance

    private static      SwipeInput _instance;
    public  static SwipeInput Instance
    {
        get
        {
            _instance = FindObjectOfType<SwipeInput>();

            if(_instance == null)
            {
                _instance = new GameObject("New_SwipeInput", typeof(SwipeInput)).GetComponent<SwipeInput>();
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    #endregion

    [Header("Tweaks")]

    [SerializeField] private float _deadZone = 100.0f;
    [SerializeField] private float _doubleTapDelta = 0.5f;

    [Header("Logic")]

    public  bool    tap, doubleTap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;
    private float   lastTap;
    private float   sqrDeadzone;

    #region Public Properties

    public bool     Tap        { get { return tap; } }
    public bool     DoubleTap  { get { return doubleTap; } }
    public Vector2  SwipeDelta { get { return swipeDelta; } }
    public bool     SwipeUp    { get { return swipeUp; } }
    public bool     SwipeDown  { get { return swipeDown; } }
    public bool     SwipeRight { get { return swipeRight; } }
    public bool     SwipeLeft  { get { return swipeLeft; } }

    #endregion

    private void Start()
    {
        sqrDeadzone = _deadZone * _deadZone;
    }

    private void Update()
    {
        //Reset bools
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                //tap = false;
                //doubleTap = false;
                swipeRight = false;
                swipeLeft = false;
                swipeDown = false;
                swipeUp = false;
            }

        }

        tap = false;
        doubleTap = false;
        //ORIGINAL CODE
        /*
        
        swipeRight = false;
        swipeLeft = false;
        swipeDown = false;
        swipeUp = false;
        */


        UpdateMobile();

#if UNITY_EDITOR
        UpdateStandalone();
#else
        
#endif

    }

    private void UpdateStandalone()
    {
        if(Input.GetMouseButtonDown(0))
        {
            tap        = true;
            startTouch = Input.mousePosition;
            doubleTap  = Time.time - lastTap < _doubleTapDelta;
            lastTap    = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }

        //Reset distance, gett the new swipe delta
        swipeDelta = Vector2.zero;

        if(startTouch != Vector2.zero && Input.GetMouseButton(0))
        {
            swipeDelta = (Vector2)Input.mousePosition - startTouch;  
        }

        //check if delta is bigger than deadzone, validate the swipe as big enough
        if(swipeDelta.sqrMagnitude > sqrDeadzone)
        {
            //if yes confirm swipe and get the direction

            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right
                if(x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                //left or right
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }

            //reset values to avoid more than 1 swipe
            startTouch = swipeDelta = Vector2.zero;
        }

    }

    private void UpdateMobile()
    {
        if(Input.touches.Length != 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
                doubleTap = Time.time - lastTap < _doubleTapDelta;
                lastTap = Time.time;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
               startTouch = swipeDelta = Vector2.zero;
            }
        }

        //Reset distance, gett the new swipe delta
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.touches.Length != 0)
        {
            swipeDelta = Input.touches[0].position - startTouch;
        }

        //check if delta is bigger than deadzone, validate the swipe as big enough
        if (swipeDelta.sqrMagnitude > sqrDeadzone)
        {
            //if yes confirm swipe and get the direction

            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right
                if (x < 0)
                {
                    swipeLeft = true;
                    //Debug.Log("swipeLeft");
                }
                else
                {
                    swipeRight = true;
                   //Debug.Log("swipeRight");
                }
            }
            else
            {
                //left or right
                if (y < 0)
                {
                    swipeDown = true;
                    //Debug.Log("swipeDown");
                }
                else
                {
                    swipeUp = true;
                    //Debug.Log("swipeUP");
                }
            }

            //reset values to avoid more than 1 swipe
            startTouch = swipeDelta = Vector2.zero;
        }
    }


}
