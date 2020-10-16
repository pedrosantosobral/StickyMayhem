using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventSystem;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private VoidEvent spawnPlayer;
    [SerializeField] private VoidEvent spawnNotes;

    public int rows = 7;
    public int columns = 7;
    public int scale = 1;

    public GameObject gridPrefab;
    public GameObject[,] gridArray;

    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    private void Start()
    {
        gridArray = new GameObject[columns, rows];
    }
    public void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj =Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i,
                leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);

                obj.transform.SetParent(gameObject.transform);

                //Set grid position values in each sell
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;

                //Assign objects to the grid
                gridArray[i, j] = obj;

            }
        }

        spawnPlayer.Raise();
        spawnNotes.Raise();
    }

    public void DestroyGrid()
    {
        foreach(GameObject obj in gridArray)
        {
            Destroy(obj);
        }
    }

}
