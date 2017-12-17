using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {

    public string name;
    public GameObject tileVisualPrefab;


    public bool ContainsUnit = false;
    public float movementCost = 1;
    
}