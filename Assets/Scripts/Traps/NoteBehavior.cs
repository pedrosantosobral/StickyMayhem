using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class NoteBehavior : MonoBehaviour
{
    [SerializeField] private LeanTweenType note_out_type;
    [SerializeField] private float out_speed;
    [SerializeField] private float out_delay;

    public  MyColor color;

    public  Material[] noteColors;
    private Rigidbody rigidReference;
    
    public  GameObject mark;
    private GameObject _centerReference;
    private GameObject _markReference;

    private int _fallSpeed;

    public float timeToDisapear;

    private bool startTimerToDisapear;

    private GameObject playerReference;


    private void Awake()
    {
        _centerReference = gameObject.transform.GetChild(0).gameObject;
        rigidReference   = gameObject.GetComponent<Rigidbody>();
        _fallSpeed = 3;
        rigidReference.drag = _fallSpeed;
    }

    private void Start()
    {
        color     = (MyColor)Random.Range(0, 4);
        gameObject.GetComponent<MeshRenderer>().material = noteColors[(int)color];
        _markReference = Instantiate(mark, new Vector3(_centerReference.transform.position.x, 0.02f, _centerReference.transform.position.z), Quaternion.Euler(0f,Random.Range(-8,8),0f));
        _markReference.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        _markReference.GetComponent<FallMarker>().myNote = gameObject;
       
    }

    private void FixedUpdate()
    {
        IncreaseMarkSize();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mark"))
        {
            if(other.gameObject == _markReference)
            {
                Destroy(other.gameObject);
            }
        }

        if(other.CompareTag("Floor"))
        {
            
            LeanTween.scale(gameObject, new Vector3(0, 0, 0), out_speed).setEase(note_out_type).setDelay(out_delay).setOnComplete(DestroyMe);
            //Destroy(gameObject, 3);
        }

        if (other.CompareTag("Player"))
        {
            playerReference = other.gameObject;
            
            if(playerReference.GetComponentInParent<Player>().playerColor == color)
            {
                Destroy(_markReference);
                Destroy(transform.parent.gameObject);
                Destroy(gameObject,1f);
            }
        }
    }

    private void IncreaseMarkSize()
    {
        if(_markReference != null)
        _markReference.transform.localScale += new Vector3(0.0016f, 0.000f, 0.0016f);
    }

    private void DestroyMe()
    {
        Destroy(transform.parent.gameObject); 
    }
}
