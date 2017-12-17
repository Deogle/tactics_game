using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public float unitX;
    public float unitY;
    public float movementPoints;
    public GameManager map;

    public bool hasMoved = false;

    public List<List<Node>> possiblePaths;

    public List<Node> currentPath = null;

    private void OnMouseUp()
    {
        if (!hasMoved)
        {
            if (map.selectedUnit == null)
            {
                SelectUnit();
                map.selectedUnit = this.gameObject;
                FindPossibleMoves();
            }
            else if (map.selectedUnit == this.gameObject)
            {
                DeselectUnit();
                map.ClearAllMoves();
                possiblePaths = null;
            }
        }
    }

    //Build a list of all possible paths with current movement points
    public void FindPossibleMoves()
    {
        possiblePaths = new List<List<Node>>();

        //incredibly inefficient method of finding possible moves.
        //could constrain it to look at most movementPoints tiles away
        //but this works in acceptable time given small grids
        /*for(int x = 0; x < map.sizeX; x++)
        {
            for(int y = 0; y < map.sizeY; y++)
            {
               map.GeneratePathTo(x, y);        
            }
        }*/

        //New constraints - check to the up and right of unit
        //then check to the left and down. this should eliminate
        //having to check every tile on the map
        
        //up and right
        for(int x = (int)unitX; x < unitX + movementPoints && x < map.sizeX; x++)
        {
            for(int y = (int)unitY; y < unitY + movementPoints && y < map.sizeY; y++)
            {
                if(y >= 0 && x >= 0 && x < map.sizeX && y < map.sizeY)
                {
                    map.GeneratePathTo(x, y);
                }
               
            }
        }
        //down and left
        for (int x = (int)(unitX - movementPoints); x <= unitX+movementPoints; x++)
        {
            for(int y = (int)(unitY-movementPoints); y <= unitY+movementPoints; y++)
            {
                //Debug.Log(x + "," + y);
                if(y >= 0 && x >= 0 && x < map.sizeX && y < map.sizeY)
                {
                    map.GeneratePathTo(x, y);
                }
            }
        }
        

        foreach(List<Node> pPath in possiblePaths)
        {
            //Debug.Log("logging new path of length "+pPath.Count);
            if (pPath != null)
            {
                int currentNode = 1;
                while (currentNode < pPath.Count)
                {
                    map.referenceTiles[(int)pPath[currentNode].x, (int)pPath[currentNode].y].HighlightPossibleMove();

                    currentNode++;
                }
            }
        }
    }

    public void MakeCurrentPath(float x, float y)
    {
        possiblePaths.Clear();
        hasMoved = true;
        //Debug.Log("Pathing to " + x+"," + y);
        map.GeneratePathTo(x, y);
        //Debug.Log("Lenght of possiblePaths: " + possiblePaths.Count);
        currentPath = possiblePaths[0];
        //Debug.Log("Length of new currentPath " + currentPath.Count);
        MoveToSelection();
        DeselectUnit();
    }

    public void MoveToSelection()
    {
        map.MoveOffTile(unitX, unitY);
        int currentNode = 1;
        while(currentNode < currentPath.Count)
        {
            transform.position = new Vector3(currentPath[currentNode].x, currentPath[currentNode].y, 0);
            unitX = transform.position.x;
            unitY = transform.position.y;
            currentNode++;
        }
        map.MoveToTile(unitX, unitY);
    }

    private void SelectUnit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void DeselectUnit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        map.selectedUnit = null;
    }
}
