﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour {

    private enum TileFloors
    {
        GRASS_TILE,
        WOOD_TILE,
        WATER_TILE,
        COUNT
    };

    private enum UnitTypeEnum
    {
        MELEE_UNIT,
        RANGED_UNIT,
        COUNT
    };

    public TileType[] tileTypes;
    public UnitType[] unitTypes;

    public GameObject selectedUnit;

    int[,] tiles;
    Node[,] graph;

    public ClickableTile[,] referenceTiles;
    public List<GameObject> units;

    public int sizeX;
    public int sizeY;

    public bool addUnitMode = false;

    void Start()
    {
        GenerateMapData();
        GeneratePathGraph();
        GenerateMapVisual();
        SpawnUnits(0,0);
        selectedUnit = null;
    }//Start


    void GenerateMapData()
    {
        Debug.Log("making map data");
        //Allocate map tiles
        tiles = new int[sizeX, sizeY];
        referenceTiles = new ClickableTile[sizeX, sizeY];

        //Initial map tiles 
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                tiles[x, y] = 0;
            }
        }

        //Instantiate Board
        //TODO change generation algorithm to trend toward aesthetically pleasing maps
        //  with more natural formations like grasslands, 'forts', and lakes.
        int val;
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                if(x == 0 || y == 0 || x == sizeX-1 || y == sizeY - 1)
                {
                    val = (int)TileFloors.WOOD_TILE;
                    tiles[x, y] = val;
                } else
                {
                    val = Random.Range(0, System.Enum.GetValues(typeof(TileFloors)).Length - 1);
                    tiles[x, y] = val;
                }
                
            }
        }

    }//GenerateMapData

    float TileCost(float sourceX, float sourceY, float targetX, float targetY,bool hasUnit)
    {
        
        TileType tt = tileTypes[tiles[(int)targetX,(int)targetY]];

        float cost = tt.movementCost;

        if(sourceX != targetX && sourceY != targetY)
        {
            cost += 0.0001f;
        }

        if (hasUnit)
        {
            cost += 100000;
        }

        return cost;
    }

    void GeneratePathGraph()
    {
        Debug.Log("Generating Path Graph");
        //Init array
        graph = new Node[sizeX, sizeY];

        //Init a node for each tile being added to the graph 
        //Problem is that this is also including the grass tiles which shouldn't be part of the graph
        for(int x= 0; x < sizeX; x++)
        {
            for(int y= 0; y < sizeY; y++)
            {
                graph[x, y] = new Node
                {
                    x = x,
                    y = y
                };
            }
        }

        for (int x = 0; x < sizeX; x++)
        {
            for(int y= 0; y < sizeY; y++)
            {
                //4 edges per node
                if(x > 0)                
                    graph[x, y].edges.Add(graph[x - 1, y]); //connect via left
                if(x < sizeX-1)                
                    graph[x, y].edges.Add(graph[x + 1, y]); //connect via right
                if(y > 0)
                    graph[x, y].edges.Add(graph[x, y - 1]); //connect via bottom
                if (y < sizeY - 1)
                    graph[x, y].edges.Add(graph[x, y + 1]); //connect via top
            }
        }
    }

    void GenerateMapVisual()
    {
        Debug.Log("Generating Map Visual");

        for (int x = 0; x < sizeX; x++)
        {
            for(int y=0; y < sizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                GameObject tile = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
                
                ClickableTile ct = tile.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
                referenceTiles[x, y] = ct;
                                
            }
        }

        //selectedUnit = (GameObject)  Instantiate(unit, new Vector3(startY, startX, 0), Quaternion.identity);

    }//GenerateMapVisual

    public Vector3 TileCoordToWorldCoord(float x, float y)
    {
        return new Vector3(x, y, 0);
    }

    public void GeneratePathTo(float x, float y)
    {
        //Clear old path
        //selectedUnit.GetComponent<Unit>().currentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //list of unvisited nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            (int)selectedUnit.GetComponent<Unit>().unitX,
            (int)selectedUnit.GetComponent<Unit>().unitY
            ];

        Node target = graph[(int)x,(int)y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            //u is going to be the unvisited node with the smallest distance
            Node u = null;

            foreach(Node possibleU in unvisited)
            {
                if(u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            unvisited.Remove(u);

            foreach(Node v in u.edges)
            {
                float alt = dist[u] + TileCost(u.x,u.y,v.x,v.y,referenceTiles[(int)v.x,(int)v.y].ContainsUnit);
                //float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v] && alt <= selectedUnit.GetComponent<Unit>().movementPoints)
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if(prev[target] == null)
        {
            //No route
            return;
        }

        List<Node> currentPath = new List<Node>();
        Node current = target;

        //Step back through path
        while(current != null)
        {
            currentPath.Add(current);
            current = prev[current];
        }

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().possiblePaths.Add(currentPath);

    }//Generate Path using Dijkstra's pathfinding algorithm

    public void ClearAllMoves()
    {
        foreach(ClickableTile tile in referenceTiles)
        {
            tile.ClearMove();
        }
    }
    
    public void SpawnUnits(int x, int y)
    {
        /*GameObject go = (GameObject)Instantiate(units[0], new Vector3(0, 0, 0), Quaternion.identity);
        go.GetComponent<Unit>().unitX = 0;
        go.GetComponent<Unit>().unitY = 0;
        go.GetComponent<Unit>().map = this;

        go = Instantiate(units[1], new Vector3(1, 0, 0), Quaternion.identity);

        go.GetComponent<Unit>().unitX = 1;
        go.GetComponent<Unit>().unitY = 0;
        go.GetComponent<Unit>().map = this;*/

        GameObject go = (GameObject)Instantiate(units[1], new Vector3(x, y, 0), Quaternion.identity);
        go.GetComponent<Unit>().unitX = x;
        go.GetComponent<Unit>().unitY = y;
        go.GetComponent<Unit>().map = this;


    }

    public void MoveToTile(float x, float y)
    {
        referenceTiles[(int)x, (int)y].ContainsUnit = true;
    }

    public void MoveOffTile(float x, float y)
    {
        referenceTiles[(int)x, (int)y].ContainsUnit = false;
    }

    public void ToggleAddUnitMode()
    {
        addUnitMode = !addUnitMode;
    }
}
