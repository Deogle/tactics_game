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

    public void FindPossibleMoves()
    {
        possiblePaths = new List<List<Node>>();
        for(int x = 1; x < map.sizeX; x++)
        {
            for(int y = 1; y < map.sizeY; y++)
            {
               map.GeneratePathTo(x, y);        
            }
        }

        foreach(List<Node> pPath in possiblePaths)
        {
            if (pPath != null)
            {
                int currentNode = 0;
                while (currentNode < pPath.Count - 1)
                {
                    Vector3 start = map.TileCoordToWorldCoord(pPath[currentNode].x, pPath[currentNode].y);
                    Vector3 end = map.TileCoordToWorldCoord(pPath[currentNode + 1].x, pPath[currentNode + 1].y);

                    Debug.DrawLine(start, end, Color.red);

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
