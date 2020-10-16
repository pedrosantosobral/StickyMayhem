using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAIBehavior : MonoBehaviour
{

    public GridGeneration gridReference;

    private int _rows;
    private int _columns;
    private int _scale;

    float movespeed = 2f;

    public bool findDistance = false;

    public int startX   = 0;
    public int startY   = 0;

    public int endX     = 0;
    public int endY     = 0;

    public List<GameObject> path = new List<GameObject>();

    public GameObject[,] gridCopy;

    private void Start()
    {
        
       // gridCopy = gridReference.gridArray;
        _rows    = gridReference.rows;
        _columns = gridReference.columns;
        _scale   = gridReference.scale;

        gridCopy = new GameObject[_rows, _columns];

        DeepCopyArray(gridReference.gridArray, gridCopy);

        transform.position = gridReference.gridArray[startX, startY].transform.position;
    }

    void DeepCopyArray(GameObject[,] arrayToCopy, GameObject[,] arrayToPaste)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                arrayToPaste[i, j] = Instantiate(arrayToCopy[i, j], new Vector3(0 + _scale * i, 0, 0 + _scale * j), Quaternion.identity);
                arrayToPaste[i, j].transform.SetParent(gameObject.transform);

            }
            
        }
    }

    private void Update()
    {
        if(findDistance == true)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }

        foreach(GameObject cell in path)
        {
           transform.position =  Vector3.MoveTowards(gameObject.transform.position, cell.transform.position, movespeed);
        }
    }


    //Set all grid cells as not visited, exept the starting position
    void InitialSetup()
    {
        foreach(GameObject obj in gridCopy)
        {
            obj.GetComponent<GridStats>().visited = -1;
        }
        gridCopy[startX, startY].GetComponent<GridStats>().visited = 0;
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
        int[] testArray = new int[_rows * _columns];

        //set value of each cell and set value acording to the target
        for(int step = 1; step < _rows * _columns; step ++)
        {
            //check every cell
            foreach(GameObject obj in gridCopy)
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
        switch(direction)
        {
            case 1:
                if (y + 1 < _rows && gridCopy[x, y + 1] && gridCopy[x, y + 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (x + 1 < _columns && gridCopy[x + 1, y] && gridCopy[x + 1, y].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (y - 1 > - 1 && gridCopy[x, y - 1] && gridCopy[x, y - 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if (x - 1 > - 1 && gridCopy[x - 1, y] && gridCopy[x - 1, y].GetComponent<GridStats>().visited == step)
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
        if(gridCopy[x,y])
        {
            gridCopy[x, y].GetComponent<GridStats>().visited = step;
        }
    }

    void TestFourDirections(int x, int y, int step)
    {
        //last parameter of TestDirection is the direction 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
        if (TestDirection(x,y,-1,1))
        {
            SetVisited(x, y + 1, step);
        }
        if (TestDirection(x, y,-1, 2))
        {
            SetVisited(x + 1, y, step);
        }
        if (TestDirection(x, y, -1, 3))
        {
            SetVisited(x, y - 1, step);
        }
        if (TestDirection(x, y, -1, 4))
        {
            SetVisited(x - 1, y , step);
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
        if(gridCopy[endX,endY] && gridCopy[endX,endY].GetComponent<GridStats>().visited > 0)
        {
            //add cell to path array
            path.Add(gridCopy[x, y]);
            step = gridCopy[x, y].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            print("Path to target was not found");
            return;
        }

        for(int i = step; step > -1; step--)
        {

            //last parameter of TestDirection is the direction 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
            if (TestDirection(x,y,step,1))
            {
                tempList.Add(gridCopy[x, y + 1]);
            }

            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(gridCopy[x +1, y]);
            }

            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(gridCopy[x, y - 1]);
            }

            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(gridCopy[x - 1, y]);
            }

            //get closest optimal cell
            GameObject tempObj = FindClosest(gridCopy[endX, endY].transform, tempList);

            //add to the path
            path.Add(tempObj);

            x = tempObj.GetComponent<GridStats>().x;
            y = tempObj.GetComponent<GridStats>().y;

            tempList.Clear();
        }

    }

    GameObject FindClosest(Transform targetLocation,List<GameObject> list)
    {
        //set max possible distance
        float currentDistance = _scale * _rows * _columns;
        int indexNumber = 0;
        
        for(int i = 0; i < list.Count; i++)
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
