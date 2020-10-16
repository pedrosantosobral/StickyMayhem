using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridbehaviorBackup : MonoBehaviour
{

    public GridGeneration gridReference;

    public bool findDistance = false;

    public int rows = 7;
    public int columns = 7;
    public int scale = 1;

    public int startX = 0;
    public int startY = 0;
    public int endX = 0;
    public int endY = 0;

    public GameObject gridPrefab;
    public GameObject[,] gridArray;

    public List<GameObject> path = new List<GameObject>();


    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);

    private void Awake()
    {

        gridArray = new GameObject[columns, rows];

        if (gridPrefab)
        {
            GenerateGrid();
        }
        else
        {
            print("missing grid prefab, please assign");
        }
    }

    private void Update()
    {
        if (findDistance == true)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    public void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i,
                leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);

                obj.transform.SetParent(gameObject.transform);

                //Set grid position values in each sell
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;

                //Assign objects to the grid
                gridArray[i, j] = obj;

            }
        }
    }

    //Set all grid cells as not visited, exept the starting position
    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStats>().visited = 0;
    }

    /// <summary>
    /// Give values to each cell acording to the distance of the start position
    /// </summary>
    void SetDistance()
    {
        //set initial position and empty search
        InitialSetup();

        //set starting position for the search
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];

        //set value of each cell and set value acording to the target
        for (int step = 1; step < rows * columns; step++)
        {
            //check every cell
            foreach (GameObject obj in gridArray)
            {
                //if a cell was not  visited
                if (obj && obj.GetComponent<GridStats>().visited == step - 1)
                {
                    //start on the start position and give a cell a step cost acording to the distance of the starting position
                    //more iteration,more the cost
                    TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);
                }
            }
        }
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        //int direction tells witch case to use 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }

        }

        return false;
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
        {
            gridArray[x, y].GetComponent<GridStats>().visited = step;
        }
    }

    void TestFourDirections(int x, int y, int step)
    {
        //last parameter of TestDirection is the direction 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
        if (TestDirection(x, y, -1, 1))
        {
            SetVisited(x, y + 1, step);
        }
        if (TestDirection(x, y, -1, 2))
        {
            SetVisited(x + 1, y, step);
        }
        if (TestDirection(x, y, -1, 3))
        {
            SetVisited(x, y - 1, step);
        }
        if (TestDirection(x, y, -1, 4))
        {
            SetVisited(x - 1, y, step);
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;

        List<GameObject> tempList = new List<GameObject>();

        //clear previous used paths
        path.Clear();

        //check if end exists and was found
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
        {
            //add cell to path array
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            print("Path to target was not found");
            return;
        }

        for (int i = step; step > -1; step--)
        {

            //last parameter of TestDirection is the direction 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
            if (TestDirection(x, y, step, 1))
            {
                tempList.Add(gridArray[x, y + 1]);
            }

            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(gridArray[x + 1, y]);
            }

            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(gridArray[x, y - 1]);
            }

            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(gridArray[x - 1, y]);
            }

            //get closest optimal cell
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);

            //add to the path
            path.Add(tempObj);

            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;

            tempList.Clear();
        }

    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        //set max possible distance
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;

        for (int i = 0; i < list.Count; i++)
        {
            //Get optimal cell, with the shortest distance;
            //Checks between the end goal and current place
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                //set new shorter distance
                indexNumber = i;
            }
        }
        //reeturn closer distance
        return list[indexNumber];
    }
}
