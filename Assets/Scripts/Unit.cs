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



    private void Update()
    {
        if(currentPath != null)
        {
            int currentNode = 0;
            while(currentNode < currentPath.Count-1)
            {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currentNode].x,currentPath[currentNode].y);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currentNode+1].x, currentPath[currentNode+1].y);

                Debug.DrawLine(start,end, Color.red);


                currentNode++;
            }
        }
    }

    public void MoveNextTile()
    {
        
    }


}
