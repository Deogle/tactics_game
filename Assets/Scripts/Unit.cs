using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public float unitX;
    public float unitY;
    public float movementPoints;
    public Map map;

    public List<List<Node>> possiblePaths;

    public List<Node> currentPath = null;

    private void OnMouseUp()
    {
        Debug.Log("Moving Unit: " + this.gameObject.tag);
        SelectUnit();
        map.selectedUnit = this.gameObject;
        FindPossibleMoves();
    }

    //Build a list of all possible paths with current movement points
    public void FindPossibleMoves()
    {
        possiblePaths = new List<List<Node>>();

        //fairly inefficient method of finding possible moves.
        //could constrain it to look at most movementPoints tiles away
        //but this works in acceptable time given small grids
        for(int x = 0; x < map.sizeX; x++)
        {
            for(int y = 0; y < map.sizeY; y++)
            {
               map.GeneratePathTo(x, y);        
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
        Debug.Log("Pathing to " + x+"," + y);
        map.GeneratePathTo(x, y);
        Debug.Log("Lenght of possiblePaths: " + possiblePaths.Count);
        currentPath = possiblePaths[0];
        Debug.Log("Length of new currentPath " + currentPath.Count);
        MoveToSelection();
        DeselectUnit();
    }

    public void MoveToSelection()
    {
        int currentNode = 1;
        while(currentNode < currentPath.Count)
        {
            transform.position = new Vector3(currentPath[currentNode].x, currentPath[currentNode].y, 0);
            unitX = transform.position.x;
            unitY = transform.position.y;
            currentNode++;
        }
        
    }

    private void SelectUnit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void DeselectUnit()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
