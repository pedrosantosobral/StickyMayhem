using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEventSystem;

public class InstancesManager : MonoBehaviour
{
    public bool allowSpawn;
    public Color32[] colorList;
    [SerializeField] private VoidEvent  add10Score;
    [SerializeField] private GameObject noteToSpawn;

    public List<GameObject> objectsToDestroy;

    //grid reference;
    private GridGeneration   _gridReference;

    public float speedIncrease;
    public float timeBetweenSpeedIncrease;
    public float timeBetweenTraps;

    private float _trapTimer;
    private float _increaseSpeedTimer;

    private float _savedTimeBetweenTraps;

    public GameObject trap;
    public GameObject[] trapX2;
    public GameObject[] trapX3;

    public GameObject collectable;

    private void Start()
    {
        _savedTimeBetweenTraps = timeBetweenTraps;
        _gridReference = GameObject.Find("GridGenerator").GetComponent<GridGeneration>();
        //set timmers
        _trapTimer          = timeBetweenTraps;
        _increaseSpeedTimer = timeBetweenSpeedIncrease;
       
    }

    private void FixedUpdate()
    {
        if(allowSpawn == true)
        {
            TrapTimer();

            if (timeBetweenTraps >= 0.2)
            {
                IncreaseSpawnSpeed();
            }
        }  
    }


    private void TrapTimer()
    {
        _trapTimer -= Time.deltaTime;

        if (_trapTimer <= 0)
        {
            SpawnObject(trap);
            /*
            int i = Random.Range(0, 100);

            if (i <= 80)
            {
                SpawnObject(trap);
            }
            else if (i > 80 && i < 95)
            {
                SpawnObject(trapX2[Random.Range(0, trapX2.Length)]);
            }
            else if (i >= 95)
            {
                SpawnObject(trapX3[Random.Range(0, trapX3.Length)]);
            }
            */
            //reset timmer
            _trapTimer = timeBetweenTraps;
        }

    }

    private void SpawnObject(GameObject objectToSpawn)
    {
        objectsToDestroy.Add(Instantiate(objectToSpawn, new Vector3(
              Random.Range(0, _gridReference.columns) + _gridReference.leftBottomLocation.x
            , Random.Range(15, 10)
            , Random.Range(0, _gridReference.rows) + _gridReference.leftBottomLocation.z - 0.3f)
            , Quaternion.Euler(0, Random.Range(-6, 6), 0)));
    }



    private void IncreaseSpawnSpeed()
    {
        
        _increaseSpeedTimer -= Time.deltaTime;
        
        if(_increaseSpeedTimer <= 0)
        {
            timeBetweenTraps -= speedIncrease;

            add10Score.Raise();

            _increaseSpeedTimer = timeBetweenSpeedIncrease;
        }

    }

    public void StartSpawn()
    {
        allowSpawn = true;
    }

    public void StopSpawn()
    {
        allowSpawn = false;

    }

    public void ResetTrapsTimer()
    {
        timeBetweenTraps = _savedTimeBetweenTraps;
       
    }

    public void DestroyClones()
    {
        foreach(GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
           
        }
    }

    public void SpawnObjectOverPlayer(int x, int y)
    {
        if(allowSpawn == true)
        {
            objectsToDestroy.Add(Instantiate(noteToSpawn, new Vector3(
              x + _gridReference.leftBottomLocation.x
            , 8
            , y + _gridReference.leftBottomLocation.z - 0.3f)
            , Quaternion.Euler(0, 0, 0)));

            x = 0;
            y = 0;
        }
        
    }


}
