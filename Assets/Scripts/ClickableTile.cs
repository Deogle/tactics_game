using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    public float tileX;
    public float tileY;
    public Map map;

    private void OnMouseUp()
    {
        Debug.Log("Clicked tile " + tileX + "," + tileY);
        Debug.Log("Pathing to" + map.TileCoordToWorldCoord(tileX, tileY));
        map.GeneratePathTo(tileX, tileY);
    }

}
