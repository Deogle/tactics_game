using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public float unitX;
    public float unitY;
    public Map map;

    public List<Map.Node> currentPath = null;

    private void Update()
    {
        if(currentPath != null)
        {
            int currentNode = 0;
            while(currentNode < currentPath.Count - 1)
            {
                Vector3 start = map.TileCoordToWorldCoord(currentPath[currentNode].x,currentPath[currentNode].y);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currentNode+1].x, currentPath[currentNode+1].y);

                Debug.Log("Drawing a line");
                Debug.DrawLine(start,end, Color.red);


                currentNode++;
            }
        }
    }



}
