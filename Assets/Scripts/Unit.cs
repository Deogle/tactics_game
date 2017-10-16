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
        Debug.Log("Listing");
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
                Debug.Log("logging new path of length " + pPath.Count);
                int currentNode = 1;
                while (currentNode < pPath.Count)
                {
                    map.referenceTiles[(int)pPath[currentNode].x, (int)pPath[currentNode].y].HighlightPossibleMove();

                    currentNode++;
                }
            }
        }
    }


    private void Update()
    {
        /*if(currentPath != null)
        {
            int currentNode = 0;
            while(currentNode < currentPath.Count-1)
            {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currentNode].x,currentPath[currentNode].y);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currentNode+1].x, currentPath[currentNode+1].y);

                Debug.DrawLine(start,end, Color.red);


                currentNode++;
            }
        }*/
    }

    public void MoveNextTile()
    {
        
    }


}
