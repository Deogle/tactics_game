﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    public float tileX;
    public float tileY;
    public Map map;

    private void OnMouseUp()
    {
        //map.GeneratePathTo(tileX, tileY);
    }

}
