using UnityEngine;

public class Colectable : MonoBehaviour
{
    private Rigidbody  rigidReference;
    private ScoreShow scoreReference;

    private int _fallSpeed;

    public float timerToDestroy;
    public float timerToColect;

    private bool _startTimerToDestroy;
    private bool _startTimerToColect;

    private GameObject playerReference;


    private void Awake()
    {
        rigidReference = gameObject.GetComponent<Rigidbody>();
        scoreReference = GameObject.Find("Score").GetComponent<ScoreShow>();

        _fallSpeed = 2; //Random.Range(3, 6);
        rigidReference.drag = _fallSpeed;

    }

    private void FixedUpdate()
    {
        if(_startTimerToDestroy == true)
        {
            Timer();
        }

        if(_startTimerToColect == true)
        {
            TimerToColect();
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            _startTimerToColect = true;
        }

        if (other.CompareTag("Player"))
        {
            _startTimerToDestroy = true;
            scoreReference.score += 1;     
        }
    }

    private void Timer()
    {
        timerToDestroy -= Time.deltaTime;

        if (timerToDestroy <= 0)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }

    }
    private void TimerToColect()
    {
        timerToColect -= Time.deltaTime;

        if (timerToColect <= 0)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }

    }

}
