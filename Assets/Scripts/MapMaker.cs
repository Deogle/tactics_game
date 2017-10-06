using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour {

    public float cols;
    public float rows;

    public GameObject grassTile;
    public GameObject woodTile;

    private Transform boardHolder;
    private List<List<Vector3>> gridPositions = new List<List<Vector3>>();
    private List<Vector3> positionList;

    private void FillGrid()
    {       
        for (float x = 0; x < (0.63f * cols); x += 0.63f)
        {
            positionList = new List<Vector3>();
            for (float y = 0; y < (0.63f * rows); y += 0.63f)
            {
                Vector3 v = new Vector3(x, y, 0f);
                positionList.Add(v);
            }
            gridPositions.Add(positionList);
        }

        int countx = 0;
        int county = 0;
        foreach(List<Vector3> list in gridPositions)
        {
            county = 0;
            foreach(Vector3 v in list)
            {
                county++;
            }
            countx++;
        }
    }

    //Creates new board of col x row size
    public void BoardSetup()
    {
        //Add Rows and Columns for Border
        cols += 2;
        rows += 2;
        
        FillGrid();
        boardHolder = new GameObject("Board").transform;
        int countX = 0;
        int countY = 0;
               

        for(float x = 0; x < (0.63f*cols); x+=0.63f)
        {
            countX++;
            countY = 0;
            for (float y = 0; y < (0.63f * rows); y += 0.63f)
            {
                countY++;
                if (x == 0 || countX == cols || y == 0 || countY == rows)
                {
                    GameObject grass = Instantiate(grassTile, new Vector3(x, y, 0f), Quaternion.identity);
                    grass.transform.SetParent(boardHolder);
                } else
                {
                    GameObject wood = Instantiate(woodTile, new Vector3(x, y, 0f), Quaternion.identity);
                    wood.transform.SetParent(boardHolder);
                }
            }
        }
    }

    //Returns center of new board
    public Vector3 GetCenter()
    {
        Vector3 center = new Vector3( ((cols*0.63f)/2) , ((rows * 0.63f) / 2), -10f);
        return center;
    }

    //Insansiate unit GameObject at gridPosition
    public void SpawnUnit(GameObject unit, int x, int y)
    {
        unit.GetComponent<Player>().x = x;
        unit.GetComponent<Player>().y = y;
        Instantiate(unit, gridPositions[x][y], Quaternion.identity);  
    }

    public void MoveUnit(GameObject unit, int x, int y)
    {
        Vector3 newLoc = gridPositions[x][y];
        MovingObject unitMov = unit.GetComponent<MovingObject>();
        unitMov.Move(newLoc);
    }
   
	
}
