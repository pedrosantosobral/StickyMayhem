using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventSystem;
using EZCameraShake;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip pop;
    [SerializeField] private float timeBetweenFootsteps;
    [SerializeField] private VoidEvent onPlayerNoteTouch;
    [SerializeField] private VoidEvent addScore;
    [SerializeField] private VoidEvent playerDeath;
    [SerializeField] private VoidEvent stopSpawn;
    [SerializeField] private float mapbasePopSpeed;
    [SerializeField] private LeanTweenType mapPopAnimation;

    [SerializeField] private float footSteps_Volume;
    [SerializeField] private AudioClip[] explosionClips;
    [SerializeField] private AudioClip[] runSounds;

    public bool playerUnlocked = false;

    public MyColor playerColor;
    private MyColor nextToAvoid;
    private SkinnedMeshRenderer _playerColor;
    private MeshRenderer        _playerBaseColor;
    public MeshRenderer         mapBase;
    public MeshRenderer         mapBaseLower;
    public GameObject           hideSpot;

    private GameObject          _noteReference;
    private int                 _dificultyResetCounter = 0;

    public Material[] playerColors;

    public Animator   animator;
    public GameObject meshToCenter;
    public GameObject explosion;

    [SerializeField] private float _timeLeftToSpawnNote;
    [SerializeField] private float _actualTimeLeft;

    public int actualX = 0;
    public int actualY = 0;
    private int checkX = 0;
    private int checkY = 0;
    private bool playingfootSteps = false;

    public float moveSpeed;

    public  bool running = false;
    private bool moveUp,moveDown,moveRight,moveLeft;

    public SwipeInput       inputReference;
    public GridGeneration   gridReference;

    public InstancesManager instancesManager;
         
    // Start is called before the first frame update
    void Start()
    {
        _actualTimeLeft = _timeLeftToSpawnNote;
        _playerColor     = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _playerBaseColor = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        
        running = false;

        int pickColor = Random.Range(0, playerColors.Length);

        playerColor = (MyColor)Random.Range(0, 4);
        nextToAvoid = playerColor;
        mapBase.material = playerColors[(int)playerColor];
        mapBaseLower.material = playerColors[(int)playerColor];

    }

    // Update is called once per frame
    void  FixedUpdate()
    {

        if(playerUnlocked)
        {
            //change player color realtime
            if (_playerColor != null && _playerBaseColor != null)
            {
                _playerColor.material = playerColors[(int)playerColor];
                _playerBaseColor.material = playerColors[(int)playerColor];
                

                //fix transform change due to animation
                meshToCenter.transform.position = transform.position;

                animator.SetBool("Running", running);

                //start and stop animation
                if (inputReference.swipeUp == true || inputReference.swipeDown == true || inputReference.swipeLeft == true || inputReference.swipeRight == true)
                {
                    running = true;
                    if (!playingfootSteps)
                    {
                        StartCoroutine("PlayFootstepsSound");
                    }
                }
                else
                {
                    running = false;
                    
                }
                //check for inputs and validate that input
                if (inputReference.swipeUp == true && actualY + 1 < gridReference.rows)
                {
                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY].transform.position) < 0.001f)
                    {
                        moveUp = true;
                    }
                }

                if (inputReference.swipeDown == true && actualY - 1 >= 0)
                {
                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY].transform.position) < 0.001f)
                    {
                        moveDown = true;
                    }
                }

                if (inputReference.swipeRight == true && actualX + 1 < gridReference.columns)
                {
                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY].transform.position) < 0.001f)
                    {
                        moveRight = true;
                    }
                }

                if (inputReference.swipeLeft == true && actualX - 1 >= 0)
                {
                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY].transform.position) < 0.001f)
                    {
                        moveLeft = true;
                    }
                }

                //Actualy move the player 

                if (moveUp == true)
                {

                    moveDown = false;
                    moveLeft = false;
                    moveRight = false;

                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.y = 0;
                    transform.rotation = Quaternion.Euler(rotationVector);

                    transform.position = Vector3.MoveTowards(transform.position, gridReference.gridArray[actualX, actualY + 1].transform.position, moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY + 1].transform.position) < 0.001f)
                    {
                        actualY += 1;
                        moveUp = false;
                    }
                }

                if (moveDown == true)
                {
                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.y = 180;
                    transform.rotation = Quaternion.Euler(rotationVector);

                    moveUp = false;
                    moveLeft = false;
                    moveRight = false;

                    transform.position = Vector3.MoveTowards(transform.position, gridReference.gridArray[actualX, actualY - 1].transform.position, moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX, actualY - 1].transform.position) < 0.001f)
                    {
                        actualY -= 1;
                        moveDown = false;
                    }
                }

                if (moveRight == true)
                {
                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.y = 90;
                    transform.rotation = Quaternion.Euler(rotationVector);

                    moveUp = false;
                    moveLeft = false;
                    moveDown = false;

                    transform.position = Vector3.MoveTowards(transform.position, gridReference.gridArray[actualX + 1, actualY].transform.position, moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX + 1, actualY].transform.position) < 0.001f)
                    {
                        actualX += 1;
                        moveRight = false;
                    }
                }

                if (moveLeft == true)
                {
                    var rotationVector = transform.rotation.eulerAngles;
                    rotationVector.y = -90;
                    transform.rotation = Quaternion.Euler(rotationVector);

                    moveUp = false;
                    moveRight = false;
                    moveDown = false;

                    transform.position = Vector3.MoveTowards(transform.position, gridReference.gridArray[actualX - 1, actualY].transform.position, moveSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, gridReference.gridArray[actualX - 1, actualY].transform.position) < 0.001f)
                    {
                        actualX -= 1;
                        moveLeft = false;

                    }
                }

                _actualTimeLeft -= Time.deltaTime;
               
                if (_actualTimeLeft < 0)
                {
                    instancesManager.SpawnObjectOverPlayer(actualX, actualY);
                    _actualTimeLeft = _timeLeftToSpawnNote;
                }

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            _noteReference = other.gameObject;

            if (_noteReference.GetComponentInParent<NoteBehavior>().color == playerColor)
            {
                while(playerColor == nextToAvoid)
                {
                    playerColor = (MyColor)Random.Range(0, 4);
                    mapBase.material = playerColors[(int)playerColor];
                    LeanTween.scale(mapBase.gameObject, new Vector3(0, 0, 0),0).setOnComplete(ScaleFloor);
                }

                nextToAvoid = playerColor;


                addScore.Raise();
                AudioManager.Instance.PlaySFX(pop,0.7f);

            }
            else if(_noteReference.GetComponentInParent<NoteBehavior>().color != playerColor)
            {
                Instantiate(explosion,gameObject.transform.position,Quaternion.Euler(-90,0,0));
                HidePlayer();
                playerDeath.Raise();
                stopSpawn.Raise();
                CameraShaker.Instance.ShakeOnce(15f, 30f, 0.3f, 2f);
                AudioManager.Instance.PlaySFX(explosionClips[Random.Range(0,explosionClips.Length)]);
            }
        }
    }

    public void SetPlayer()
    {
        playerUnlocked = true;
        gameObject.transform.position = gridReference.gridArray[actualX, actualY].transform.position;
    }

    public void HidePlayer()
    {
        playerUnlocked = false;
        gameObject.transform.position = hideSpot.transform.position;
    }

    public void ScaleFloor()
    {
        LeanTween.scale(mapBase.gameObject, new Vector3(1, 1, 2), mapbasePopSpeed).setEase(mapPopAnimation).setOnComplete(SetLowerBaseColor);
    }

    public void SetLowerBaseColor()
    {
        mapBaseLower.material = playerColors[(int)playerColor];
    }
    IEnumerator PlayFootstepsSound()
    {
        playingfootSteps = true;
        AudioManager.Instance.PlaySFX(runSounds[Random.Range(0, runSounds.Length)], footSteps_Volume);
        yield return new WaitForSeconds(timeBetweenFootsteps);
        playingfootSteps = false;
    }


}
