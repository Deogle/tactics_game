using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour {

    private const int GRASS_TILE = 0;
    private const int WOOD_TILE = 1;

    public TileType[] tileTypes;
    public GameObject unit;

    public GameObject selectedUnit;

    int[,] tiles;
    Node[,] graph;

    public int sizeX;
    public int sizeY;

    void Start()
    {
        GenerateMapData();
        GeneratePathGraph();
        GenerateMapVisual();

        //Setup unit variables
        selectedUnit.GetComponent<Unit>().unitX = selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().unitY = selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

    }//Start


    void GenerateMapData()
    {
        Debug.Log("making map data");
        //Allocate map tiles
        tiles = new int[sizeX, sizeY];

        //Initial map tiles 
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                tiles[x, y] = 0;
            }
        }

        //Instantiate Board
        for (int x = 0; x < sizeX; ++x)
        {
            for (int y = 0; y < sizeY; ++y)
            {
                int val = (int)Random.Range(GRASS_TILE, WOOD_TILE+1);
                tiles[x, y] = val;
            }
        }
    }//GenerateMapData

    float TileCost(float sourceX, float sourceY, float targetX, float targetY)
    {
        
        TileType tt = tileTypes[tiles[(int)targetX,(int)targetY]];

        float cost = tt.movementCost;

        if(sourceX != targetX && sourceY != targetY)
        {
            cost += 0.0001f;
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
                /*4 edges per node
                if(x > 0)                
                    graph[x, y].edges.Add(graph[x - 1, y]); //connect via left
                if(x < sizeX-1)                
                    graph[x, y].edges.Add(graph[x + 1, y]); //connect via right
                if(y > 0)
                    graph[x, y].edges.Add(graph[x, y - 1]); //connect via bottom
                if (y < sizeY - 1)
                    graph[x, y].edges.Add(graph[x, y + 1]); //connect via top*/

                //8 edges per node
                if (x > 0)
                {
                    graph[x, y].edges.Add(graph[x - 1, y]); //connect via left
                    if (y > 0)
                        graph[x, y].edges.Add(graph[x - 1, y - 1]);
                    if (y < sizeY - 1)
                        graph[x, y].edges.Add(graph[x - 1, y + 1]);
                }
                if (x < sizeX - 1)
                { 
                    graph[x, y].edges.Add(graph[x + 1, y]); //connect via right
                    if (y > 0)
                        graph[x, y].edges.Add(graph[x + 1, y - 1]);
                    if (y < sizeY - 1)
                        graph[x, y].edges.Add(graph[x + 1, y + 1]);
                }
                if (y > 0)
                    graph[x, y].edges.Add(graph[x, y - 1]); //connect via bottom
                if (y < sizeY - 1)
                    graph[x, y].edges.Add(graph[x, y + 1]); //connect via top
            }
        }
    }

    void GenerateMapVisual()
    {
        Debug.Log("Generating Map Visual");
        int startX = 0;
        int startY = 0;

        for (int x = 0; x < sizeX; x++)
        {
            for(int y=0; y < sizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
                
                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
                
                
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
        selectedUnit.GetComponent<Unit>().currentPath = null;

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
                float alt = dist[u] + TileCost(u.x,u.y,v.x,v.y);
                //float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v] && alt <= selectedUnit.GetComponent<Unit>().movementPoints+1)
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

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;

    }//Generate Path using Dijkstra's pathfinding algorithm

}
